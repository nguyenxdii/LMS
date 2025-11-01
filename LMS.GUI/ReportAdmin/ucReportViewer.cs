using LMS.BUS.Dtos;                 // ChartDataPoint
using Microsoft.Reporting.WinForms; // ReportViewer
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static LMS.BUS.Dtos.ChartDataPoint;

// Alias cho các DTO chi tiết dùng trong RDLC
using ShipmentDetailDto = LMS.BUS.Dtos.ShipmentDetailDto1;
using CustomerOrderDetailDto = LMS.BUS.Dtos.CustomerOrderDetailDto;
using DriverShipmentDetailDto = LMS.BUS.Dtos.DriverShipmentDetailDto;
using TopCustomerDto = LMS.BUS.Dtos.ChartDataPoint.TopCustomerDto;
using TopDriverDto = LMS.BUS.Dtos.ChartDataPoint.TopDriverDto;

namespace LMS.GUI.ReportAdmin
{
    public partial class ucReportViewer : UserControl
    {
        // --- DRAG STATE ---
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucReportViewer()
        {
            InitializeComponent();

            // chống mờ, chống flicker
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            // Gắn event export
            btnExportPdf.Click += btnExportPdf_Click;
            btnExportExcel.Click += btnExportExcel_Click;

            // Kéo form bằng pnlTop
            pnlTop.MouseDown += PnlTop_MouseDown;
            pnlTop.MouseMove += PnlTop_MouseMove;
            pnlTop.MouseUp += PnlTop_MouseUp;


        }

        // Cho phép set tiêu đề từ ngoài
        public string ReportTitle
        {
            get => lblReportTitle.Text;
            set => lblReportTitle.Text = value ?? "Báo cáo";
        }

        public void LoadOrderStatusReport(
                                        List<ChartDataPoint> summary,
                                        List<OrderStatusDetailDto> details,
                                        string dateRange)
        {
            try
            {
                // 1) Reset cứng để xóa “dư âm” RDLC trước (rất quan trọng khi đổi tab/rdlc)
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                // 2) Ép danh sách != null
                if (summary == null) summary = new List<ChartDataPoint>();
                if (details == null) details = new List<OrderStatusDetailDto>();


                // 3) Trỏ đúng RDLC + nạp DataSets
                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_OrderStatus.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_OrderStatus", summary));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_OrderDetails", details));

                // 4) Tuỳ chọn hiển thị
                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ZoomPercent = 100;
                reportViewer1.ShowToolBar = false;

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (OrderStatus): " +
                                (ex.InnerException?.Message ?? ex.Message),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadRevenueReport(
            List<TimeSeriesDataPoint> summaryData,
            List<RevenueDetailDto> detailData,
            string dateRange)
        {
            try
            {
                // 1) Reset để xóa cấu hình RDLC cũ
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                // 2) Chống null
                if (summaryData == null) summaryData = new List<TimeSeriesDataPoint>();
                if (detailData == null) detailData = new List<RevenueDetailDto>();

                // 3) Trỏ RDLC + DataSets (đúng tên trong RDLC)
                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_RevenueReport.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet_Summary", summaryData));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet_Details", detailData));

                // 4) Parameters (nếu RDLC có)
                //var param = new[]
                //{
                //    new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Doanh Thu"),
                //    new ReportParameter("pDateRange",  dateRange ?? "")
                //};
                // Nếu RDLC CHƯA tạo 2 parameter trên, comment dòng dưới:
                //reportViewer1.LocalReport.SetParameters(param);

                try
                {
                    var param = new[]
                    {
                        new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Doanh Thu"),
                        new ReportParameter("pDateRange",  dateRange ?? "")
                    };
                    reportViewer1.LocalReport.SetParameters(param);
                }
                catch
                {
                    // RDLC chưa có 2 tham số này thì bỏ qua
                }

                // 5) Tuỳ chọn hiển thị
                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ZoomPercent = 100;
                reportViewer1.ShowToolBar = true; // báo cáo doanh thu có thể cần toolbar

                reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (Revenue): " +
                                (ex.InnerException?.Message ?? ex.Message),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void LoadOperationsReport(
            List<ChartDataPoint> shipmentStatus,
            List<ChartDataPoint> topRoutes,
            List<ShipmentDetailDto> details,   // <== THÊM
            string dateRange)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                shipmentStatus = shipmentStatus ?? new List<ChartDataPoint>();
                topRoutes = topRoutes ?? new List<ChartDataPoint>();
                details = details ?? new List<ShipmentDetailDto>(); // <== THÊM

                reportViewer1.LocalReport.ReportEmbeddedResource =
                    "LMS.GUI.ReportAdmin.rpt_Operations.rdlc";

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource("DS_ShipmentStatus", shipmentStatus));
                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource("DS_TopRoutes", topRoutes));
                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource("DS_Details", details)); // <== THÊM


                var param = new[]
                {
                    new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Vận Hành"),
                    new ReportParameter("pDateRange",  dateRange ?? "")
                };
                reportViewer1.LocalReport.SetParameters(param);

                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ZoomPercent = 100;
                reportViewer1.ShowToolBar = true;

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (Operations): " +
                                (ex.InnerException?.Message ?? ex.Message),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadCustomersReport(
            List<ChartDataPoint.TopCustomerDto> rows,
            List<CustomerOrderDetailDto> details,   // <== THÊM
            string dateRange)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                rows = rows ?? new List<ChartDataPoint.TopCustomerDto>();
                details = details ?? new List<CustomerOrderDetailDto>(); // <== THÊM

                reportViewer1.LocalReport.ReportEmbeddedResource =
                    "LMS.GUI.ReportAdmin.rpt_Customers.rdlc";

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource("DS_Customers", rows));
                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource("DS_Details", details)); // <== THÊM

                var param = new[]
                {
            new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Khách Hàng"),
            new ReportParameter("pDateRange",  dateRange ?? "")
        };
                reportViewer1.LocalReport.SetParameters(param);

                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ZoomPercent = 100;
                reportViewer1.ShowToolBar = true;

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (Customers): " +
                                (ex.InnerException?.Message ?? ex.Message),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadDriversReport(
            List<ChartDataPoint.TopDriverDto> rows,
            List<DriverShipmentDetailDto> details,  // <== THÊM
            string dateRange)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                rows = rows ?? new List<ChartDataPoint.TopDriverDto>();
                details = details ?? new List<DriverShipmentDetailDto>(); // <== THÊM

                reportViewer1.LocalReport.ReportEmbeddedResource =
                    "LMS.GUI.ReportAdmin.rpt_Drivers.rdlc";

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource("DS_Drivers", rows));
                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource("DS_Details", details)); // <== THÊM

                var param = new[]
                {
            new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Tài Xế"),
            new ReportParameter("pDateRange",  dateRange ?? "")
        };
                reportViewer1.LocalReport.SetParameters(param);

                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.ZoomPercent = 100;
                reportViewer1.ShowToolBar = true;

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo (Drivers): " +
                                (ex.InnerException?.Message ?? ex.Message),
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

        private void ExportReport(string format, string filter, string extension)
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
                    sfd.Title = $"Lưu báo cáo dưới dạng {extension.ToUpper()}";
                    sfd.FileName = $"BaoCao_DonHang_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, bytes);
                        MessageBox.Show("Xuất báo cáo thành công!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ======== Drag parent Form via pnlTop ========
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var parentForm = this.FindForm();
            if (parentForm == null) return;
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = parentForm.Location;
        }

        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            var parentForm = this.FindForm();
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
