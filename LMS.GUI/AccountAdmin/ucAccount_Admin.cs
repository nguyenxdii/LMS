using Guna.UI2.WinForms;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.AccountAdmin
{
    public partial class ucAccount_Admin : UserControl
    {
        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
        private BindingList<object> _binding;

        private enum EditMode { None, Edit }
        private EditMode _mode = EditMode.None;

        public ucAccount_Admin()
        {
            InitializeComponent();
            this.Load += (s, e) => { ConfigureGrid(); LoadData(); Wire(); SetMode(EditMode.None); };
        }

        private void ConfigureGrid()
        {
            dgvAccounts.Columns.Clear();
            dgvAccounts.AutoGenerateColumns = false;
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.ReadOnly = true;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.MultiSelect = false;

            dgvAccounts.EnableHeadersVisualStyles = false;
            dgvAccounts.GridColor = Color.Gainsboro;
            dgvAccounts.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvAccounts.BorderStyle = BorderStyle.FixedSingle;
            dgvAccounts.ColumnHeadersHeight = 36;
            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", DataPropertyName = "Username", HeaderText = "Tài khoản" });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", DataPropertyName = "Role", HeaderText = "Vai trò" });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinkedTo", DataPropertyName = "LinkedTo", HeaderText = "Liên kết" });
            dgvAccounts.Columns.Add(new DataGridViewCheckBoxColumn { Name = "IsActive", DataPropertyName = "IsActive", HeaderText = "Kích hoạt" });

            dgvAccounts.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = dgvAccounts.Rows[e.RowIndex];
                dynamic it = row.DataBoundItem;
                if (it == null) return;
                bool active = (bool)it.IsActive;
                row.DefaultCellStyle.ForeColor = active ? Color.Black : Color.Gray;
            };
        }

        //private void LoadData(string username = null, string name = null)
        //{
        //    var data = _svc.GetAll(username, name)
        //        .Select(a => new
        //        {
        //            a.Id,
        //            a.Username,
        //            a.Role,
        //            LinkedTo = a.CustomerId != null ? "KH: " + (a.Customer?.Name ?? a.CustomerId.ToString())
        //                      : a.DriverId != null ? "TX: " + (a.Driver?.FullName ?? a.DriverId.ToString())
        //                      : "(N/A)",
        //            a.IsActive
        //        }).ToList();

        //    _binding = new BindingList<object>(data.Cast<object>().ToList());
        //    dgvAccounts.DataSource = _binding;

        //    if (dgvAccounts.Rows.Count > 0)
        //    {
        //        dgvAccounts.Rows[0].Selected = true;
        //        dgvAccounts.CurrentCell = dgvAccounts.Rows[0].Cells["Username"];
        //    }
        //    UpdateToggleUi();
        //}
        private void LoadData(string username = null, string name = null)
        {
            var data = _svc.GetAll(username, name)
                .Where(a => a.Role != UserRole.Admin) // <-- ẨN ADMIN Ở ĐÂY
                .Select(a => new
                {
                    a.Id,
                    a.Username,
                    a.Role,
                    LinkedTo = a.CustomerId != null ? "KH: " + (a.Customer?.Name ?? a.CustomerId.ToString())
                              : a.DriverId != null ? "TX: " + (a.Driver?.FullName ?? a.DriverId.ToString())
                              : "(N/A)",
                    a.IsActive
                })
                .ToList();

            _binding = new BindingList<object>(data.Cast<object>().ToList());
            dgvAccounts.DataSource = _binding;

            if (dgvAccounts.Rows.Count > 0)
            {
                dgvAccounts.Rows[0].Selected = true;
                dgvAccounts.CurrentCell = dgvAccounts.Rows[0].Cells["Username"];
            }
            UpdateToggleUi();
        }


        private (int id, string username, bool isActive)? Current()
        {
            if (dgvAccounts.CurrentRow == null) return null;
            dynamic it = dgvAccounts.CurrentRow.DataBoundItem;
            return ((int)it.Id, (string)it.Username, (bool)it.IsActive);
        }

        private void Wire()
        {
            btnReload.Click += (s, e) => { LoadData(); ClearInputs(); SetMode(EditMode.None); };
            btnEdit.Click += (s, e) => { if (Current() != null) { SetMode(EditMode.Edit); txtUser.Text = Current()?.username; } };
            btnSave.Click += (s, e) => SaveEdit();
            //btnCancel.Click += (s, e) => { ClearInputs(); SetMode(EditMode.None); };

            btnCancel.Click += (s, e) =>
            {
                if (_mode == EditMode.Edit)
                {
                    bool hasChange = (txtUser.Text.Trim() != (Current()?.username ?? string.Empty)) ||
                                     !string.IsNullOrWhiteSpace(txtPass.Text);

                    if (hasChange)
                    {
                        var ask = MessageBox.Show("Huỷ các thay đổi đang sửa?", "Xác nhận",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (ask != DialogResult.Yes) return;
                    }
                }
                ClearInputs();
                SetMode(EditMode.None);
            };

            btnDelete.Click += (s, e) => DeleteAccount();
            btnToggle.Click += (s, e) => ToggleActive();
            btnSearch.Click += (s, e) => OpenSearch();
            dgvAccounts.SelectionChanged += (s, e) => UpdateToggleUi();
        }

        private void SetMode(EditMode m)
        {
            _mode = m;
            bool editing = (m == EditMode.Edit);
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
            dgvAccounts.Enabled = !editing;
            btnEdit.Enabled = !editing;
            btnDelete.Enabled = !editing;
            btnToggle.Enabled = !editing;
            btnSearch.Enabled = !editing;
            btnReload.Enabled = !editing;
        }

        private void ClearInputs() => txtUser.Clear();

        private void SaveEdit()
        {
            var cur = Current(); if (cur == null) return;
            try
            {
                var confirm = MessageBox.Show("Lưu thay đổi tài khoản?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                _svc.UpdateBasic(cur.Value.id, txtUser.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtPass.Text) ? null : txtPass.Text);

                LoadData();
                MessageBox.Show("Đã cập nhật tài khoản.", "LMS");
                ClearInputs(); SetMode(EditMode.None);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        //private void DeleteAccount()
        //{
        //    var cur = Current(); if (cur == null) return;
        //    if (MessageBox.Show("Xoá tài khoản này?", "Xác nhận",
        //        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        //    try
        //    {
        //        _svc.Delete(cur.Value.id);
        //        LoadData();
        //        MessageBox.Show("Đã xoá.", "LMS");
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        //}

        private void DeleteAccount()
        {
            var cur = Current(); if (cur == null) return;

            try
            {
                // 1) Kiểm tra ràng buộc sử dụng
                var rpt = _svc.InspectUsage(cur.Value.id);

                // 2) Build thông điệp theo vai trò
                string msg;
                bool allowDelete = true;   // sau xác nhận

                if (rpt.Role == UserRole.Customer)
                {
                    if (rpt.OrdersActive > 0)
                    {
                        MessageBox.Show(
                            $"Khách hàng đang có {rpt.OrdersActive} đơn đang xử lý. Không thể xoá tài khoản.",
                            "Không thể xoá", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (rpt.OrdersPending > 0)
                    {
                        msg = $"Khách hàng đang có {rpt.OrdersPending} đơn chờ duyệt.\n" +
                              "Bạn có chắc muốn xoá tài khoản? (Đơn hàng vẫn giữ nguyên, khách sẽ không thể đăng nhập)";
                        allowDelete = MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                        if (!allowDelete) return;
                    }
                    else if (rpt.OrdersCancelled > 0 || rpt.OrdersCompleted > 0)
                    {
                        msg = $"Khách hàng có {rpt.OrdersCompleted} đơn hoàn tất, {rpt.OrdersCancelled} đơn đã huỷ.\n" +
                              "Bạn có muốn xoá tài khoản không?";
                        allowDelete = MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                        if (!allowDelete) return;
                    }
                    else
                    {
                        // không có đơn nào
                        allowDelete = MessageBox.Show("Xoá tài khoản này?", "Xác nhận",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                        if (!allowDelete) return;
                    }
                }
                else if (rpt.Role == UserRole.Driver)
                {
                    if (rpt.ShipActive > 0)
                    {
                        MessageBox.Show(
                            $"Tài xế đang có {rpt.ShipActive} chuyến đang thực hiện. Không thể xoá tài khoản.",
                            "Không thể xoá", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (rpt.ShipPending > 0)
                    {
                        msg = $"Tài xế đang có {rpt.ShipPending} chuyến chờ/được phân.\n" +
                              "Bạn có chắc muốn xoá tài khoản? (Chuyến vẫn giữ, tài xế sẽ không thể đăng nhập)";
                        allowDelete = MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                        if (!allowDelete) return;
                    }
                    else if (rpt.ShipCancelled > 0 || rpt.ShipCompleted > 0)
                    {
                        msg = $"Tài xế có {rpt.ShipCompleted} chuyến hoàn tất, {rpt.ShipCancelled} chuyến đã huỷ.\n" +
                              "Bạn có muốn xoá tài khoản không?";
                        allowDelete = MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                        if (!allowDelete) return;
                    }
                    else
                    {
                        allowDelete = MessageBox.Show("Xoá tài khoản này?", "Xác nhận",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                        if (!allowDelete) return;
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản Admin không hiển thị để xoá tại đây.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 3) Thực hiện xoá (chỉ xoá UserAccount, KH/Driver & dữ liệu nghiệp vụ giữ nguyên)
                _svc.DeleteOnlyAccount(cur.Value.id);

                // 4) Reload
                LoadData();
                MessageBox.Show("Đã xoá tài khoản.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //private void ToggleActive()
        //{
        //    var cur = Current(); if (cur == null) return;
        //    try
        //    {
        //        _svc.SetActive(cur.Value.id, !cur.Value.isActive);
        //        LoadData();
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        //}
        private void ToggleActive()
        {
            var cur = Current(); if (cur == null) return;

            try
            {
                bool newState = !cur.Value.isActive;

                // 1) Đổi trạng thái trên DB
                _svc.SetActive(cur.Value.id, newState);

                // 2) Reload + giữ lại dòng đang chọn
                int keepId = cur.Value.id;
                LoadData();
                SelectRowById(keepId);

                // 3) Cập nhật nút theo trạng thái mới
                UpdateToggleUi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        // Tìm & chọn lại dòng theo Id sau khi Reload
        private void SelectRowById(int id)
        {
            if (dgvAccounts.Rows.Count == 0) return;

            for (int i = 0; i < dgvAccounts.Rows.Count; i++)
            {
                var row = dgvAccounts.Rows[i];
                if (row?.DataBoundItem == null) continue;

                // DataBoundItem là anonymous type => dùng dynamic cho gọn
                dynamic it = row.DataBoundItem;

                try
                {
                    if ((int)it.Id == id)
                    {
                        row.Selected = true;
                        // Đặt ô hiện tại để cập nhật selection chính xác
                        var cell = row.Cells["Username"]; // đúng tên cột bạn đã đặt
                        if (cell != null) dgvAccounts.CurrentCell = cell;

                        // Cuộn tới dòng
                        if (i >= 0 && i < dgvAccounts.Rows.Count)
                            dgvAccounts.FirstDisplayedScrollingRowIndex = i;

                        break;
                    }
                }
                catch { /* bỏ qua dòng lỗi cast nếu có */ }
            }

            // Cập nhật lại nút Khoá/Mở theo dòng vừa chọn
            UpdateToggleUi();
        }


        private void UpdateToggleUi()
        {
            var cur = Current();
            if (cur == null) return;
            btnToggle.Text = cur.Value.isActive ? "Khoá" : "Mở";
            btnToggle.FillColor = cur.Value.isActive ? Color.FromArgb(200, 50, 50) : Color.FromArgb(34, 177, 76);
        }

        private void OpenSearch()
        {
            int? pickedId = null;
            using (var dlg = new Form
            {
                Text = "Tìm tài khoản",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(900, 600)
            })
            {
                var uc = new ucAccountSearch_Admin { Dock = DockStyle.Fill };
                uc.AccountSelected += (s, id) => { pickedId = id; dlg.DialogResult = DialogResult.OK; };
                dlg.Controls.Add(uc);
                if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK && pickedId.HasValue)
                {
                    foreach (DataGridViewRow r in dgvAccounts.Rows)
                    {
                        dynamic it = r.DataBoundItem;
                        if ((int)it.Id == pickedId.Value)
                        {
                            r.Selected = true;
                            dgvAccounts.CurrentCell = r.Cells["Username"];
                            break;
                        }
                    }
                }
            }
        }
    }
}
