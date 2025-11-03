using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.BUS.Dtos;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class ucDashboard_Cus : UserControl
    {
        private readonly OrderService_Customer _orderService;

        // sự kiện cho cha đăng ký: double-click vào đơn và bấm tạo đơn
        public event EventHandler<int> OrderDoubleClick;
        public event EventHandler CreateOrderClick;

        public ucDashboard_Cus()
        {
            InitializeComponent();
            _orderService = new OrderService_Customer();
            this.Load += ucDashboard_Cus_Load;
        }

        private void ucDashboard_Cus_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            // gắn click cho nút tạo đơn (đảm bảo đúng name trong designer)
            if (btnCreateOrder != null)
            {
                btnCreateOrder.Enabled = true;
                btnCreateOrder.Click -= btnCreateOrder_Click;
                btnCreateOrder.Click += btnCreateOrder_Click;
                btnCreateOrder.BringToFront();
            }
            else
            {
                MessageBox.Show("Không tìm thấy nút 'btnCreateOrder' – kiểm tra Name trong Designer.");
            }

            ConfigureRecentOrdersGrid();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            if (!AppSession.CustomerId.HasValue)
            {
                MessageBox.Show("Không thể tải dashboard. Vui lòng đăng nhập lại.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int customerId = AppSession.CustomerId.Value;
            lblWelcome.Text = $"Chào mừng trở lại, {AppSession.DisplayName}!";
            //lblWelcome.Left = (this.Width - lblWelcome.Width) / 2;
            //lblWelcome.Top = (this.Height - lblWelcome.Height) / 2;
            lblWelcome.Left = (this.Width - lblWelcome.Width) / 2;

            try
            {
                var allMyOrders = _orderService.GetAllByCustomer(customerId);

                // kpi cơ bản
                lblPendingCount.Text = allMyOrders.Count(o => o.Status == OrderStatus.Pending).ToString();
                lblInTransitCount.Text = allMyOrders.Count(o => o.Status == OrderStatus.Approved).ToString();
                lblCompletedCount.Text = allMyOrders.Count(o => o.Status == OrderStatus.Completed).ToString();

                // 5 đơn gần nhất
                var latest5 = allMyOrders
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(5)
                    .ToList();

                // nếu OrderNo trống thì phát sinh từ id để hiển thị
                foreach (var it in latest5)
                {
                    if (string.IsNullOrWhiteSpace(it.OrderNo))
                    {
                        it.OrderNo = OrderCode.ToCode(it.Id);
                    }
                }

                dgvRecentOrders.DataSource = latest5;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu dashboard: {ex.Message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureRecentOrdersGrid()
        {
            dgvRecentOrders.ApplyBaseStyle();
            dgvRecentOrders.AutoGenerateColumns = false;
            dgvRecentOrders.Columns.Clear();

            var colId = new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            };

            var colOrderNo = new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                DataPropertyName = "OrderNo",
                HeaderText = "Mã đơn",
                Width = 140
            };

            var colStatus = new DataGridViewTextBoxColumn
            {
                Name = "StatusVN",
                DataPropertyName = "StatusVN",
                HeaderText = "Trạng thái",
                Width = 140
            };

            var colCreated = new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                DataPropertyName = "CreatedAt",
                HeaderText = "Ngày tạo",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            };

            dgvRecentOrders.Columns.AddRange(colId, colOrderNo, colStatus, colCreated);

            // double-click để mở chi tiết
            dgvRecentOrders.CellDoubleClick -= dgvRecentOrders_CellDoubleClick;
            dgvRecentOrders.CellDoubleClick += dgvRecentOrders_CellDoubleClick;
        }

        private void dgvRecentOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    var orderId = (int)dgvRecentOrders.Rows[e.RowIndex].Cells["Id"].Value;
                    if (orderId > 0)
                    {
                        OrderDoubleClick?.Invoke(this, orderId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chọn đơn hàng: {ex.Message}");
                }
            }
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            CreateOrderClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
