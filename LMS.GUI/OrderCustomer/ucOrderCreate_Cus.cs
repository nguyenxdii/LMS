//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos;
//using LMS.BUS.Services;
//using LMS.DAL;
//using LMS.DAL.Models;
//using LMS.GUI.Main;                // để cast FindForm()
//using System;
//using System.Data.Entity;          // nếu bạn dùng Include ở nơi khác
//using System.Drawing;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Windows.Forms;

//namespace LMS.GUI.OrderCustomer
//{
//    public partial class ucOrderCreate_Cus : UserControl
//    {
//        private readonly int _customerId;
//        private readonly ErrorProvider err = new ErrorProvider();
//        private readonly RoutePricingService _pricing = new RoutePricingService();
//        private readonly OrderDraft _initDraft;
//        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();

//        public ucOrderCreate_Cus(int customerId, OrderDraft draft)
//        {
//            InitializeComponent();
//            _customerId = customerId;
//            _initDraft = draft;
//            err.ContainerControl = this;
//            this.Load += UcOrderCreate_Cus_Load;

//            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
//            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
//            htip.ForeColor = Color.Black;
//            htip.BackColor = Color.White;
//            htip.BorderColor = Color.FromArgb(210, 210, 210);
//            htip.MaximumSize = new Size(300, 0);
//            htip.InitialDelay = 100;
//            htip.AutoPopDelay = 30000;
//            htip.ReshowDelay = 80;
//            htip.ShowAlways = true;
//            htip.UseAnimation = false;
//            htip.UseFading = false;

//            OrderCreateCusFieldTips();
//        }

//        public ucOrderCreate_Cus(int customerId)
//        {
//            InitializeComponent();
//            _customerId = customerId;
//            _initDraft = null;  // không có draft
//            err.ContainerControl = this;
//            this.Load += UcOrderCreate_Cus_Load;

//            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
//            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
//            htip.ForeColor = Color.Black;
//            htip.BackColor = Color.White;
//            htip.BorderColor = Color.FromArgb(210, 210, 210);
//            htip.MaximumSize = new Size(300, 0);
//            htip.InitialDelay = 100;
//            htip.AutoPopDelay = 30000;
//            htip.ReshowDelay = 80;
//            htip.ShowAlways = true;
//            htip.UseAnimation = false;
//            htip.UseFading = false;

//            OrderCreateCusFieldTips();
//        }

//        private void UcOrderCreate_Cus_Load(object sender, EventArgs e)
//        {
//            // Nạp danh sách vùng tiếng Việt
//            var vnZones = new[]
//            {
//                new ZoneItem { Value = Zone.North,   Text = "Miền Bắc" },
//                new ZoneItem { Value = Zone.Central, Text = "Miền Trung" },
//                new ZoneItem { Value = Zone.South,   Text = "Miền Nam" }
//            };

//            cmbOriginZone.Items.Clear();
//            cmbDestZone.Items.Clear();
//            foreach (var z in vnZones)
//            {
//                cmbOriginZone.Items.Add(z);
//                cmbDestZone.Items.Add(z);
//            }

//            // Khi đổi vùng gửi -> nạp kho gửi + lọc vùng nhận
//            cmbOriginZone.SelectedIndexChanged += (s, ev) =>
//            {
//                cmbOriginWarehouse.Items.Clear();
//                LoadWarehousesByZoneCombo(cmbOriginZone, cmbOriginWarehouse);

//                // lọc vùng nhận (bỏ vùng gửi)
//                cmbDestZone.Items.Clear();
//                var selected = cmbOriginZone.SelectedItem as ZoneItem;
//                foreach (var z in vnZones.Where(x => selected == null || x.Value != selected.Value))
//                    cmbDestZone.Items.Add(z);

//                cmbDestWarehouse.Items.Clear();
//            };

//            // Khi đổi vùng nhận -> nạp kho nhận
//            cmbDestZone.SelectedIndexChanged += (s, ev) =>
//            {
//                cmbDestWarehouse.Items.Clear();
//                LoadWarehousesByZoneCombo(cmbDestZone, cmbDestWarehouse);
//            };

