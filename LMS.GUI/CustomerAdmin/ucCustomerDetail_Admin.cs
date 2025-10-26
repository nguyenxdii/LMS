using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services; // Ensure CustomerService is here
using LMS.DAL.Models;   // Ensure Order, OrderStatus etc. are here
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.CustomerAdmin
{
    public partial class ucCustomerDetail_Admin : UserControl
    {
        private readonly int _customerId;
        private readonly CustomerService _customerSvc = new CustomerService();

        public ucCustomerDetail_Admin(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            // Ensure necessary Labels (lblFullName, lblPhone, etc.) and
            // dgvCustomerOrders exist in your Designer.cs
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureOrderGrid();
            LoadDetails();

            // Wire up the Close button (assuming its name is btnClose)
            var closeButton = this.Controls.Find("btnClose", true).FirstOrDefault();
            if (closeButton != null)
            {
                closeButton.Click += (s, ev) => this.FindForm()?.Close();
            }
            else
            {
                // Optional: Log or warn if close button not found
                Console.WriteLine("Warning: btnClose not found in ucCustomerDetail_Admin.");
            }
        }

        private void ConfigureOrderGrid()
        {
            dgvCustomerOrders.Columns.Clear();
            dgvCustomerOrders.ApplyBaseStyle(); // Use your helper

            // Define columns for the order grid
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                DataPropertyName = "OrderNo",
                HeaderText = "Mã ĐH",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                DataPropertyName = "CreatedAt",
                HeaderText = "Ngày tạo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Origin",
                DataPropertyName = "Origin",
                HeaderText = "Kho gửi",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Destination",
                DataPropertyName = "Destination",
                HeaderText = "Kho nhận",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalFee",
                DataPropertyName = "TotalFee",
                HeaderText = "Tổng phí",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Fill remaining space
            });

            // Add formatting event handler for status column
            dgvCustomerOrders.CellFormatting += dgvCustomerOrders_CellFormatting;
        }

        private void LoadDetails()
        {
            try
            {
                var dto = _customerSvc.GetCustomerDetails(_customerId);

                // Populate Customer Info Labels
                lblFullName.Text = $"Họ tên: {dto.Customer?.Name ?? "(trống)"}";
                lblPhone.Text = $"SĐT: {dto.Customer?.Phone ?? "(trống)"}";
                lblEmail.Text = $"Email: {dto.Customer?.Email ?? "(trống)"}";
                lblAddress.Text = $"Địa chỉ: {dto.Customer?.Address ?? "(trống)"}";

                // Populate Account Info Labels
                if (dto.Account != null)
                {
                    lblUsername.Text = $"Tài khoản: {dto.Account.Username}";
                    // Display Password Hash (ensure lblPasswordHash exists)
                    lblPassword.Text = $"Mật khẩu: {dto.Account.PasswordHash ?? "(chưa đặt)"}";
                    lblAccountStatus.Text = $"Trạng thái TK: {(dto.Account.IsActive ? "Đang hoạt động" : "Bị khóa")}";
                }
                else
                {
                    lblUsername.Text = "Tài khoản: (Chưa có)";
                    lblPassword.Text = "Mật khẩu: (N/A)";
                    lblAccountStatus.Text = "Trạng thái TK: (N/A)";
                }

                // Populate Order Grid
                var orderData = dto.Orders.Select(o => new
                {
                    OrderNo = string.IsNullOrWhiteSpace(o.OrderNo) ? OrderCode.ToCode(o.Id) : o.OrderNo,
                    o.CreatedAt,
                    Origin = o.OriginWarehouse?.Name ?? $"#{o.OriginWarehouseId}",
                    Destination = o.DestWarehouse?.Name ?? $"#{o.DestWarehouseId}",
                    o.TotalFee,
                    o.Status // Keep Enum type for formatting
                }).ToList();

                dgvCustomerOrders.DataSource = orderData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải chi tiết khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Close the form if essential data cannot be loaded
                this.FindForm()?.Close();
            }
        }

        // Format OrderStatus Enum to Vietnamese string for display
        private void dgvCustomerOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCustomerOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value is OrderStatus status)
            {
                switch (status)
                {
                    case OrderStatus.Pending: e.Value = "Chờ duyệt"; break;
                    case OrderStatus.Approved: e.Value = "Đã duyệt"; break;
                    case OrderStatus.Completed: e.Value = "Hoàn thành"; break;
                    case OrderStatus.Cancelled: e.Value = "Đã hủy"; break;
                    // Add other statuses if needed (e.g., InTransit)
                    default: e.Value = status.ToString(); break; // Fallback
                }
                e.FormattingApplied = true; // Indicate value has been formatted
            }
        }
    }
}