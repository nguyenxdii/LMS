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
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using LMS.BUS.Helpers;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucOrderSearch_Admin : UserControl
    {
        public event EventHandler<int> OrderPicked;
        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

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
                var customers = db.Customers.OrderBy(c => c.Name)
                    .Select(c => new { c.Id, c.Name }).ToList();
                customers.Insert(0, new { Id = 0, Name = "— Tất cả —" });
                cmbCustomer.DataSource = customers;
                cmbCustomer.ValueMember = "Id";
                cmbCustomer.DisplayMember = "Name";

                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                    .Select(s => new { Id = (int)s, Name = s.ToString() }).ToList();
                statuses.Insert(0, new { Id = -1, Name = "— Tất cả —" });
                cmbStatus.DataSource = statuses;
                cmbStatus.ValueMember = "Id";
                cmbStatus.DisplayMember = "Name";

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
            g.ApplyBaseStyle();
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustomerName", DataPropertyName = "CustomerName", HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OriginWarehouseName", DataPropertyName = "OriginWarehouseName", HeaderText = "Kho gửi", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DestWarehouseName", DataPropertyName = "DestWarehouseName", HeaderText = "Kho nhận", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalFee", DataPropertyName = "TotalFee", HeaderText = "Tổng phí", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepositAmount", DataPropertyName = "DepositAmount", HeaderText = "Đặt cọc", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Ngày tạo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });
            TryEnableDoubleBuffer(g);
        }

        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                var property = grid.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (property != null)
                {
                    property.SetValue(grid, true, null);
                }
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
                ResetSortGlyphs();
            };
            btnClose.Click += (s, e) =>
            {
                var form = this.FindForm();
                if (form != null)
                {
                    form.Close();
                }
            };
            dgvSearchResults.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
            dgvSearchResults.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvSearchResults.Rows[e.RowIndex];
                    var id = Convert.ToInt32(row.Cells["Id"].Value);
                    if (OrderPicked != null)
                    {
                        OrderPicked(this, id);
                    }
                }
            };
        }

        private void DoSearch()
        {
            int? cusId = null, originId = null, destId = null;
            OrderStatus? status = null;
            DateTime? from = null, to = null;
            if (cmbCustomer != null && (int)cmbCustomer.SelectedValue != 0) cusId = (int)cmbCustomer.SelectedValue;
            if (cmbOrigin != null && (int)cmbOrigin.SelectedValue != 0) originId = (int)cmbOrigin.SelectedValue;
            if (cmbDest != null && (int)cmbDest.SelectedValue != 0) destId = (int)cmbDest.SelectedValue;
            var st = (int)(cmbStatus != null ? cmbStatus.SelectedValue : -1);
            if (st != -1) status = (OrderStatus)st;
            if (dtFrom != null && dtFrom.Checked) from = dtFrom.Value.Date;
            if (dtTo != null && dtTo.Checked) to = dtTo.Value.Date;
            var list = _orderSvc.SearchForAdmin(cusId, status, originId, destId, from, to, txtCode != null ? txtCode.Text?.Trim() : null);
            if (dgvSearchResults != null)
            {
                dgvSearchResults.DataSource = list;
            }
            ResetSortGlyphs();
        }

        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvSearchResults != null ? dgvSearchResults.DataSource as List<OrderListItemDto> : null;
            if (list == null || list.Count == 0) return;

            var column = dgvSearchResults != null ? dgvSearchResults.Columns[e.ColumnIndex] : null;
            if (column == null || column.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            // Xác định hướng sắp xếp
            if (_sortedColumn == column)
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
                _sortedColumn = column;
            }

            // Sắp xếp danh sách bằng reflection
            var property = typeof(OrderListItemDto).GetProperty(column.DataPropertyName);
            if (property != null)
            {
                IOrderedEnumerable<OrderListItemDto> sortedList;
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = list.OrderBy(x => property.GetValue(x));
                else
                    sortedList = list.OrderByDescending(x => property.GetValue(x));

                if (dgvSearchResults != null)
                {
                    dgvSearchResults.DataSource = sortedList.ToList();
                    column.HeaderCell.SortGlyphDirection = _sortOrder;
                }
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null)
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
        }
    }
}