//            chkPickupAtSender.CheckedChanged += (s, ev) =>
//            {
//                txtPickupAddress.Enabled = chkPickupAtSender.Checked;
//                if (!chkPickupAtSender.Checked) txtPickupAddress.Clear();
//            };

//            btnCalcFee.Click += (s, ev) => ShowEstimatedFee();
//            btnNext.Click += (s, ev) => GoConfirm();
//            btnClear.Click += (s, ev) => ClearForm();

//            txtPickupAddress.Enabled = false;
//            lblEstimatedFee.Text = "Tổng phí: —";

//            // Cấu hình Ngày gửi hàng
//            dtpDesiredTime.Value = DateTime.Today;
//            dtpDesiredTime.Format = DateTimePickerFormat.Custom;
//            dtpDesiredTime.CustomFormat = "dd/MM/yyyy";
//            dtpDesiredTime.MinDate = DateTime.Today;

//            if (_initDraft != null) PrefillFromDraft(_initDraft);

//            // ẩn nút Next ban đầu, chờ tính phí xong mới cho Next
//            btnNext.Enabled = false;
//        }

//        private void PrefillFromDraft(OrderDraft d)
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                var o = db.Warehouses.Find(d.OriginWarehouseId);
//                var dst = db.Warehouses.Find(d.DestWarehouseId);

//                // chọn vùng gửi
//                SelectZoneInCombo(cmbOriginZone, o.ZoneId);
//                LoadWarehousesByZoneCombo(cmbOriginZone, cmbOriginWarehouse);
//                SelectWarehouseInCombo(cmbOriginWarehouse, d.OriginWarehouseId);

//                // lọc vùng nhận theo vùng gửi rồi chọn vùng nhận
//                var vnZones = cmbDestZone.Items.Cast<ZoneItem>().ToList();
//                cmbDestZone.Items.Clear();
//                foreach (var z in vnZones.Where(x => x.Value != o.ZoneId)) cmbDestZone.Items.Add(z);

//                SelectZoneInCombo(cmbDestZone, dst.ZoneId);
//                LoadWarehousesByZoneCombo(cmbDestZone, cmbDestWarehouse);
//                SelectWarehouseInCombo(cmbDestWarehouse, d.DestWarehouseId);

//                // pickup + mô tả + thời gian
//                chkPickupAtSender.Checked = d.NeedPickup;
//                txtPickupAddress.Enabled = d.NeedPickup;
//                txtPickupAddress.Text = d.PickupAddress ?? "";
//                txtPackageDescription.Text = d.PackageDescription ?? "";
//                if (d.DesiredTime.HasValue) dtpDesiredTime.Value = d.DesiredTime.Value;

//                // tính lại phí & hiển thị
//                var route = _pricing.GetRouteFee(d.OriginWarehouseId, d.DestWarehouseId);
//                var pickupFee = _pricing.GetPickupFee(d.NeedPickup);
//                var total = route + pickupFee;
//                lblEstimatedFee.Text = $"Tổng phí: {total:N0} đ \n" +
//                    $"(Tuyến: {route:N0} đ{(d.NeedPickup ? ", Pickup: 100.000 đ" : "")})";
//            }
//        }

//        private void SelectZoneInCombo(Guna2ComboBox cmb, Zone zone)
//        {
//            for (int i = 0; i < cmb.Items.Count; i++)
//                if ((cmb.Items[i] as ZoneItem)?.Value == zone) { cmb.SelectedIndex = i; break; }
//        }
//        private void SelectWarehouseInCombo(Guna2ComboBox cmb, int warehouseId)
//        {
//            for (int i = 0; i < cmb.Items.Count; i++)
//                if ((cmb.Items[i] as ComboItem)?.Value == warehouseId) { cmb.SelectedIndex = i; break; }
//        }

