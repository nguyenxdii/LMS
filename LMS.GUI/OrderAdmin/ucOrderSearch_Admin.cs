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
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Tìm kiếm Đơn Hàng";
            }

            BindFilters();
            ConfigureGrid();
            WireEvents();
        }

        private void BindFilters()
        {
            using (var db = new LogisticsDbContext())
            {
                var customers = db.Customers
                    .OrderBy(c => c.Name)
                    .Select(c => new { c.Id, c.Name })
                    .ToList();
                customers.Insert(0, new { Id = 0, Name = "— Tất cả —" });
                cmbCustomer.DataSource = customers;
                cmbCustomer.ValueMember = "Id";
                cmbCustomer.DisplayMember = "Name";

                var statuses = Enum.GetValues(typeof(OrderStatus))
                    .Cast<OrderStatus>()
                    .Select(s => new { Id = (int)s, Name = GetVietnameseOrderStatus(s) })
                    .ToList();
                statuses.Insert(0, new { Id = -1, Name = "— Tất cả —" });
                cmbStatus.DataSource = statuses;
                cmbStatus.ValueMember = "Id";
                cmbStatus.DisplayMember = "Name";

                var wh = db.Warehouses
                    .OrderBy(w => w.Name)
                    .Select(w => new { w.Id, w.Name })
                    .ToList();
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
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã ĐH", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustomerName", DataPropertyName = "CustomerName", HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OriginWarehouseName", DataPropertyName = "OriginWarehouseName", HeaderText = "Kho gửi", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DestWarehouseName", DataPropertyName = "DestWarehouseName", HeaderText = "Kho nhận", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalFee", DataPropertyName = "TotalFee", HeaderText = "Tổng phí", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepositAmount", DataPropertyName = "DepositAmount", HeaderText = "Đặt cọc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Ngày tạo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 120, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });

            TryEnableDoubleBuffer(g);
        }

        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                var property = grid.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (property != null) property.SetValue(grid, true, null);
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
                form?.Close();
            };
            dgvSearchResults.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
            dgvSearchResults.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvSearchResults.Rows[e.RowIndex];
                    var id = Convert.ToInt32(row.Cells["Id"].Value);
                    OrderPicked?.Invoke(this, id);
                }
            };
            dgvSearchResults.CellFormatting += dgvSearchResults_CellFormatting;
        }

        private void DoSearch()
        {
            int? cusId = null, originId = null, destId = null;
            OrderStatus? status = null;
            DateTime? from = null, to = null;

            if (cmbCustomer?.SelectedValue != null && (int)cmbCustomer.SelectedValue != 0) cusId = (int)cmbCustomer.SelectedValue;
            if (cmbOrigin?.SelectedValue != null && (int)cmbOrigin.SelectedValue != 0) originId = (int)cmbOrigin.SelectedValue;
            if (cmbDest?.SelectedValue != null && (int)cmbDest.SelectedValue != 0) destId = (int)cmbDest.SelectedValue;
            if (cmbStatus?.SelectedValue != null)
            {
                var st = (int)cmbStatus.SelectedValue;
                if (st != -1) status = (OrderStatus)st;
            }
            if (dtFrom != null && dtFrom.Checked) from = dtFrom.Value.Date;
            if (dtTo != null && dtTo.Checked) to = dtTo.Value.Date.AddDays(1).AddTicks(-1);

            var list = _orderSvc.SearchForAdmin(cusId, status, originId, destId, from, to, txtCode?.Text?.Trim());

            if (dgvSearchResults != null)
            {
                dgvSearchResults.DataSource = list;
            }
            ResetSortGlyphs();
        }

        private void dgvSearchResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSearchResults.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                if (e.Value is OrderStatus statusEnum)
                {
                    e.Value = GetVietnameseOrderStatus(statusEnum);
                    e.FormattingApplied = true;
                }
            }
        }

        private string GetVietnameseOrderStatus(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Pending: return "Chờ duyệt";
                case OrderStatus.Approved: return "Đã duyệt";
                case OrderStatus.Completed: return "Hoàn thành";
                case OrderStatus.Cancelled: return "Đã hủy";
                default: return status.ToString();
            }
        }

        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvSearchResults?.DataSource as List<OrderListItemDto>;
            if (list == null || list.Count == 0) return;

            var column = dgvSearchResults?.Columns[e.ColumnIndex];
            if (column == null || column.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == column)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn?.HeaderCell != null)
                {
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = column;
            }

            var property = typeof(OrderListItemDto).GetProperty(column.DataPropertyName);
            if (property != null)
            {
                IEnumerable<OrderListItemDto> sortedList =
                    (_sortOrder == SortOrder.Ascending)
                        ? list.OrderBy(x => property.GetValue(x))
                        : list.OrderByDescending(x => property.GetValue(x));

                if (dgvSearchResults != null)
                {
                    dgvSearchResults.DataSource = sortedList.ToList();
                    if (column.HeaderCell != null)
                    {
                        column.HeaderCell.SortGlyphDirection = _sortOrder;
                    }
                }
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn?.HeaderCell != null)
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
        }
    }
}
