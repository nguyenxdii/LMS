// LMS.GUI/WarehouseAdmin/ucWarehouseEditor_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // May need DTO later
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Drawing;
using System.Linq; // For Linq operations if needed
using System.Text.RegularExpressions; // If using Regex for validation
using System.Windows.Forms;

namespace LMS.GUI.WarehouseAdmin
{
    public partial class ucWarehouseEditor_Admin : UserControl
    {
        public enum EditorMode { Add, Edit }

        // State & Dependencies
        private EditorMode _mode = EditorMode.Add;
        private int _warehouseId = 0;
        private readonly WarehouseService_Admin _warehouseSvc = new WarehouseService_Admin(); // Ensure service exists
        private Warehouse _originalData; // To track changes

        // UI Helpers
        private ErrorProvider errProvider;
        // Tooltip can be added similarly to Customer/Driver editors if needed

        // Dragging State
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        public ucWarehouseEditor_Admin()
        {
            InitializeComponent();
            this.errProvider = new ErrorProvider { ContainerControl = this };
            WireEvents();
            LoadComboBoxes(); // Load enum values into comboboxes
            this.Load += (s, e) => { txtWarehouseName.Focus(); }; // Focus first field on load
        }

        #region Data Loading & Mode Handling

        public void LoadData(EditorMode mode, int? warehouseId)
        {
            _mode = mode;
            errProvider.Clear(); // Clear previous errors

            Font titleFont = new Font("Segoe UI", 14F, FontStyle.Bold); // Example font
            lblTitle.Font = titleFont; // Set font

            if (mode == EditorMode.Edit)
            {
                if (!warehouseId.HasValue) throw new ArgumentNullException(nameof(warehouseId), "Warehouse ID is required for Edit mode.");
                _warehouseId = warehouseId.Value;
                lblTitle.Text = "Sửa Thông Tin Kho"; // Update title

                try
                {
                    // Fetch data from service - Assuming GetWarehouseForEdit returns Warehouse model
                    _originalData = _warehouseSvc.GetWarehouseForEdit(_warehouseId);
                    if (_originalData == null) throw new Exception($"Warehouse with ID {_warehouseId} not found.");

                    // Populate controls
                    txtWarehouseName.Text = _originalData.Name;
                    txtAddress.Text = _originalData.Address;
                    cmbZone.SelectedValue = _originalData.ZoneId; // Assuming combobox ValueMember is set
                    cmbType.SelectedValue = _originalData.Type;   // Assuming combobox ValueMember is set
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tải thông tin kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Optionally close the form if data load fails critically
                    // this.FindForm()?.Close();
                }
            }
            else // Add Mode
            {
                lblTitle.Text = "Thêm Kho Mới"; // Update title
                _warehouseId = 0; // Ensure ID is 0 for new item
                _originalData = new Warehouse(); // Create empty original data for change tracking

                // Clear controls
                txtWarehouseName.Clear();
                txtAddress.Clear();
                cmbZone.SelectedIndex = -1; // Deselect
                cmbType.SelectedIndex = -1; // Deselect
                cmbZone.Text = "Chọn vùng..."; // Placeholder
                cmbType.Text = "Chọn loại..."; // Placeholder
            }
        }

        private void LoadComboBoxes()
        {
            // Load Zone ComboBox
            cmbZone.DataSource = Enum.GetValues(typeof(Zone))
                                     .Cast<Zone>()
                                     .Select(z => new { Value = z, Text = FormatZone(z) })
                                     .ToList();
            cmbZone.DisplayMember = "Text";
            cmbZone.ValueMember = "Value";
            cmbZone.SelectedIndex = -1;

            // Load Type ComboBox
            cmbType.DataSource = Enum.GetValues(typeof(WarehouseType))
                                     .Cast<WarehouseType>()
                                     .Select(t => new { Value = t, Text = FormatWarehouseType(t) })
                                     .ToList();
            cmbType.DisplayMember = "Text";
            cmbType.ValueMember = "Value";
            cmbType.SelectedIndex = -1;
        }

