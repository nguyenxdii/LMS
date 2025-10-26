//using Guna.UI2.WinForms;
//using LMS.BUS.Helpers;
//using LMS.BUS.Services; // Đảm bảo using này đúng
//using LMS.DAL.Models;   // Đảm bảo using này đúng
//using System;
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;

//namespace LMS.GUI.AccountAdmin // Đảm bảo namespace này đúng
//{
//    public partial class ucAccount_Admin : UserControl
//    {
//        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
//        private BindingList<object> _binding; // Dùng object vì select anonymous type

//        private enum EditMode { None, Edit }
//        private EditMode _mode = EditMode.None;

//        public ucAccount_Admin()
//        {
//            InitializeComponent();
//            // Gán sự kiện Load để cấu hình và tải dữ liệu khi UC được hiển thị
//            this.Load += UcAccount_Admin_Load;
//        }

//        private void UcAccount_Admin_Load(object sender, EventArgs e)
//        {
//            ConfigureGrid(); // Cấu hình grid trước
//            LoadData();      // Tải dữ liệu ban đầu
//            Wire();          // Gán sự kiện cho các nút
//            SetMode(EditMode.None); // Đặt trạng thái giao diện ban đầu
//        }

//        #region Grid Configuration and Formatting
//        private void ConfigureGrid()
//        {
//            dgvAccounts.Columns.Clear();
//            //dgvAccounts.AutoGenerateColumns = false; // Rất quan trọng khi định nghĩa cột thủ công
//            //dgvAccounts.AllowUserToAddRows = false;
//            //dgvAccounts.ReadOnly = true;
//            //dgvAccounts.RowHeadersVisible = false;
//            //dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            //dgvAccounts.MultiSelect = false;

//            //// Áp dụng Style (giống GridHelper nhưng làm trực tiếp)
//            //dgvAccounts.EnableHeadersVisualStyles = false; // Cho phép tùy chỉnh header
//            //dgvAccounts.GridColor = Color.Gainsboro;
//            //dgvAccounts.CellBorderStyle = DataGridViewCellBorderStyle.Single;
//            //dgvAccounts.BorderStyle = BorderStyle.FixedSingle;
//            //dgvAccounts.ColumnHeadersHeight = 36;
//            //dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185); // Màu nền header
//            //dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Màu chữ header
//            //dgvAccounts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold); // Font header
//            //dgvAccounts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Canh giữa header

//            //// Style cho các dòng dữ liệu (cell)
//            //dgvAccounts.DefaultCellStyle.Font = new Font("Segoe UI", 10); // Font chữ trong cell
//            //dgvAccounts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 223, 226); // Màu nền khi chọn dòng
//            //dgvAccounts.DefaultCellStyle.SelectionForeColor = Color.Black; // Màu chữ khi chọn dòng
//            //dgvAccounts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255); // Màu dòng xen kẽ
//            //dgvAccounts.RowTemplate.Height = 40; // Chiều cao mỗi dòng

//            dgvAccounts.ApplyBaseStyle();

//            // --- Định nghĩa các cột ---
//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Id",
//                DataPropertyName = "Id", // Phải khớp tên thuộc tính trong anonymous type trả về từ LoadData
//                Visible = false // Ẩn cột ID
//            });
//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Username",
//                DataPropertyName = "Username",
//                HeaderText = "Tài khoản",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Cho cột này co giãn lấp đầy
//            });
//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Role",
//                DataPropertyName = "Role", // Lấy giá trị Enum UserRole
//                HeaderText = "Vai trò",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells // Kích thước tự động theo nội dung
//            });
//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "LinkedTo",
//                DataPropertyName = "LinkedTo", // Lấy chuỗi đã tạo trong LoadData
//                HeaderText = "Liên kết",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Cho cột này co giãn lấp đầy
//            });

//            // --- Cột trạng thái bằng chữ (ĐÃ SỬA) ---
//            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "StatusText",          // Đặt tên riêng cho cột hiển thị
//                DataPropertyName = "IsActive", // Vẫn lấy dữ liệu gốc từ thuộc tính boolean IsActive
//                HeaderText = "Trạng thái",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, // Kích thước tự động
//                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } // Canh giữa chữ trong cột này
//            });
//            // --- Kết thúc sửa cột trạng thái ---

