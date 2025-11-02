using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models; // Cần cho Enum OrderStatus
using LMS.BUS.Dtos; // Cần cho OrderListItemDto
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class ucDashboard_Cus : UserControl
    {
        private readonly OrderService_Customer _orderService;

        // 1. Định nghĩa 2 sự kiện (events)
        public event EventHandler<int> OrderDoubleClick;
        public event EventHandler CreateOrderClick;

        public ucDashboard_Cus()
        {
            InitializeComponent();
            _orderService = new OrderService_Customer();
            this.Load += ucDashboard_Cus_Load;
        }

        //private void ucDashboard_Cus_Load(object sender, EventArgs e)
        //{
        //    if (DesignMode) return;

        //    // Gắn click cho nút Tạo đơn (đúng Name trong Designer)
        //    if (btnCreateOrder != null)
        //    {
        //        btnCreateOrder.Enabled = true;
        //        btnCreateOrder.Click -= btnCreateOrder_Click; // tránh gắn trùng
        //        btnCreateOrder.Click += btnCreateOrder_Click;
        //        btnCreateOrder.BringToFront(); // phòng trường hợp bị control khác che
        //    }
        //    else
        //    {
        //        MessageBox.Show("Không tìm thấy nút 'btnCreateOrder' – kiểm tra Name trong Designer.");
        //    }

        //    // Cấu hình lưới trước khi bind
        //    ConfigureRecentOrdersGrid();

        //    // Nạp dữ liệu
        //    LoadDashboardData();
        //}
        private void ucDashboard_Cus_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            // Gắn click cho nút Tạo đơn (đúng Name trong Designer)
            if (btnCreateOrder != null)
            {
                btnCreateOrder.Enabled = true;
                btnCreateOrder.Click -= btnCreateOrder_Click; // tránh gắn trùng
                btnCreateOrder.Click += btnCreateOrder_Click;
                btnCreateOrder.BringToFront(); // phòng trường hợp bị control khác che
            }
            else
            {
                MessageBox.Show("Không tìm thấy nút 'btnCreateOrder' – kiểm tra Name trong Designer.");
            }

            // Cấu hình lưới trước khi bind
            ConfigureRecentOrdersGrid();

            // Nạp dữ liệu
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

            try
            {
                var allMyOrders = _orderService.GetAllByCustomer(customerId);

                // KPI
                lblPendingCount.Text = allMyOrders.Count(o => o.Status == OrderStatus.Pending).ToString();
                lblInTransitCount.Text = allMyOrders.Count(o => o.Status == OrderStatus.Approved).ToString();
                lblCompletedCount.Text = allMyOrders.Count(o => o.Status == OrderStatus.Completed).ToString();

                // 5 đơn gần nhất
                var latest5 = allMyOrders
                                .OrderByDescending(o => o.CreatedAt)
                                .Take(5)
                                .ToList();

                // (tuỳ chọn) nếu OrderNo rỗng -> hiển thị mã phát sinh từ Id
                foreach (var it in latest5)
                    if (string.IsNullOrWhiteSpace(it.OrderNo))
                        it.OrderNo = OrderCode.ToCode(it.Id);

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

            // 0) Id (ẩn)
            var colId = new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            };

            // 1) Mã đơn -> dùng OrderNo (DTO của bạn có OrderNo)
            var colOrderNo = new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                DataPropertyName = "OrderNo",
                HeaderText = "Mã đơn",
                Width = 140
            };

            // 2) Trạng thái (tiếng Việt) -> StatusVN
            var colStatus = new DataGridViewTextBoxColumn
            {
                Name = "StatusVN",
                DataPropertyName = "StatusVN",
                HeaderText = "Trạng thái",
                Width = 140
            };

            // 3) Ngày tạo -> CreatedAt
            var colCreated = new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                DataPropertyName = "CreatedAt",
                HeaderText = "Ngày tạo",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            };

            dgvRecentOrders.Columns.AddRange(colId, colOrderNo, colStatus, colCreated);

            // Double-click để mở chi tiết
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
                        // Phát sự kiện OrderDoubleClick
                        OrderDoubleClick?.Invoke(this, orderId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chọn đơn hàng: {ex.Message}");
                }
            }
        }

        // Hàm xử lý sự kiện click cho nút "Tạo đơn hàng"
        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            // Phát sự kiện CreateOrderClick
            CreateOrderClick?.Invoke(this, EventArgs.Empty);
        }
    }
}