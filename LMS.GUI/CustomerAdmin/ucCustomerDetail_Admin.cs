using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
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
            this.Load += UcCustomerDetail_Admin_Load;
        }

        private void UcCustomerDetail_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Chi Tiết Khách Hàng";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureOrderGrid();
            LoadDetails();

            // gán nút đóng nếu tồn tại (btnClose)
            var closeButton = this.Controls.Find("btnClose", true).FirstOrDefault();
            if (closeButton != null)
            {
                closeButton.Click += (s, ev) => this.FindForm()?.Close();
            }
        }

        // cấu hình lưới đơn hàng của khách
        private void ConfigureOrderGrid()
        {
            dgvCustomerOrders.Columns.Clear();
            dgvCustomerOrders.ApplyBaseStyle();

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
                HeaderText = "Ngày Tạo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Origin",
                DataPropertyName = "Origin",
                HeaderText = "Kho Gửi",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Destination",
                DataPropertyName = "Destination",
                HeaderText = "Kho Nhận",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalFee",
                DataPropertyName = "TotalFee",
                HeaderText = "Tổng Phí",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvCustomerOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "Trạng Thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvCustomerOrders.CellFormatting += dgvCustomerOrders_CellFormatting;
        }

        // nạp chi tiết khách + danh sách đơn hàng
        private void LoadDetails()
        {
            try
            {
                var dto = _customerSvc.GetCustomerDetails(_customerId);

                lblFullName.Text = $"Họ Tên: {dto.Customer?.Name ?? "(Trống)"}";
                lblPhone.Text = $"SĐT: {dto.Customer?.Phone ?? "(Trống)"}";
                lblEmail.Text = $"Email: {dto.Customer?.Email ?? "(Trống)"}";
                lblAddress.Text = $"Địa Chỉ: {dto.Customer?.Address ?? "(Trống)"}";

                if (dto.Account != null)
                {
                    lblUsername.Text = $"Tài Khoản: {dto.Account.Username}";
                    lblPassword.Text = $"Mật Khẩu: {dto.Account.PasswordHash ?? "(Chưa Đặt)"}";
                    lblAccountStatus.Text = $"Trạng Thái TK: {(dto.Account.IsActive ? "Đang Hoạt Động" : "Bị Khóa")}";
                }
                else
                {
                    lblUsername.Text = "Tài Khoản: (Chưa Có)";
                    lblPassword.Text = "Mật Khẩu: (N/A)";
                    lblAccountStatus.Text = "Trạng Thái TK: (N/A)";
                }

                var orderData = dto.Orders.Select(o => new
                {
                    OrderNo = string.IsNullOrWhiteSpace(o.OrderNo) ? OrderCode.ToCode(o.Id) : o.OrderNo,
                    o.CreatedAt,
                    Origin = o.OriginWarehouse?.Name ?? $"#{o.OriginWarehouseId}",
                    Destination = o.DestWarehouse?.Name ?? $"#{o.DestWarehouseId}",
                    o.TotalFee,
                    o.Status
                }).ToList();

                dgvCustomerOrders.DataSource = orderData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Tải Chi Tiết Khách Hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.FindForm()?.Close();
            }
        }

        // hiển thị trạng thái đơn hàng bằng tiếng việt
        private void dgvCustomerOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCustomerOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value is OrderStatus status)
            {
                switch (status)
                {
                    case OrderStatus.Pending: e.Value = "Chờ Duyệt"; break;
                    case OrderStatus.Approved: e.Value = "Đã Duyệt"; break;
                    case OrderStatus.Completed: e.Value = "Hoàn Thành"; break;
                    case OrderStatus.Cancelled: e.Value = "Đã Hủy"; break;
                    default: e.Value = status.ToString(); break;
                }
                e.FormattingApplied = true;
            }
        }
    }
}
