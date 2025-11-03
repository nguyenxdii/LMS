using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static LMS.GUI.OrderAdmin.ucDriverPicker_Admin;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucOrder_Admin : UserControl
    {
        private readonly OrderService_Admin _svc = new OrderService_Admin();
        private BindingList<OrderListItemDto> _binding;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

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

        private void ConfigureGrid()
        {
            var g = dgvOrders;
            g.Columns.Clear();
            g.ApplyBaseStyle();

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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                DataPropertyName = "CustomerName",
                HeaderText = "Khách hàng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
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
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepositAmount",
                DataPropertyName = "DepositAmount",
                HeaderText = "Đặt cọc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
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
                Name = "CreatedAt",
                DataPropertyName = "CreatedAt",
                HeaderText = "Ngày tạo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 120,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter },
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            try
            {
                var dgvType = g.GetType();
                var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (pi != null) pi.SetValue(g, true, null);
            }
            catch (Exception)
            {
            }
        }

        private void LoadData()
        {
            var list = _svc.GetAllForAdmin();
            _binding = new BindingList<OrderListItemDto>(list);
            if (dgvOrders != null)
            {
                dgvOrders.DataSource = _binding;
                if (dgvOrders.Rows.Count > 0)
                {
                    dgvOrders.Rows[0].Selected = true;
                    if (dgvOrders.Columns.Contains("OrderNo"))
                        dgvOrders.CurrentCell = dgvOrders.Rows[0].Cells["OrderNo"];
                }
                UpdateButtonsState();
                ResetSortGlyphs();
            }
        }

        private OrderListItemDto Current()
        {
            if (dgvOrders != null && dgvOrders.CurrentRow != null && dgvOrders.CurrentRow.DataBoundItem is OrderListItemDto it)
            {
                return it;
            }
            return null;
        }

        private void KeepAndReload(int keepId)
        {
            LoadData();
            if (dgvOrders != null && keepId > 0)
            {
                foreach (DataGridViewRow r in dgvOrders.Rows)
                {
                    if (r.Cells["Id"].Value != null && Convert.ToInt32(r.Cells["Id"].Value) == keepId)
                    {
                        r.Selected = true;
                        if (dgvOrders.Columns.Contains("OrderNo"))
                            dgvOrders.CurrentCell = r.Cells["OrderNo"];
                        if (r.Index >= 0 && r.Index < dgvOrders.RowCount)
                            dgvOrders.FirstDisplayedScrollingRowIndex = r.Index;
                        break;
                    }
                }
            }
        }

        private void Wire()
        {
            btnReload.Click += (s, e) => LoadData();
            dgvOrders.SelectionChanged += (s, e) => UpdateButtonsState();
            btnSearch.Click += (s, e) => OpenSearchDialog();
            btnViewDetail.Click += (s, e) => ShowDetail();
            btnApprove.Click += (s, e) => Approve();
            btnReject.Click += (s, e) => Reject();
            btnDelete.Click += (s, e) => Delete();
            btnShipment.Click += (s, e) => CreateShipment();
            dgvOrders.ColumnHeaderMouseClick += dgvOrders_ColumnHeaderMouseClick;
            dgvOrders.CellFormatting += dgvOrders_CellFormatting;
            dgvOrders.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) ShowDetail();
            };
        }

        private void UpdateButtonsState()
        {
            var it = Current();
            bool enable = (it != null);

            btnApprove.Enabled = enable && it.Status == OrderStatus.Pending;
            btnReject.Enabled = enable && it.Status == OrderStatus.Pending;
            btnDelete.Enabled = enable && (it.Status == OrderStatus.Cancelled || it.Status == OrderStatus.Completed);
            btnViewDetail.Enabled = enable;
            btnShipment.Enabled = enable && it.Status == OrderStatus.Approved;
        }

        private void OpenSearchDialog()
        {
            int? pickedId = null;
            using (var dlg = new Form
            {
                Text = "Tìm đơn hàng",
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(1195, 762)
            })
            {
                var uc = new ucOrderSearch_Admin { Dock = DockStyle.Fill };
                uc.OrderPicked += (s, id) =>
                {
                    pickedId = id;
                    dlg.DialogResult = DialogResult.OK;
                };
                dlg.Controls.Add(uc);
                if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK && pickedId.HasValue)
                {
                    KeepAndReload(pickedId.Value);
                }
            }
        }

        private void ShowDetail()
        {
            var it = Current();
            if (it == null) return;

            using (var f = new Form
            {
                Text = $"Chi tiết đơn hàng ORD{it.Id}",
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(500, 500),
                FormBorderStyle = FormBorderStyle.None,
            })
            {
                var uc = new ucOrderDetail_Admin(it.Id) { Dock = DockStyle.Fill };
                f.Controls.Add(uc);
                f.ShowDialog(this.FindForm());
            }
        }

        private void Approve()
        {
            var it = Current();
            if (it == null) return;
            if (it.Status != OrderStatus.Pending)
            {
                MessageBox.Show("Chỉ duyệt đơn trạng thái Pending.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Duyệt đơn hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _svc.Approve(it.Id);
                KeepAndReload(it.Id);
                MessageBox.Show("Đã duyệt đơn.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void Reject()
        {
            var it = Current();
            if (it == null || it.Status != OrderStatus.Pending)
            {
                MessageBox.Show("Chỉ từ chối đơn trạng thái 'Chờ duyệt'.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ucReject = new ucOrderReject_Admin();
            ucReject.LoadOrderInfo(it.OrderNo);

            using (var popup = new Form())
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.FormBorderStyle = FormBorderStyle.None;
                popup.Size = ucReject.Size;
                popup.Controls.Add(ucReject);
                ucReject.Dock = DockStyle.Fill;

                ucReject.Confirmed += (sender, reason) =>
                {
                    try
                    {
                        _svc.Reject(it.Id, reason);
                        KeepAndReload(it.Id);
                        MessageBox.Show("Đã từ chối đơn hàng thành công.", "LMS");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi");
                    }
                };

                ucReject.Cancelled += (sender, e) => { };

                popup.ShowDialog(this.FindForm());
            }
        }

        private void Delete()
        {
            var it = Current();
            if (it == null) return;

            var note =
                "- Chỉ xoá được đơn 'Đã hủy' hoặc 'Hoàn thành'.\n" +
                "- Xóa đơn hàng sẽ XÓA VĨNH VIỄN (không thể khôi phục).\n\n" +
                "Bạn có chắc muốn xoá đơn này?";

            if (MessageBox.Show(note, "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                _svc.Delete(it.Id);
                LoadData();
                MessageBox.Show("Đã xoá đơn.", "LMS");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void CreateShipment()
        {
            var it = Current();
            if (it == null) return;

            if (it.Status != OrderStatus.Approved)
            {
                MessageBox.Show("Chỉ tạo Shipment cho đơn đã được duyệt.", "LMS",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? selectedDriverId = null;

            using (var fPicker = new Form
            {
                Text = "Chọn Tài Xế Cho Chuyến Hàng",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(583, 539),
                FormBorderStyle = FormBorderStyle.None,
            })
            {
                var ucPicker = new ucDriverPicker_Admin(DriverPickerMode.CreateShipment);

                ucPicker.DriverSelected += (selectedId) =>
                {
                    selectedDriverId = selectedId;
                    fPicker.DialogResult = DialogResult.OK;
                };

                fPicker.Controls.Add(ucPicker);
                ucPicker.Dock = DockStyle.Fill;

                if (fPicker.ShowDialog(this.FindForm()) == DialogResult.OK && selectedDriverId.HasValue)
                {
                    try
                    {
                        int shipId = _svc.CreateShipmentFromOrder(it.Id, selectedDriverId.Value);
                        KeepAndReload(it.Id);
                        MessageBox.Show($"Đã tạo Shipment SHP{shipId}.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tạo Shipment: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dgvOrders_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvOrders == null || _binding == null || _binding.Count == 0) return;

            var column = dgvOrders.Columns[e.ColumnIndex];
            if (column.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == column)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = column;
            }

            var property = typeof(OrderListItemDto).GetProperty(column.DataPropertyName);
            if (property != null)
            {
                IEnumerable<OrderListItemDto> sortedList =
                    (_sortOrder == SortOrder.Ascending)
                        ? _binding.OrderBy(x => property.GetValue(x, null))
                        : _binding.OrderByDescending(x => property.GetValue(x, null));

                var newList = sortedList.ToList();
                _binding = new BindingList<OrderListItemDto>(newList);

                dgvOrders.DataSource = null;
                dgvOrders.DataSource = _binding;
                column.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null && _sortedColumn.HeaderCell != null)
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
        }

        private void dgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                if (e.Value is OrderStatus status)
                {
                    switch (status)
                    {
                        case OrderStatus.Pending: e.Value = "Chờ duyệt"; break;
                        case OrderStatus.Approved: e.Value = "Đã duyệt"; break;
                        case OrderStatus.Completed: e.Value = "Hoàn thành"; break;
                        case OrderStatus.Cancelled: e.Value = "Đã hủy"; break;
                        default: e.Value = status.ToString(); break;
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        public void SelectOrderByNo(string orderNo)
        {
            if (string.IsNullOrWhiteSpace(orderNo) || dgvOrders == null) return;

            foreach (DataGridViewRow r in dgvOrders.Rows)
            {
                var ordNo = r.Cells["OrderNo"]?.Value?.ToString();
                if (string.Equals(ordNo, orderNo, StringComparison.OrdinalIgnoreCase))
                {
                    dgvOrders.ClearSelection();
                    r.Selected = true;
                    dgvOrders.CurrentCell = r.Cells["OrderNo"];
                    break;
                }
            }
        }
    }
}
