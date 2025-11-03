//using Guna.UI2.WinForms;
//using LMS.BUS.Helpers;
//using LMS.BUS.Services; // Ensure this using is correct
//using LMS.DAL.Models;   // Ensure this using is correct
//using System;
//using System.Collections.Generic; // Required for List
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Reflection; // Required for Sorting
//using System.Windows.Forms;

//namespace LMS.GUI.AccountAdmin // Ensure this namespace is correct
//{
//    public partial class ucAccount_Admin : UserControl
//    {
//        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
//        private BindingList<object> _binding; // Use object for anonymous type selection

//        private enum EditMode { None, Edit }
//        private EditMode _mode = EditMode.None;

//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        public ucAccount_Admin()
//        {
//            InitializeComponent();
//            this.Load += UcAccount_Admin_Load;
//        }

//        private void UcAccount_Admin_Load(object sender, EventArgs e)
//        {
//            ConfigureGrid();
//            Wire();
//            LoadData();
//        }

//        #region Grid Configuration and Formatting
//        private void ConfigureGrid()
//        {
//            dgvAccounts.Columns.Clear();
//            dgvAccounts.ApplyBaseStyle(); // Use your helper

//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", DataPropertyName = "Username", HeaderText = "Tài khoản", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic }); // Adjust FillWeight

//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Password",
//                DataPropertyName = "Password", // Match the anonymous type property
//                HeaderText = "Mật khẩu",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, // Or Fill if needed
//                SortMode = DataGridViewColumnSortMode.NotSortable, // IMPORTANT: Don't allow sorting by password/hash
//                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft } // Align left is typical for text
//            });

//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", DataPropertyName = "Role", HeaderText = "Vai trò", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinkedTo", DataPropertyName = "LinkedTo", HeaderText = "Liên kết", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic }); // Adjust FillWeight
//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "StatusText", DataPropertyName = "IsActive", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });

//            dgvAccounts.RowPrePaint += DgvAccounts_RowPrePaint;
//            dgvAccounts.CellFormatting += DgvAccounts_CellFormatting;
//            dgvAccounts.ColumnHeaderMouseClick += DgvAccounts_ColumnHeaderMouseClick; // Sort event
//            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged; // Row selection event
//        }

//        private void DgvAccounts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
//        {
//            if (e.RowIndex < 0) return;
//            var row = dgvAccounts.Rows[e.RowIndex];
//            dynamic it = row.DataBoundItem;
//            if (it == null) return;
//            try
//            {
//                bool active = (bool)it.IsActive;
//                row.DefaultCellStyle.ForeColor = active ? Color.Black : Color.Gray;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in RowPrePaint accessing IsActive: {ex.Message}");
//            }
//        }
//        private void DgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (dgvAccounts.Columns[e.ColumnIndex].Name == "StatusText" && e.Value is bool isActive)
//            {
//                e.Value = isActive ? "Hoạt động" : "Đã khóa";
//                e.FormattingApplied = true;
//            }
//            else if (dgvAccounts.Columns[e.ColumnIndex].Name == "Role" && e.Value is UserRole role)
//            {
//                switch (role)
//                {
//                    case UserRole.Customer: e.Value = "Khách hàng"; break;
//                    case UserRole.Driver: e.Value = "Tài xế"; break;
//                    default: e.Value = role.ToString(); break; // Fallback for unexpected roles
//                }
//                e.FormattingApplied = true;
//            }
//        }
//        #endregion

//        #region Data Loading and Selection
//        private void LoadData(string username = null, string name = null)
//        {
//            try
//            {
//                var data = _svc.GetAll(username, name)
//                    .Where(a => a.Role != UserRole.Admin) // Filter Admin
//                    .Select(a => new
//                    {
//                        a.Id,
//                        a.Username,
//                        Password = a.PasswordHash, // *** INCLUDE PasswordHash HERE ***
//                        a.Role,
//                        LinkedTo = a.CustomerId != null ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}"
//                                   : a.DriverId != null ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}"
//                                   : "(Không liên kết)",
//                        a.IsActive
//                    })
//                    .ToList(); // Fetch data first

//                _binding = new BindingList<object>(data.Cast<object>().ToList());
//                dgvAccounts.DataSource = _binding; // Assign DataSource

//                ApplySort(); // Apply sort (if there's a previous sort state)
//                UpdateSortGlyphs(); // Update arrows

