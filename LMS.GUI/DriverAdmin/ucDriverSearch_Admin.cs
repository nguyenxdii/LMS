using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.DriverAdmin
{
    public partial class ucDriverSearch_Admin : UserControl
    {
        public event EventHandler<int> DriverPicked;

        private readonly DriverService _driverSvc = new DriverService();
        private BindingList<Driver> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        // live search: debounce 350ms
        private readonly Timer _debounceTimer = new Timer { Interval = 350 };

        // kéo thả form cha qua pnlTop
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucDriverSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcDriverSearch_Admin_Load;
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void UcDriverSearch_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null) this.lblTitle.Text = "Tìm Kiếm Tài Xế";

            BindFilters();
            ConfigureGrid();
            WireEvents();
            DoSearch();
            txtFullName.Focus();
        }

        // bind bộ lọc hạng bằng lái
        private void BindFilters()
        {
            var licenseTypes = new List<object> { new { Value = "", Text = "— Tất Cả —" } };
            licenseTypes.AddRange(new object[]
            {
                new { Value = "B2", Text = "B2" },
                new { Value = "C",  Text = "C"  },
                new { Value = "CE", Text = "CE" },
                new { Value = "D",  Text = "D"  },
                new { Value = "DE", Text = "DE" },
                new { Value = "FC", Text = "FC" }
            });

            cmbLicenseType.DataSource = licenseTypes;
            cmbLicenseType.DisplayMember = "Text";
            cmbLicenseType.ValueMember = "Value";
            cmbLicenseType.SelectedIndex = 0;
        }

        // cấu hình grid
        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                DataPropertyName = "FullName",
                HeaderText = "Họ Tên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                DataPropertyName = "Phone",
                HeaderText = "Số điện thoại",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CitizenId",
                DataPropertyName = "CitizenId",
                HeaderText = "CCCD",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LicenseType",
                DataPropertyName = "LicenseType",
                HeaderText = "Hạng Bằng Lái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
        }

        // gán sự kiện
        private void WireEvents()
        {
            btnReset.Click += BtnReset_Click;

            txtFullName.TextChanged += FilterControl_Changed;
            txtPhone.TextChanged += FilterControl_Changed;
            txtCitizenId.TextChanged += FilterControl_Changed;
            cmbLicenseType.SelectedIndexChanged += FilterControl_Changed;

            dgvSearchResults.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
            dgvSearchResults.CellDoubleClick += dgvSearchResults_CellDoubleClick;

            var top = this.pnlTop;
            if (top != null)
            {
                top.MouseDown += PnlTop_MouseDown;
                top.MouseMove += PnlTop_MouseMove;
                top.MouseUp += PnlTop_MouseUp;
            }
        }

        // debounce nhập filter
        private void FilterControl_Changed(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        // hết thời gian debounce -> tìm kiếm
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            DoSearch();
        }

        // thực hiện tìm kiếm
        private void DoSearch()
        {
            try
            {
                string nameFilter = txtFullName.Text.Trim();
                string phoneFilter = txtPhone.Text.Trim();
                string citizenIdFilter = txtCitizenId.Text.Trim();
                string licenseFilter = cmbLicenseType.SelectedValue?.ToString();

                var results = _driverSvc.SearchDriversForAdmin(
                    string.IsNullOrWhiteSpace(nameFilter) ? null : nameFilter,
                    string.IsNullOrWhiteSpace(phoneFilter) ? null : phoneFilter,
                    string.IsNullOrWhiteSpace(citizenIdFilter) ? null : citizenIdFilter,
                    string.IsNullOrWhiteSpace(licenseFilter) ? null : licenseFilter
                );

                _bindingList = new BindingList<Driver>(results);
                dgvSearchResults.DataSource = _bindingList;

                ApplySort();
                UpdateSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi");
                dgvSearchResults.DataSource = null;
            }
        }

        // đặt lại bộ lọc
        private void BtnReset_Click(object sender, EventArgs e)
        {
            cmbLicenseType.SelectedIndexChanged -= FilterControl_Changed;

            txtFullName.Clear();
            txtPhone.Clear();
            txtCitizenId.Clear();
            cmbLicenseType.SelectedIndex = 0;

            cmbLicenseType.SelectedIndexChanged += FilterControl_Changed;

            ResetSortGlyphs();
            DoSearch();
            txtFullName.Focus();
        }

        // double click chọn tài xế
        private void dgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                if (dgvSearchResults.Rows[e.RowIndex].DataBoundItem is Driver selectedDriver)
                {
                    DriverPicked?.Invoke(this, selectedDriver.Id);
                    this.FindForm()?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn: {ex.Message}", "Lỗi");
            }
        }

        // sort theo cột
        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            var newColumn = dgvSearchResults.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else
            {
                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            ApplySort();
            UpdateSortGlyphs();
        }

        // áp dụng sort cho BindingList
        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;

            string propertyName = _sortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(Driver).GetProperty(propertyName);
            if (propInfo == null) return;

            var items = _bindingList.ToList();
            List<Driver> sortedList;

            try
            {
                sortedList = (_sortOrder == SortOrder.Ascending)
                    ? items.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                    : items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                _bindingList = new BindingList<Driver>(sortedList);
                dgvSearchResults.DataSource = _bindingList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi sắp xếp: {ex.Message}");
                ResetSortGlyphs();
            }
        }

        // cập nhật mũi tên sort
        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvSearchResults.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;

                if (_sortedColumn != null && col == _sortedColumn)
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        // reset trạng thái sort
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
            UpdateSortGlyphs();
        }

        // kéo thả form cha
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            var parentForm = this.FindForm();
            if (parentForm == null) return;
            if (e.Button != MouseButtons.Left) return;

            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = parentForm.Location;
        }

        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            var parentForm = this.FindForm();
            if (parentForm == null || !dragging) return;

            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            parentForm.Location = Point.Add(dragFormPoint, new Size(dif));
        }

        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) dragging = false;
        }
    }
}
