// LMS.GUI/Main/frmMain_Driver.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using LMS.BUS.Helpers;          // AppSession
using LMS.GUI.OrderDriver;      // ucMyShipments_Drv

namespace LMS.GUI.Main
{
    public partial class frmMain_Driver : Form
    {
        private UserControl _current;               // UC hiện tại trong pnlContent
        private ucMyShipments_Drv _pageMyShipments; // cache để tái sử dụng

        public frmMain_Driver()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 650);

            // wire
            this.Load += FrmMain_Driver_Load;
            btnMyShipments.Click += BtnMyShipments_Click;
        }

        private void FrmMain_Driver_Load(object sender, EventArgs e)
        {
            // Yêu cầu tài khoản driver
            if (!AppSession.IsLoggedIn || AppSession.Role != LMS.DAL.Models.UserRole.Driver)
            {
                MessageBox.Show("Bạn cần đăng nhập bằng tài khoản tài xế.", "LMS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            // ❗ KHÔNG tự mở My Shipments nữa.
            // Giữ dashboard/blank: xóa mọi control trong panel cho chắc.
            pnlContent.Controls.Clear();

            // (tuỳ chọn) hiện một label chào mừng ngắn gọn
            var lbl = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Text = "Chọn mục ở sidebar để bắt đầu (ví dụ: Đơn của tôi)."
            };
            pnlContent.Controls.Add(lbl);
            _current = null;
        }

        private void BtnMyShipments_Click(object sender, EventArgs e)
        {
            // ✅ Chỉ khi nhấn nút mới mở trang My Shipments
            OpenMyShipments();
        }

        private void OpenMyShipments()
        {
            if (_pageMyShipments == null || _pageMyShipments.IsDisposed)
            {
                _pageMyShipments = new ucMyShipments_Drv
                {
                    Dock = DockStyle.Fill
                };

                // Nếu muốn bắt sự kiện mở chi tiết:
                // _pageMyShipments.OpenDetailRequested += (s, shipmentId) => OpenShipmentDetail(shipmentId);
            }

            ShowPage(_pageMyShipments);
        }

        /// <summary>Thay UC đang hiển thị trong pnlContent.</summary>
        private void ShowPage(UserControl page)
        {
            if (_current == page) return;

            pnlContent.SuspendLayout();

            if (_current != null)
                pnlContent.Controls.Remove(_current);

            _current = page;
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(page);

            pnlContent.ResumeLayout();
        }

        // (tuỳ chọn) Nếu muốn mở trang chi tiết Shipment
        // private void OpenShipmentDetail(int shipmentId)
        // {
        //     var detail = new ucShipmentDetail_Drv(shipmentId) { Dock = DockStyle.Fill };
        //     ShowPage(detail);
        // }
    }
}