//                if (dgvAccounts.Rows.Count > 0 && _mode == EditMode.None)
//                {
//                    dgvAccounts.ClearSelection();
//                    dgvAccounts.Rows[0].Selected = true;
//                    if (dgvAccounts.Columns.Contains("Username"))
//                        dgvAccounts.CurrentCell = dgvAccounts.Rows[0].Cells["Username"];
//                }
//                else if (dgvAccounts.Rows.Count == 0) // Handle empty grid
//                {
//                    ClearInputs(); // Clear input fields
//                    UpdateButtonsBasedOnSelection(); // Disable Edit/Delete/Toggle buttons
//                }
//                UpdateToggleUi();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải danh sách tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvAccounts.DataSource = null; // Clear old data on error
//                UpdateButtonsBasedOnSelection(); // Update buttons for empty grid
//            }
//            finally
//            {
//                SetMode(EditMode.None);
//            }
//        }

//        private (int id, string username, bool isActive)? Current()
//        {
//            if (dgvAccounts.CurrentRow?.DataBoundItem == null) return null;
//            dynamic it = dgvAccounts.CurrentRow.DataBoundItem;
//            try { return ((int)it.Id, (string)it.Username, (bool)it.IsActive); }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error getting current row data: {ex.Message}");
//                return null;
//            }
//        }

//        private void SelectRowById(int id)
//        {
//            if (dgvAccounts.Rows.Count == 0) return;
//            foreach (DataGridViewRow row in dgvAccounts.Rows)
//            {
//                if (row?.DataBoundItem == null) continue;
//                dynamic it = row.DataBoundItem;
//                try
//                {
//                    if ((int)it.Id == id)
//                    {
//                        dgvAccounts.ClearSelection();
//                        row.Selected = true;
//                        var cell = row.Cells["Username"]; // Focus Username column
//                        if (cell != null && cell.Visible) dgvAccounts.CurrentCell = cell;
//                        if (!row.Displayed) // Scroll to row if hidden
//                        {
//                            int rowIndexToShow = Math.Max(0, row.Index - dgvAccounts.DisplayedRowCount(false) / 2);
//                            rowIndexToShow = Math.Min(rowIndexToShow, dgvAccounts.Rows.Count - 1);
//                            if (rowIndexToShow >= 0) dgvAccounts.FirstDisplayedScrollingRowIndex = rowIndexToShow;
//                        }
//                        break; // Found it
//                    }
//                }
//                catch (Exception ex)
//                {
//                    System.Diagnostics.Debug.WriteLine($"Error selecting row by ID {id}: {ex.Message}");
//                }
//            }
//            UpdateToggleUi(); // Update Toggle button after selection
//        }
//        #endregion

//        #region UI Event Wiring and Mode Handling
//        private void Wire()
//        {
//            btnReload.Click += BtnReload_Click;
//            btnEdit.Click += BtnEdit_Click;
//            btnSave.Click += BtnSave_Click;
//            btnCancel.Click += BtnCancel_Click;
//            btnDelete.Click += BtnDelete_Click;
//            btnToggle.Click += BtnToggle_Click;
//            btnSearch.Click += BtnSearch_Click;
//            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged;
//        }

//        private void SetMode(EditMode m)
//        {
//            _mode = m; bool isEditing = (m == EditMode.Edit);
//            txtUser.Enabled = isEditing; txtPass.Enabled = isEditing;
//            btnSave.Enabled = isEditing; btnCancel.Enabled = isEditing;
//            dgvAccounts.Enabled = !isEditing;
//            bool canInteract = !isEditing && Current() != null; // Can interact if a row is selected & not editing
//            btnEdit.Enabled = canInteract; btnDelete.Enabled = canInteract; btnToggle.Enabled = canInteract;
//            btnSearch.Enabled = !isEditing; btnReload.Enabled = !isEditing;
//        }

//        private void ClearInputs() { txtUser.Clear(); txtPass.Clear(); }

//        private void UpdateButtonsBasedOnSelection()
//        {
//            bool hasSelection = Current() != null;
//            bool isNotEditing = (_mode == EditMode.None);
//            btnEdit.Enabled = hasSelection && isNotEditing;
//            btnDelete.Enabled = hasSelection && isNotEditing;
//            btnToggle.Enabled = hasSelection && isNotEditing;
//            // Update Toggle button
//            if (hasSelection) { UpdateToggleUi(); }
//            else { btnToggle.Text = "Khóa/Mở"; btnToggle.FillColor = Color.Gray; btnToggle.Enabled = false; }
//        }

