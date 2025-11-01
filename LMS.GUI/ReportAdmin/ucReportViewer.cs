using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LMS.BUS.Dtos;
using static LMS.BUS.Dtos.ChartDataPoint;

// Alias DTO chi tiết dùng trong RDLC (đổi cho khớp dự án của bạn)
using ShipmentDetailDto1 = LMS.BUS.Dtos.ShipmentDetailDto1;
using CustomerOrderDetailDto = LMS.BUS.Dtos.CustomerOrderDetailDto;
using DriverShipmentDetailDto = LMS.BUS.Dtos.DriverShipmentDetailDto;
using TimeSeriesDataPoint = LMS.BUS.Dtos.TimeSeriesDataPoint;
using TopCustomerDto = LMS.BUS.Dtos.ChartDataPoint.TopCustomerDto;
using TopDriverDto = LMS.BUS.Dtos.ChartDataPoint.TopDriverDto;

namespace LMS.GUI.ReportAdmin
{
    public partial class ucReportViewer : UserControl
    {
        // --- Drag state (kéo Form qua pnlTop) ---
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucReportViewer()
        {
            InitializeComponent();

            // Chống flicker
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            // Gắn event export
            if (btnExportPdf != null) btnExportPdf.Click += btnExportPdf_Click;
            if (btnExportExcel != null) btnExportExcel.Click += btnExportExcel_Click;

            // Kéo form bằng pnlTop
            if (pnlTop != null)
            {
                pnlTop.MouseDown += PnlTop_MouseDown;
                pnlTop.MouseMove += PnlTop_MouseMove;
                pnlTop.MouseUp += PnlTop_MouseUp;
            }
        }

        // ======== Public API ========
        public string ReportTitle
        {
            get { return lblReportTitle != null ? lblReportTitle.Text : ""; }
            set
            {
                if (lblReportTitle != null)
                    lblReportTitle.Text = string.IsNullOrEmpty(value) ? "Báo cáo" : value;
            }
        }

        // -------- Tổng quan / Trạng thái đơn hàng --------
        public void LoadOrderStatusReport(
            List<ChartDataPoint> summary,
            List<OrderStatusDetailDto> details,
            string dateRange)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                if (summary == null) summary = new List<ChartDataPoint>();
                if (details == null) details = new List<OrderStatusDetailDto>();

                // RDLC
                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_OrderStatus.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_OrderStatus", summary));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_OrderDetails", details));

                SetParamsSafe("Báo Cáo Tình Trạng Đơn Hàng", dateRange);

                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ShowToolBar = true;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (OrderStatus): " + GetDeepMessage(ex),
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------- Doanh thu --------
        public void LoadRevenueReport(
            List<TimeSeriesDataPoint> summaryData,
            List<RevenueDetailDto> detailData,
            string dateRange)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                if (summaryData == null) summaryData = new List<TimeSeriesDataPoint>();
                if (detailData == null) detailData = new List<RevenueDetailDto>();

                // RDLC (bản bạn upload dùng DataSet_Summary / DataSet_Details)
                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_RevenueReport.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet_Summary", summaryData));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet_Details", detailData));

                SetParamsSafe("Báo Cáo Doanh Thu", dateRange);

                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ShowToolBar = true;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (Revenue): " + GetDeepMessage(ex),
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------- Vận hành --------
        public void LoadOperationsReport(
            List<ChartDataPoint> shipmentStatus,
            List<ChartDataPoint> topRoutes,
            List<ShipmentDetailDto1> details,
            string dateRange)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                if (shipmentStatus == null) shipmentStatus = new List<ChartDataPoint>();
                if (topRoutes == null) topRoutes = new List<ChartDataPoint>();
                if (details == null) details = new List<ShipmentDetailDto1>();

                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_Operations.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_ShipmentStatus", shipmentStatus));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_TopRoutes", topRoutes));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_Details", details));

                SetParamsSafe("Báo Cáo Vận Hành", dateRange);

                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ShowToolBar = true;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (Operations): " + GetDeepMessage(ex),
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------- Khách hàng --------
        public void LoadCustomersReport(
            List<TopCustomerDto> rows,
            List<CustomerOrderDetailDto> details,
            string dateRange)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                if (rows == null) rows = new List<TopCustomerDto>();
                if (details == null) details = new List<CustomerOrderDetailDto>();

                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_Customers.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_Customers", rows));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_Details", details));

                SetParamsSafe("Báo Cáo Khách Hàng", dateRange);

                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ShowToolBar = true;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (Customers): " + GetDeepMessage(ex),
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------- Tài xế --------
        public void LoadDriversReport(
            List<TopDriverDto> rows,
            List<DriverShipmentDetailDto> details,
            string dateRange)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                if (rows == null) rows = new List<TopDriverDto>();
                if (details == null) details = new List<DriverShipmentDetailDto>();

                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_Drivers.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_Drivers", rows));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_Details", details));

                SetParamsSafe("Báo Cáo Tài Xế", dateRange);

                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ShowToolBar = true;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (Drivers): " + GetDeepMessage(ex),
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ======== Export ========
        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            ExportReport("PDF", "PDF files (*.pdf)|*.pdf", "pdf");
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportReport("EXCELOPENXML", "Excel files (*.xlsx)|*.xlsx", "xlsx");
        }

        private void ExportReport(string format, string filter, string ext)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, filenameExtension;

                byte[] bytes = reportViewer1.LocalReport.Render(
                    format, null, out mimeType, out encoding,
                    out filenameExtension, out streamids, out warnings);

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = filter;
                    sfd.Title = "Lưu báo cáo";
                    sfd.FileName = "BaoCao_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "." + ext;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, bytes);
                        MessageBox.Show("Xuất báo cáo thành công!", "OK",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + GetDeepMessage(ex),
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ======== Helper: set parameters an toàn ========
        private void SetParamsSafe(string defaultTitle, string dateRange)
        {
            try
            {
                string title = string.IsNullOrEmpty(this.ReportTitle) ? defaultTitle : this.ReportTitle;
                ReportParameter p1 = new ReportParameter("pReportTitle", title);
                ReportParameter p2 = new ReportParameter("pDateRange", dateRange ?? "");
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });
            }
            catch
            {
                // RDLC không khai báo các parameter thì bỏ qua
            }
        }

        private static string GetDeepMessage(Exception ex)
        {
            return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }

        // ======== Drag parent Form via pnlTop ========
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Form parentForm = this.FindForm();
            if (parentForm == null) return;
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = parentForm.Location;
        }

        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            Form parentForm = this.FindForm();
            if (parentForm == null) return;
            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            parentForm.Location = Point.Add(dragFormPoint, new Size(dif));
        }

        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) dragging = false;
        }
    }
}
