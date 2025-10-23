// LMS.GUI/OrderAdmin/ucOrderSearch_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucOrderSearch_Admin : UserControl
    {
        public event EventHandler<int> OrderPicked;

        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();

        public ucOrderSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcOrderSearch_Admin_Load;
        }

        private void UcOrderSearch_Admin_Load(object sender, EventArgs e)
        {
            BindFilters();
            ConfigureGrid();
            WireEvents();
        }

        private void BindFilters()
        {
            using (var db = new LogisticsDbContext())
            {
                // Customer
                var customers = db.Customers.OrderBy(c => c.Name)
                    .Select(c => new { c.Id, c.Name }).ToList();
                customers.Insert(0, new { Id = 0, Name = "— Tất cả —" });
                cmbCustomer.DataSource = customers;
                cmbCustomer.ValueMember = "Id";
                cmbCustomer.DisplayMember = "Name";

                // Status
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                    .Select(s => new { Id = (int)s, Name = s.ToString() }).ToList();
                statuses.Insert(0, new { Id = -1, Name = "— Tất cả —" });
                cmbStatus.DataSource = statuses;
                cmbStatus.ValueMember = "Id";
                cmbStatus.DisplayMember = "Name";

                // Warehouses
                var wh = db.Warehouses.OrderBy(w => w.Name)
                    .Select(w => new { w.Id, w.Name }).ToList();
                var whAll = wh.ToList();
                whAll.Insert(0, new { Id = 0, Name = "— Tất cả —" });

                cmbOrigin.DataSource = whAll.ToList();
                cmbOrigin.ValueMember = "Id";
                cmbOrigin.DisplayMember = "Name";

                cmbDest.DataSource = whAll.ToList();
                cmbDest.ValueMember = "Id";
                cmbDest.DisplayMember = "Name";
            }

            dtFrom.Checked = false;
            dtTo.Checked = false;
        }

        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.AutoGenerateColumns = false;
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;
            g.RowHeadersVisible = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.BorderStyle = BorderStyle.FixedSingle;
            g.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            g.EnableHeadersVisualStyles = false;
            g.GridColor = Color.Gainsboro;

            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 36;

            g.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            g.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustomerName", DataPropertyName = "CustomerName", HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OriginWarehouseName", DataPropertyName = "OriginWarehouseName", HeaderText = "Kho gửi", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DestWarehouseName", DataPropertyName = "DestWarehouseName", HeaderText = "Kho nhận", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalFee", DataPropertyName = "TotalFee", HeaderText = "Tổng phí", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepositAmount", DataPropertyName = "DepositAmount", HeaderText = "Đặt cọc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Ngày tạo", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });

            TryEnableDoubleBuffer(g);
        }

        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                grid.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(grid, true, null);
            }
            catch { }
        }

        private void WireEvents()
        {
            btnDoSearch.Click += (s, e) => DoSearch();
            btnReset.Click += (s, e) =>
            {
                cmbCustomer.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
                cmbOrigin.SelectedIndex = 0;
                cmbDest.SelectedIndex = 0;
                txtCode.Clear();
                dtFrom.Checked = false;
                dtTo.Checked = false;
                dgvSearchResults.DataSource = null;
            };
            btnClose.Click += (s, e) => this.FindForm()?.Close();

            dgvSearchResults.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvSearchResults.Rows[e.RowIndex];
                    var id = Convert.ToInt32(row.Cells["Id"].Value);
                    OrderPicked?.Invoke(this, id); // giao lại cho UC cha
                }
            };
        }

        private void DoSearch()
        {
            int? cusId = null, originId = null, destId = null;
            OrderStatus? status = null;
            DateTime? from = null, to = null;

            if ((int)cmbCustomer.SelectedValue != 0) cusId = (int)cmbCustomer.SelectedValue;
            if ((int)cmbOrigin.SelectedValue != 0) originId = (int)cmbOrigin.SelectedValue;
            if ((int)cmbDest.SelectedValue != 0) destId = (int)cmbDest.SelectedValue;

            var st = (int)cmbStatus.SelectedValue;
            if (st != -1) status = (OrderStatus)st;

            if (dtFrom.Checked) from = dtFrom.Value.Date;
            if (dtTo.Checked) to = dtTo.Value.Date;

            var list = _orderSvc.SearchForAdmin(cusId, status, originId, destId, from, to, txtCode.Text?.Trim());
            dgvSearchResults.DataSource = list;
        }
    }
}