//        private void UpdateToggleUi()
//        {
//            var cur = Current();
//            bool isNotEditing = (_mode == EditMode.None);
//            if (cur == null) { btnToggle.Enabled = false; btnToggle.Text = "Khóa/Mở"; btnToggle.FillColor = Color.Gray; return; }
//            btnToggle.Enabled = isNotEditing; // Only enable when not editing
//            btnToggle.Text = cur.Value.isActive ? "Khóa" : "Mở khóa";
//            btnToggle.FillColor = cur.Value.isActive ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69);
//        }
//        #endregion

//        #region Button Click Handlers
//        private void BtnReload_Click(object sender, EventArgs e) { LoadData(); } // LoadData handles other tasks

//        private void BtnEdit_Click(object sender, EventArgs e)
//        {
//            var cur = Current();
//            if (cur != null)
//            {
//                SetMode(EditMode.Edit);
//                txtUser.Text = cur.Value.username;
//                txtPass.Clear(); // Clear password for editing
//                txtUser.Focus();
//                txtUser.SelectAll();
//            }
//        }

//        private void BtnSave_Click(object sender, EventArgs e) { SaveEdit(); }

//        private void BtnCancel_Click(object sender, EventArgs e)
//        {
//            if (_mode == EditMode.Edit)
//            {
//                bool usernameChanged = (txtUser.Text.Trim() != (Current()?.username ?? string.Empty));
//                bool passwordEntered = !string.IsNullOrWhiteSpace(txtPass.Text);
//                if (usernameChanged || passwordEntered)
//                {
//                    var ask = MessageBox.Show("Bạn có thay đổi chưa lưu. Hủy bỏ các thay đổi này?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                    if (ask != DialogResult.Yes) return;
//                }
//            }
//            ClearInputs();
//            SetMode(EditMode.None);
//            var curId = Current()?.id; // Re-select current row
//            if (curId.HasValue) SelectRowById(curId.Value);
//        }

//        private void BtnDelete_Click(object sender, EventArgs e) { DeleteAccount(); }

//        private void BtnToggle_Click(object sender, EventArgs e) { ToggleActive(); }

//        private void BtnSearch_Click(object sender, EventArgs e) { OpenSearch(); }

//        private void DgvAccounts_SelectionChanged(object sender, EventArgs e)
//        {
//            // Only update buttons if not in Edit mode
//            if (_mode == EditMode.None)
//            {
//                UpdateButtonsBasedOnSelection();
//            }
//        }
//        #endregion

//        #region Core Logic Methods (Save, Delete, Toggle, Search)
//        private void SaveEdit()
//        {
//            var cur = Current(); if (cur == null || _mode != EditMode.Edit) return;
//            string newUsername = txtUser.Text.Trim(); string newPassword = txtPass.Text;
//            bool usernameChanged = newUsername != cur.Value.username; bool passwordEntered = !string.IsNullOrWhiteSpace(newPassword);

//            if (!usernameChanged && !passwordEntered) { MessageBox.Show("Không có thay đổi."); SetMode(EditMode.None); return; }
//            if (usernameChanged && string.IsNullOrWhiteSpace(newUsername)) { MessageBox.Show("Tên tài khoản trống."); txtUser.Focus(); return; }
//            if (passwordEntered && newPassword.Length < 6) { MessageBox.Show("Mật khẩu mới >= 6 ký tự."); txtPass.Focus(); return; }

//            try
//            {
//                var confirm = MessageBox.Show("Lưu thay đổi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                if (confirm != DialogResult.Yes) return;
//                _svc.UpdateBasic(cur.Value.id, newUsername, passwordEntered ? newPassword : null);
//                MessageBox.Show("Đã cập nhật.", "Thành công");
//                int keepId = cur.Value.id; LoadData(); SelectRowById(keepId); // LoadData sets Mode to None, SelectRowById updates buttons
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi cập nhật: {ex.Message}", "Lỗi"); } // Keep Edit Mode on error
//        }

//        private void DeleteAccount()
//        {
//            var cur = Current(); if (cur == null || _mode != EditMode.None) return;
//            try
//            {
//                var rpt = _svc.InspectUsage(cur.Value.id); string msg; DialogResult confirm = DialogResult.No;

