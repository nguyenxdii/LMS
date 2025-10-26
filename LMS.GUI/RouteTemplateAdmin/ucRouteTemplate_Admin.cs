// LMS.GUI/RouteTemplateAdmin/ucRouteTemplate_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // You'll create this DTO
using LMS.BUS.Helpers;
using LMS.BUS.Services; // You'll create this Service
using LMS.DAL.Models;   // For RouteTemplate if needed directly
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.RouteTemplateAdmin
{
    public partial class ucRouteTemplate_Admin : UserControl
    {
        // Service Dependency
        private readonly RouteTemplateService_Admin _templateSvc = new RouteTemplateService_Admin(); // Ensure this service exists

        // Grid Data & Sorting State
        private BindingList<RouteTemplateListItemDto> _bindingList; // Use the specific DTO
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucRouteTemplate_Admin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData(); // Load initial data
        }

        #region Grid Configuration

        private void ConfigureGrid()
        {
            dgvTemplates.Columns.Clear();
            dgvTemplates.ApplyBaseStyle(); // Use GridHelper extension

            // Define Columns using RouteTemplateListItemDto properties
            dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false // Keep ID for selection
            });
            dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Tên Tuyến đường",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FromWarehouseName",
                DataPropertyName = "FromWarehouseName",
                HeaderText = "Kho đi",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ToWarehouseName",
                DataPropertyName = "ToWarehouseName",
                HeaderText = "Kho đến",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            // *** KIỂM TRA KỸ CỘT NÀY ***
            dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StopsCount", // Tên cột (có thể tùy ý)
                DataPropertyName = "StopsCount", // *** Rất quan trọng: Phải khớp chính xác tên thuộc tính trong DTO ***
                HeaderText = "Số chặng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                SortMode = DataGridViewColumnSortMode.Programmatic // Nên để Programmatic nếu bạn tự xử lý sort
            });
            // *** KẾT THÚC KIỂM TRA ***

            // Add Event Handlers
            dgvTemplates.ColumnHeaderMouseClick += DgvTemplates_ColumnHeaderMouseClick;
            dgvTemplates.SelectionChanged += DgvTemplates_SelectionChanged;
            dgvTemplates.CellDoubleClick += (s, ev) => { if (ev.RowIndex >= 0) OpenEditorPopup(GetSelectedTemplateId()); }; // Double-click to Edit
        }

        #endregion

        #region Data Loading & Helpers

        private void LoadData()
        {
            try
            {
                // Call service to get list DTOs
                var templates = _templateSvc.GetRouteTemplateListItems(); // This method needs to exist in the service
                _bindingList = new BindingList<RouteTemplateListItemDto>(templates);

                // *** Gán DataSource ***
                dgvTemplates.DataSource = _bindingList; // Đảm bảo DataSource được gán đúng

                ApplySort(); // Re-apply sort if needed
                UpdateSortGlyphs();
                UpdateButtonsState(); // Update buttons based on selection/data

                if (dgvTemplates.Rows.Count > 0)
                {
                    dgvTemplates.ClearSelection();
                    dgvTemplates.Rows[0].Selected = true;
                }
                else
                {
                    // Handle empty grid case if needed (e.g., disable buttons)
                    UpdateButtonsState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách tuyến đường: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvTemplates.DataSource = null;
            }
        }

        private int? GetSelectedTemplateId()
        {
            if (dgvTemplates.CurrentRow != null && dgvTemplates.CurrentRow.DataBoundItem is RouteTemplateListItemDto dto)
            {
                return dto.Id;
            }
            return null;
        }

        private RouteTemplateListItemDto GetSelectedTemplateDto()
        {
            return dgvTemplates.CurrentRow?.DataBoundItem as RouteTemplateListItemDto;
        }


        private void UpdateButtonsState()
        {
            bool hasSelection = GetSelectedTemplateId() != null;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            // btnAdd, btnSearch, btnReload are usually always enabled
        }

        // Select row after search or action
        private void SelectRowById(int templateId)
        {
            if (dgvTemplates.Rows.Count == 0 || _bindingList == null) return;

            RouteTemplateListItemDto itemToSelect = _bindingList.FirstOrDefault(t => t.Id == templateId);
            if (itemToSelect != null)
            {
                int rowIndex = _bindingList.IndexOf(itemToSelect);
                if (rowIndex >= 0 && rowIndex < dgvTemplates.Rows.Count)
                {
                    dgvTemplates.ClearSelection();
                    dgvTemplates.Rows[rowIndex].Selected = true;
                    if (dgvTemplates.Columns.Contains("Name")) // Focus Name column
                        dgvTemplates.CurrentCell = dgvTemplates.Rows[rowIndex].Cells["Name"];

                    // Scroll to the row if it's not visible
                    if (!dgvTemplates.Rows[rowIndex].Displayed)
                    {
                        int firstDisplayed = Math.Max(0, rowIndex - dgvTemplates.DisplayedRowCount(false) / 2);
                        dgvTemplates.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvTemplates.RowCount - 1);
                    }
                }
            }
            UpdateButtonsState();
        }

        #endregion

        #region Event Wiring

        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null); // null ID means Add mode
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedTemplateId());
            btnDelete.Click += (s, e) => DeleteTemplate();
            //btnSearch.Click += (s, e) => OpenSearchPopup();

            dgvTemplates.SelectionChanged += (s, e) => UpdateButtonsState();
            // Double click is handled in ConfigureGrid
        }

        #endregion

        #region Actions (Popup Opening, Delete)

        private void OpenEditorPopup(int? templateId)
        {
            // Prevent opening if buttons are disabled
            if (templateId == null && !btnAdd.Enabled) return;
            if (templateId.HasValue && !btnEdit.Enabled) return;

            var mode = templateId.HasValue ? ucRouteTemplateEditor_Admin.EditorMode.Edit : ucRouteTemplateEditor_Admin.EditorMode.Add;

            try
            {
                // Assuming ucRouteTemplateEditor_Admin has a LoadData method
                var ucEditor = new ucRouteTemplateEditor_Admin();
                ucEditor.LoadData(mode, templateId); // Load data within the UC

                // Create the popup form
                using (var popupForm = new Form
                {
                    // Adjust Title based on mode maybe inside LoadData of editor UC
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    // Define appropriate Size for the editor UC
                    Size = new Size(1384, 763), // *** ADJUST SIZE AS NEEDED ***
                })
                {
                    popupForm.Controls.Add(ucEditor);
                    ucEditor.Dock = DockStyle.Fill;

                    Form ownerForm = this.FindForm();
                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

                    if (popupForm.ShowDialog(ownerForm) == DialogResult.OK)
                    {
                        LoadData(); // Reload grid if save was successful
                        // Optionally try to re-select the added/edited item
                        // You might need the editor to return the saved ID
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở trình chỉnh sửa tuyến đường: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteTemplate()
        {
            int? templateId = GetSelectedTemplateId();
            RouteTemplateListItemDto selectedDto = GetSelectedTemplateDto(); // Get DTO for name

            if (!templateId.HasValue || selectedDto == null)
            {
                MessageBox.Show("Vui lòng chọn một tuyến đường để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Confirmation
            string confirmMsg = $"Bạn có chắc muốn xóa tuyến đường '{selectedDto.Name}'?";
            if (MessageBox.Show(confirmMsg, "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // **Important Check before Deleting**
                // You need a service method to check if this template is currently used by any *active* Shipments.
                bool isInUse = _templateSvc.IsRouteTemplateInUse(templateId.Value); // Implement this in service
                if (isInUse)
                {
                    MessageBox.Show("Không thể xóa tuyến đường này vì đang được sử dụng bởi một hoặc nhiều chuyến hàng đang hoạt động.",
                                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Call the service to delete
                _templateSvc.DeleteRouteTemplate(templateId.Value); // Implement this in service

                LoadData(); // Reload data after deletion

                MessageBox.Show("Đã xóa tuyến đường thành công.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa tuyến đường: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Sorting Logic

        private void DgvTemplates_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            var newColumn = dgvTemplates.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            ApplySort();
            UpdateSortGlyphs();
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;

            string propertyName = _sortedColumn.DataPropertyName;
            // *** Use the DTO type here ***
            PropertyInfo propInfo = typeof(RouteTemplateListItemDto).GetProperty(propertyName);
            if (propInfo == null) return;

            List<RouteTemplateListItemDto> items = _bindingList.ToList();
            List<RouteTemplateListItemDto> sortedList;

            try
            {
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
                else
                    sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                _bindingList = new BindingList<RouteTemplateListItemDto>(sortedList);
                dgvTemplates.DataSource = _bindingList; // Rebind sorted data
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sắp xếp: {ex.Message}", "Lỗi Sắp Xếp");
                ResetSortGlyphs();
            }
        }

        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvTemplates.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_sortedColumn != null && col == _sortedColumn)
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
            UpdateSortGlyphs(); // Visually reset all glyphs
        }

        #endregion

        // Handle selection change on the grid
        private void DgvTemplates_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonsState();
        }
    }
}