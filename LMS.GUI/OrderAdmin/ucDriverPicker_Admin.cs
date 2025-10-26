//// LMS.GUI/OrderAdmin/ucDriverPicker_Admin.cs
//using Guna.UI2.WinForms; // Cần thiết nếu dùng các control Guna
//using LMS.DAL;
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;
//using LMS.BUS.Helpers; // <-- THÊM USING NÀY (hoặc namespace chứa GridHelper)

//namespace LMS.GUI.OrderAdmin
//{
//    public partial class ucDriverPicker_Admin : UserControl // Sửa tên class nếu cần
//    {
//        public event Action<int> DriverSelected;

//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        // ==========================================================
//        // === (1) THÊM CÁC BIẾN ĐỂ KÉO THẢ FORM CHỨA UC NÀY ===
//        private bool dragging = false;
//        private Point dragCursorPoint;
//        private Point dragFormPoint;
//        // ==========================================================

//        public ucDriverPicker_Admin() // Sửa tên constructor nếu cần
//        {
//            InitializeComponent();
//            this.Load += UcDriverPicker_Admin_Load;
//        }

//        private void UcDriverPicker_Admin_Load(object sender, EventArgs e)
//        {
//            // Đặt tiêu đề cho Form chứa UC này
//            var parentForm = this.FindForm();
//            if (parentForm != null)
//            {
//                parentForm.Text = "Chọn Tài xế";
//            }

//            ConfigureGrid();
//            LoadDrivers();
//            WireEvents();

//            // Đặt tiêu đề cho Label trên UC (Ví dụ: nếu bạn có lblTitle)
//            var titleLabel = this.Controls.Find("lblTitle", true).FirstOrDefault() as Label;
//            if (titleLabel != null)
//            {
//                titleLabel.Text = "Chọn Tài xế Giao hàng";
//            }
//        }

//        // --- Cấu hình DataGridView ---
//        private void ConfigureGrid()
//        {
//            var g = dgvDrivers;
//            g.Columns.Clear(); // Xóa cột cũ

//            // *** ÁP DỤNG STYLE CHUNG TỪ HELPER ***
//            g.ApplyBaseStyle(); // Gọi extension method

//            // *** CHỈ ĐỊNH NGHĨA CỘT RIÊNG CHO GRID NÀY ***
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false }); // ID ẩn
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng GPLX", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });

//            // TryEnableDoubleBuffer đã được gọi bên trong ApplyBaseStyle()
//        }

//        // Tải danh sách tài xế đang hoạt động
//        private void LoadDrivers()
//        {
//            try
//            {
//                // Thay thế phần này bằng service nếu có
//                using (var db = new LogisticsDbContext())
//                {
//                    var activeDrivers = db.Drivers
//                        .Where(d => d.IsActive)
//                        .OrderBy(d => d.FullName)
//                        .Select(d => new
//                        {
//                            d.Id,
//                            d.FullName,
//                            d.Phone,
//                            d.LicenseType
//                        })
//                        .ToList();

//                    dgvDrivers.DataSource = null;
//                    dgvDrivers.DataSource = activeDrivers;
//                }
//                ResetSortGlyphs();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải danh sách tài xế: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Gán sự kiện cho các nút và grid
//        private void WireEvents()
//        {
//            btnSelect.Click += BtnSelect_Click;
//            btnCancel.Click += BtnCancel_Click;
//            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;

//            // ==========================================================
//            // === (2) GÁN SỰ KIỆN KÉO THẢ CHO pnlTop và các control con của nó ===
//            Control pnlTop = this.Controls.Find("pnlTop", true).FirstOrDefault() as Control;
//            if (pnlTop != null)
//            {
//                EnableDrag(pnlTop);
//            }
//            // ==========================================================
//        }

//        // HÀM HỖ TRỢ ĐỆ QUY GÁN SỰ KIỆN KÉO THẢ CHO TẤT CẢ CONTROL CON
//        private void EnableDrag(Control control)
//        {
//            control.MouseDown += PnlTop_MouseDown;
//            control.MouseMove += PnlTop_MouseMove;
//            control.MouseUp += PnlTop_MouseUp;