//                if (rpt.Role == UserRole.Customer)
//                {
//                    if (rpt.OrdersActive > 0) { MessageBox.Show($"KH có {rpt.OrdersActive} đơn đang xử lý. Không thể xóa.", "Không thể xóa"); return; }
//                    if (rpt.OrdersPending > 0) msg = $"KH có {rpt.OrdersPending} đơn chờ duyệt.\nXóa TK sẽ khiến họ không đăng nhập được.\nTiếp tục xóa?";
//                    else if (rpt.OrdersCompleted > 0 || rpt.OrdersCancelled > 0) msg = $"KH có lịch sử đơn hàng.\nXóa tài khoản đăng nhập này?";
//                    else msg = "Xóa tài khoản đăng nhập này?";
//                    confirm = MessageBox.Show(msg, "Xác nhận xóa TK Khách hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                }
//                else if (rpt.Role == UserRole.Driver)
//                {
//                    if (rpt.ShipActive > 0) { MessageBox.Show($"TX có {rpt.ShipActive} chuyến đang chạy. Không thể xóa.", "Không thể xóa"); return; }
//                    if (rpt.ShipPending > 0) msg = $"TX có {rpt.ShipPending} chuyến chờ nhận.\nXóa TK sẽ khiến họ không đăng nhập được.\nTiếp tục xóa?";
//                    else if (rpt.ShipCompleted > 0 || rpt.ShipCancelled > 0) msg = $"TX có lịch sử chuyến đi.\nXóa tài khoản đăng nhập này?";
//                    else msg = "Xóa tài khoản đăng nhập này?";
//                    confirm = MessageBox.Show(msg, "Xác nhận xóa TK Tài xế", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                }
//                else { MessageBox.Show("Không thể xóa Admin."); return; }

//                if (confirm == DialogResult.Yes)
//                {
//                    _svc.DeleteOnlyAccount(cur.Value.id);
//                    MessageBox.Show("Đã xóa tài khoản.", "Thành công");
//                    LoadData(); // Reload
//                }
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi"); }
//        }

//        private void ToggleActive()
//        {
//            var cur = Current(); if (cur == null || _mode != EditMode.None) return;
//            bool newState = !cur.Value.isActive; string actionText = newState ? "Mở khóa" : "Khóa";
//            try
//            {
//                var confirm = MessageBox.Show($"Bạn chắc muốn {actionText} tài khoản '{cur.Value.username}'?", $"Xác nhận {actionText}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                if (confirm == DialogResult.Yes)
//                {
//                    _svc.SetActive(cur.Value.id, newState);
//                    int keepId = cur.Value.id; LoadData(); SelectRowById(keepId); // LoadData goes to Mode None, SelectRowById updates buttons
//                    MessageBox.Show($"Đã {actionText} tài khoản.", "Thành công");
//                }
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi khi {actionText}: {ex.Message}", "Lỗi"); }
//        }

//        private void OpenSearch()
//        {
//            if (_mode != EditMode.None) return; int? selectedId = null;
//            using (var searchForm = new Form
//            {
//                StartPosition = FormStartPosition.CenterScreen,
//                FormBorderStyle = FormBorderStyle.None,
//                Size = new Size(1199, 725), // Adjust size as needed for ucAccountSearch_Admin
//            })
//            {
//                var ucSearch = new ucAccountSearch_Admin
//                {
//                    Dock = DockStyle.Fill
//                };
//                ucSearch.AccountSelected += (s, id) =>
//                {
//                    selectedId = id;
//                    searchForm.DialogResult = DialogResult.OK;
//                    searchForm.Close();
//                };
//                searchForm.Controls.Add(ucSearch);
//                Form owner = this.FindForm(); // Get owner form
//                if (owner == null) { MessageBox.Show("Cannot find parent form.", "Error"); return; } // Handle error
//                if (searchForm.ShowDialog(owner) == DialogResult.OK && selectedId.HasValue) { SelectRowById(selectedId.Value); }
//            }
//        }
//        #endregion

//        #region Sorting Logic
//        private void DgvAccounts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (_binding == null || _binding.Count == 0) return;
//            var newColumn = dgvAccounts.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
//            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }

//            ApplySort();
//            UpdateSortGlyphs();
//        }
//        private void ApplySort()
//        {
//            if (_sortedColumn == null || _binding == null || _binding.Count == 0) return; // Add Count check
//            string propertyName = _sortedColumn.DataPropertyName; PropertyInfo propInfo = null;
//            // Get type from first valid item
//            var firstItem = _binding.FirstOrDefault(item => item != null);
//            if (firstItem != null) propInfo = firstItem.GetType().GetProperty(propertyName);

//            if (propInfo == null) return; // Prop not found -> cannot sort

