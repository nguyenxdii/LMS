// LMS.GUI/OrderAdmin/ucDriverPicker_Admin.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucDriverPicker_Admin : UserControl
    {
        // bắn id tài xế đã chọn cho form cha
        public event Action<int> DriverSelected;

        private readonly DriverService _driverSvc = new DriverService();
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public enum DriverPickerMode
        {
            AssignVehicle,
            CreateShipment
        }

        private DriverPickerMode _currentMode;

        public ucDriverPicker_Admin(DriverPickerMode mode = DriverPickerMode.AssignVehicle)
        {
            InitializeComponent();
            _currentMode = mode;
            this.Load += UcDriverPicker_Admin_Load;
        }

        private void UcDriverPicker_Admin_Load(object sender, EventArgs e)
        {
            // cập nhật tiêu đề nếu có lblTitle trong designer
            if (this.Controls.Find("lblTitle", true).FirstOrDefault() is Label lbl)
                lbl.Text = "Chọn Tài xế (chỉ hiển thị tài xế rảnh & chưa có xe)";

            ConfigureGrid();
            LoadDrivers();
            WireEvents();

            // đăng ký tự reload khi có thay đổi gán/gỡ xe
            AppSession.VehicleAssignmentChanged += OnVehicleAssignmentChanged;
            this.HandleDestroyed += (s, _) =>
            {
                try { AppSession.VehicleAssignmentChanged -= OnVehicleAssignmentChanged; } catch { }
            };
        }

        private void OnVehicleAssignmentChanged()
        {
            if (!IsDisposed) LoadDrivers();
        }

        private void ConfigureGrid()
        {
            var g = dgvDrivers;
            g.Columns.Clear();
            g.ApplyBaseStyle();
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                DataPropertyName = "FullName",
                HeaderText = "Họ tên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                DataPropertyName = "Phone",
                HeaderText = "Điện thoại",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LicenseType",
                DataPropertyName = "LicenseType",
                HeaderText = "Hạng GPLX",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            // nếu muốn hiện cccd thì mở dòng dưới:
            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitizenId", DataPropertyName = "CitizenId", HeaderText = "CCCD", AutoSizeMode = AllCells, SortMode = Programmatic });
        }

        private void LoadDrivers()
        {
            try
            {
                List<Driver> drivers;

                if (_currentMode == DriverPickerMode.CreateShipment)
                {
                    drivers = _driverSvc.GetDriversWithVehiclesForShipment();
                    if (this.Controls.Find("lblTitle", true).FirstOrDefault() is Label lbl)
                    {
                        lbl.Text = "Chọn Tài xế (Đã có xe & Rảnh)";
                    }
                }
                else
                {
                    drivers = _driverSvc.GetDriversWithoutVehicles();
                    if (this.Controls.Find("lblTitle", true).FirstOrDefault() is Label lbl)
                    {
                        lbl.Text = "Chọn Tài xế (Chưa có xe & Rảnh)";
                    }
                }

                var data = drivers.Select(d => new
                {
                    d.Id,
                    d.FullName,
                    d.Phone,
                    d.LicenseType
                }).ToList();

                dgvDrivers.DataSource = null;
                dgvDrivers.DataSource = data;

                if (dgvDrivers.Rows.Count > 0)
                {
                    dgvDrivers.ClearSelection();
                    dgvDrivers.Rows[0].Selected = true;
                }

                ResetSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách tài xế: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvDrivers.DataSource = null;
            }
        }

        private void WireEvents()
        {
            // gắn click cho nút chọn/huỷ nếu tồn tại
            if (this.Controls.Find("btnSelect", true).FirstOrDefault() is Button btnSelect)
                btnSelect.Click += BtnSelect_Click;
            if (this.Controls.Find("btnCancel", true).FirstOrDefault() is Button btnCancel)
                btnCancel.Click += (s, e) => this.FindForm()?.Close();

            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
            dgvDrivers.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) PickCurrent(); };
            dgvDrivers.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; PickCurrent(); } };
        }

        private void BtnSelect_Click(object sender, EventArgs e) => PickCurrent();

        private void PickCurrent()
        {
            if (dgvDrivers.CurrentRow?.DataBoundItem == null)
            {
                MessageBox.Show("Vui lòng chọn một tài xế.", "Thông báo");
                return;
            }

            try
            {
                // datasource là anonymous type => dùng dynamic hoặc reflection
                dynamic row = dgvDrivers.CurrentRow.DataBoundItem;
                int id = (int)row.Id;
                DriverSelected?.Invoke(id);
                this.FindForm()?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể lấy ID tài xế đã chọn: {ex.Message}", "Lỗi");
            }
        }

        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvDrivers.DataSource as IEnumerable<object>;
            if (list == null || !list.Any()) return;

            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn?.HeaderCell != null)
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _sortedColumn = newColumn;
                _sortOrder = SortOrder.Ascending;
            }

            try
            {
                string propName = newColumn.DataPropertyName;
                var first = list.FirstOrDefault();
                if (first == null) return;

                PropertyInfo pi = first.GetType().GetProperty(propName);
                if (pi == null) return;

                IEnumerable<object> sorted = (_sortOrder == SortOrder.Ascending)
                    ? list.OrderBy(x => pi.GetValue(x, null))
                    : list.OrderByDescending(x => pi.GetValue(x, null));

                dgvDrivers.DataSource = sorted.ToList();
                if (newColumn.HeaderCell != null) newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
            }
            catch
            {
                ResetSortGlyphs();
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn?.HeaderCell != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null;
            _sortOrder = SortOrder.None;

            foreach (DataGridViewColumn col in dgvDrivers.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable && col.HeaderCell != null)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        // kéo form bằng control bất kỳ (gọi enableFormDragging(lblTitle) nếu cần)
        private void EnableFormDragging(Control dragControl)
        {
            dragControl.MouseDown += DragControl_MouseDown;
            dragControl.MouseMove += DragControl_MouseMove;
            dragControl.MouseUp += DragControl_MouseUp;
        }

        private void DragControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.FindForm()?.Location ?? Point.Empty;
            }
        }

        private void DragControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && this.FindForm() != null)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.FindForm().Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void DragControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }
    }
}