//        // ===== Helpers (hàm phụ trợ trong UC) =====
//        private void LoadWarehousesByZoneCombo(Guna2ComboBox cmbZone, Guna2ComboBox cmbWarehouse)
//        {
//            cmbWarehouse.Items.Clear();
//            if (cmbZone.SelectedItem is ZoneItem zi)
//            {
//                using (var db = new LogisticsDbContext())
//                {
//                    var list = db.Warehouses
//                                 .Where(w => w.ZoneId == zi.Value)
//                                 .OrderBy(w => w.Name)
//                                 .Select(w => new { w.Id, w.Name })
//                                 .ToList();

//                    foreach (var w in list)
//                        cmbWarehouse.Items.Add(new ComboItem { Value = w.Id, Text = w.Name });
//                }
//            }
//        }

//        private bool ValidateAll()
//        {
//            err.Clear();
//            bool ok = true;

//            if (cmbOriginZone.SelectedItem == null) { err.SetError(cmbOriginZone, "Chọn vùng gửi"); ok = false; }
//            if (cmbOriginWarehouse.SelectedItem == null) { err.SetError(cmbOriginWarehouse, "Chọn kho gửi"); ok = false; }
//            if (cmbDestZone.SelectedItem == null) { err.SetError(cmbDestZone, "Chọn vùng nhận"); ok = false; }
//            if (cmbDestWarehouse.SelectedItem == null) { err.SetError(cmbDestWarehouse, "Chọn kho nhận"); ok = false; }

//            if (cmbOriginWarehouse.SelectedItem is ComboItem o &&
//                cmbDestWarehouse.SelectedItem is ComboItem d &&
//                o.Value == d.Value)
//            {
//                err.SetError(cmbDestWarehouse, "Kho nhận phải khác kho gửi");
//                ok = false;
//            }

//            if (chkPickupAtSender.Checked && string.IsNullOrWhiteSpace(txtPickupAddress.Text))
//            {
//                err.SetError(txtPickupAddress, "Nhập địa chỉ lấy hàng");
//                ok = false;
//            }

//            return ok;
//        }

//        private void ShowEstimatedFee()
//        {
//            if (!ValidateAll())
//            {
//                btnNext.Enabled = false;
//                return;
//            }

//            int originId = ((ComboItem)cmbOriginWarehouse.SelectedItem).Value;
//            int destId = ((ComboItem)cmbDestWarehouse.SelectedItem).Value;
//            bool pickup = chkPickupAtSender.Checked;

//            var route = _pricing.GetRouteFee(originId, destId);
//            var pickupFee = _pricing.GetPickupFee(pickup);
//            var total = route + pickupFee;

//            if (pickup)
//            {
//                lblEstimatedFee.Text =
//                $"- Tuyến: {route:N0} đ \n" +
//                $"- {(pickup ? "Lấy hàng: 100.000 đ" : "")}\n" +
//                $"- Tổng phí: {total:N0} đ \n";
//            }
//            else
//            {
//                lblEstimatedFee.Text =
//                $"- Tuyến: {route:N0} đ \n" +
//                //$"- {(pickup ? "Lấy hàng: 100.000 đ" : "")}\n" +
//                $"- Tổng phí: {total:N0} đ \n";
//            }

//            btnNext.Enabled = true;
//        }
//        private void GoConfirm()
//        {
//            // <--- THÊM ĐOẠN KIỂM TRA NÀY ---
//            if (_customerId <= 0)
//            {
//                MessageBox.Show("Lỗi: Không thể xác định ID khách hàng. \nVui lòng đăng nhập lại.", "Lỗi Người Dùng", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return; // Dừng lại
//            }

//            if (!btnNext.Enabled)
//            {
//                return; // Không cần ShowTip nữa vì đã có ErrorProvider
//            }
//            if (!ValidateAll())
//            {
//                return;
//            }

