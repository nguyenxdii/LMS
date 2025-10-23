// LMS.GUI/OrderCustomer/ucOrderList_Cus.cs
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static LMS.BUS.Services.OrderService_Admin;

namespace LMS.GUI.OrderCustomer
{
    public partial class ucOrderList_Cus : UserControl
    {
        private readonly int _customerId;
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();

        public ucOrderList_Cus(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            this.Load += UcOrderList_Cus_Load;
        }

        private void UcOrderList_Cus_Load(object sender, EventArgs e)
        {
            // filter trạng thái
            cmbStatusFilter.Items.Clear();
            cmbStatusFilter.Items.Add("Tất cả");
            cmbStatusFilter.Items.AddRange(Enum.GetNames(typeof(OrderStatus)));
            cmbStatusFilter.SelectedIndex = 0;

            // cấu hình grid đơn giản
            dgvOrders.AutoGenerateColumns = false;
            dgvOrders.ReadOnly = true;
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.RowHeadersVisible = false;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (dgvOrders.Columns.Count == 0)
            {
                //dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Mã đơn", Width = 70 });
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OrderNo", HeaderText = "Mã đơn", Width = 70 });
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CreatedAt", HeaderText = "Ngày tạo", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OriginWarehouseName", HeaderText = "Kho gửi", Width = 160 });
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DestWarehouseName", HeaderText = "Kho nhận", Width = 160 });
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalFee", HeaderText = "Tổng phí", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Trạng thái", Width = 110 });
            }

            btnReload.Click += (s, ev) => LoadData();
            btnTrack.Click += (s, ev) => OpenTrack();

            ConfigureOrderGrid();
            LoadData();
        }

        private void ConfigureOrderGrid()
        {
            var g = dgvOrders;

            g.Columns.Clear();
            g.AutoGenerateColumns = false;
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;
            g.RowHeadersVisible = false;

            g.ScrollBars = ScrollBars.Both;
            g.Dock = DockStyle.None;
            g.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.EnableHeadersVisualStyles = false;
            g.GridColor = Color.Gainsboro;
            g.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            g.BorderStyle = BorderStyle.FixedSingle;

            g.ColumnHeadersVisible = true;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 36;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            g.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            g.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);

            // Cột
            //g.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    Name = "Id",
            //    DataPropertyName = "Id",
            //    HeaderText = "Mã đơn",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            //    SortMode = DataGridViewColumnSortMode.Programmatic
            //});
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                DataPropertyName = "OrderNo",
                HeaderText = "Mã Đơn",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                DataPropertyName = "CreatedAt",
                HeaderText = "Ngày tạo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OriginWarehouseName",
                DataPropertyName = "OriginWarehouseName",
                HeaderText = "Kho gửi",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DestWarehouseName",
                DataPropertyName = "DestWarehouseName",
                HeaderText = "Kho nhận",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalFee",
                DataPropertyName = "TotalFee",
                HeaderText = "Tổng phí",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            // Bật DoubleBuffer để giảm nhấp nháy
            TryEnableDoubleBuffer(g);
        }

        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                grid.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(grid, true, null);
            }
            catch { /* ignore */ }
        }



        private void LoadData()
        {
            OrderStatus? status = null;
            if (cmbStatusFilter.SelectedIndex > 0)
            {
                var name = cmbStatusFilter.SelectedItem.ToString();
                status = (OrderStatus)Enum.Parse(typeof(OrderStatus), name);
            }

            var list = _orderSvc.GetOrdersByCustomer(_customerId, status)
                                .Select(o => new
                                {
                                    //o.Id,
                                    OrderNo = OrderCode.ToCode(o.Id),
                                    o.CreatedAt,
                                    OriginWarehouseName = o.OriginWarehouse != null ? o.OriginWarehouse.Name : $"#{o.OriginWarehouseId}",
                                    DestWarehouseName = o.DestWarehouse != null ? o.DestWarehouse.Name : $"#{o.DestWarehouseId}",
                                    o.TotalFee,
                                    o.Status
                                })
                                .ToList();

            dgvOrders.DataSource = list;
        }

        private int? GetSelectedOrderId()
        {
            if (dgvOrders.CurrentRow == null) return null;
            var idObj = dgvOrders.CurrentRow.Cells[0].Value;
            return idObj == null ? (int?)null : Convert.ToInt32(idObj);
        }

        private void OpenTrack()
        {
            var oid = GetSelectedOrderId();
            if (oid == null) { MessageBox.Show("Hãy chọn một đơn để xem.", "LMS"); return; }

            var host = this.FindForm() as frmMain_Customer;
            host?.LoadUc(new ucTracking_Cus(oid.Value));
        }
    }
}
