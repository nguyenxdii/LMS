// LMS.GUI/OrderAdmin/ucDriverPicker_Admin.cs
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
using LMS.BUS.Helpers; // <-- THÊM USING NÀY (hoặc namespace chứa GridHelper)

namespace LMS.GUI.OrderAdmin
{
    public partial class ucDriverPicker_Admin : UserControl // Sửa tên class nếu cần
    {
        public event Action<int> DriverSelected;

        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucDriverPicker_Admin() // Sửa tên constructor nếu cần
        {
            InitializeComponent();
            this.Load += UcDriverPicker_Admin_Load;
        }

        private void UcDriverPicker_Admin_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            LoadDrivers();
            WireEvents();
        }

        // --- Cấu hình DataGridView ---
        private void ConfigureGrid()
        {
            var g = dgvDrivers;
            g.Columns.Clear(); // Xóa cột cũ

            // *** ÁP DỤNG STYLE CHUNG TỪ HELPER ***
            g.ApplyBaseStyle(); // Gọi extension method

            // *** CHỈ ĐỊNH NGHĨA CỘT RIÊNG CHO GRID NÀY ***
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false }); // ID ẩn
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng GPLX", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });

            // TryEnableDoubleBuffer đã được gọi bên trong ApplyBaseStyle()
        }

        // Tải danh sách tài xế đang hoạt động
        private void LoadDrivers()
        {
            try
            {
                using (var db = new LogisticsDbContext())
                {
                    var activeDrivers = db.Drivers
                        .Where(d => d.IsActive)
                        .OrderBy(d => d.FullName)
                        .Select(d => new
                        {
                            d.Id,
                            d.FullName,
                            d.Phone,
                            d.LicenseType
                        })
                        .ToList();

                    dgvDrivers.DataSource = null;
                    dgvDrivers.DataSource = activeDrivers;
                }
                ResetSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách tài xế: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Gán sự kiện cho các nút và grid
        private void WireEvents()
        {
            btnSelect.Click += BtnSelect_Click;
            btnCancel.Click += BtnCancel_Click;
            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
        }

        // --- Xử lý sự kiện ---

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (dgvDrivers.CurrentRow?.Cells["Id"]?.Value != null)
            {
                if (int.TryParse(dgvDrivers.CurrentRow.Cells["Id"].Value.ToString(), out int selectedId))
                {
                    DriverSelected?.Invoke(selectedId);
                    this.FindForm()?.Close();
                }
                else
                {
                    MessageBox.Show("Không thể lấy ID tài xế.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài xế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm()?.Close();
        }

        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvDrivers.DataSource as IEnumerable<object>;
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
                var propInfo = list.First().GetType().GetProperty(propertyName);
                if (propInfo == null) return;

                IEnumerable<object> sortedList;
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = list.OrderBy(x => propInfo.GetValue(x, null));
                else
                    sortedList = list.OrderByDescending(x => propInfo.GetValue(x, null));

                dgvDrivers.DataSource = null;
                dgvDrivers.DataSource = sortedList.ToList();
                newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
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
        }

        // TryEnableDoubleBuffer đã được gọi trong ApplyBaseStyle() nên không cần ở đây nữa
        // private static void TryEnableDoubleBuffer(DataGridView grid) { ... }
    }
}