//            // --- Tạo đối tượng OrderDraft (giữ nguyên) ---
//            var draft = new OrderDraft
//            {
//                CustomerId = _customerId,
//                OriginWarehouseId = ((ComboItem)cmbOriginWarehouse.SelectedItem).Value,
//                DestWarehouseId = ((ComboItem)cmbDestWarehouse.SelectedItem).Value,
//                NeedPickup = chkPickupAtSender.Checked,
//                PickupAddress = chkPickupAtSender.Checked ? txtPickupAddress.Text.Trim() : null,
//                PackageDescription = txtPackageDescription.Text.Trim(),
//                DesiredTime = dtpDesiredTime.Value
//            };
//            draft.RouteFee = _pricing.GetRouteFee(draft.OriginWarehouseId, draft.DestWarehouseId);
//            draft.PickupFee = _pricing.GetPickupFee(draft.NeedPickup);
//            draft.TotalFee = draft.RouteFee + draft.PickupFee;
//            draft.DepositAmount = Math.Round(draft.TotalFee * draft.DepositPercent, 0);
//            // --- Kết thúc tạo OrderDraft ---


//            // === SỬA TỪ ĐÂY: Thay vì LoadUc, tạo Form popup ===

//            // Bỏ dòng cũ:
//            // var uc = new ucOrderConfirm_Cus(draft);
//            // var host = this.FindForm() as frmMain_Customer;
//            // host?.LoadUc(uc);

//            // Thay bằng code tạo Form popup:
//            using (var fConfirm = new Form
//            {
//                Text = "Xác nhận và Thanh toán", // Tiêu đề popup
//                StartPosition = FormStartPosition.CenterScreen, // Hiện giữa màn hình cha
//                                                                // Đặt kích thước phù hợp với ucOrderConfirm_Cus
//                                                                // Bạn cần xem kích thước của ucOrderConfirm_Cus trong design và cộng thêm lề
//                FormBorderStyle = FormBorderStyle.None,
//                Size = new Size(484, 522), // Ví dụ: Kích thước của Form chứa UC
//                //FormBorderStyle = FormBorderStyle.FixedDialog, // Kiểu viền dialog (có nút đóng)
//                //MaximizeBox = false,
//                //MinimizeBox = false
//            })
//            {
//                // Tạo UserControl xác nhận, truyền draft vào
//                var ucConfirm = new ucOrderConfirm_Cus(draft)
//                {
//                    Dock = DockStyle.Fill // Cho UC lấp đầy Form popup
//                };

//                // Gán sự kiện khi đơn hàng được tạo thành công TỪ BÊN TRONG ucOrderConfirm_Cus
//                // (Giả sử ucOrderConfirm_Cus có sự kiện tên là OrderCreated)
//                ucConfirm.OrderCreated += (s, args) =>
//                {
//                    fConfirm.DialogResult = DialogResult.OK; // Đóng popup và báo thành công
//                };
//                // Hoặc nếu ucOrderConfirm_Cus tự đóng form cha khi thành công thì không cần dòng trên

//                // Thêm UC vào Form popup
//                fConfirm.Controls.Add(ucConfirm);

//                // Hiển thị Form popup (dạng Dialog)
//                var dialogResult = fConfirm.ShowDialog(this.FindForm());

//                // Xử lý KẾT QUẢ sau khi popup đóng lại
//                if (dialogResult == DialogResult.OK)
//                {
//                    // Nếu người dùng đã xác nhận và tạo đơn thành công trong popup
//                    MessageBox.Show("Đã tạo đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

//                    // Tùy chọn: Quay về màn hình danh sách đơn hàng hoặc xóa form hiện tại
//                    ClearForm(); // Xóa trắng form tạo đơn
//                                 // Hoặc:
//                                 // var host = this.FindForm() as frmMain_Customer;
//                                 // host?.LoadUc(new ucOrderList_Cus(_customerId)); // Chuyển sang danh sách đơn
//                }
//                else
//                {
//                    // Người dùng đóng popup mà không xác nhận (hoặc có lỗi)
//                    // Không cần làm gì thêm ở đây, người dùng vẫn ở màn hình tạo đơn.
//                }
//            }
//            // === KẾT THÚC SỬA ===
//        }

//        private void ClearForm()
//        {
//            cmbOriginZone.SelectedItem = null;
//            cmbOriginWarehouse.Items.Clear();
//            cmbDestZone.Items.Clear();
//            cmbDestWarehouse.Items.Clear();