//            // Đệ quy gán cho tất cả các control con của control này
//            foreach (Control child in control.Controls)
//            {
//                EnableDrag(child);
//            }
//        }


//        // --- Xử lý sự kiện ---

//        private void BtnSelect_Click(object sender, EventArgs e)
//        {
//            if (dgvDrivers.CurrentRow?.Cells["Id"]?.Value != null)
//            {
//                if (int.TryParse(dgvDrivers.CurrentRow.Cells["Id"].Value.ToString(), out int selectedId))
//                {
//                    DriverSelected?.Invoke(selectedId);
//                    this.FindForm()?.Close();
//                }
//                else
//                {
//                    MessageBox.Show("Không thể lấy ID tài xế.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Vui lòng chọn một tài xế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//        }

//        private void BtnCancel_Click(object sender, EventArgs e)
//        {
//            this.FindForm()?.Close();
//        }

//        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            var list = dgvDrivers.DataSource as IEnumerable<object>;
//            if (list == null || !list.Any()) return;

//            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            if (_sortedColumn == null) _sortOrder = SortOrder.Ascending;
//            else if (_sortedColumn == newColumn) _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
//            else { _sortOrder = SortOrder.Ascending; if (_sortedColumn?.HeaderCell != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }

//            _sortedColumn = newColumn;

//            try
//            {
//                string propertyName = newColumn.DataPropertyName;
//                var propInfo = list.First().GetType().GetProperty(propertyName);
//                if (propInfo == null) return;

//                IEnumerable<object> sortedList;
//                if (_sortOrder == SortOrder.Ascending)
//                    sortedList = list.OrderBy(x => propInfo.GetValue(x, null));
//                else
//                    sortedList = list.OrderByDescending(x => propInfo.GetValue(x, null));

//                dgvDrivers.DataSource = null;
//                dgvDrivers.DataSource = sortedList.ToList();
//                newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Sort Error on {newColumn.Name}: {ex.Message}");
//                ResetSortGlyphs();
//            }
//        }

//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn?.HeaderCell != null)
//            {
//                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            }
//            _sortedColumn = null;
//            _sortOrder = SortOrder.None;
//        }

//        // ==========================================================
//        // === (3) 3 HÀM XỬ LÝ KÉO THẢ CHO FORM CHỨA UC NÀY ===
//        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
//        {
//            Form parentForm = this.FindForm();
//            if (parentForm == null) return;

//            if (e.Button == MouseButtons.Left)
//            {
//                dragging = true;
//                dragCursorPoint = Cursor.Position;
//                dragFormPoint = parentForm.Location;
//            }
//        }

//        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
//        {
//            Form parentForm = this.FindForm();
//            if (parentForm == null) return;

//            if (dragging)
//            {
//                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
//                parentForm.Location = Point.Add(dragFormPoint, new Size(dif));
//            }
//        }

//        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                dragging = false;
//            }
//        }
//        // ==========================================================
//    }
//}

// LMS.GUI/OrderAdmin/ucDriverPicker_Admin.cs (Hoặc LMS.GUI/DriverAdmin/ nếu bạn đã di chuyển)
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LMS.BUS.Helpers;
using LMS.BUS.Services; // Đảm bảo có using DriverService

namespace LMS.GUI.OrderAdmin // Hoặc namespace đúng của bạn
{
    public partial class ucDriverPicker_Admin : UserControl
    {
        public event Action<int> DriverSelected;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        // === THÊM HOẶC KHAI BÁO DRIVER SERVICE ===
        private readonly DriverService _driverSvc = new DriverService();


        public ucDriverPicker_Admin()
        {
            InitializeComponent();
            this.Load += UcDriverPicker_Admin_Load;
        }

        private void UcDriverPicker_Admin_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            LoadDrivers(); // Gọi hàm LoadDrivers đã sửa
            WireEvents();
        }

        private void ConfigureGrid()
        {
            var g = dgvDrivers;
            g.Columns.Clear();
            g.ApplyBaseStyle();
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng GPLX", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            // Thêm cột CitizenId nếu muốn hiển thị ở đây
            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitizenId", DataPropertyName = "CitizenId", HeaderText = "CCCD", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
        }

