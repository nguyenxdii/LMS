// LMS.GUI/OrderAdmin/ucOrder_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucOrder_Admin : UserControl
    {
        private readonly OrderService_Admin _svc = new OrderService_Admin();
        private BindingList<OrderListItemDto> _binding;

        public ucOrder_Admin()
        {
            InitializeComponent();
            this.Load += (s, e) =>
            {
                ConfigureGrid();
                Wire();
                LoadData();
            };
        }

        #region Grid config
        private void ConfigureGrid()
        {
            var g = dgvOrders;
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

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                DataPropertyName = "OrderNo",
                HeaderText = "Mã ĐH",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                DataPropertyName = "CustomerName",
                HeaderText = "Khách hàng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OriginWarehouseName",
                DataPropertyName = "OriginWarehouseName",
                HeaderText = "Kho gửi",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DestWarehouseName",
                DataPropertyName = "DestWarehouseName",
                HeaderText = "Kho nhận",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalFee",
                DataPropertyName = "TotalFee",
                HeaderText = "Tổng phí",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepositAmount",
                DataPropertyName = "DepositAmount",
                HeaderText = "Đặt cọc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                DataPropertyName = "CreatedAt",
                HeaderText = "Ngày tạo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // double buffer
            try
            {
                g.GetType().GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                 ?.SetValue(g, true, null);
            }
            catch { }
        }
        #endregion

        #region Load + helpers
        private void LoadData()
        {
            var list = _svc.GetAllForAdmin();
            _binding = new BindingList<OrderListItemDto>(list);
            dgvOrders.DataSource = _binding;

            if (dgvOrders.Rows.Count > 0)
            {
                dgvOrders.Rows[0].Selected = true;
                dgvOrders.CurrentCell = dgvOrders.Rows[0].Cells["OrderNo"];
            }
            UpdateButtonsState();
        }

        private OrderListItemDto Current()
        {
            if (dgvOrders.CurrentRow?.DataBoundItem is OrderListItemDto it)
                return it;
            return null;
        }

        private void KeepAndReload(int keepId)
        {
            LoadData();
            if (keepId <= 0) return;
            foreach (DataGridViewRow r in dgvOrders.Rows)
            {
                if ((int)r.Cells["Id"].Value == keepId)
                {
                    r.Selected = true;
                    dgvOrders.CurrentCell = r.Cells["OrderNo"];
                    dgvOrders.FirstDisplayedScrollingRowIndex = r.Index;
                    break;
                }
            }
        }
        #endregion

        #region Wire events
        private void Wire()
        {
            btnReload.Click += (s, e) => LoadData();
            dgvOrders.SelectionChanged += (s, e) => UpdateButtonsState();

            // mở UC tìm kiếm riêng
            btnSearch.Click += (s, e) => OpenSearchDialog();

            btnViewDetail.Click += (s, e) => ShowDetail();
            btnApprove.Click += (s, e) => Approve();
            btnReject.Click += (s, e) => Reject();
            btnDelete.Click += (s, e) => Delete();
            btnShipment.Click += (s, e) => CreateShipment();
        }

        private void UpdateButtonsState()
        {
            var it = Current();
            if (it == null)
            {
                btnApprove.Enabled = btnReject.Enabled = btnDelete.Enabled =
                btnViewDetail.Enabled = btnShipment.Enabled = false;
                return;
            }

            bool isPending = it.Status == OrderStatus.Pending;
            bool isApproved = it.Status == OrderStatus.Approved;
            bool isFinal = it.Status == OrderStatus.Completed || it.Status == OrderStatus.Cancelled;

            btnApprove.Enabled = isPending;
            btnReject.Enabled = isPending;
            btnDelete.Enabled = isPending || it.Status == OrderStatus.Cancelled; // chỉ xóa Pending/Cancelled
            btnViewDetail.Enabled = true;
            btnShipment.Enabled = isApproved; // tạo ship khi đã duyệt
        }
        #endregion

        #region Actions
        private void OpenSearchDialog()
        {
            int? pickedId = null;

            using (var dlg = new Form
            {
                Text = "Tìm đơn hàng",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(1100, 700)
            })
            {
                var uc = new ucOrderSearch_Admin { Dock = DockStyle.Fill };
                uc.OrderPicked += (s, id) => { pickedId = id; dlg.DialogResult = DialogResult.OK; };
                dlg.Controls.Add(uc);

                if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK && pickedId.HasValue)
                {
                    // load lại & focus về đơn vừa chọn
                    KeepAndReload(pickedId.Value);
                }
            }
        }

        private void ShowDetail()
        {
            var it = Current(); if (it == null) return;
            try
            {
                var o = _svc.GetByIdWithAll(it.Id);
                if (o == null) { MessageBox.Show("Không tìm thấy đơn.", "LMS"); return; }

                // tạo dialog chi tiết đơn (nhẹ, không phụ thuộc UC khác)
                using (var f = new Form
                {
                    Text = $"Chi tiết đơn {o.OrderNo ?? ("ORD" + o.Id)}",
                    StartPosition = FormStartPosition.CenterParent,
                    Size = new Size(700, 520)
                })
                {
                    var txt = new TextBox
                    {
                        Multiline = true,
                        ReadOnly = true,
                        Dock = DockStyle.Fill,
                        ScrollBars = ScrollBars.Vertical,
                        Font = new Font("Consolas", 10)
                    };

                    string lines = string.Join(Environment.NewLine, new[]
                    {
                        $"Mã đơn: {o.OrderNo ?? ("ORD"+o.Id)}",
                        $"Khách hàng: {o.Customer?.Name} (#{o.CustomerId})",
                        $"Tuyến: {o.OriginWarehouse?.Name} → {o.DestWarehouse?.Name}",
                        $"Tổng phí: {o.TotalFee:N0}  |  Đặt cọc: {o.DepositAmount:N0}",
                        $"Trạng thái: {o.Status}",
                        $"Ngày tạo: {o.CreatedAt:dd/MM/yyyy HH:mm}",
                        "",
                        $"Shipment: {(o.ShipmentId.HasValue ? "SHP"+o.ShipmentId : "(chưa có)")}",
                        (o.ShipmentId.HasValue ? $"Driver: {o.Shipment?.Driver?.FullName}" : ""),
                        "",
                        "Các chặng (nếu có):"
                    }.Where(s => !string.IsNullOrEmpty(s)));

                    if (o.Shipment?.RouteStops != null && o.Shipment.RouteStops.Any())
                    {
                        foreach (var rs in o.Shipment.RouteStops.OrderBy(x => x.Seq))
                        {
                            var stopName = rs.Warehouse?.Name ?? rs.StopName ?? "(N/A)";
                            lines += Environment.NewLine + $"  - [{rs.Seq}] {stopName}  |  {rs.Status} " +
                                     $"{(rs.ArrivedAt.HasValue ? "Arrive:" + rs.ArrivedAt.Value.ToString("dd/MM HH:mm") : "")} " +
                                     $"{(rs.DepartedAt.HasValue ? " / Depart:" + rs.DepartedAt.Value.ToString("dd/MM HH:mm") : "")}";
                        }
                    }
                    else
                    {
                        lines += Environment.NewLine + "  (chưa có RouteStops)";
                    }

                    txt.Text = lines;
                    f.Controls.Add(txt);
                    f.ShowDialog(this.FindForm());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Approve()
        {
            var it = Current(); if (it == null) return;
            if (it.Status != OrderStatus.Pending)
            {
                MessageBox.Show("Chỉ duyệt đơn trạng thái Pending.", "LMS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Duyệt đơn hàng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _svc.Approve(it.Id);
                KeepAndReload(it.Id);
                MessageBox.Show("Đã duyệt đơn.", "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void Reject()
        {
            var it = Current(); if (it == null) return;
            if (it.Status != OrderStatus.Pending)
            {
                MessageBox.Show("Chỉ từ chối đơn trạng thái Pending.", "LMS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Từ chối đơn hàng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _svc.Reject(it.Id);
                KeepAndReload(it.Id);
                MessageBox.Show("Đã từ chối đơn.", "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void Delete()
        {
            var it = Current(); if (it == null) return;

            // CẢNH BÁO: chỉ xóa Pending/Cancelled; service đã chặn thêm ShipmentId != null
            var note =
                "- Chỉ xoá được đơn Pending/Cancelled.\n" +
                "- Nếu đơn đã gắn Shipment hoặc đang/đã vận chuyển thì KHÔNG thể xoá.\n\n" +
                "Bạn có chắc muốn xoá đơn này?";
            if (MessageBox.Show(note, "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                _svc.Delete(it.Id);
                LoadData();
                MessageBox.Show("Đã xoá đơn.", "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void CreateShipment()
        {
            var it = Current(); if (it == null) return;
            if (it.Status != OrderStatus.Approved)
            {
                MessageBox.Show("Chỉ tạo Shipment cho đơn đã được duyệt.", "LMS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1) chọn tài xế active
            int? driverId = PickDriverId();
            if (!driverId.HasValue) return;

            // 2) tạo shipment
            try
            {
                int shipId = _svc.CreateShipmentFromOrder(it.Id, driverId.Value);
                KeepAndReload(it.Id);
                MessageBox.Show($"Đã tạo Shipment SHP{shipId}.", "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }
        #endregion

        #region Small picker dialog
        private int? PickDriverId()
        {
            using (var db = new LogisticsDbContext())
            {
                var list = db.Drivers.Where(d => d.IsActive)
                    .OrderBy(d => d.FullName)
                    .Select(d => new { d.Id, d.FullName, d.Phone, d.LicenseType })
                    .ToList();

                if (list.Count == 0)
                {
                    MessageBox.Show("Chưa có tài xế hoạt động.", "LMS");
                    return null;
                }

                int? picked = null;
                using (var f = new Form
                {
                    Text = "Chọn tài xế",
                    StartPosition = FormStartPosition.CenterParent,
                    Size = new Size(600, 420)
                })
                {
                    var grid = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        ReadOnly = true,
                        AllowUserToAddRows = false,
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        AutoGenerateColumns = false,
                        RowHeadersVisible = false
                    };

                    // header style
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    grid.ColumnHeadersHeight = 36;

                    grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng GPLX", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

                    grid.DataSource = list;

                    grid.CellDoubleClick += (s, e) =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            picked = Convert.ToInt32(grid.Rows[e.RowIndex].Cells["Id"].Value);
                            f.DialogResult = DialogResult.OK;
                        }
                    };

                    var btnOk = new Guna2Button
                    {
                        Text = "Chọn",
                        Dock = DockStyle.Bottom,
                        Height = 40,
                        FillColor = Color.FromArgb(31, 113, 185),
                        ForeColor = Color.White
                    };
                    btnOk.Click += (s, e) =>
                    {
                        if (grid.CurrentRow != null)
                        {
                            picked = Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
                            f.DialogResult = DialogResult.OK;
                        }
                    };

                    f.Controls.Add(grid);
                    f.Controls.Add(btnOk);

                    return f.ShowDialog(this.FindForm()) == DialogResult.OK ? picked : null;
                }
            }
        }
        #endregion
    }
}