//            // nạp lại 3 vùng tiếng Việt cho CẢ 2 combobox
//            cmbOriginZone.Items.Clear();
//            cmbDestZone.Items.Clear();
//            cmbOriginZone.Items.Add(new ZoneItem { Value = Zone.North, Text = "Miền Bắc" });
//            cmbOriginZone.Items.Add(new ZoneItem { Value = Zone.Central, Text = "Miền Trung" });
//            cmbOriginZone.Items.Add(new ZoneItem { Value = Zone.South, Text = "Miền Nam" });

//            cmbDestZone.Items.Add(new ZoneItem { Value = Zone.North, Text = "Miền Bắc" });
//            cmbDestZone.Items.Add(new ZoneItem { Value = Zone.Central, Text = "Miền Trung" });
//            cmbDestZone.Items.Add(new ZoneItem { Value = Zone.South, Text = "Miền Nam" });

//            chkPickupAtSender.Checked = false;
//            txtPickupAddress.Clear();
//            txtPackageDescription.Clear();
//            lblEstimatedFee.Text = "Tổng phí: —";
//            err.Clear();
//        }

//        private void OrderCreateCusFieldTips()
//        {
//            AttachHtmlTip(cmbOriginZone, "<b>Vùng gửi hàng</b>: Chọn vùng (Miền Bắc, Miền Trung, Miền Nam).");
//            AttachHtmlTip(cmbOriginWarehouse, "<b>Kho gửi hàng</b>: Chọn kho từ danh sách sẵn có.");
//            AttachHtmlTip(cmbDestZone, "<b>Vùng nhận hàng</b>: Chọn vùng đến (Miền Bắc, Miền Trung, Miền Nam).");
//            AttachHtmlTip(cmbDestWarehouse, "<b>Kho nhận hàng</b>: Chọn kho nhận hàng từ danh sách.");
//            AttachHtmlTip(txtPickupAddress, "<b>Địa chỉ lấy hàng</b>: Nhập đầy đủ (số nhà, đường, phường/xã, quận/huyện, tỉnh/thành).");
//            AttachHtmlTip(dtpDesiredTime, "<b>Thời gian mong muốn</b>: Chọn ngày và giờ giao hàng mong muốn.");
//        }

//        private void AttachHtmlTip(Control ctl, string html)
//        {
//            ctl.Enter += (s, e) =>
//            {
//                var ttSize = TextRenderer.MeasureText(
//                    Regex.Replace(html, "<.*?>", string.Empty),
//                    SystemFonts.DefaultFont);

//                int estWidth = Math.Min(htip.MaximumSize.Width, ttSize.Width + 24);
//                int estHeight = Math.Max(26, ttSize.Height + 14);

//                int x = ctl.Width + 6;
//                int y = (ctl.Height - estHeight) / 2 - 2;

//                Point scr = ctl.PointToScreen(new Point(x, y));
//                int screenRight = Screen.FromControl(this).WorkingArea.Right;
//                int screenTop = Screen.FromControl(this).WorkingArea.Top;
//                int screenBottom = Screen.FromControl(this).WorkingArea.Bottom;

//                if (scr.X + estWidth > screenRight) x = -estWidth - 8;

//                scr = ctl.PointToScreen(new Point(x, y));
//                if (scr.Y < screenTop) y += (screenTop - scr.Y);
//                else if (scr.Y + estHeight > screenBottom) y -= (scr.Y + estHeight - screenBottom);

//                htip.Show(html, ctl, x, y, int.MaxValue);
//            };

//            ctl.Leave += (s, e) => htip.Hide(ctl);
//            ctl.EnabledChanged += (s, e) => { if (!ctl.Enabled) htip.Hide(ctl); };
//            ctl.VisibleChanged += (s, e) => { if (!ctl.Visible) htip.Hide(ctl); };
//        }

//        // ===== Inner types for ComboBox binding =====
//        private class ZoneItem
//        {
//            public Zone Value { get; set; }
//            public string Text { get; set; }
//            public override string ToString() => Text;
//        }

