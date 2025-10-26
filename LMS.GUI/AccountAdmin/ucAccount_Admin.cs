using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services; // Đảm bảo using này đúng
using LMS.DAL.Models;   // Đảm bảo using này đúng
using System;
using System.Collections.Generic; // Cần cho List
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection; // Cần cho Sorting
using System.Windows.Forms;

namespace LMS.GUI.AccountAdmin // Đảm bảo namespace này đúng
{
    public partial class ucAccount_Admin : UserControl
    {
        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
        private BindingList<object> _binding; // Dùng object vì select anonymous type

        private enum EditMode { None, Edit }
        private EditMode _mode = EditMode.None;

        // --- Biến cho Sorting ---
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;
        // --- Kết thúc biến ---

        public ucAccount_Admin()
        {
            InitializeComponent();
            this.Load += UcAccount_Admin_Load;
        }

        private void UcAccount_Admin_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            Wire(); // Gán sự kiện trước khi LoadData lần đầu
            LoadData(); // LoadData sẽ gọi SetMode(None) trong finally
        }

        #region Grid Configuration and Formatting
        private void ConfigureGrid()
        {
            dgvAccounts.Columns.Clear();
            dgvAccounts.ApplyBaseStyle(); // Sử dụng helper của bạn

            // --- Định nghĩa các cột (Thêm SortMode) ---
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", DataPropertyName = "Username", HeaderText = "Tài khoản", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", DataPropertyName = "Role", HeaderText = "Vai trò", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinkedTo", DataPropertyName = "LinkedTo", HeaderText = "Liên kết", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "StatusText", DataPropertyName = "IsActive", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });

            // Gán các sự kiện cho Grid
            dgvAccounts.RowPrePaint += DgvAccounts_RowPrePaint;
            dgvAccounts.CellFormatting += DgvAccounts_CellFormatting;
            dgvAccounts.ColumnHeaderMouseClick += DgvAccounts_ColumnHeaderMouseClick; // Sự kiện sort
            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged; // Sự kiện chọn dòng
        }

