using Guna.UI2.WinForms;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LMS.GUI.AccountAdmin
{
    public partial class ucAccountSearch_Admin : UserControl
    {
        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
        public event EventHandler<int> AccountSelected;

        private readonly Timer _debounce = new Timer { Interval = 250 };

        public ucAccountSearch_Admin()
        {
            InitializeComponent();
            this.Load += (s, e) => { ConfigGrid(); LoadAll(); Wire(); };
        }

        private void ConfigGrid()
        {
            dgvResults.Columns.Clear();
            dgvResults.AutoGenerateColumns = false;
            dgvResults.AllowUserToAddRows = false;
            dgvResults.ReadOnly = true;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvResults.EnableHeadersVisualStyles = false;
            dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
            dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResults.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvResults.ColumnHeadersHeight = 36;

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", DataPropertyName = "Username", HeaderText = "Tài khoản" });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", DataPropertyName = "Role", HeaderText = "Vai trò" });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "PersonName", DataPropertyName = "PersonName", HeaderText = "Họ tên" });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinkedTo", DataPropertyName = "LinkedTo", HeaderText = "Liên kết" });
            dgvResults.Columns.Add(new DataGridViewCheckBoxColumn { Name = "IsActive", DataPropertyName = "IsActive", HeaderText = "Kích hoạt" });

            dgvResults.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var id = (int)((dynamic)dgvResults.Rows[e.RowIndex].DataBoundItem).Id;
                AccountSelected?.Invoke(this, id);
                this.FindForm()?.Close();
            };
        }

        private void Wire()
        {
            btnSearch.Click += (s, e) => DoSearch();
            btnReload.Click += (s, e) => { txtUser.Clear(); txtName.Clear(); cmbRole.SelectedIndex = 0; cmbStatus.SelectedIndex = 0; LoadAll(); };

            _debounce.Tick += (s, e) => { _debounce.Stop(); DoSearch(); };
            txtUser.TextChanged += (s, e) => { _debounce.Stop(); _debounce.Start(); };
            txtName.TextChanged += (s, e) => { _debounce.Stop(); _debounce.Start(); };
        }

        private void LoadAll()
        {
            var data = _svc.GetAll()
                .Select(a => new
                {
                    a.Id,
                    a.Username,
                    a.Role,
                    PersonName = a.Customer != null ? a.Customer.Name :
                                 a.Driver != null ? a.Driver.FullName : "",
                    LinkedTo = a.CustomerId != null ? "KH: " + (a.Customer?.Name ?? a.CustomerId.ToString()) :
                               a.DriverId != null ? "TX: " + (a.Driver?.FullName ?? a.DriverId.ToString()) :
                               "(N/A)",
                    a.IsActive
                }).ToList();

            dgvResults.DataSource = new BindingList<object>(data.Cast<object>().ToList());
        }

        private void DoSearch()
        {
            var username = string.IsNullOrWhiteSpace(txtUser.Text) ? null : txtUser.Text.Trim();
            var name = string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim();
            UserRole? role = cmbRole.SelectedIndex > 0 ? (UserRole)(cmbRole.SelectedIndex - 1) : (UserRole?)null;
            bool? isActive = cmbStatus.SelectedIndex == 1 ? true :
                             cmbStatus.SelectedIndex == 2 ? false : (bool?)null;

            var data = _svc.GetAll(username, name, role, isActive)
                .Select(a => new
                {
                    a.Id,
                    a.Username,
                    a.Role,
                    PersonName = a.Customer != null ? a.Customer.Name :
                                 a.Driver != null ? a.Driver.FullName : "",
                    LinkedTo = a.CustomerId != null ? "KH: " + (a.Customer?.Name ?? a.CustomerId.ToString()) :
                               a.DriverId != null ? "TX: " + (a.Driver?.FullName ?? a.DriverId.ToString()) :
                               "(N/A)",
                    a.IsActive
                }).ToList();

            dgvResults.DataSource = new BindingList<object>(data.Cast<object>().ToList());
        }
    }
}