//            // Gán sự kiện để xử lý giao diện (đổi màu chữ) và format dữ liệu (đổi bool->text, enum->text)
//            dgvAccounts.RowPrePaint += DgvAccounts_RowPrePaint;
//            dgvAccounts.CellFormatting += DgvAccounts_CellFormatting;

//            // Bật Double Buffer để grid mượt hơn (nên có)
//            try
//            {
//                var dgvType = dgvAccounts.GetType();
//                var prop = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
//                if (prop != null) prop.SetValue(dgvAccounts, true, null);
//            }
//            catch { /* Bỏ qua nếu lỗi */ }
//        }

//        // Đổi màu chữ của cả dòng dựa trên trạng thái Active/Khóa
//        private void DgvAccounts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
//        {
//            if (e.RowIndex < 0) return; // Bỏ qua header row
//            var row = dgvAccounts.Rows[e.RowIndex];
//            dynamic it = row.DataBoundItem; // Lấy đối tượng dữ liệu (anonymous type) của dòng
//            if (it == null) return; // Bỏ qua nếu chưa có dữ liệu

//            try // Dùng try-catch khi truy cập dynamic type để tránh lỗi runtime
//            {
//                bool active = (bool)it.IsActive; // Lấy trạng thái boolean gốc
//                // Đặt màu chữ cho cả dòng
//                row.DefaultCellStyle.ForeColor = active ? Color.Black : Color.Gray;
//                // Tùy chọn: Thêm style khác cho dòng bị khóa, ví dụ in nghiêng
//                // if (!active) row.DefaultCellStyle.Font = new Font(dgvAccounts.Font, FontStyle.Italic);
//            }
//            catch (Exception ex)
//            {
//                // Ghi lỗi ra Output window để debug nếu có vấn đề ép kiểu
//                System.Diagnostics.Debug.WriteLine($"Error in RowPrePaint accessing IsActive: {ex.Message}");
//            }
//        }

//        // Format giá trị hiển thị cho các ô cụ thể (đổi boolean -> text, enum -> text)
//        private void DgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            // Xử lý cột trạng thái ("StatusText")
//            if (dgvAccounts.Columns[e.ColumnIndex].Name == "StatusText" && e.Value != null)
//            {
//                // Kiểm tra giá trị gốc có phải boolean không
//                if (e.Value is bool isActive)
//                {
//                    // Thay đổi giá trị hiển thị thành chuỗi Tiếng Việt
//                    e.Value = isActive ? "Hoạt động" : "Đã khóa";
//                    e.FormattingApplied = true; // Báo cho grid biết đã xử lý xong
//                }
//            }
//            // Xử lý cột vai trò ("Role")
//            else if (dgvAccounts.Columns[e.ColumnIndex].Name == "Role" && e.Value is UserRole role)
//            {
//                // Thay đổi giá trị Enum thành chuỗi Tiếng Việt
//                switch (role)
//                {
//                    case UserRole.Admin: e.Value = "Quản trị"; break; // Dù đã lọc nhưng để cho đủ
//                    case UserRole.Customer: e.Value = "Khách hàng"; break;
//                    case UserRole.Driver: e.Value = "Tài xế"; break;
//                    default: e.Value = role.ToString(); break; // Giữ nguyên nếu có vai trò lạ
//                }
//                e.FormattingApplied = true; // Báo cho grid biết đã xử lý xong
//            }
//        }
//        #endregion

//        #region Data Loading and Selection
//        // Tải hoặc tải lại dữ liệu vào grid
//        private void LoadData(string username = null, string name = null)
//        {
//            try
//            {
//                // Gọi service để lấy danh sách tài khoản, lọc bỏ Admin
//                var data = _svc.GetAll(username, name)
//                    .Where(a => a.Role != UserRole.Admin)
//                    .Select(a => new // Tạo anonymous type chỉ chứa các thông tin cần hiển thị
//                    {
//                        a.Id,
//                        a.Username,
//                        a.Role, // Giữ kiểu Enum để format sau
//                        // Tạo chuỗi mô tả liên kết
//                        LinkedTo = a.CustomerId != null ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}"
//                                  : a.DriverId != null ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}"
//                                  : "(Không liên kết)",
//                        a.IsActive // Giữ kiểu boolean gốc để format và đổi màu chữ
//                    })
//                    .OrderBy(a => a.Role).ThenBy(a => a.Username) // Sắp xếp theo Vai trò rồi Tên TK
//                    .ToList();

