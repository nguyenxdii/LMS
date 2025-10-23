using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMS.BUS.Dtos;


namespace LMS.GUI.OrderDriver
{
    public partial class ucMyShipments_Drv : UserControl
    {
        private readonly DriverShipmentService _svc = new DriverShipmentService();

        public ucMyShipments_Drv()
        {
            InitializeComponent();
            this.Load += (s, e) => { ConfigureGrids(); InitFilters(); LoadActive(); LoadAll(); Wire(); };

        }
        //private void ConfigureGrids()
        //{
        //    // ACTIVE
        //    dgvActive.Columns.Clear();
        //    dgvActive.AutoGenerateColumns = false;
        //    dgvActive.AllowUserToAddRows = false;
        //    dgvActive.ReadOnly = true;
        //    dgvActive.RowHeadersVisible = false;
        //    dgvActive.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //    dgvActive.MultiSelect = false;
        //    dgvActive.EnableHeadersVisualStyles = false;
        //    dgvActive.GridColor = Color.Gainsboro;
        //    dgvActive.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        //    dgvActive.BorderStyle = BorderStyle.FixedSingle;
        //    dgvActive.ColumnHeadersHeight = 36;
        //    dgvActive.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
        //    dgvActive.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        //    dgvActive.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

        //    dgvActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
        //    dgvActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã CH" });
        //    dgvActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã đơn" });
        //    dgvActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến" });
        //    dgvActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái" });
        //    dgvActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Cập nhật" });

        //    // ALL
        //    dgvAll.Columns.Clear();
        //    dgvAll.AutoGenerateColumns = false;
        //    dgvAll.AllowUserToAddRows = false;
        //    dgvAll.ReadOnly = true;
        //    dgvAll.RowHeadersVisible = false;
        //    dgvAll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //    dgvAll.MultiSelect = false;
        //    dgvAll.EnableHeadersVisualStyles = false;
        //    dgvAll.GridColor = Color.Gainsboro;
        //    dgvAll.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        //    dgvAll.BorderStyle = BorderStyle.FixedSingle;
        //    dgvAll.ColumnHeadersHeight = 36;
        //    dgvAll.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
        //    dgvAll.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        //    dgvAll.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã CH" });
        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã đơn" });
        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến" });
        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái" });
        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stops", DataPropertyName = "Stops", HeaderText = "Số chặng" });
        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartedAt", DataPropertyName = "StartedAt", HeaderText = "Bắt đầu" });
        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeliveredAt", DataPropertyName = "DeliveredAt", HeaderText = "Kết thúc" });
        //    dgvAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Duration", DataPropertyName = "Duration", HeaderText = "Thời gian" });

        //    dgvActive.CellDoubleClick += (s, e) => OpenDetailFromGrid(dgvActive, e.RowIndex);
        //    dgvAll.CellDoubleClick += (s, e) => OpenDetailFromGrid(dgvAll, e.RowIndex);
        //}
        private void ConfigureGrids()
        {
            // helper áp style chung
            Action<DataGridView> styleGrid = g =>
            {
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

                // double buffer
                try
                {
                    g.GetType().GetProperty("DoubleBuffered",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                     ?.SetValue(g, true, null);
                }
                catch { }
            };

            // ===== ACTIVE =====
            styleGrid(dgvActive);

            dgvActive.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            dgvActive.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ShipmentNo",
                DataPropertyName = "ShipmentNo",
                HeaderText = "Mã CH",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvActive.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                DataPropertyName = "OrderNo",
                HeaderText = "Mã đơn",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvActive.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Route",
                DataPropertyName = "Route",
                HeaderText = "Tuyến",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvActive.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvActive.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UpdatedAt",
                DataPropertyName = "UpdatedAt",
                HeaderText = "Cập nhật",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // ===== ALL =====
            styleGrid(dgvAll);

            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ShipmentNo",
                DataPropertyName = "ShipmentNo",
                HeaderText = "Mã CH",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                DataPropertyName = "OrderNo",
                HeaderText = "Mã đơn",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Route",
                DataPropertyName = "Route",
                HeaderText = "Tuyến",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Stops",
                DataPropertyName = "Stops",
                HeaderText = "Số chặng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartedAt",
                DataPropertyName = "StartedAt",
                HeaderText = "Bắt đầu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DeliveredAt",
                DataPropertyName = "DeliveredAt",
                HeaderText = "Kết thúc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvAll.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Duration",
                DataPropertyName = "Duration",
                HeaderText = "Thời gian",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                // TimeSpan => định dạng hh:mm:ss
                DefaultCellStyle = { Format = @"hh\:mm\:ss", Alignment = DataGridViewContentAlignment.MiddleCenter, NullValue = "" }
            });