//            List<object> items = _binding.ToList(); List<object> sortedList;
//            try
//            {
//                if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
//                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
//                _binding = new BindingList<object>(sortedList); dgvAccounts.DataSource = _binding;
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp cột '{_sortedColumn.HeaderText}': {ex.Message}"); ResetSortGlyphs(); } // Report sort error
//        }
//        private void UpdateSortGlyphs()
//        {
//            foreach (DataGridViewColumn col in dgvAccounts.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
//                if (_sortedColumn != null && col == _sortedColumn) { col.HeaderCell.SortGlyphDirection = _sortOrder; }
//            }
//        }
//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            _sortedColumn = null; _sortOrder = SortOrder.None; UpdateSortGlyphs(); // Call Update to reset UI
//        }
//        #endregion
//    }
//}
using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection; // dùng cho việc sắp xếp (Sorting) bằng reflection
using System.Windows.Forms;

namespace LMS.GUI.AccountAdmin
{
    public partial class ucAccount_Admin : UserControl
    {
        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
        private BindingList<object> _binding; // dùng 'object' vì chúng ta select về kiểu dữ liệu nặc danh (anonymous type)

        private enum EditMode { None, Edit }
        private EditMode _mode = EditMode.None;

        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucAccount_Admin()
        {
            InitializeComponent();
            this.Load += UcAccount_Admin_Load;
        }

        private void UcAccount_Admin_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            Wire();
            LoadData();
        }