        // === SỬA LẠI PHƯƠNG THỨC NÀY ===
        /// <summary>
        /// Tải danh sách tài xế ĐANG RẢNH vào grid.
        /// </summary>
        private void LoadDrivers()
        {
            try
            {
                // Gọi hàm mới để lấy tài xế RẢNH
                var availableDrivers = _driverSvc.GetAvailableDriversForAdmin();

                // Project sang kiểu dữ liệu Anonymous Type để chỉ hiển thị các cột cần thiết
                // (hoặc tạo DTO riêng nếu muốn)
                var displayData = availableDrivers.Select(d => new
                {
                    d.Id,
                    d.FullName,
                    d.Phone,
                    d.LicenseType
                    // d.CitizenId // Bỏ comment nếu muốn hiển thị CCCD
                }).ToList();

                dgvDrivers.DataSource = null; // Xóa nguồn cũ
                dgvDrivers.DataSource = displayData; // Gán nguồn mới chỉ chứa tài xế rảnh
                ResetSortGlyphs(); // Reset sort
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách tài xế rảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvDrivers.DataSource = null;
            }
        }
        // === KẾT THÚC SỬA ===

        private void WireEvents()
        {
            btnSelect.Click += BtnSelect_Click;
            btnCancel.Click += BtnCancel_Click;
            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
            // Thêm DoubleClick để chọn nếu muốn
            // dgvDrivers.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnSelect_Click(s, e); };
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            // Sử dụng dynamic để truy cập thuộc tính Id từ Anonymous Type
            if (dgvDrivers.CurrentRow?.DataBoundItem != null)
            {
                dynamic selectedItem = dgvDrivers.CurrentRow.DataBoundItem;
                try
                {
                    int selectedId = (int)selectedItem.Id;
                    DriverSelected?.Invoke(selectedId); // Gửi ID ra ngoài
                    this.FindForm()?.Close(); // Đóng popup
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể lấy ID tài xế đã chọn: {ex.Message}", "Lỗi");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài xế.", "Thông báo");
            }
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm()?.Close();
        }

        // --- Sorting Logic (Giữ nguyên, nhưng cần typeof phù hợp nếu dùng DTO) ---
        // Nếu DataSource là Anonymous Type, sort bằng Reflection sẽ phức tạp hơn chút
        // Cách đơn giản là sort trực tiếp List trước khi gán DataSource trong LoadDrivers nếu cần sort mặc định
        // Hoặc giữ code sort hiện tại và đảm bảo DataPropertyName khớp với tên thuộc tính Anonymous Type
        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvDrivers.DataSource as IEnumerable<object>; // Dùng IEnumerable<object> cho anonymous type
            if (list == null || !list.Any()) return;

            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == null) _sortOrder = SortOrder.Ascending;
            else if (_sortedColumn == newColumn) _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else { _sortOrder = SortOrder.Ascending; if (_sortedColumn?.HeaderCell != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }

            _sortedColumn = newColumn;

            try
            {
                string propertyName = newColumn.DataPropertyName;
                // Lấy PropertyInfo từ kiểu dữ liệu của item đầu tiên
                var firstItem = list.FirstOrDefault();
                if (firstItem == null) return;
                var propInfo = firstItem.GetType().GetProperty(propertyName);
                if (propInfo == null) return;

                IEnumerable<object> sortedList;
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = list.OrderBy(x => propInfo.GetValue(x, null));
                else
                    sortedList = list.OrderByDescending(x => propInfo.GetValue(x, null));

                dgvDrivers.DataSource = null;
                dgvDrivers.DataSource = sortedList.ToList(); // Gán lại List đã sort
                if (newColumn.HeaderCell != null) newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sort Error on {newColumn.Name}: {ex.Message}");
                ResetSortGlyphs();
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn?.HeaderCell != null)
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
            // Cập nhật lại UI cho tất cả các cột
            foreach (DataGridViewColumn col in dgvDrivers.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable && col.HeaderCell != null)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }
    }
}