//                // Tạo BindingList (DataSource linh động cho grid)
//                _binding = new BindingList<object>(data.Cast<object>().ToList());
//                dgvAccounts.DataSource = _binding; // Gán DataSource cho grid

//                // Chọn dòng đầu tiên nếu có dữ liệu
//                if (dgvAccounts.Rows.Count > 0)
//                {
//                    dgvAccounts.ClearSelection();
//                    dgvAccounts.Rows[0].Selected = true;
//                    // Đặt ô hiện tại (ví dụ: cột Username) để người dùng thấy focus
//                    if (dgvAccounts.Columns.Contains("Username"))
//                        dgvAccounts.CurrentCell = dgvAccounts.Rows[0].Cells["Username"];
//                }
//                else
//                {
//                    // Nếu grid trống, xóa text ở ô input và cập nhật nút
//                    ClearInputs();
//                    UpdateButtonsBasedOnSelection();
//                }

//                // Cập nhật lại nút Khóa/Mở (quan trọng sau khi load)
//                UpdateToggleUi();
//            }
//            catch (Exception ex) // Bắt lỗi nếu service hoặc DB có vấn đề
//            {
//                MessageBox.Show($"Lỗi tải danh sách tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvAccounts.DataSource = null; // Xóa dữ liệu cũ trên grid
//                UpdateButtonsBasedOnSelection(); // Cập nhật nút khi grid trống
//            }
//            finally // Khối này luôn chạy dù có lỗi hay không
//            {
//                // Đảm bảo luôn quay về chế độ xem (None) sau khi tải dữ liệu
//                SetMode(EditMode.None);
//            }
//        }

//        // Lấy thông tin cơ bản (Id, Username, IsActive) của dòng đang được chọn
//        private (int id, string username, bool isActive)? Current()
//        {
//            // Kiểm tra xem có dòng nào đang được chọn và dòng đó có dữ liệu không
//            if (dgvAccounts.CurrentRow == null || dgvAccounts.CurrentRow.DataBoundItem == null)
//                return null; // Không có dòng nào -> trả về null

//            // Lấy đối tượng dữ liệu của dòng (là anonymous type)
//            dynamic it = dgvAccounts.CurrentRow.DataBoundItem;
//            try
//            {
//                // Trả về tuple chứa thông tin cần thiết
//                return ((int)it.Id, (string)it.Username, (bool)it.IsActive);
//            }
//            catch (Exception ex) // Bắt lỗi nếu ép kiểu thất bại (ít xảy ra)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error getting current row data: {ex.Message}");
//                return null;
//            }
//        }

//        // Tìm và chọn lại dòng trên grid dựa vào ID (thường dùng sau khi sửa/khóa/mở)
//        private void SelectRowById(int id)
//        {
//            if (dgvAccounts.Rows.Count == 0) return; // Không tìm nếu grid trống

//            foreach (DataGridViewRow row in dgvAccounts.Rows)
//            {
//                if (row?.DataBoundItem == null) continue; // Bỏ qua dòng trống
//                dynamic it = row.DataBoundItem;
//                try
//                {
//                    if ((int)it.Id == id) // Nếu tìm thấy đúng ID
//                    {
//                        dgvAccounts.ClearSelection(); // Bỏ chọn các dòng khác
//                        row.Selected = true; // Chọn dòng này
//                        // Đặt ô hiện tại (CurrentCell) để focus và selection được cập nhật đúng
//                        var cell = row.Cells["Username"]; // Chọn cột Username làm ô hiện tại
//                        if (cell != null && cell.Visible) dgvAccounts.CurrentCell = cell;