        private void ConfigureGrid()
        {
            dgvAccounts.Columns.Clear();
            dgvAccounts.ApplyBaseStyle(); // áp dụng style cơ bản (helper)

            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", DataPropertyName = "Username", HeaderText = "Tài khoản", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic });

            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Password",
                DataPropertyName = "Password",
                HeaderText = "Mật khẩu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.NotSortable, // quan trọng: không cho phép sắp xếp cột mật khẩu
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", DataPropertyName = "Role", HeaderText = "Vai trò", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinkedTo", DataPropertyName = "LinkedTo", HeaderText = "Liên kết", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "StatusText", DataPropertyName = "IsActive", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });

            dgvAccounts.RowPrePaint += DgvAccounts_RowPrePaint;
            dgvAccounts.CellFormatting += DgvAccounts_CellFormatting;
            dgvAccounts.ColumnHeaderMouseClick += DgvAccounts_ColumnHeaderMouseClick; // sự kiện bấm vào header cột để sắp xếp
            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged; // sự kiện khi chọn dòng khác
        }

        private void DgvAccounts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            var row = dgvAccounts.Rows[e.RowIndex];
            dynamic it = row.DataBoundItem;
            if (it == null)
            {
                return;
            }
            try
            {
                bool active = (bool)it.IsActive;
                row.DefaultCellStyle.ForeColor = active ? Color.Black : Color.Gray;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in RowPrePaint accessing IsActive: {ex.Message}");
            }
        }
        private void DgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvAccounts.Columns[e.ColumnIndex].Name == "StatusText" && e.Value is bool isActive)
            {
                e.Value = isActive ? "Hoạt động" : "Đã khóa";
                e.FormattingApplied = true;
            }
            else if (dgvAccounts.Columns[e.ColumnIndex].Name == "Role" && e.Value is UserRole role)
            {
                switch (role)
                {
                    case UserRole.Customer: e.Value = "Khách hàng"; break;
                    case UserRole.Driver: e.Value = "Tài xế"; break;
                    default: e.Value = role.ToString(); break; // dự phòng cho các role không xác định
                }
                e.FormattingApplied = true;
            }
        }

        private void LoadData(string username = null, string name = null)
        {
            try
            {
                var data = _svc.GetAll(username, name)
                    .Where(a => a.Role != UserRole.Admin) // lọc tài khoản admin ra khỏi danh sách
                    .Select(a => new
                    {
                        a.Id,
                        a.Username,
                        Password = a.PasswordHash,
                        a.Role,
                        LinkedTo = a.CustomerId != null ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}"
                                   : a.DriverId != null ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}"
                                   : "(Không liên kết)",
                        a.IsActive
                    })
                    .ToList(); // lấy dữ liệu về memory trước khi gán

                _binding = new BindingList<object>(data.Cast<object>().ToList());
                dgvAccounts.DataSource = _binding;

                ApplySort(); // áp dụng lại trạng thái sắp xếp (nếu có)
                UpdateSortGlyphs(); // cập nhật mũi tên sắp xếp trên header

                if (dgvAccounts.Rows.Count > 0 && _mode == EditMode.None)
                {
                    dgvAccounts.ClearSelection();
                    dgvAccounts.Rows[0].Selected = true;
                    if (dgvAccounts.Columns.Contains("Username"))
                    {
                        dgvAccounts.CurrentCell = dgvAccounts.Rows[0].Cells["Username"];
                    }
                }
                else if (dgvAccounts.Rows.Count == 0) // xử lý khi lưới không có dữ liệu
                {
                    ClearInputs(); // xóa các ô nhập liệu
                    UpdateButtonsBasedOnSelection(); // vô hiệu hóa các nút (sửa/xóa/khóa)
                }
                UpdateToggleUi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvAccounts.DataSource = null; // xóa dữ liệu cũ khi có lỗi
                UpdateButtonsBasedOnSelection(); // cập nhật nút cho lưới rỗng
            }
            finally
            {
                SetMode(EditMode.None);
            }
        }

        private (int id, string username, bool isActive)? Current()
        {
            if (dgvAccounts.CurrentRow?.DataBoundItem == null)
            {
                return null;
            }
            dynamic it = dgvAccounts.CurrentRow.DataBoundItem;
            try
            {
                return ((int)it.Id, (string)it.Username, (bool)it.IsActive);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting current row data: {ex.Message}");
                return null;
            }
        }

        private void SelectRowById(int id)
        {
            if (dgvAccounts.Rows.Count == 0)
            {
                return;
            }
            foreach (DataGridViewRow row in dgvAccounts.Rows)
            {
                if (row?.DataBoundItem == null)
                {
                    continue;
                }
                dynamic it = row.DataBoundItem;
                try
                {
                    if ((int)it.Id == id)
                    {
                        dgvAccounts.ClearSelection();
                        row.Selected = true;
                        var cell = row.Cells["Username"]; // focus vào cột username
                        if (cell != null && cell.Visible)
                        {
                            dgvAccounts.CurrentCell = cell;
                        }
                        if (!row.Displayed) // cuộn đến dòng nếu nó đang bị che
                        {
                            int rowIndexToShow = Math.Max(0, row.Index - dgvAccounts.DisplayedRowCount(false) / 2);
                            rowIndexToShow = Math.Min(rowIndexToShow, dgvAccounts.Rows.Count - 1);
                            if (rowIndexToShow >= 0)
                            {
                                dgvAccounts.FirstDisplayedScrollingRowIndex = rowIndexToShow;
                            }
                        }
                        break; // đã tìm thấy
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error selecting row by ID {id}: {ex.Message}");
                }
            }
            UpdateToggleUi(); // cập nhật lại nút khóa/mở
        }

        private void Wire()
        {
            btnReload.Click += BtnReload_Click;
            btnEdit.Click += BtnEdit_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnDelete.Click += BtnDelete_Click;
            btnToggle.Click += BtnToggle_Click;
            btnSearch.Click += BtnSearch_Click;
            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged;
        }

        private void SetMode(EditMode m)
        {
            _mode = m; bool isEditing = (m == EditMode.Edit);
            txtUser.Enabled = isEditing; txtPass.Enabled = isEditing;
            btnSave.Enabled = isEditing; btnCancel.Enabled = isEditing;
            dgvAccounts.Enabled = !isEditing;

            // kiểm tra xem người dùng có thể tương tác (sửa/xóa/khóa) không
            bool canInteract = !isEditing && Current() != null;
            btnEdit.Enabled = canInteract;
            btnDelete.Enabled = canInteract;
            btnToggle.Enabled = canInteract;

            btnSearch.Enabled = !isEditing;
            btnReload.Enabled = !isEditing;
        }

        private void ClearInputs() { txtUser.Clear(); txtPass.Clear(); }

        private void UpdateButtonsBasedOnSelection()
        {
            bool hasSelection = Current() != null;
            bool isNotEditing = (_mode == EditMode.None);
            btnEdit.Enabled = hasSelection && isNotEditing;
            btnDelete.Enabled = hasSelection && isNotEditing;
            btnToggle.Enabled = hasSelection && isNotEditing;

            // cập nhật riêng cho nút khóa/mở
            if (hasSelection)
            {
                UpdateToggleUi();
            }
            else
            {
                btnToggle.Text = "Khóa/Mở";
                btnToggle.FillColor = Color.Gray;
                btnToggle.Enabled = false;
            }
        }

        private void UpdateToggleUi()
        {
            var cur = Current();
            bool isNotEditing = (_mode == EditMode.None);
            if (cur == null)
            {
                btnToggle.Enabled = false;
                btnToggle.Text = "Khóa/Mở";
                btnToggle.FillColor = Color.Gray;
                return;
            }
            btnToggle.Enabled = isNotEditing; // chỉ cho phép khi không ở chế độ sửa
            btnToggle.Text = cur.Value.isActive ? "Khóa" : "Mở khóa";
            btnToggle.FillColor = cur.Value.isActive ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69);
        }

        private void BtnReload_Click(object sender, EventArgs e) { LoadData(); } // loadData đã xử lý các tác vụ khác

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var cur = Current();
            if (cur != null)
            {
                SetMode(EditMode.Edit);
                txtUser.Text = cur.Value.username;
                txtPass.Clear(); // xóa mật khẩu để chuẩn bị chỉnh sửa
                txtUser.Focus();
                txtUser.SelectAll();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e) { SaveEdit(); }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_mode == EditMode.Edit)
            {
                bool usernameChanged = (txtUser.Text.Trim() != (Current()?.username ?? string.Empty));
                bool passwordEntered = !string.IsNullOrWhiteSpace(txtPass.Text);
                if (usernameChanged || passwordEntered)
                {
                    var ask = MessageBox.Show("Bạn có thay đổi chưa lưu. Hủy bỏ các thay đổi này?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ask != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            ClearInputs();
            SetMode(EditMode.None);
            var curId = Current()?.id; // chọn lại dòng hiện tại
            if (curId.HasValue)
            {
                SelectRowById(curId.Value);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e) { DeleteAccount(); }

        private void BtnToggle_Click(object sender, EventArgs e) { ToggleActive(); }

        private void BtnSearch_Click(object sender, EventArgs e) { OpenSearch(); }

        private void DgvAccounts_SelectionChanged(object sender, EventArgs e)
        {
            // chỉ cập nhật các nút khi không ở chế độ sửa
            if (_mode == EditMode.None)
            {
                UpdateButtonsBasedOnSelection();
            }
        }

        private void SaveEdit()
        {
            var cur = Current();
            if (cur == null || _mode != EditMode.Edit)
            {
                return;
            }
            string newUsername = txtUser.Text.Trim(); string newPassword = txtPass.Text;
            bool usernameChanged = newUsername != cur.Value.username; bool passwordEntered = !string.IsNullOrWhiteSpace(newPassword);

            if (!usernameChanged && !passwordEntered)
            {
                MessageBox.Show("Không có thay đổi.");
                SetMode(EditMode.None);
                return;
            }
            if (usernameChanged && string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("Tên tài khoản trống.");
                txtUser.Focus();
                return;
            }
            if (passwordEntered && newPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới >= 6 ký tự.");
                txtPass.Focus();
                return;
            }

            try
            {
                var confirm = MessageBox.Show("Lưu thay đổi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes)
                {
                    return;
                }
                _svc.UpdateBasic(cur.Value.id, newUsername, passwordEntered ? newPassword : null);
                MessageBox.Show("Đã cập nhật.", "Thành công");
                int keepId = cur.Value.id;
                LoadData();
                SelectRowById(keepId); // loadData sẽ set mode về None, selectRowById cập nhật lại các nút
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật: {ex.Message}", "Lỗi");
            } // giữ nguyên chế độ sửa khi có lỗi
        }

        private void DeleteAccount()
        {
            var cur = Current();
            if (cur == null || _mode != EditMode.None)
            {
                return;
            }
            try
            {
                var rpt = _svc.InspectUsage(cur.Value.id); string msg; DialogResult confirm = DialogResult.No;

                if (rpt.Role == UserRole.Customer)
                {
                    if (rpt.OrdersActive > 0)
                    {
                        MessageBox.Show($"KH có {rpt.OrdersActive} đơn đang xử lý. Không thể xóa.", "Không thể xóa");
                        return;
                    }

                    if (rpt.OrdersPending > 0)
                    {
                        msg = $"KH có {rpt.OrdersPending} đơn chờ duyệt.\nXóa TK sẽ khiến họ không đăng nhập được.\nTiếp tục xóa?";
                    }
                    else if (rpt.OrdersCompleted > 0 || rpt.OrdersCancelled > 0)
                    {
                        msg = $"KH có lịch sử đơn hàng.\nXóa tài khoản đăng nhập này?";
                    }
                    else
                    {
                        msg = "Xóa tài khoản đăng nhập này?";
                    }
                    confirm = MessageBox.Show(msg, "Xác nhận xóa TK Khách hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else if (rpt.Role == UserRole.Driver)
                {
                    if (rpt.ShipActive > 0)
                    {
                        MessageBox.Show($"TX có {rpt.ShipActive} chuyến đang chạy. Không thể xóa.", "Không thể xóa");
                        return;
                    }

                    if (rpt.ShipPending > 0)
                    {
                        msg = $"TX có {rpt.ShipPending} chuyến chờ nhận.\nXóa TK sẽ khiến họ không đăng nhập được.\nTiếp tục xóa?";
                    }
                    else if (rpt.ShipCompleted > 0 || rpt.ShipCancelled > 0)
                    {
                        msg = $"TX có lịch sử chuyến đi.\nXóa tài khoản đăng nhập này?";
                    }
                    else
                    {
                        msg = "Xóa tài khoản đăng nhập này?";
                    }
                    confirm = MessageBox.Show(msg, "Xác nhận xóa TK Tài xế", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else
                {
                    MessageBox.Show("Không thể xóa Admin.");
                    return;
                }

                if (confirm == DialogResult.Yes)
                {
                    _svc.DeleteOnlyAccount(cur.Value.id);
                    MessageBox.Show("Đã xóa tài khoản.", "Thành công");
                    LoadData(); // tải lại dữ liệu
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi");
            }
        }

        private void ToggleActive()
        {
            var cur = Current();
            if (cur == null || _mode != EditMode.None)
            {
                return;
            }
            bool newState = !cur.Value.isActive; string actionText = newState ? "Mở khóa" : "Khóa";
            try
            {
                var confirm = MessageBox.Show($"Bạn chắc muốn {actionText} tài khoản '{cur.Value.username}'?", $"Xác nhận {actionText}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _svc.SetActive(cur.Value.id, newState);
                    int keepId = cur.Value.id;
                    LoadData();
                    SelectRowById(keepId); // loadData sẽ set mode về None, selectRowById cập nhật lại các nút
                    MessageBox.Show($"Đã {actionText} tài khoản.", "Thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi {actionText}: {ex.Message}", "Lỗi");
            }
        }

        private void OpenSearch()
        {
            if (_mode != EditMode.None)
            {
                return;
            }
            int? selectedId = null;
            using (var searchForm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(1199, 725), // điều chỉnh kích thước form cho vừa với ucAccountSearch_Admin
            })
            {
                var ucSearch = new ucAccountSearch_Admin
                {
                    Dock = DockStyle.Fill
                };
                ucSearch.AccountSelected += (s, id) =>
                {
                    selectedId = id;
                    searchForm.DialogResult = DialogResult.OK;
                    searchForm.Close();
                };
                searchForm.Controls.Add(ucSearch);
                Form owner = this.FindForm(); // lấy form cha (form chính)
                if (owner == null)
                {
                    MessageBox.Show("Không tìm thấy form cha.", "Lỗi");
                    return;
                } // xử lý lỗi
                if (searchForm.ShowDialog(owner) == DialogResult.OK && selectedId.HasValue)
                {
                    SelectRowById(selectedId.Value);
                }
            }
        }

        private void DgvAccounts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_binding == null || _binding.Count == 0)
            {
                return;
            }
            var newColumn = dgvAccounts.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable)
            {
                return;
            }

            if (_sortedColumn == newColumn)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn != null)
                {
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            ApplySort();
            UpdateSortGlyphs();
        }
        private void ApplySort()
        {
            if (_sortedColumn == null || _binding == null || _binding.Count == 0)
            {
                return;
            } // thêm kiểm tra count
            string propertyName = _sortedColumn.DataPropertyName; PropertyInfo propInfo = null;

            // lấy kiểu dữ liệu (type) từ item đầu tiên
            var firstItem = _binding.FirstOrDefault(item => item != null);
            if (firstItem != null)
            {
                propInfo = firstItem.GetType().GetProperty(propertyName);
            }

            if (propInfo == null)
            {
                return;
            } // không tìm thấy thuộc tính -> không thể sắp xếp

            List<object> items = _binding.ToList(); List<object> sortedList;
            try
            {
                if (_sortOrder == SortOrder.Ascending)
                {
                    sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
                }
                else
                {
                    sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
                }
                _binding = new BindingList<object>(sortedList); dgvAccounts.DataSource = _binding;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi sắp xếp cột '{_sortedColumn.HeaderText}': {ex.Message}");
                ResetSortGlyphs();
            } // báo cáo lỗi sắp xếp
        }
        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvAccounts.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                {
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                if (_sortedColumn != null && col == _sortedColumn)
                {
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
                }
            }
        }
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null)
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null; _sortOrder = SortOrder.None; UpdateSortGlyphs(); // gọi update để reset lại giao diện
        }
    }
}