//        private class ComboItem
//        {
//            public int Value { get; set; }
//            public string Text { get; set; }
//            public override string ToString() => Text;
//        }
//    }
//}

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
using System.Collections.Generic; // Cần cho List

namespace LMS.GUI.OrderCustomer
{
    public partial class ucOrderCreate_Cus : UserControl
    {
        private readonly int _customerId;
        private readonly ErrorProvider err = new ErrorProvider();
        private readonly RoutePricingService _pricing = new RoutePricingService();
        private readonly OrderDraft _initDraft;
        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();

        // --- KHAI BÁO BIẾN TẠM ĐỂ TẢI VÙNG ---
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

        // Tách hàm khởi tạo Tooltip ra riêng
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
            // Nạp danh sách vùng tiếng Việt (Lưu vào biến toàn cục)
            vnZones = new List<ZoneItem>
            {
                new ZoneItem { Value = Zone.North,   Text = "Miền Bắc" },
                new ZoneItem { Value = Zone.Central, Text = "Miền Trung" },
                new ZoneItem { Value = Zone.South,   Text = "Miền Nam" }
            };

            cmbOriginZone.Items.Clear();
            cmbDestZone.Items.Clear();
            cmbOriginZone.Items.AddRange(vnZones.Cast<object>().ToArray());
            cmbDestZone.Items.AddRange(vnZones.Cast<object>().ToArray());

            // === SỬA LẠI LOGIC GÁN SỰ KIỆN ===
            cmbOriginZone.SelectedIndexChanged += (s, ev) =>
            {
                LoadOriginWarehouses();
                LoadDestinationWarehouses(); // Lọc lại kho đến
                btnNext.Enabled = false; // Yêu cầu tính lại phí
            };

            cmbOriginWarehouse.SelectedIndexChanged += (s, ev) =>
            {
                LoadDestinationWarehouses(); // Lọc lại kho đến
                btnNext.Enabled = false; // Yêu cầu tính lại phí
            };

            cmbDestZone.SelectedIndexChanged += (s, ev) =>
            {
                LoadDestinationWarehouses();
                btnNext.Enabled = false; // Yêu cầu tính lại phí
            };

            // Sự kiện này chỉ cần reset nút Next
            cmbDestWarehouse.SelectedIndexChanged += (s, ev) =>
            {
                btnNext.Enabled = false; // Yêu cầu tính lại phí
            };
            // === KẾT THÚC SỬA LOGIC ===

            chkPickupAtSender.CheckedChanged += (s, ev) =>
            {
                txtPickupAddress.Enabled = chkPickupAtSender.Checked;
                if (!chkPickupAtSender.Checked) txtPickupAddress.Clear();
                btnNext.Enabled = false; // Yêu cầu tính lại phí
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

        private void PrefillFromDraft(OrderDraft d)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Warehouses.Find(d.OriginWarehouseId);
                var dst = db.Warehouses.Find(d.DestWarehouseId);
                if (o == null || dst == null) return;

                // 1. chọn vùng gửi
                SelectZoneInCombo(cmbOriginZone, o.ZoneId);
                // 2. nạp kho gửi
                LoadOriginWarehouses();
                SelectWarehouseInCombo(cmbOriginWarehouse, d.OriginWarehouseId);

                // 3. chọn vùng nhận
                SelectZoneInCombo(cmbDestZone, dst.ZoneId);
                // 4. nạp kho nhận (hàm này sẽ tự động lọc kho gửi)
                LoadDestinationWarehouses();
                SelectWarehouseInCombo(cmbDestWarehouse, d.DestWarehouseId);

                // (Phần còn lại giữ nguyên)
                chkPickupAtSender.Checked = d.NeedPickup;
                txtPickupAddress.Enabled = d.NeedPickup;
                txtPickupAddress.Text = d.PickupAddress ?? "";
                txtPackageDescription.Text = d.PackageDescription ?? "";
                if (d.DesiredTime.HasValue) dtpDesiredTime.Value = d.DesiredTime.Value;

                var route = _pricing.GetRouteFee(d.OriginWarehouseId, d.DestWarehouseId);
                var pickupFee = _pricing.GetPickupFee(d.NeedPickup);
                var total = route + pickupFee;
                lblEstimatedFee.Text = $"Tổng phí: {total:N0} đ \n" +
                    $"(Tuyến: {route:N0} đ{(d.NeedPickup ? ", Pickup: 100.000 đ" : "")})";

                // Cho phép Next nếu đã tải lại
                btnNext.Enabled = true;
            }
        }