//                        // Tự động cuộn đến dòng nếu nó đang bị che khuất
//                        if (!row.Displayed)
//                        {
//                            // Tính toán vị trí cuộn hợp lý (gần giữa màn hình)
//                            int rowIndexToShow = Math.Max(0, row.Index - dgvAccounts.DisplayedRowCount(false) / 2);
//                            rowIndexToShow = Math.Min(rowIndexToShow, dgvAccounts.Rows.Count - 1); // Đảm bảo không vượt quá index cuối
//                            if (rowIndexToShow >= 0) // Kiểm tra lần nữa cho chắc
//                                dgvAccounts.FirstDisplayedScrollingRowIndex = rowIndexToShow; // Cuộn đến vị trí đó
//                        }
//                        break; // Đã tìm thấy, thoát vòng lặp
//                    }
//                }
//                catch (Exception ex) // Bắt lỗi nếu ép kiểu 'it.Id' thất bại
//                {
//                    System.Diagnostics.Debug.WriteLine($"Error selecting row by ID {id}: {ex.Message}");
//                    /* Có thể ghi log lỗi ở đây */
//                }
//            }
//            // Cập nhật lại giao diện nút Khóa/Mở sau khi đã chọn xong dòng
//            UpdateToggleUi();
//        }
//        #endregion

//        #region UI Event Wiring and Mode Handling
//        // Gán sự kiện cho các nút và grid
//        private void Wire()
//        {
//            // Gán sự kiện Click cho các nút trên Toolbar
//            btnReload.Click += BtnReload_Click;
//            btnEdit.Click += BtnEdit_Click;
//            btnSave.Click += BtnSave_Click;
//            btnCancel.Click += BtnCancel_Click;
//            btnDelete.Click += BtnDelete_Click;
//            btnToggle.Click += BtnToggle_Click;
//            btnSearch.Click += BtnSearch_Click;

//            // Gán sự kiện khi lựa chọn dòng trên grid thay đổi
//            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged;

//            // (Tùy chọn) Gán sự kiện TextChanged nếu muốn lọc/tìm kiếm trực tiếp khi gõ
//            // txtUser.TextChanged += (s, e) => { /* Logic lọc nếu cần */ };
//        }

//        // Chuyển đổi trạng thái giao diện giữa chế độ Xem (None) và Sửa (Edit)
//        private void SetMode(EditMode m)
//        {
//            _mode = m; // Lưu lại trạng thái hiện tại
//            bool isEditing = (m == EditMode.Edit); // Kiểm tra có phải đang sửa không

//            // Bật/tắt các ô nhập liệu Username và Password
//            txtUser.Enabled = isEditing;
//            txtPass.Enabled = isEditing;

//            // Bật/tắt các nút Lưu và Hủy
//            btnSave.Enabled = isEditing;
//            btnCancel.Enabled = isEditing;

//            // Các nút còn lại và grid chỉ được bật khi KHÔNG sửa
//            dgvAccounts.Enabled = !isEditing;
//            bool canInteractWithGrid = !isEditing && Current() != null; // Có dòng chọn và không sửa
//            btnEdit.Enabled = canInteractWithGrid;
//            btnDelete.Enabled = canInteractWithGrid;
//            btnToggle.Enabled = canInteractWithGrid;
//            btnSearch.Enabled = !isEditing;
//            btnReload.Enabled = !isEditing;

//            // (Tùy chọn) Xóa các dấu lỗi validation nếu có khi chuyển mode
//            // errProvider?.Clear();
//        }

//        // Xóa nội dung trong các ô nhập liệu
//        private void ClearInputs()
//        {
//            txtUser.Clear();
//            txtPass.Clear(); // Luôn xóa password để tránh lộ hoặc lưu nhầm
//            // errProvider?.Clear(); // Nếu có ErrorProvider
//        }

//        // Cập nhật trạng thái Enabled của các nút dựa trên việc có dòng nào được chọn hay không
//        private void UpdateButtonsBasedOnSelection()
//        {
//            bool hasSelection = Current() != null; // Kiểm tra có dòng nào đang được chọn
//            bool isNotEditing = (_mode == EditMode.None); // Kiểm tra không ở chế độ sửa

