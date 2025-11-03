using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using LMS.DAL;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LMS.GUI.OrderCustomer
{
    public partial class ucOrderCreate_Cus : UserControl
    {
        private readonly int _customerId;
        private readonly ErrorProvider err = new ErrorProvider();
        private readonly RoutePricingService _pricing = new RoutePricingService();
        private readonly OrderDraft _initDraft;
        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();

        private List<ZoneItem> vnZones;

        public ucOrderCreate_Cus(int customerId, OrderDraft draft)
        {
            InitializeComponent();
            _customerId = customerId;
            _initDraft = draft;
            err.ContainerControl = this;
            this.Load += UcOrderCreate_Cus_Load;
            InitializeTooltip();
            OrderCreateCusFieldTips();
        }

        public ucOrderCreate_Cus(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            _initDraft = null;
            err.ContainerControl = this;
            this.Load += UcOrderCreate_Cus_Load;
            InitializeTooltip();
            OrderCreateCusFieldTips();
        }

        // cấu hình style tooltip
        private void InitializeTooltip()
        {
            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
            htip.ForeColor = Color.Black;
            htip.BackColor = Color.White;
            htip.BorderColor = Color.FromArgb(210, 210, 210);
            htip.MaximumSize = new Size(300, 0);
            htip.InitialDelay = 100;
            htip.AutoPopDelay = 30000;
            htip.ReshowDelay = 80;
            htip.ShowAlways = true;
            htip.UseAnimation = false;
            htip.UseFading = false;
        }

        private void UcOrderCreate_Cus_Load(object sender, EventArgs e)
        {
            // load vùng VN
            vnZones = new List<ZoneItem>
            {
                new ZoneItem { Value = Zone.North, Text = "Miền Bắc" },
                new ZoneItem { Value = Zone.Central, Text = "Miền Trung" },
                new ZoneItem { Value = Zone.South, Text = "Miền Nam" }
            };

            cmbOriginZone.Items.Clear();
            cmbDestZone.Items.Clear();
            cmbOriginZone.Items.AddRange(vnZones.Cast<object>().ToArray());
            cmbDestZone.Items.AddRange(vnZones.Cast<object>().ToArray());

            // thay đổi vùng/kho → reset nút Next và reload danh sách
            cmbOriginZone.SelectedIndexChanged += (s, ev) =>
            {
                LoadOriginWarehouses();
                LoadDestinationWarehouses();
                btnNext.Enabled = false;
            };

            cmbOriginWarehouse.SelectedIndexChanged += (s, ev) =>
            {
                LoadDestinationWarehouses();
                btnNext.Enabled = false;
            };

            cmbDestZone.SelectedIndexChanged += (s, ev) =>
            {
                LoadDestinationWarehouses();
                btnNext.Enabled = false;
            };

            cmbDestWarehouse.SelectedIndexChanged += (s, ev) => btnNext.Enabled = false;

            chkPickupAtSender.CheckedChanged += (s, ev) =>
            {
                txtPickupAddress.Enabled = chkPickupAtSender.Checked;
                if (!chkPickupAtSender.Checked) txtPickupAddress.Clear();
                btnNext.Enabled = false;
            };

            btnCalcFee.Click += (s, ev) => ShowEstimatedFee();
            btnNext.Click += (s, ev) => GoConfirm();
            btnClear.Click += (s, ev) => ClearForm();

            txtPickupAddress.Enabled = false;
            lblEstimatedFee.Text = "Tổng phí: —";

            dtpDesiredTime.Value = DateTime.Today;
            dtpDesiredTime.Format = DateTimePickerFormat.Custom;
            dtpDesiredTime.CustomFormat = "dd/MM/yyyy";
            dtpDesiredTime.MinDate = DateTime.Today;

            if (_initDraft != null) PrefillFromDraft(_initDraft);

            btnNext.Enabled = false;
        }

        // set lại form theo draft cũ (user quay lại)
        private void PrefillFromDraft(OrderDraft d)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Warehouses.Find(d.OriginWarehouseId);
                var dst = db.Warehouses.Find(d.DestWarehouseId);
                if (o == null || dst == null) return;

                SelectZoneInCombo(cmbOriginZone, o.ZoneId);
                LoadOriginWarehouses();
                SelectWarehouseInCombo(cmbOriginWarehouse, d.OriginWarehouseId);

                SelectZoneInCombo(cmbDestZone, dst.ZoneId);
                LoadDestinationWarehouses();
                SelectWarehouseInCombo(cmbDestWarehouse, d.DestWarehouseId);

                chkPickupAtSender.Checked = d.NeedPickup;
                txtPickupAddress.Enabled = d.NeedPickup;
                txtPickupAddress.Text = d.PickupAddress ?? "";
                txtPackageDescription.Text = d.PackageDescription ?? "";
                if (d.DesiredTime.HasValue) dtpDesiredTime.Value = d.DesiredTime.Value;

                var route = _pricing.GetRouteFee(d.OriginWarehouseId, d.DestWarehouseId);
                var pickupFee = _pricing.GetPickupFee(d.NeedPickup);
                var total = route + pickupFee;

                lblEstimatedFee.Text =
                    $"Tổng phí: {total:N0} đ \n(Tuyến: {route:N0} đ{(d.NeedPickup ? ", Pickup: 100.000 đ" : "")})";

                btnNext.Enabled = true;
            }
        }

        private void SelectZoneInCombo(Guna2ComboBox cmb, Zone zone)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
                if ((cmb.Items[i] as ZoneItem)?.Value == zone)
                { cmb.SelectedIndex = i; break; }
        }

        private void SelectWarehouseInCombo(Guna2ComboBox cmb, int warehouseId)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
                if ((cmb.Items[i] as ComboItem)?.Value == warehouseId)
                { cmb.SelectedIndex = i; break; }
        }

        // load kho gửi theo vùng
        private void LoadOriginWarehouses()
        {
            int? oldSelectedId = (cmbOriginWarehouse.SelectedItem as ComboItem)?.Value;
            cmbOriginWarehouse.Items.Clear();
            cmbOriginWarehouse.Text = "";

            if (cmbOriginZone.SelectedItem is ZoneItem zi)
            {
                using (var db = new LogisticsDbContext())
                {
                    var list = db.Warehouses
                        .Where(w => w.ZoneId == zi.Value && w.IsActive)
                        .OrderBy(w => w.Name)
                        .Select(w => new ComboItem { Value = w.Id, Text = w.Name })
                        .ToList();

                    cmbOriginWarehouse.Items.AddRange(list.Cast<object>().ToArray());
                    if (oldSelectedId.HasValue) SelectWarehouseInCombo(cmbOriginWarehouse, oldSelectedId.Value);
                }
            }
        }

        // load kho nhận, tự loại kho gửi nếu cùng vùng
        private void LoadDestinationWarehouses()
        {
            int? oldSelectedId = (cmbDestWarehouse.SelectedItem as ComboItem)?.Value;
            cmbDestWarehouse.Items.Clear();
            cmbDestWarehouse.Text = "";

            if (cmbDestZone.SelectedItem is ZoneItem ziDest)
            {
                int? excludeId = null;

                if (cmbOriginZone.SelectedItem is ZoneItem ziOrigin && ziOrigin.Value == ziDest.Value)
                    if (cmbOriginWarehouse.SelectedItem is ComboItem originWh)
                        excludeId = originWh.Value;

                using (var db = new LogisticsDbContext())
                {
                    var query = db.Warehouses.Where(w => w.ZoneId == ziDest.Value && w.IsActive);
                    if (excludeId.HasValue) query = query.Where(w => w.Id != excludeId.Value);

                    var list = query.OrderBy(w => w.Name)
                        .Select(w => new ComboItem { Value = w.Id, Text = w.Name })
                        .ToList();

                    cmbDestWarehouse.Items.AddRange(list.Cast<object>().ToArray());
                    if (oldSelectedId.HasValue) SelectWarehouseInCombo(cmbDestWarehouse, oldSelectedId.Value);
                }
            }
        }

        // validate input cơ bản
        private bool ValidateAll()
        {
            err.Clear();
            bool ok = true;

            if (cmbOriginZone.SelectedItem == null) { err.SetError(cmbOriginZone, "Chọn vùng gửi"); ok = false; }
            if (cmbOriginWarehouse.SelectedItem == null) { err.SetError(cmbOriginWarehouse, "Chọn kho gửi"); ok = false; }
            if (cmbDestZone.SelectedItem == null) { err.SetError(cmbDestZone, "Chọn vùng nhận"); ok = false; }
            if (cmbDestWarehouse.SelectedItem == null) { err.SetError(cmbDestWarehouse, "Chọn kho nhận"); ok = false; }

            if (ok && ((ComboItem)cmbOriginWarehouse.SelectedItem).Value ==
                      ((ComboItem)cmbDestWarehouse.SelectedItem).Value)
            {
                err.SetError(cmbDestWarehouse, "Kho nhận phải khác kho gửi");
                ok = false;
            }

            if (chkPickupAtSender.Checked && string.IsNullOrWhiteSpace(txtPickupAddress.Text))
            {
                err.SetError(txtPickupAddress, "Nhập địa chỉ lấy hàng");
                ok = false;
            }

            return ok;
        }

        // tính phí xem trước
        private void ShowEstimatedFee()
        {
            if (!ValidateAll()) { btnNext.Enabled = false; return; }

            int originId = ((ComboItem)cmbOriginWarehouse.SelectedItem).Value;
            int destId = ((ComboItem)cmbDestWarehouse.SelectedItem).Value;
            bool pickup = chkPickupAtSender.Checked;

            var route = _pricing.GetRouteFee(originId, destId);
            var pickupFee = _pricing.GetPickupFee(pickup);
            var total = route + pickupFee;

            if (pickup)
            {
                lblEstimatedFee.Text =
                    $"- Tuyến: {route:N0} đ \n" +
                    $"- Lấy hàng: 100.000 đ\n" +
                    $"- Tổng phí: {total:N0} đ \n";
            }
            else
            {
                lblEstimatedFee.Text =
                    $"- Tuyến: {route:N0} đ \n" +
                    $"- Tổng phí: {total:N0} đ \n";
            }

            btnNext.Enabled = true;
        }

        // đi tới màn xác nhận
        private void GoConfirm()
        {
            if (_customerId <= 0)
            {
                MessageBox.Show("Không xác định được khách hàng. Đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!btnNext.Enabled || !ValidateAll()) return;

            var draft = new OrderDraft
            {
                CustomerId = _customerId,
                OriginWarehouseId = ((ComboItem)cmbOriginWarehouse.SelectedItem).Value,
                DestWarehouseId = ((ComboItem)cmbDestWarehouse.SelectedItem).Value,
                NeedPickup = chkPickupAtSender.Checked,
                PickupAddress = chkPickupAtSender.Checked ? txtPickupAddress.Text.Trim() : null,
                PackageDescription = txtPackageDescription.Text.Trim(),
                DesiredTime = dtpDesiredTime.Value
            };

            draft.RouteFee = _pricing.GetRouteFee(draft.OriginWarehouseId, draft.DestWarehouseId);
            draft.PickupFee = _pricing.GetPickupFee(draft.NeedPickup);
            draft.TotalFee = draft.RouteFee + draft.PickupFee;
            draft.DepositAmount = Math.Round(draft.TotalFee * draft.DepositPercent, 0);

            using (var fConfirm = new Form
            {
                Text = "Xác nhận và Thanh toán",
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(484, 449)
            })
            {
                var ucConfirm = new ucOrderConfirm_Cus(draft) { Dock = DockStyle.Fill };

                ucConfirm.OrderCreated += (s, args) => fConfirm.DialogResult = DialogResult.OK;

                fConfirm.Controls.Add(ucConfirm);
                var dialogResult = fConfirm.ShowDialog(this.FindForm());

                if (dialogResult == DialogResult.OK)
                {
                    MessageBox.Show("Đã tạo đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
            }
        }

        // reset form tạo mới
        private void ClearForm()
        {
            cmbOriginZone.SelectedItem = null;
            cmbOriginWarehouse.Items.Clear();
            cmbDestZone.SelectedItem = null;
            cmbDestWarehouse.Items.Clear();

            cmbOriginZone.Items.Clear();
            cmbDestZone.Items.Clear();
            cmbOriginZone.Items.AddRange(vnZones.Cast<object>().ToArray());
            cmbDestZone.Items.AddRange(vnZones.Cast<object>().ToArray());

            chkPickupAtSender.Checked = false;
            txtPickupAddress.Clear();
            txtPackageDescription.Clear();
            lblEstimatedFee.Text = "Tổng phí: —";
            btnNext.Enabled = false;
            err.Clear();
        }

        // tooltip hướng dẫn các trường nhập liệu
        private void OrderCreateCusFieldTips()
        {
            AttachHtmlTip(cmbOriginZone, "<b>Vùng gửi hàng</b>: Chọn vùng.");
            AttachHtmlTip(cmbOriginWarehouse, "<b>Kho gửi hàng</b>: Chọn kho gửi.");
            AttachHtmlTip(cmbDestZone, "<b>Vùng nhận hàng</b>: Chọn vùng nhận.");
            AttachHtmlTip(cmbDestWarehouse, "<b>Kho nhận hàng</b>: Chọn kho nhận.");
            AttachHtmlTip(txtPickupAddress, "<b>Địa chỉ lấy hàng</b>: Nhập địa chỉ đầy đủ.");
            AttachHtmlTip(dtpDesiredTime, "<b>Thời gian mong muốn</b>: Chọn ngày/giờ.");
        }

        // hiển thị tooltip cạnh control
        private void AttachHtmlTip(Control ctl, string html)
        {
            ctl.Enter += (s, e) =>
            {
                var ttSize = TextRenderer.MeasureText(
                    Regex.Replace(html, "<.*?>", string.Empty),
                    SystemFonts.DefaultFont);

                int estWidth = Math.Min(htip.MaximumSize.Width, ttSize.Width + 24);
                int estHeight = Math.Max(26, ttSize.Height + 14);

                int x = ctl.Width + 6;
                int y = (ctl.Height - estHeight) / 2 - 2;

                Point scr = ctl.PointToScreen(new Point(x, y));
                int screenRight = Screen.FromControl(this).WorkingArea.Right;
                int screenTop = Screen.FromControl(this).WorkingArea.Top;
                int screenBottom = Screen.FromControl(this).WorkingArea.Bottom;

                if (scr.X + estWidth > screenRight) x = -estWidth - 8;

                scr = ctl.PointToScreen(new Point(x, y));
                if (scr.Y < screenTop) y += (screenTop - scr.Y);
                else if (scr.Y + estHeight > screenBottom) y -= (scr.Y + estHeight - screenBottom);

                htip.Show(html, ctl, x, y, int.MaxValue);
            };

            ctl.Leave += (s, e) => htip.Hide(ctl);
            ctl.EnabledChanged += (s, e) => { if (!ctl.Enabled) htip.Hide(ctl); };
            ctl.VisibleChanged += (s, e) => { if (!ctl.Visible) htip.Hide(ctl); };
        }

        // lớp bind combobox vùng
        private class ZoneItem
        {
            public Zone Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        // lớp bind combobox kho
        private class ComboItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }
    }
}
