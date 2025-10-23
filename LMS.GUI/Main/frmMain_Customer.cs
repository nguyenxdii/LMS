// LMS.GUI/Main/frmMain_Customer.cs
using LMS.BUS.Helpers;                // AppSession
using LMS.BUS.Services;               // OrderService (để dùng sau)
using LMS.GUI.OrderCustomer;          // ucOrderCreate_Cus, ucOrderList_Cus, ucTracking_Cus (khi bạn dùng)
using System;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class frmMain_Customer : Form
    {
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();

        // Lưu id customer đang đăng nhập
        private int _customerId;

        // ===== Cách 1: truyền thẳng id từ frmLogin sau khi đăng nhập =====
        public frmMain_Customer(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            WireEvents();
        }

        // ===== Cách 2: không truyền id -> lấy từ AppSession =====
        public frmMain_Customer() : this(0) { }

        private void frmMain_Customer_Load(object sender, EventArgs e)
        {
            // Nếu chưa có id thì lấy từ AppSession
            if (_customerId <= 0)
                _customerId = AppSession.CustomerId ?? 0;

            if (_customerId <= 0)
            {
                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Customer.", "LMS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            // bạn có thể hiển thị lời chào nếu muốn:
            // lblWelcome.Text = $"Xin chào, {AppSession.DisplayName}";
        }

        //private void WireEvents()
        //{
        //    this.Load += frmMain_Customer_Load;

        //    btnNewOrder.Click -= BtnNewOrder_Click;
        //    btnNewOrder.Click += BtnNewOrder_Click;

        //    btnRefresh.Click -= BtnRefresh_Click;
        //    btnRefresh.Click += BtnRefresh_Click;

        //    btnViewTracking.Click -= BtnViewTracking_Click;
        //    btnViewTracking.Click += BtnViewTracking_Click;
        //}

        private void WireEvents()
        {
            this.Load += frmMain_Customer_Load;
            btnNewOrder.Click += (s, e) => LoadUc(new LMS.GUI.OrderCustomer.ucOrderCreate_Cus(_customerId));
            btnRefresh.Click += (s, e) => LoadUc(new LMS.GUI.OrderCustomer.ucOrderList_Cus(_customerId));
            btnViewTracking.Click += (s, e) => LoadUc(new LMS.GUI.OrderCustomer.ucTracking_Cus());
        }

        // =============== HANDLERS ===============

        private void BtnNewOrder_Click(object sender, EventArgs e)
        {
            // mở UC tạo đơn
            var uc = new ucOrderCreate_Cus(_customerId) { Dock = DockStyle.Fill };
            LoadUc(uc);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // nếu UC hiện tại có method Reload thì bạn có thể gọi qua interface sau này
            // tạm thời cứ làm mới nội dung: quay về danh sách đơn (nếu bạn đã làm UC này)
            // var uc = new ucOrderList_Cus(_customerId) { Dock = DockStyle.Fill };
            // LoadUc(uc);
        }

        private void BtnViewTracking_Click(object sender, EventArgs e)
        {
            // mở UC tracking (nhập mã đơn để xem)
            var uc = new ucTracking_Cus() { Dock = DockStyle.Fill };
            LoadUc(uc);
        }

        // =============== UTILS ===============

        // trong class frmMain_Customer
        public void LoadUc(UserControl uc)
        {
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
        }

    }
}
