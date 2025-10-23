// LMS.GUI/OrderAdmin/ucOrder_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucOrder_Admin : UserControl
    {
        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();

        public ucOrder_Admin()
        {
            InitializeComponent();
            this.Load += UcOrder_Admin_Load;

        }

        private void UcOrder_Admin_Load(object sender, EventArgs e)
        {
            ConfigureOrderGrid();
            WireButtons();
            LoadData();

            dgvOrders.CellDoubleClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0)
                {
                    var id = GetSelectedOrderId();
                    if (id != null) OpenDetailPopup(id.Value);
                }
            };
            dgvOrders.SelectionChanged += (s, ev) => UpdateActionButtonsState();
        }

        private void ConfigureOrderGrid()
        {
            var g = dgvOrders;
            g.Columns.Clear();
            g.AutoGenerateColumns = false;
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;
            g.RowHeadersVisible = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.BorderStyle = BorderStyle.FixedSingle;
            g.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            g.EnableHeadersVisualStyles = false;
            g.GridColor = Color.Gainsboro;

            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 36;

            g.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            g.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells }); // NEW
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustomerName", DataPropertyName = "CustomerName", HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OriginWarehouseName", DataPropertyName = "OriginWarehouseName", HeaderText = "Kho gửi", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DestWarehouseName", DataPropertyName = "DestWarehouseName", HeaderText = "Kho nhận", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalFee", DataPropertyName = "TotalFee", HeaderText = "Tổng phí", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepositAmount", DataPropertyName = "DepositAmount", HeaderText = "Đặt cọc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Ngày tạo", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });

            TryEnableDoubleBuffer(g);
        }

        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try { grid.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(grid, true, null); }
            catch { }
        }

        private void LoadData()
        {
            var list = _orderSvc.GetAllForAdmin();
            dgvOrders.DataSource = list;
            UpdateActionButtonsState();
        }

        private int? GetSelectedOrderId()
        {
            if (dgvOrders.CurrentRow == null) return null;
            var val = dgvOrders.CurrentRow.Cells["Id"].Value;
            if (val == null) return null;
            return Convert.ToInt32(val);
        }

        private void WireButtons()
        {
            btnReload.Click += (s, e) => LoadData();

            btnViewDetail.Click += (s, e) =>
            {
                var id = GetSelectedOrderId();
                if (id != null) OpenDetailPopup(id.Value);
            };

            btnSearch.Click += (s, e) => OpenSearchPopup();

            btnApprove.Click += (s, e) =>
            {
                var id = GetSelectedOrderId();
                if (id == null) return;
                if (MessageBox.Show("Duyệt đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { _orderSvc.Approve(id.Value); LoadData(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "LMS"); }
            };

            btnReject.Click += (s, e) =>
            {
                var id = GetSelectedOrderId();
                if (id == null) return;
                if (MessageBox.Show("Từ chối đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { _orderSvc.Reject(id.Value); LoadData(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "LMS"); }
            };

            btnDelete.Click += (s, e) =>
            {
                var id = GetSelectedOrderId();
                if (id == null) return;
                if (MessageBox.Show("Xoá đơn này vĩnh viễn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                try { _orderSvc.Delete(id.Value); LoadData(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "LMS"); }
            };

            btnShipment.Click += (s, e) =>
            {
                var id = GetSelectedOrderId();
                if (id == null) return;
                if (MessageBox.Show("Tạo shipment cho đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try
                {
                    var sid = _orderSvc.CreateShipmentFromOrder(id.Value);
                    MessageBox.Show($"Đã tạo Shipment #{sid}.", "LMS");
                    LoadData();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "LMS"); }
            };
        }

        private void UpdateActionButtonsState()
        {
            bool has = dgvOrders.CurrentRow != null;
            btnViewDetail.Enabled = has;
            btnSearch.Enabled = true;
            btnReload.Enabled = true;
            btnApprove.Enabled = false;
            btnReject.Enabled = false;
            btnDelete.Enabled = false;
            btnShipment.Enabled = false;

            if (!has) return;

            var statusObj = dgvOrders.CurrentRow.Cells["Status"].Value;
            if (statusObj == null) return;
            var status = (OrderStatus)Enum.Parse(typeof(OrderStatus), statusObj.ToString());

            if (status == OrderStatus.Pending)
            {
                btnApprove.Enabled = true;
                btnReject.Enabled = true;
                btnDelete.Enabled = true;
            }
            else if (status == OrderStatus.Approved)
            {
                btnShipment.Enabled = true;
            }
            else if (status == OrderStatus.Cancelled)
            {
                btnDelete.Enabled = true; // cho xoá khi Cancelled
            }
        }

        // === mở Detail dạng POPUP ===
        private void OpenDetailPopup(int id)
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Chi tiết đơn hàng";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.Size = new Size(720, 600);
                var uc = new ucOrderDetail_Admin(id);
                dlg.Controls.Add(uc);
                dlg.ShowDialog(this.FindForm());
            }
        }

        // === mở Search dạng POPUP, double-click trả về → mở Detail POPUP luôn ===
        //private void OpenSearchPopup()
        //{
        //    using (var dlg = new Form())
        //    {
        //        dlg.Text = "Tìm kiếm đơn hàng";
        //        dlg.StartPosition = FormStartPosition.CenterParent;
        //        dlg.Size = new Size(1100, 700);

        //        var uc = new ucOrderSearch_Admin { Dock = DockStyle.Fill };
        //        uc.OrderPicked += (s, orderId) =>
        //        {
        //            //dlg.Close();              // đóng popup search
        //            OpenDetailPopup(orderId); // mở popup chi tiết
        //        };
        //        dlg.Controls.Add(uc);
        //        dlg.ShowDialog(this.FindForm());
        //    }
        //}
        private void OpenSearchPopup()
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Tìm kiếm đơn hàng";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.Size = new Size(1100, 700);

                var uc = new ucOrderSearch_Admin();
                //var uc = new ucOrderSearch_Admin { Dock = DockStyle.Fill };

                uc.OrderPicked += (s, orderId) =>
                {
                    // Mở popup chi tiết CHỒNG LÊN, owner = dlg (popup tìm kiếm)
                    OpenDetailPopup(orderId, dlg);   // <— truyền owner

                    // (tuỳ chọn) sau khi đóng detail có thể refresh lại kết quả tìm kiếm:
                    // uc.RefreshCurrentSearch(); // nếu bạn có method này
                };

                dlg.Controls.Add(uc);
                dlg.ShowDialog(this.FindForm());
            }
        }
        private void OpenDetailPopup(int id, IWin32Window owner = null)
        {
            using (var frm = new Form())
            {
                frm.Text = "Chi tiết đơn hàng";
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Size = new Size(720, 600);

                var uc = new ucOrderDetail_Admin(id);
                //var uc = new ucOrderDetail_Admin(id) { Dock = DockStyle.Fill };
                frm.Controls.Add(uc);

                // nếu có owner (ví dụ: popup tìm kiếm) thì ShowDialog(owner)
                frm.ShowDialog(owner ?? this.FindForm());
            }
        }


    }
}