        // Helper methods for formatting enums (copied from ucWarehouse_Admin)
        private string FormatZone(Zone zone)
        {
            switch (zone) { case Zone.North: return "Bắc"; case Zone.Central: return "Trung"; case Zone.South: return "Nam"; default: return zone.ToString(); }
        }
        private string FormatWarehouseType(WarehouseType type)
        {
            switch (type) { case WarehouseType.Main: return "Kho chính"; case WarehouseType.Hub: return "Trung chuyển"; case WarehouseType.Local: return "Địa phương"; default: return type.ToString(); }
        }


        #endregion

        #region Event Wiring & Handlers

        private void WireEvents()
        {
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // Drag and Drop for the parent Form
            Control dragHandle = pnlTop; // Use the top panel for dragging
            if (dragHandle != null)
            {
                dragHandle.MouseDown += DragHandle_MouseDown;
                dragHandle.MouseMove += DragHandle_MouseMove;
                dragHandle.MouseUp += DragHandle_MouseUp;
            }

            // Close button on the form
            btnClose.Click += (s, e) => BtnCancel_Click(s, e); // Reuse Cancel logic for closing
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin đã nhập.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmMsg = (_mode == EditorMode.Add) ? "Thêm kho mới này?" : "Lưu thay đổi cho kho này?";
            if (MessageBox.Show(confirmMsg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // Create or update the Warehouse object
            Warehouse warehouseToSave = (_mode == EditorMode.Edit) ? _originalData : new Warehouse();

            warehouseToSave.Name = txtWarehouseName.Text.Trim();
            warehouseToSave.Address = txtAddress.Text.Trim();
            warehouseToSave.ZoneId = (Zone)cmbZone.SelectedValue;
            warehouseToSave.Type = (WarehouseType)cmbType.SelectedValue;
            // IsActive is handled by Toggle in the main UC, default is true for new

            try
            {
                if (_mode == EditorMode.Add)
                {
                    _warehouseSvc.CreateWarehouse(warehouseToSave);
                }
                else // Edit Mode
                {
                    _warehouseSvc.UpdateWarehouse(warehouseToSave);
                }

                MessageBox.Show("Lưu thông tin kho thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the form with OK result
                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Keep the editor open on error
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (HasChanges())
            {
                var confirm = MessageBox.Show("Thông tin đã thay đổi chưa được lưu. Bạn có chắc muốn hủy?", "Xác nhận hủy",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return; // Stay if user clicks No
            }

            // Close the form with Cancel result
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.Cancel;
                parentForm.Close();
            }
        }

        #endregion

        #region Validation & Change Tracking

        private bool ValidateInput()
        {
            errProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtWarehouseName.Text))
            {
                errProvider.SetError(txtWarehouseName, "Tên kho không được để trống.");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                errProvider.SetError(txtAddress, "Địa chỉ không được để trống.");
                isValid = false;
            }
            if (cmbZone.SelectedIndex < 0)
            {
                errProvider.SetError(cmbZone, "Vui lòng chọn vùng.");
                isValid = false;
            }
            if (cmbType.SelectedIndex < 0)
            {
                errProvider.SetError(cmbType, "Vui lòng chọn loại kho.");
                isValid = false;
            }

            return isValid;
        }

        private bool HasChanges()
        {
            if (_originalData == null) return false; // Should not happen after LoadData

            // Compare current control values with original data
            return _originalData.Name != txtWarehouseName.Text.Trim() ||
                   _originalData.Address != txtAddress.Text.Trim() ||
                   _originalData.ZoneId != (Zone?)cmbZone.SelectedValue || // Use nullable comparison
                   _originalData.Type != (WarehouseType?)cmbType.SelectedValue; // Use nullable comparison
        }

        #endregion

        #region Form Dragging Logic

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
                dragStartPoint = Point.Empty;
                parentFormStartPoint = Point.Empty;
            }
        }

        #endregion
    }
}