//            // Chỉ bật các nút thao tác (Sửa, Xóa, Khóa/Mở) khi có dòng được chọn VÀ không đang sửa
//            btnEdit.Enabled = hasSelection && isNotEditing;
//            btnDelete.Enabled = hasSelection && isNotEditing;
//            btnToggle.Enabled = hasSelection && isNotEditing;

//            // Cập nhật giao diện nút Toggle (chữ và màu) nếu có dòng được chọn
//            if (hasSelection)
//            {
//                UpdateToggleUi();
//            }
//            else
//            {
//                // Nếu không có dòng nào được chọn, reset nút Toggle về trạng thái mặc định/vô hiệu hóa
//                btnToggle.Text = "Khóa/Mở";
//                btnToggle.FillColor = Color.Gray; // Màu xám khi bị vô hiệu hóa
//            }
//        }


//        // Cập nhật Text và Màu của nút Khóa/Mở dựa trên trạng thái của dòng đang chọn
//        private void UpdateToggleUi()
//        {
//            var cur = Current(); // Lấy thông tin dòng hiện tại
//            bool isNotEditing = (_mode == EditMode.None); // Chỉ cập nhật nếu không đang sửa

//            // Nếu không có dòng nào được chọn
//            if (cur == null)
//            {
//                btnToggle.Enabled = false; // Vô hiệu hóa nút
//                btnToggle.Text = "Khóa/Mở"; // Text mặc định
//                btnToggle.FillColor = Color.Gray; // Màu mặc định
//                return;
//            }

//            // Bật nút nếu có dòng chọn và không đang sửa
//            btnToggle.Enabled = isNotEditing;

//            // Cập nhật Text và Màu
//            btnToggle.Text = cur.Value.isActive ? "Khóa" : "Mở khóa"; // Đổi chữ
//            btnToggle.FillColor = cur.Value.isActive
//                ? Color.FromArgb(220, 53, 69)  // Màu đỏ (cho hành động "Khóa")
//                : Color.FromArgb(40, 167, 69); // Màu xanh (cho hành động "Mở khóa")
//        }
//        #endregion

//        #region Button Click Handlers (Các hàm xử lý khi nhấn nút)
//        // Nút Tải lại
//        private void BtnReload_Click(object sender, EventArgs e)
//        {
//            LoadData(); // LoadData đã gọi ClearInputs và SetMode(None)
//        }

//        // Nút Sửa
//        private void BtnEdit_Click(object sender, EventArgs e)
//        {
//            var cur = Current();
//            if (cur != null) // Chỉ thực hiện nếu có dòng được chọn
//            {
//                SetMode(EditMode.Edit); // Chuyển sang chế độ Sửa
//                txtUser.Text = cur.Value.username; // Điền username hiện tại vào ô
//                txtPass.Clear(); // Xóa ô password (chỉ dùng để đặt lại)
//                txtUser.Focus(); // Đặt con trỏ vào ô username
//                txtUser.SelectAll(); // Chọn hết text để người dùng dễ dàng gõ đè lên
//            }
//        }

//        // Nút Lưu
//        private void BtnSave_Click(object sender, EventArgs e)
//        {
//            SaveEdit(); // Gọi hàm xử lý lưu
//        }

//        // Nút Hủy (trong chế độ Sửa)
//        private void BtnCancel_Click(object sender, EventArgs e)
//        {
//            // Chỉ kiểm tra thay đổi nếu đang ở chế độ Sửa
//            if (_mode == EditMode.Edit)
//            {
//                // Kiểm tra xem username có khác hoặc password có được nhập mới không
//                bool usernameChanged = (txtUser.Text.Trim() != (Current()?.username ?? string.Empty));
//                bool passwordEntered = !string.IsNullOrWhiteSpace(txtPass.Text);

//                // Nếu có thay đổi -> hỏi xác nhận
//                if (usernameChanged || passwordEntered)
//                {
//                    var ask = MessageBox.Show("Bạn có thay đổi chưa lưu. Hủy bỏ các thay đổi này?", "Xác nhận hủy",
//                                              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                    if (ask != DialogResult.Yes) return; // Nếu chọn No -> không làm gì cả
//                }
//            }
//            // Nếu không có thay đổi HOẶC người dùng chọn Yes để hủy thay đổi
//            ClearInputs(); // Xóa ô nhập liệu
//            SetMode(EditMode.None); // Quay về chế độ Xem
//                                    // Tùy chọn: Chọn lại dòng đang được focus trước đó (nếu muốn)
//            var curId = Current()?.id;
//            if (curId.HasValue) SelectRowById(curId.Value);
//        }