        private void SelectZoneInCombo(Guna2ComboBox cmb, Zone zone)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
                if ((cmb.Items[i] as ZoneItem)?.Value == zone) { cmb.SelectedIndex = i; break; }
        }
        private void SelectWarehouseInCombo(Guna2ComboBox cmb, int warehouseId)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
                if ((cmb.Items[i] as ComboItem)?.Value == warehouseId) { cmb.SelectedIndex = i; break; }
        }

        // ===== Helpers (hàm phụ trợ trong UC) =====

        // --- HÀM MỚI: TẢI KHO GỬI ---
        private void LoadOriginWarehouses()
        {
            // Lưu lại lựa chọn cũ (nếu có)
            int? oldSelectedId = (cmbOriginWarehouse.SelectedItem as ComboItem)?.Value;

            cmbOriginWarehouse.Items.Clear();
            cmbOriginWarehouse.SelectedItem = null;
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

                    // Chọn lại kho cũ nếu còn trong danh sách mới
                    if (oldSelectedId.HasValue)
                    {
                        SelectWarehouseInCombo(cmbOriginWarehouse, oldSelectedId.Value);
                    }
                }
            }
        }

        // --- HÀM MỚI: TẢI KHO NHẬN (CÓ LỌC) ---
        private void LoadDestinationWarehouses()
        {
            // Lưu lại lựa chọn cũ
            int? oldSelectedId = (cmbDestWarehouse.SelectedItem as ComboItem)?.Value;

            cmbDestWarehouse.Items.Clear();
            cmbDestWarehouse.SelectedItem = null;
            cmbDestWarehouse.Text = "";

            if (cmbDestZone.SelectedItem is ZoneItem ziDest)
            {
                int? excludeId = null;

                // Kiểm tra xem có cần lọc không (chỉ lọc khi Vùng Gửi và Vùng Nhận giống nhau)
                if (cmbOriginZone.SelectedItem is ZoneItem ziOrigin && ziOrigin.Value == ziDest.Value)
                {
                    if (cmbOriginWarehouse.SelectedItem is ComboItem originWh)
                    {
                        excludeId = originWh.Value; // Lấy ID kho gửi để loại trừ
                    }
                }

                using (var db = new LogisticsDbContext())
                {
                    var query = db.Warehouses
                                  .Where(w => w.ZoneId == ziDest.Value && w.IsActive);

                    if (excludeId.HasValue)
                    {
                        query = query.Where(w => w.Id != excludeId.Value); // Lọc bỏ kho gửi
                    }

                    var list = query.OrderBy(w => w.Name)
                                    .Select(w => new ComboItem { Value = w.Id, Text = w.Name })
                                    .ToList();

                    cmbDestWarehouse.Items.AddRange(list.Cast<object>().ToArray());

                    // Chọn lại kho cũ nếu còn trong danh sách mới
                    if (oldSelectedId.HasValue)
                    {
                        SelectWarehouseInCombo(cmbDestWarehouse, oldSelectedId.Value);
                    }
                }
            }
        }

        private bool ValidateAll()
        {
            err.Clear();
            bool ok = true;

            if (cmbOriginZone.SelectedItem == null) { err.SetError(cmbOriginZone, "Chọn vùng gửi"); ok = false; }
            if (cmbOriginWarehouse.SelectedItem == null) { err.SetError(cmbOriginWarehouse, "Chọn kho gửi"); ok = false; }
            if (cmbDestZone.SelectedItem == null) { err.SetError(cmbDestZone, "Chọn vùng nhận"); ok = false; }
            if (cmbDestWarehouse.SelectedItem == null) { err.SetError(cmbDestWarehouse, "Chọn kho nhận"); ok = false; }

            // Lỗi trùng kho chỉ xảy ra nếu người dùng chọn kho nhận trước,
            // sau đó đổi kho gửi thành kho nhận (đã được xử lý bằng cách lọc)
            // Nhưng chúng ta vẫn nên kiểm tra lại cho chắc chắn.
            if (ok && ((ComboItem)cmbOriginWarehouse.SelectedItem).Value == ((ComboItem)cmbDestWarehouse.SelectedItem).Value)
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

        // (Hàm ShowEstimatedFee, GoConfirm, ClearForm, ... giữ nguyên như file bạn cung cấp)
        private void ShowEstimatedFee()
        {
            if (!ValidateAll())
            {
                btnNext.Enabled = false;
                return;
            }

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
                $"- {(pickup ? "Lấy hàng: 100.000 đ" : "")}\n" +
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
        private void GoConfirm()
        {
            if (_customerId <= 0)
            {
                MessageBox.Show("Lỗi: Không thể xác định ID khách hàng. \nVui lòng đăng nhập lại.", "Lỗi Người Dùng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!btnNext.Enabled)
            {
                return;
            }
            if (!ValidateAll())
            {
                return;
            }

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
                Size = new Size(484, 522),
            })
            {
                var ucConfirm = new ucOrderConfirm_Cus(draft)
                {
                    Dock = DockStyle.Fill
                };

                ucConfirm.OrderCreated += (s, args) =>
                {
                    fConfirm.DialogResult = DialogResult.OK;
                };

                fConfirm.Controls.Add(ucConfirm);
                var dialogResult = fConfirm.ShowDialog(this.FindForm());

                if (dialogResult == DialogResult.OK)
                {
                    MessageBox.Show("Đã tạo đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
            }
        }

        private void ClearForm()
        {
            cmbOriginZone.SelectedItem = null;
            cmbOriginWarehouse.Items.Clear();
            cmbDestZone.SelectedItem = null;
            cmbDestWarehouse.Items.Clear();

            // Tải lại danh sách vùng (đã bị lọc trước đó)
            cmbOriginZone.Items.Clear();
            cmbDestZone.Items.Clear();
            cmbOriginZone.Items.AddRange(vnZones.Cast<object>().ToArray());
            cmbDestZone.Items.AddRange(vnZones.Cast<object>().ToArray());

            chkPickupAtSender.Checked = false;
            txtPickupAddress.Clear();
            txtPackageDescription.Clear();
            lblEstimatedFee.Text = "Tổng phí: —";
            btnNext.Enabled = false; // Tắt nút Next
            err.Clear();
        }

        private void OrderCreateCusFieldTips()
        {
            AttachHtmlTip(cmbOriginZone, "<b>Vùng gửi hàng</b>: Chọn vùng (Miền Bắc, Miền Trung, Miền Nam).");
            AttachHtmlTip(cmbOriginWarehouse, "<b>Kho gửi hàng</b>: Chọn kho từ danh sách sẵn có.");
            AttachHtmlTip(cmbDestZone, "<b>Vùng nhận hàng</b>: Chọn vùng đến (Miền Bắc, Miền Trung, Miền Nam).");
            AttachHtmlTip(cmbDestWarehouse, "<b>Kho nhận hàng</b>: Chọn kho nhận hàng từ danh sách.");
            AttachHtmlTip(txtPickupAddress, "<b>Địa chỉ lấy hàng</b>: Nhập đầy đủ (số nhà, đường, phường/xã, quận/huyện, tỉnh/thành).");
            AttachHtmlTip(dtpDesiredTime, "<b>Thời gian mong muốn</b>: Chọn ngày và giờ giao hàng mong muốn.");
        }

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

        // ===== Inner types for ComboBox binding =====
        private class ZoneItem
        {
            public Zone Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        private class ComboItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }
    }
}