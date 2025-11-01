using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucOrderReject_Admin : UserControl
    {
        // Sự kiện (Event) để báo cho Form cha biết kết quả
        public event EventHandler<string> Confirmed; // Gửi kèm lý do (string)
        public event EventHandler Cancelled;

        // Biến để kéo thả Form Cha
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        public ucOrderReject_Admin()
        {
            InitializeComponent();
        }

        public void LoadOrderInfo(string orderNo)
        {
            lblInfo.Text = $"Lý do từ chối đơn {orderNo}";
            lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lblInfo.BackColor = Color.Transparent;
            lblInfo.AutoSize = true;

            txtReason.Focus();
            txtReason.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            txtReason.PlaceholderText = "Nhập lý do từ chối...";
            txtReason.Multiline = true;
        }

        private void ucOrderReject_Admin_Load(object sender, EventArgs e)
        {
            // Gán sự kiện cho các nút
            btnConfirm.Click += BtnConfirm_Click;
            btnCancel.Click += BtnCancel_Click;

            // Gán sự kiện kéo thả cho pnlTop
            pnlTop.MouseDown += DragHandle_MouseDown;
            pnlTop.MouseMove += DragHandle_MouseMove;
            pnlTop.MouseUp += DragHandle_MouseUp;
        }

        // THÊM LOGIC CHO NÚT XÁC NHẬN
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string reason = txtReason.Text.Trim();
            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Vui lòng nhập lý do từ chối.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReason.Focus();
                return;
            }

            Confirmed?.Invoke(this, reason);

            this.FindForm()?.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtReason.Text))
            {
                var ask = MessageBox.Show("Bạn có thay đổi chưa lưu. Hủy bỏ thao tác?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.No)
                {
                    return; // Không làm gì cả, ở lại form
                }
            }

            Cancelled?.Invoke(this, EventArgs.Empty);

            this.FindForm()?.Close();
        }

        #region Kéo thả Form Cha
        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    isDragging = true;
                    dragStartPoint = Cursor.Position;
                    parentFormStartPoint = parentForm.Location;
                }
            }
        }

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint));
                    parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff));
                }
            }
        }

        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }
        #endregion
    }
}