//        // Nút Xóa
//        private void BtnDelete_Click(object sender, EventArgs e)
//        {
//            DeleteAccount(); // Gọi hàm xử lý xóa
//        }

//        // Nút Khóa/Mở khóa
//        private void BtnToggle_Click(object sender, EventArgs e)
//        {
//            ToggleActive(); // Gọi hàm xử lý khóa/mở
//        }

//        // Nút Tìm kiếm
//        private void BtnSearch_Click(object sender, EventArgs e)
//        {
//            OpenSearch(); // Gọi hàm mở form tìm kiếm
//        }

//        // Sự kiện khi chọn dòng khác trên grid
//        private void DgvAccounts_SelectionChanged(object sender, EventArgs e)
//        {
//            // Chỉ cập nhật trạng thái các nút nếu đang ở chế độ Xem
//            if (_mode == EditMode.None)
//            {
//                UpdateButtonsBasedOnSelection(); // Cập nhật Enabled/Disabled và nút Toggle
//            }
//        }
//        #endregion

//        #region Core Logic Methods (Save, Delete, Toggle, Search) - Logic nghiệp vụ chính

//        // Lưu thay đổi khi Sửa tài khoản
//        private void SaveEdit()
//        {
//            var cur = Current();
//            // Chỉ thực hiện nếu đang ở chế độ Sửa và có dòng được chọn
//            if (cur == null || _mode != EditMode.Edit) return;

//            string newUsername = txtUser.Text.Trim();
//            string newPassword = txtPass.Text; // Lấy cả chuỗi password, không Trim

//            // Kiểm tra xem có gì thay đổi không
//            bool usernameChanged = newUsername != cur.Value.username;
//            bool passwordEntered = !string.IsNullOrWhiteSpace(newPassword);

//            // Nếu không có gì thay đổi -> thông báo và quay về chế độ Xem
//            if (!usernameChanged && !passwordEntered)
//            {
//                MessageBox.Show("Không có thay đổi nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                SetMode(EditMode.None);
//                return;
//            }

//            // --- Validation cơ bản ---
//            if (usernameChanged && string.IsNullOrWhiteSpace(newUsername))
//            {
//                MessageBox.Show("Tên tài khoản không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                txtUser.Focus(); // Đặt focus lại vào ô lỗi
//                return; // Không tiếp tục lưu
//            }
//            if (passwordEntered && newPassword.Length < 6) // Ví dụ: Mật khẩu tối thiểu 6 ký tự
//            {
//                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                txtPass.Focus(); // Đặt focus lại vào ô lỗi
//                return; // Không tiếp tục lưu
//            }
//            // --- Kết thúc Validation ---

//            try
//            {
//                // Hỏi xác nhận trước khi lưu
//                var confirm = MessageBox.Show("Lưu thay đổi cho tài khoản này?", "Xác nhận lưu",
//                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                if (confirm != DialogResult.Yes) return; // Không lưu nếu chọn No

//                // Gọi service để cập nhật CSDL
//                // Truyền password mới nếu người dùng đã nhập, ngược lại truyền null
//                _svc.UpdateBasic(cur.Value.id, newUsername, passwordEntered ? newPassword : null);

//                MessageBox.Show("Đã cập nhật tài khoản thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

//                int keepId = cur.Value.id; // Lưu lại ID để chọn lại sau khi reload
//                LoadData();                // Tải lại toàn bộ grid
//                SelectRowById(keepId);     // Chọn lại đúng dòng vừa sửa
//                // ClearInputs và SetMode(None) đã được gọi trong LoadData -> finally
//            }
//            catch (Exception ex) // Bắt lỗi từ service (ví dụ: username đã tồn tại)
//            {
//                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                // Không thoát khỏi chế độ Edit khi lỗi, để người dùng sửa lại
//            }
//        }

