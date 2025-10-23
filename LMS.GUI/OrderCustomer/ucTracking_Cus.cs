// LMS.GUI/OrderCustomer/ucTracking_Cus.cs
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.OrderCustomer
{
    public partial class ucTracking_Cus : UserControl
    {
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();
        private readonly int? _initOrderId;

        public ucTracking_Cus(int? orderId = null)
        {
            InitializeComponent();
            _initOrderId = orderId;
            this.Load += UcTracking_Cus_Load;
        }

        private void UcTracking_Cus_Load(object sender, EventArgs e)
        {
            dgvRouteStops.AutoGenerateColumns = false;
            dgvRouteStops.ReadOnly = true;
            dgvRouteStops.AllowUserToAddRows = false;
            dgvRouteStops.RowHeadersVisible = false;

            if (dgvRouteStops.Columns.Count == 0)
            {
                dgvRouteStops.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Seq", HeaderText = "#", Width = 40 });
                dgvRouteStops.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Warehouse", HeaderText = "Kho", Width = 220 });
                dgvRouteStops.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Trạng thái", Width = 120 });
                dgvRouteStops.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ArrivedAt", HeaderText = "Đến lúc", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
                dgvRouteStops.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DepartedAt", HeaderText = "Rời lúc", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            }

            btnFind.Click += (s, ev) => DoFind();
            btnReload.Click += (s, ev) => DoFind();

            if (_initOrderId.HasValue)
            {
                txtOrderId.Text = _initOrderId.Value.ToString();
                DoFind();
            }

            ConfigureStopsGrid();
        }

        private void ConfigureStopsGrid()
        {
            var g = dgvRouteStops;

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
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Seq",
                DataPropertyName = "Seq",
                HeaderText = "#",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Warehouse",
                DataPropertyName = "Warehouse",
                HeaderText = "Kho",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 55,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ArrivedAt",
                DataPropertyName = "ArrivedAt",
                HeaderText = "Đến lúc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartedAt",
                DataPropertyName = "DepartedAt",
                HeaderText = "Rời lúc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Kích hoạt double buffer để giảm nhấp nháy
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


        private void DoFind()
        {
            if (!int.TryParse(txtOrderId.Text.Trim(), out var id))
            {
                MessageBox.Show("Nhập mã đơn hợp lệ (số).", "LMS");
                return;
            }

            var order = _orderSvc.GetOrderWithStops(id);
            if (order == null)
            {
                MessageBox.Show($"Không tìm thấy đơn #{id}.", "LMS");
                dgvRouteStops.DataSource = null;
                lblOrderStatus.Text = "—";
                return;
            }

            lblOrderStatus.Text = $"Đơn #{order.Id} – Trạng thái: {order.Status}";

            var rows = (order.Shipment?.RouteStops ?? new System.Collections.Generic.List<RouteStop>())
                        .OrderBy(rs => rs.Sequence)
                        .Select(rs => new
                        {
                            Seq = rs.Sequence,
                            Warehouse = rs.Warehouse?.Name ?? $"#{rs.WarehouseId}",
                            Status = rs.Status.ToString(),
                            ArrivedAt = rs.ArrivedAt,
                            DepartedAt = rs.DepartedAt
                        })
                        .ToList();

            dgvRouteStops.DataSource = rows;
        }
    }
}