            // double click mở chi tiết
            dgvActive.CellDoubleClick += (s, e) => OpenDetailFromGrid(dgvActive, e.RowIndex);
            dgvAll.CellDoubleClick += (s, e) => OpenDetailFromGrid(dgvAll, e.RowIndex);
        }


        private int DriverId => AppSession.DriverId ?? 0;

        private void InitFilters()
        {
            dtFrom.Value = DateTime.Today.AddDays(-30);
            dtTo.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Tất cả");
            foreach (var st in Enum.GetNames(typeof(ShipmentStatus))) cmbStatus.Items.Add(st);
            cmbStatus.SelectedIndex = 0;
        }

        private void Wire()
        {
            btnReload.Click += (s, e) => { LoadActive(); LoadAll(); };
            btnReceive.Click += (s, e) => ReceiveSelected();

            dtFrom.ValueChanged += (s, e) => LoadAll();
            dtTo.ValueChanged += (s, e) => LoadAll();
            cmbStatus.SelectedIndexChanged += (s, e) => LoadAll();

            dgvActive.SelectionChanged += (s, e) => UpdateButtons();
            dgvAll.SelectionChanged += (s, e) => UpdateButtons();
        }

        private void LoadActive()
        {
            var data = _svc.GetAssignedAndRunning(DriverId);
            //dgvActive.DataSource = new BindingList<ShipmentDetailDto>(data);
            dgvActive.DataSource = new BindingList<ShipmentRowDto>(data);
            UpdateButtons();
        }

        private void LoadAll()
        {
            DateTime? from = dtFrom.Value.Date;
            DateTime? to = dtTo.Value.Date.AddDays(1).AddTicks(-1);
            ShipmentStatus? st = null;
            if (cmbStatus.SelectedIndex > 0)
                st = (ShipmentStatus)Enum.Parse(typeof(ShipmentStatus), cmbStatus.SelectedItem.ToString());

            var data = _svc.GetAllMine(DriverId, from, to, st);
            //dgvAll.DataSource = new BindingList<object>(data);
            dgvAll.DataSource = new BindingList<ShipmentRowDto>(data);

        }

        //private (int id, string status)? CurrentShipmentFrom(DataGridView grid)
        //{
        //    if (grid.CurrentRow?.DataBoundItem == null) return null;
        //    dynamic it = grid.CurrentRow.DataBoundItem;
        //    return ((int)it.Id, (string)it.Status);
        //}
        private (int id, string status)? CurrentShipmentFrom(DataGridView grid)
        {
            var it = grid.CurrentRow?.DataBoundItem as ShipmentRowDto;
            if (it == null) return null;
            return (it.Id, it.Status);
        }


        private void ReceiveSelected()
        {
            var cur = CurrentShipmentFrom(dgvActive) ?? CurrentShipmentFrom(dgvAll);
            if (cur == null) return;

            try
            {
                if (cur.Value.status != ShipmentStatus.Pending.ToString())
                {
                    MessageBox.Show("Chỉ nhận shipment trạng thái Pending.", "LMS");
                    return;
                }

                _svc.ReceiveShipment(cur.Value.id, DriverId);
                LoadActive(); LoadAll();
                MessageBox.Show("Đã nhận shipment.", "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void UpdateButtons()
        {
            var cur = CurrentShipmentFrom(dgvActive) ?? CurrentShipmentFrom(dgvAll);
            bool canReceive = cur != null && cur.Value.status == ShipmentStatus.Pending.ToString();
            btnReceive.Enabled = canReceive;
        }

        //private void OpenDetailFromGrid(DataGridView grid, int rowIndex)
        //{
        //    if (rowIndex < 0) return;
        //    var id = (int)((dynamic)grid.Rows[rowIndex].DataBoundItem).Id;

        //    using (var f = new Form
        //    {
        //        Text = "Chi tiết Shipment",
        //        StartPosition = FormStartPosition.CenterParent,
        //        Size = new Size(1000, 650)
        //    })
        //    {
        //        var uc = new ucShipmentDetail_Drv { Dock = DockStyle.Fill };
        //        uc.LoadShipment(id);
        //        f.Controls.Add(uc);
        //        f.ShowDialog(this.FindForm());
        //    }

        //    LoadActive(); LoadAll();
        //}
        private void OpenDetailFromGrid(DataGridView grid, int rowIndex)
        {
            if (rowIndex < 0) return;
            var it = grid.Rows[rowIndex].DataBoundItem as ShipmentRowDto;
            if (it == null) return;
            int id = it.Id;

            using (var f = new Form
            {
                Text = "Chi tiết Shipment",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(1000, 650)
            })
            {
                var uc = new ucShipmentDetail_Drv { Dock = DockStyle.Fill };
                uc.LoadShipment(id);
                f.Controls.Add(uc);
                f.ShowDialog(this.FindForm());
            }

            LoadActive(); LoadAll();
        }

    }
}