//        // Xóa tài khoản (chỉ xóa UserAccount, không xóa Customer/Driver)
//        private void DeleteAccount()
//        {
//            var cur = Current();
//            // Chỉ cho phép xóa khi đang ở chế độ Xem và có dòng được chọn
//            if (cur == null || _mode != EditMode.None) return;

//            try
//            {
//                // Gọi service để kiểm tra xem tài khoản có đang dính dáng đến nghiệp vụ không
//                var rpt = _svc.InspectUsage(cur.Value.id);
//                string msg; // Chuỗi thông báo xác nhận
//                DialogResult confirm = DialogResult.No; // Kết quả xác nhận của người dùng

//                // Xây dựng thông điệp dựa trên vai trò và trạng thái nghiệp vụ
//                if (rpt.Role == UserRole.Customer)
//                {
//                    // Nếu có đơn hàng đang hoạt động -> KHÔNG CHO XÓA
//                    if (rpt.OrdersActive > 0)
//                    {
//                        MessageBox.Show($"Khách hàng này đang có {rpt.OrdersActive} đơn hàng đang được xử lý.\nKhông thể xóa tài khoản.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                        return; // Dừng lại
//                    }
//                    // Nếu có đơn chờ duyệt -> Cảnh báo
//                    if (rpt.OrdersPending > 0)
//                        msg = $"Khách hàng có {rpt.OrdersPending} đơn hàng đang chờ duyệt.\nViệc xóa tài khoản sẽ khiến họ không thể đăng nhập để theo dõi.\n\nBạn có chắc muốn xóa tài khoản đăng nhập này?";
//                    // Nếu chỉ có đơn cũ -> Thông báo khác
//                    else if (rpt.OrdersCompleted > 0 || rpt.OrdersCancelled > 0)
//                        msg = $"Khách hàng này có lịch sử đơn hàng ({rpt.OrdersCompleted} hoàn thành, {rpt.OrdersCancelled} đã hủy).\n\nBạn có chắc muốn xóa tài khoản đăng nhập này? (Thông tin khách hàng và lịch sử đơn hàng sẽ được giữ lại)";
//                    // Nếu chưa có đơn nào -> Xác nhận xóa bình thường
//                    else
//                        msg = "Xóa vĩnh viễn tài khoản đăng nhập này? (Thông tin khách hàng liên kết sẽ được giữ lại)";

//                    confirm = MessageBox.Show(msg, "Xác nhận xóa tài khoản Khách hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                }
//                else if (rpt.Role == UserRole.Driver)
//                {
//                    // Nếu có chuyến đang chạy -> KHÔNG CHO XÓA
//                    if (rpt.ShipActive > 0)
//                    {
//                        MessageBox.Show($"Tài xế này đang có {rpt.ShipActive} chuyến hàng đang thực hiện.\nKhông thể xóa tài khoản.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                        return; // Dừng lại
//                    }
//                    // Nếu có chuyến đang chờ -> Cảnh báo
//                    if (rpt.ShipPending > 0)
//                        msg = $"Tài xế có {rpt.ShipPending} chuyến hàng đang chờ nhận hoặc được phân công.\nViệc xóa tài khoản sẽ khiến họ không thể đăng nhập để xử lý.\n\nBạn có chắc muốn xóa tài khoản đăng nhập này?";
//                    // Nếu chỉ có chuyến cũ -> Thông báo khác
//                    else if (rpt.ShipCompleted > 0 || rpt.ShipCancelled > 0)
//                        msg = $"Tài xế này có lịch sử chuyến đi ({rpt.ShipCompleted} hoàn thành, {rpt.ShipCancelled} hủy/lỗi).\n\nBạn có chắc muốn xóa tài khoản đăng nhập này? (Thông tin tài xế và lịch sử chuyến đi sẽ được giữ lại)";
//                    // Nếu chưa có chuyến nào -> Xác nhận xóa bình thường
//                    else
//                        msg = "Xóa vĩnh viễn tài khoản đăng nhập này? (Thông tin tài xế liên kết sẽ được giữ lại)";

//                    confirm = MessageBox.Show(msg, "Xác nhận xóa tài khoản Tài xế", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                }
//                else // Không cho xóa Admin qua giao diện này
//                {
//                    MessageBox.Show("Không thể xóa tài khoản Quản trị viên từ giao diện này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    return;
//                }

