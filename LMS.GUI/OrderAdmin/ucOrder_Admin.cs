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
using System.Reflection;
using LMS.BUS.Helpers;

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

        #region Grid config
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
            // Thêm các cột với SortMode = Programmatic
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
                Name = "Status", // Giữ nguyên Name là "Status"
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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter },
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            // double buffer
            try
            {
                var property = g.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (property != null)
                {
                    property.SetValue(g, true, null);
                }
            }
            catch { }
        }
        #endregion

        #region Load + helpers
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
                    if (dgvOrders.Columns.Contains("OrderNo")) // Kiểm tra cột tồn tại
                        dgvOrders.CurrentCell = dgvOrders.Rows[0].Cells["OrderNo"];
                }
                UpdateButtonsState();
                ResetSortGlyphs(); // Reset mũi tên sort khi load lại
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
                        if (dgvOrders.Columns.Contains("OrderNo")) // Kiểm tra cột tồn tại
                            dgvOrders.CurrentCell = r.Cells["OrderNo"];
                        if (r.Index >= 0 && r.Index < dgvOrders.RowCount) // Đảm bảo index hợp lệ
                            dgvOrders.FirstDisplayedScrollingRowIndex = r.Index;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Wire events
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
            dgvOrders.ColumnHeaderMouseClick += dgvOrders_ColumnHeaderMouseClick; // Sự kiện sort
            dgvOrders.CellFormatting += dgvOrders_CellFormatting; // Thêm sự kiện format Status
            dgvOrders.CellDoubleClick += (s, e) =>
            {
                // Chỉ xử lý khi double click vào một dòng dữ liệu (không phải header)
                if (e.RowIndex >= 0)
                {
                    ShowDetail(); // Gọi lại hàm ShowDetail() đã có sẵn
                }
            };
        }

        private void UpdateButtonsState()
        {
            var it = Current();
            bool enable = (it != null);

            btnApprove.Enabled = enable && it.Status == OrderStatus.Pending;
            btnReject.Enabled = enable && it.Status == OrderStatus.Pending;
            btnDelete.Enabled = enable && (it.Status == OrderStatus.Pending || it.Status == OrderStatus.Cancelled);
            btnViewDetail.Enabled = enable;
            btnShipment.Enabled = enable && it.Status == OrderStatus.Approved;
        }
        #endregion

        #region Actions
        private void OpenSearchDialog()
        {
            int? pickedId = null;
            using (var dlg = new Form
            {
                Text = "Tìm đơn hàng",
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(1199, 689)
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
                StartPosition = FormStartPosition.CenterScreen, // Sửa CenterParent thành CenterScreen
                Size = new Size(500, 500),
                FormBorderStyle = FormBorderStyle.None,
                //MaximizeBox = false,
                //MinimizeBox = false
            })
            {
                var uc = new ucOrderDetail_Admin(it.Id)
                {
                    Dock = DockStyle.Fill
                };
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
                MessageBox.Show("Đã duyệt đơn.", "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void Reject()
        {
            var it = Current();
            if (it == null) return;
            if (it.Status != OrderStatus.Pending)
            {
                MessageBox.Show("Chỉ từ chối đơn trạng thái Pending.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Từ chối đơn hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
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
            var it = Current();
            if (it == null) return;
            var note =
                "- Chỉ xoá được đơn Pending/Cancelled.\n" +
                "- Nếu đơn đã gắn Shipment hoặc đang/đã vận chuyển thì KHÔNG thể xoá.\n\n" +
                "Bạn có chắc muốn xoá đơn này?";
            if (MessageBox.Show(note, "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                _svc.Delete(it.Id);
                LoadData(); // Load lại từ đầu sau khi xóa
                MessageBox.Show("Đã xoá đơn.", "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
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

            // === SỬA TỪ ĐÂY: Mở popup UserControl thay vì gọi PickDriverId() ===

            int? selectedDriverId = null; // Biến để lưu ID tài xế được chọn

            // Tạo Form popup để chứa UC chọn tài xế
            using (var fPicker = new Form
            {
                Text = "Chọn Tài Xế Cho Chuyến Hàng",
                StartPosition = FormStartPosition.CenterParent,
                // Đặt kích thước phù hợp với UserControl chọn tài xế của bạn
                Size = new Size(583, 539),
                FormBorderStyle = FormBorderStyle.None, // Hoặc .None
            })
            {
                var ucPicker = new ucDriverPicker_Admin();

                ucPicker.DriverSelected += (selectedId) =>
                {
                    selectedDriverId = selectedId;
                    fPicker.DialogResult = DialogResult.OK; // Đóng popup và báo thành công
                };

                // Thêm UserControl vào Form popup
                fPicker.Controls.Add(ucPicker);

                // Hiển thị popup và chờ người dùng chọn
                if (fPicker.ShowDialog(this.FindForm()) == DialogResult.OK && selectedDriverId.HasValue)
                {
                    // Người dùng đã chọn một tài xế -> Tiếp tục tạo Shipment
                    try
                    {
                        int shipId = _svc.CreateShipmentFromOrder(it.Id, selectedDriverId.Value);
                        KeepAndReload(it.Id); // Tải lại và giữ focus
                        MessageBox.Show($"Đã tạo Shipment SHP{shipId}.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tạo Shipment: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // else: Người dùng đóng popup mà không chọn -> Không làm gì cả
            }
            // === KẾT THÚC SỬA ===
        }
        #endregion

        #region Sort functionality
        private void dgvOrders_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Kiểm tra null cho dgvOrders và _binding trước khi sử dụng
            if (dgvOrders == null || _binding == null || _binding.Count == 0) return;

            var column = dgvOrders.Columns[e.ColumnIndex];
            if (column.SortMode == DataGridViewColumnSortMode.NotSortable) return;

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
                IEnumerable<OrderListItemDto> sortedList; // Đổi IOrderedEnumerable thành IEnumerable
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = _binding.OrderBy(x => property.GetValue(x, null)); // Thêm null vào GetValue
                else
                    sortedList = _binding.OrderByDescending(x => property.GetValue(x, null)); // Thêm null vào GetValue

                // Cập nhật binding với danh sách đã sắp xếp
                var newList = sortedList.ToList(); // Chuyển thành List trước
                _binding = new BindingList<OrderListItemDto>(newList); // Tạo BindingList mới

                // Gán lại DataSource và cập nhật mũi tên
                dgvOrders.DataSource = null; // Gán null trước để refresh
                dgvOrders.DataSource = _binding;
                column.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null && _sortedColumn.HeaderCell != null) // Thêm kiểm tra HeaderCell
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
        }
        #endregion

        #region Cell Formatting for Status
        // Thêm hàm xử lý format Status sang tiếng Việt
        private void dgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra xem có phải cột "Status" không
            if (dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                // Kiểm tra xem giá trị có phải là OrderStatus không
                if (e.Value is OrderStatus status)
                {
                    // Chuyển đổi enum sang tiếng Việt
                    switch (status)
                    {
                        case OrderStatus.Pending: e.Value = "Chờ duyệt"; break;
                        case OrderStatus.Approved: e.Value = "Đã duyệt"; break;
                        case OrderStatus.Completed: e.Value = "Hoàn thành"; break;
                        case OrderStatus.Cancelled: e.Value = "Đã hủy"; break;
                        default: e.Value = status.ToString(); break; // Giữ nguyên nếu không khớp
                    }
                    e.FormattingApplied = true; // Báo cho grid biết là đã xử lý xong
                }
            }
        }
        #endregion
    }
}