        // Đổi màu chữ của cả dòng dựa trên trạng thái Active/Khóa
        private void DgvAccounts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvAccounts.Rows[e.RowIndex];
            dynamic it = row.DataBoundItem;
            if (it == null) return;
            try
            {
                bool active = (bool)it.IsActive;
                row.DefaultCellStyle.ForeColor = active ? Color.Black : Color.Gray;
            }
            catch (Exception ex) // Sử dụng biến ex
            {
                // Ghi lỗi ra Output window (chỉ khi Debug) để không làm phiền người dùng
                System.Diagnostics.Debug.WriteLine($"Error in RowPrePaint accessing IsActive: {ex.Message}");
            }
        }
        // Format giá trị hiển thị cho các ô cụ thể (đổi boolean -> text, enum -> text)
        private void DgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Xử lý cột trạng thái ("StatusText")
            if (dgvAccounts.Columns[e.ColumnIndex].Name == "StatusText" && e.Value is bool isActive)
            {
                e.Value = isActive ? "Hoạt động" : "Đã khóa";
                e.FormattingApplied = true;
            }
            // Xử lý cột vai trò ("Role")
            else if (dgvAccounts.Columns[e.ColumnIndex].Name == "Role" && e.Value is UserRole role)
            {
                switch (role)
                {
                    case UserRole.Customer: e.Value = "Khách hàng"; break;
                    case UserRole.Driver: e.Value = "Tài xế"; break;
                    // Không cần case Admin vì đã lọc
                    default: e.Value = role.ToString(); break; // Giữ nguyên nếu có vai trò lạ
                }
                e.FormattingApplied = true;
            }
        }
        #endregion

        #region Data Loading and Selection
        private void LoadData(string username = null, string name = null)
        {
            try
            {
                var data = _svc.GetAll(username, name)
                    .Where(a => a.Role != UserRole.Admin) // Lọc Admin
                    .Select(a => new
                    {
                        a.Id,
                        a.Username,
                        a.Role,
                        LinkedTo = a.CustomerId != null ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}"
                                  : a.DriverId != null ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}"
                                  : "(Không liên kết)",
                        a.IsActive
                    })
                    .ToList(); // Lấy dữ liệu trước

                _binding = new BindingList<object>(data.Cast<object>().ToList());
                dgvAccounts.DataSource = _binding; // Gán DataSource

                ApplySort(); // Áp dụng sort (nếu có trạng thái sort cũ)
                UpdateSortGlyphs(); // Cập nhật mũi tên

                // Chọn dòng đầu tiên nếu có dữ liệu và không đang Edit
                if (dgvAccounts.Rows.Count > 0 && _mode == EditMode.None)
                {
                    dgvAccounts.ClearSelection();
                    dgvAccounts.Rows[0].Selected = true;
                    if (dgvAccounts.Columns.Contains("Username"))
                        dgvAccounts.CurrentCell = dgvAccounts.Rows[0].Cells["Username"];
                }
                else if (dgvAccounts.Rows.Count == 0) // Xử lý khi grid trống
                {
                    ClearInputs(); // Xóa các ô nhập liệu
                    UpdateButtonsBasedOnSelection(); // Vô hiệu hóa các nút Sửa/Xóa/Khóa
                }
                // Luôn cập nhật trạng thái nút Toggle sau khi tải xong
                UpdateToggleUi();
            }
            catch (Exception ex) // Sử dụng biến ex
            {
                MessageBox.Show($"Lỗi tải danh sách tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvAccounts.DataSource = null; // Xóa dữ liệu cũ trên grid nếu lỗi
                UpdateButtonsBasedOnSelection(); // Cập nhật nút khi grid trống
            }
            finally
            {
                // Đảm bảo luôn quay về chế độ Xem sau khi tải dữ liệu
                SetMode(EditMode.None);
            }
        }

        // Lấy thông tin cơ bản của dòng đang chọn
        private (int id, string username, bool isActive)? Current()
        {
            if (dgvAccounts.CurrentRow?.DataBoundItem == null) return null;
            dynamic it = dgvAccounts.CurrentRow.DataBoundItem;
            try { return ((int)it.Id, (string)it.Username, (bool)it.IsActive); }
            catch (Exception ex) // Sử dụng biến ex
            {
                System.Diagnostics.Debug.WriteLine($"Error getting current row data: {ex.Message}");
                return null;
            }
        }

        // Tìm và chọn lại dòng theo ID sau khi Reload
        private void SelectRowById(int id)
        {
            if (dgvAccounts.Rows.Count == 0) return;
            foreach (DataGridViewRow row in dgvAccounts.Rows)
            {
                if (row?.DataBoundItem == null) continue;
                dynamic it = row.DataBoundItem;
                try
                {
                    if ((int)it.Id == id)
                    {
                        dgvAccounts.ClearSelection();
                        row.Selected = true;
                        var cell = row.Cells["Username"]; // Focus cột Username
                        if (cell != null && cell.Visible) dgvAccounts.CurrentCell = cell;
                        if (!row.Displayed) // Cuộn đến dòng nếu bị che
                        {
                            int rowIndexToShow = Math.Max(0, row.Index - dgvAccounts.DisplayedRowCount(false) / 2);
                            rowIndexToShow = Math.Min(rowIndexToShow, dgvAccounts.Rows.Count - 1);
                            if (rowIndexToShow >= 0) dgvAccounts.FirstDisplayedScrollingRowIndex = rowIndexToShow;
                        }
                        break; // Đã tìm thấy
                    }
                }
                catch (Exception ex) // Sử dụng biến ex
                {
                    System.Diagnostics.Debug.WriteLine($"Error selecting row by ID {id}: {ex.Message}");
                }
            }
            UpdateToggleUi(); // Cập nhật nút Toggle sau khi chọn xong
        }
        #endregion

        #region UI Event Wiring and Mode Handling
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
            bool canInteract = !isEditing && Current() != null; // Có thể thao tác khi có dòng chọn & không sửa
            btnEdit.Enabled = canInteract; btnDelete.Enabled = canInteract; btnToggle.Enabled = canInteract;
            btnSearch.Enabled = !isEditing; btnReload.Enabled = !isEditing;
        }

        private void ClearInputs() { txtUser.Clear(); txtPass.Clear(); }

        private void UpdateButtonsBasedOnSelection()
        {
            bool hasSelection = Current() != null;
            bool isNotEditing = (_mode == EditMode.None);
            btnEdit.Enabled = hasSelection && isNotEditing;
            btnDelete.Enabled = hasSelection && isNotEditing;
            btnToggle.Enabled = hasSelection && isNotEditing;
            // Cập nhật nút Toggle
            if (hasSelection) { UpdateToggleUi(); }
            else { btnToggle.Text = "Khóa/Mở"; btnToggle.FillColor = Color.Gray; btnToggle.Enabled = false; }
        }

        private void UpdateToggleUi()
        {
            var cur = Current();
            bool isNotEditing = (_mode == EditMode.None);
            if (cur == null) { btnToggle.Enabled = false; btnToggle.Text = "Khóa/Mở"; btnToggle.FillColor = Color.Gray; return; }
            btnToggle.Enabled = isNotEditing; // Chỉ bật khi không sửa
            btnToggle.Text = cur.Value.isActive ? "Khóa" : "Mở khóa";
            btnToggle.FillColor = cur.Value.isActive ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69);
        }
        #endregion

        #region Button Click Handlers
        private void BtnReload_Click(object sender, EventArgs e) { LoadData(); } // LoadData đã xử lý các việc khác

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var cur = Current();
            if (cur != null)
            {
                SetMode(EditMode.Edit);
                txtUser.Text = cur.Value.username;
                txtPass.Clear();
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
                    if (ask != DialogResult.Yes) return;
                }
            }
            ClearInputs();
            SetMode(EditMode.None);
            var curId = Current()?.id; // Chọn lại dòng đang focus
            if (curId.HasValue) SelectRowById(curId.Value);
        }

        private void BtnDelete_Click(object sender, EventArgs e) { DeleteAccount(); }

        private void BtnToggle_Click(object sender, EventArgs e) { ToggleActive(); }

        private void BtnSearch_Click(object sender, EventArgs e) { OpenSearch(); }

        private void DgvAccounts_SelectionChanged(object sender, EventArgs e)
        {
            // Chỉ cập nhật nút nếu không đang ở chế độ Edit
            if (_mode == EditMode.None)
            {
                UpdateButtonsBasedOnSelection();
            }
        }
        #endregion

        #region Core Logic Methods (Save, Delete, Toggle, Search)
        private void SaveEdit()
        {
            var cur = Current(); if (cur == null || _mode != EditMode.Edit) return;
            string newUsername = txtUser.Text.Trim(); string newPassword = txtPass.Text;
            bool usernameChanged = newUsername != cur.Value.username; bool passwordEntered = !string.IsNullOrWhiteSpace(newPassword);

            if (!usernameChanged && !passwordEntered) { MessageBox.Show("Không có thay đổi."); SetMode(EditMode.None); return; }
            if (usernameChanged && string.IsNullOrWhiteSpace(newUsername)) { MessageBox.Show("Tên tài khoản trống."); txtUser.Focus(); return; }
            if (passwordEntered && newPassword.Length < 6) { MessageBox.Show("Mật khẩu mới >= 6 ký tự."); txtPass.Focus(); return; }

            try
            {
                var confirm = MessageBox.Show("Lưu thay đổi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;
                _svc.UpdateBasic(cur.Value.id, newUsername, passwordEntered ? newPassword : null);
                MessageBox.Show("Đã cập nhật.", "Thành công");
                int keepId = cur.Value.id; LoadData(); SelectRowById(keepId); // LoadData tự về Mode None
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi cập nhật: {ex.Message}", "Lỗi"); } // Giữ Edit Mode khi lỗi
        }

        private void DeleteAccount()
        {
            var cur = Current(); if (cur == null || _mode != EditMode.None) return;
            try
            {
                var rpt = _svc.InspectUsage(cur.Value.id); string msg; DialogResult confirm = DialogResult.No;

                if (rpt.Role == UserRole.Customer)
                {
                    if (rpt.OrdersActive > 0) { MessageBox.Show($"KH có {rpt.OrdersActive} đơn đang xử lý. Không thể xóa.", "Không thể xóa"); return; }
                    if (rpt.OrdersPending > 0) msg = $"KH có {rpt.OrdersPending} đơn chờ duyệt.\nXóa TK sẽ khiến họ không đăng nhập được.\nTiếp tục xóa?";
                    else if (rpt.OrdersCompleted > 0 || rpt.OrdersCancelled > 0) msg = $"KH có lịch sử đơn hàng.\nXóa tài khoản đăng nhập này?";
                    else msg = "Xóa tài khoản đăng nhập này?";
                    confirm = MessageBox.Show(msg, "Xác nhận xóa TK Khách hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else if (rpt.Role == UserRole.Driver)
                {
                    if (rpt.ShipActive > 0) { MessageBox.Show($"TX có {rpt.ShipActive} chuyến đang chạy. Không thể xóa.", "Không thể xóa"); return; }
                    if (rpt.ShipPending > 0) msg = $"TX có {rpt.ShipPending} chuyến chờ nhận.\nXóa TK sẽ khiến họ không đăng nhập được.\nTiếp tục xóa?";
                    else if (rpt.ShipCompleted > 0 || rpt.ShipCancelled > 0) msg = $"TX có lịch sử chuyến đi.\nXóa tài khoản đăng nhập này?";
                    else msg = "Xóa tài khoản đăng nhập này?";
                    confirm = MessageBox.Show(msg, "Xác nhận xóa TK Tài xế", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else { MessageBox.Show("Không thể xóa Admin."); return; }

                if (confirm == DialogResult.Yes)
                {
                    _svc.DeleteOnlyAccount(cur.Value.id);
                    MessageBox.Show("Đã xóa tài khoản.", "Thành công");
                    LoadData(); // Load lại
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi"); }
        }

        private void ToggleActive()
        {
            var cur = Current(); if (cur == null || _mode != EditMode.None) return;
            bool newState = !cur.Value.isActive; string actionText = newState ? "Mở khóa" : "Khóa";
            try
            {
                var confirm = MessageBox.Show($"Bạn chắc muốn {actionText} tài khoản '{cur.Value.username}'?", $"Xác nhận {actionText}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _svc.SetActive(cur.Value.id, newState);
                    int keepId = cur.Value.id; LoadData(); SelectRowById(keepId); // LoadData về Mode None, SelectRowById cập nhật nút
                    MessageBox.Show($"Đã {actionText} tài khoản.", "Thành công");
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi khi {actionText}: {ex.Message}", "Lỗi"); }
        }

        private void OpenSearch()
        {
            if (_mode != EditMode.None) return; int? selectedId = null;
            using (var searchForm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(1199, 725),
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
                if (searchForm.ShowDialog(this.FindForm()) == DialogResult.OK && selectedId.HasValue) { SelectRowById(selectedId.Value); }
            }
        }
        #endregion

        #region Sorting Logic
        private void DgvAccounts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_binding == null || _binding.Count == 0) return;
            var newColumn = dgvAccounts.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }

            ApplySort();
            UpdateSortGlyphs();
        }
        private void ApplySort()
        {
            if (_sortedColumn == null || _binding == null || _binding.Count == 0) return; // Thêm kiểm tra Count
            string propertyName = _sortedColumn.DataPropertyName; PropertyInfo propInfo = null;
            // Lấy kiểu từ item đầu tiên hợp lệ
            var firstItem = _binding.FirstOrDefault(item => item != null);
            if (firstItem != null) propInfo = firstItem.GetType().GetProperty(propertyName);

            if (propInfo == null) return; // Không tìm thấy prop -> không sort

            List<object> items = _binding.ToList(); List<object> sortedList;
            try
            {
                if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
                _binding = new BindingList<object>(sortedList); dgvAccounts.DataSource = _binding;
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp cột '{_sortedColumn.HeaderText}': {ex.Message}"); ResetSortGlyphs(); } // Báo lỗi sort
        }
        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvAccounts.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_sortedColumn != null && col == _sortedColumn) { col.HeaderCell.SortGlyphDirection = _sortOrder; }
            }
        }
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null; _sortOrder = SortOrder.None; UpdateSortGlyphs(); // Gọi Update để reset UI
        }
        #endregion
    }
}