//                // Chỉ thực hiện xóa nếu người dùng đã xác nhận (Yes)
//                if (confirm == DialogResult.Yes)
//                {
//                    _svc.DeleteOnlyAccount(cur.Value.id); // Gọi service chỉ xóa UserAccount
//                    MessageBox.Show("Đã xóa tài khoản đăng nhập thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    LoadData(); // Tải lại danh sách mới (LoadData tự gọi SetMode(None))
//                }
//            }
//            catch (Exception ex) // Bắt lỗi nếu service có vấn đề
//            {
//                MessageBox.Show($"Lỗi khi xóa tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Khóa hoặc Mở khóa tài khoản
//        private void ToggleActive()
//        {
//            var cur = Current();
//            // Chỉ thực hiện khi đang ở chế độ Xem và có dòng được chọn
//            if (cur == null || _mode != EditMode.None) return;

//            bool newState = !cur.Value.isActive; // Tính trạng thái mới (ngược lại hiện tại)
//            string actionText = newState ? "Mở khóa" : "Khóa"; // Chuỗi hành động để hiển thị

//            try
//            {
//                // Hỏi xác nhận hành động
//                var confirm = MessageBox.Show($"Bạn có chắc muốn {actionText} tài khoản '{cur.Value.username}'?", $"Xác nhận {actionText}",
//                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//                if (confirm == DialogResult.Yes) // Nếu người dùng đồng ý
//                {
//                    _svc.SetActive(cur.Value.id, newState); // Gọi service để cập nhật CSDL

//                    // Lưu lại ID để chọn lại sau khi tải
//                    int keepId = cur.Value.id;
//                    LoadData(); // Tải lại dữ liệu (bao gồm cả việc gọi SetMode(None))
//                    SelectRowById(keepId); // Tìm và chọn lại dòng vừa thay đổi (hàm này cũng gọi UpdateToggleUi)

//                    MessageBox.Show($"Đã {actionText} tài khoản thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
//            }
//            catch (Exception ex) // Bắt lỗi nếu service có vấn đề
//            {
//                MessageBox.Show($"Lỗi khi {actionText} tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Mở Form tìm kiếm tài khoản
//        private void OpenSearch()
//        {
//            // Không cho tìm kiếm nếu đang ở chế độ Sửa
//            if (_mode != EditMode.None) return;

//            int? selectedId = null; // Biến lưu ID tài khoản được chọn từ form tìm kiếm
//            // Tạo form tìm kiếm động
//            using (var searchForm = new Form
//            {
//                StartPosition = FormStartPosition.CenterScreen, // Canh giữa form cha (frmMain_Admin)
//                FormBorderStyle = FormBorderStyle.None, // Kiểu cửa sổ dialog chuẩn
//                Size = new Size(1199, 725) // Đặt kích thước phù hợp
//            })
//            {
//                // Tạo UserControl tìm kiếm
//                var ucSearch = new ucAccountSearch_Admin { Dock = DockStyle.Fill }; // Cho UC lấp đầy form

//                // Đăng ký sự kiện: Khi người dùng chọn tài khoản trong ucSearch
//                ucSearch.AccountSelected += (sender, id) =>
//                {
//                    selectedId = id; // Lưu lại ID được chọn
//                    searchForm.DialogResult = DialogResult.OK; // Đặt kết quả để form chính biết là đã chọn
//                    searchForm.Close(); // Đóng form tìm kiếm lại
//                };

//                searchForm.Controls.Add(ucSearch); // Thêm UC vào form

//                // Hiển thị form tìm kiếm và chờ người dùng tương tác
//                // Truyền this.FindForm() để nó biết form cha là frmMain_Admin
//                if (searchForm.ShowDialog(this.FindForm()) == DialogResult.OK && selectedId.HasValue)
//                {
//                    // Nếu người dùng chọn OK và có ID -> chọn dòng tương ứng trên grid chính
//                    SelectRowById(selectedId.Value);
//                }
//                // Nếu người dùng đóng form bằng nút X hoặc Cancel -> không làm gì cả
//            }
//        }
//        #endregion
//    }
//}
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