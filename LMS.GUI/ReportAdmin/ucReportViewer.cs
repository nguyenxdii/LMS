//using LMS.BUS.Dtos;                 // ChartDataPoint
//using Microsoft.Reporting.WinForms; // ReportViewer
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Windows.Forms;

//// Alias cho các DTO chi tiết dùng trong RDLC
//using ShipmentDetailDto = LMS.BUS.Dtos.ShipmentDetailDto1;
//using CustomerOrderDetailDto = LMS.BUS.Dtos.CustomerOrderDetailDto;
//using DriverShipmentDetailDto = LMS.BUS.Dtos.DriverShipmentDetailDto;
//using TopCustomerDto = LMS.BUS.Dtos.ChartDataPoint.TopCustomerDto;
//using TopDriverDto = LMS.BUS.Dtos.ChartDataPoint.TopDriverDto;

//// DTO ĐỨNG RIÊNG (không nằm trong ChartDataPoint)
//using TimeSeriesDataPoint = LMS.BUS.Dtos.TimeSeriesDataPoint;
//using OrderStatusDetailDto = LMS.BUS.Dtos.OrderStatusDetailDto;
//using RevenueDetailDto = LMS.BUS.Dtos.RevenueDetailDto;

//namespace LMS.GUI.ReportAdmin
//{
//    public partial class ucReportViewer : UserControl
//    {
//        // --- DRAG STATE ---
//        private bool dragging = false;
//        private Point dragCursorPoint;
//        private Point dragFormPoint;

//        public ucReportViewer()
//        {
//            InitializeComponent();

//            // chống mờ, chống flicker
//            this.DoubleBuffered = true;
//            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
//            this.UpdateStyles();

//            // Gắn event export
//            btnExportPdf.Click += btnExportPdf_Click;
//            btnExportExcel.Click += btnExportExcel_Click;

//            // Kéo form bằng pnlTop
//            pnlTop.MouseDown += PnlTop_MouseDown;
//            pnlTop.MouseMove += PnlTop_MouseMove;
//            pnlTop.MouseUp += PnlTop_MouseUp;


//        }

//        // Cho phép set tiêu đề từ ngoài
//        public string ReportTitle
//        {
//            get => lblReportTitle.Text;
//            set => lblReportTitle.Text = value ?? "Báo cáo";
//        }

//        public void LoadOrderStatusReport(
//                                        List<ChartDataPoint> summary,
//                                        List<OrderStatusDetailDto> details,
//                                        string dateRange)
//        {
//            try
//            {
//                // 1) Reset cứng để xóa “dư âm” RDLC trước (rất quan trọng khi đổi tab/rdlc)
//                reportViewer1.Reset();
//                reportViewer1.ProcessingMode = ProcessingMode.Local;

//                // 2) Ép danh sách != null
//                if (summary == null) summary = new List<ChartDataPoint>();
//                if (details == null) details = new List<OrderStatusDetailDto>();


//                // 3) Trỏ đúng RDLC + nạp DataSets
//                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_OrderStatus.rdlc";
//                reportViewer1.LocalReport.DataSources.Clear();
//                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_OrderStatus", summary));
//                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_OrderDetails", details));

//                // 4) Tuỳ chọn hiển thị
//                reportViewer1.ZoomMode = ZoomMode.PageWidth;
//                reportViewer1.ZoomPercent = 100;
//                reportViewer1.ShowToolBar = false;

//                reportViewer1.RefreshReport();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi nạp báo cáo (OrderStatus): " +
//                                (ex.InnerException?.Message ?? ex.Message),
//                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        public void LoadRevenueReport(
//            List<TimeSeriesDataPoint> summaryData,
//            List<RevenueDetailDto> detailData,
//            string dateRange)
//        {
//            try
//            {
//                // 1) Reset để xóa cấu hình RDLC cũ
//                reportViewer1.Reset();
//                reportViewer1.ProcessingMode = ProcessingMode.Local;

//                // 2) Chống null
//                if (summaryData == null) summaryData = new List<TimeSeriesDataPoint>();
//                if (detailData == null) detailData = new List<RevenueDetailDto>();

//                // 3) Trỏ RDLC + DataSets (đúng tên trong RDLC)
//                reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_RevenueReport.rdlc";
//                reportViewer1.LocalReport.DataSources.Clear();
//                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet_Summary", summaryData));
//                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet_Details", detailData));

//                // 4) Parameters (nếu RDLC có)
//                //var param = new[]
//                //{
//                //    new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Doanh Thu"),
//                //    new ReportParameter("pDateRange",  dateRange ?? "")
//                //};
//                // Nếu RDLC CHƯA tạo 2 parameter trên, comment dòng dưới:
//                //reportViewer1.LocalReport.SetParameters(param);

//                try
//                {
//                    var param = new[]
//                    {
//                        new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Doanh Thu"),
//                        new ReportParameter("pDateRange",  dateRange ?? "")
//                    };
//                    reportViewer1.LocalReport.SetParameters(param);
//                }
//                catch
//                {
//                    // RDLC chưa có 2 tham số này thì bỏ qua
//                }

//                // 5) Tuỳ chọn hiển thị
//                reportViewer1.ZoomMode = ZoomMode.PageWidth;
//                reportViewer1.ZoomPercent = 100;
//                reportViewer1.ShowToolBar = true; // báo cáo doanh thu có thể cần toolbar

//                reportViewer1.RefreshReport();

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi nạp báo cáo (Revenue): " +
//                                (ex.InnerException?.Message ?? ex.Message),
//                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }


//        public void LoadOperationsReport(
//            List<ChartDataPoint> shipmentStatus,
//            List<ChartDataPoint> topRoutes,
//            List<ShipmentDetailDto> details,   // <== THÊM
//            string dateRange)
//        {
//            try
//            {
//                reportViewer1.Reset();
//                reportViewer1.ProcessingMode = ProcessingMode.Local;

//                shipmentStatus = shipmentStatus ?? new List<ChartDataPoint>();
//                topRoutes = topRoutes ?? new List<ChartDataPoint>();
//                details = details ?? new List<ShipmentDetailDto>(); // <== THÊM

//                reportViewer1.LocalReport.ReportEmbeddedResource =
//                    "LMS.GUI.ReportAdmin.rpt_Operations.rdlc";

//                reportViewer1.LocalReport.DataSources.Clear();
//                reportViewer1.LocalReport.DataSources.Add(
//                    new ReportDataSource("DS_ShipmentStatus", shipmentStatus));
//                reportViewer1.LocalReport.DataSources.Add(
//                    new ReportDataSource("DS_TopRoutes", topRoutes));
//                reportViewer1.LocalReport.DataSources.Add(
//                    new ReportDataSource("DS_Details", details)); // <== THÊM


//                var param = new[]
//                {
//                    new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Vận Hành"),
//                    new ReportParameter("pDateRange",  dateRange ?? "")
//                };
//                reportViewer1.LocalReport.SetParameters(param);

//                reportViewer1.ZoomMode = ZoomMode.PageWidth;
//                reportViewer1.ZoomPercent = 100;
//                reportViewer1.ShowToolBar = true;

//                reportViewer1.RefreshReport();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi nạp báo cáo (Operations): " +
//                                (ex.InnerException?.Message ?? ex.Message),
//                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        public void LoadCustomersReport(
//            List<ChartDataPoint.TopCustomerDto> rows,
//            List<CustomerOrderDetailDto> details,   // <== THÊM
//            string dateRange)
//        {
//            try
//            {
//                reportViewer1.Reset();
//                reportViewer1.ProcessingMode = ProcessingMode.Local;

//                rows = rows ?? new List<ChartDataPoint.TopCustomerDto>();
//                details = details ?? new List<CustomerOrderDetailDto>(); // <== THÊM

//                reportViewer1.LocalReport.ReportEmbeddedResource =
//                    "LMS.GUI.ReportAdmin.rpt_Customers.rdlc";

//                reportViewer1.LocalReport.DataSources.Clear();
//                reportViewer1.LocalReport.DataSources.Add(
//                    new ReportDataSource("DS_Customers", rows));
//                reportViewer1.LocalReport.DataSources.Add(
//                    new ReportDataSource("DS_Details", details)); // <== THÊM

//                var param = new[]
//                {
//            new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Khách Hàng"),
//            new ReportParameter("pDateRange",  dateRange ?? "")
//        };
//                reportViewer1.LocalReport.SetParameters(param);

//                reportViewer1.ZoomMode = ZoomMode.PageWidth;
//                reportViewer1.ZoomPercent = 100;
//                reportViewer1.ShowToolBar = true;

//                reportViewer1.RefreshReport();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi nạp báo cáo (Customers): " +
//                                (ex.InnerException?.Message ?? ex.Message),
//                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        public void LoadDriversReport(
//            List<ChartDataPoint.TopDriverDto> rows,
//            List<DriverShipmentDetailDto> details,  // <== THÊM
//            string dateRange)
//        {
//            try
//            {
//                reportViewer1.Reset();
//                reportViewer1.ProcessingMode = ProcessingMode.Local;

//                rows = rows ?? new List<ChartDataPoint.TopDriverDto>();
//                details = details ?? new List<DriverShipmentDetailDto>(); // <== THÊM

//                reportViewer1.LocalReport.ReportEmbeddedResource =
//                    "LMS.GUI.ReportAdmin.rpt_Drivers.rdlc";

//                reportViewer1.LocalReport.DataSources.Clear();
//                reportViewer1.LocalReport.DataSources.Add(
//                    new ReportDataSource("DS_Drivers", rows));
//                reportViewer1.LocalReport.DataSources.Add(
//                    new ReportDataSource("DS_Details", details)); // <== THÊM

//                var param = new[]
//                {
//            new ReportParameter("pReportTitle", this.ReportTitle ?? "Báo Cáo Tài Xế"),
//            new ReportParameter("pDateRange",  dateRange ?? "")
//        };
//                reportViewer1.LocalReport.SetParameters(param);

//                reportViewer1.ZoomMode = ZoomMode.PageWidth;
//                reportViewer1.ZoomPercent = 100;
//                reportViewer1.ShowToolBar = true;

//                reportViewer1.RefreshReport();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi nạp báo cáo (Drivers): " +
//                                (ex.InnerException?.Message ?? ex.Message),
//                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }



//        // ======== Export ========
//        private void btnExportPdf_Click(object sender, EventArgs e)
//        {
//            ExportReport("PDF", "PDF files (*.pdf)|*.pdf", "pdf");
//        }

//        private void btnExportExcel_Click(object sender, EventArgs e)
//        {
//            ExportReport("EXCELOPENXML", "Excel files (*.xlsx)|*.xlsx", "xlsx");
//        }

//        private void ExportReport(string format, string filter, string extension)
//        {
//            try
//            {
//                Warning[] warnings;
//                string[] streamids;
//                string mimeType, encoding, filenameExtension;

//                byte[] bytes = reportViewer1.LocalReport.Render(
//                    format, null, out mimeType, out encoding,
//                    out filenameExtension, out streamids, out warnings);

//                using (SaveFileDialog sfd = new SaveFileDialog())
//                {
//                    sfd.Filter = filter;
//                    sfd.Title = $"Lưu báo cáo dưới dạng {extension.ToUpper()}";
//                    sfd.FileName = $"BaoCao_DonHang_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";

//                    if (sfd.ShowDialog() == DialogResult.OK)
//                    {
//                        File.WriteAllBytes(sfd.FileName, bytes);
//                        MessageBox.Show("Xuất báo cáo thành công!",
//                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi xuất file: " + ex.Message,
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // ======== Drag parent Form via pnlTop ========
//        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (e.Button != MouseButtons.Left) return;
//            var parentForm = this.FindForm();
//            if (parentForm == null) return;
//            dragging = true;
//            dragCursorPoint = Cursor.Position;
//            dragFormPoint = parentForm.Location;
//        }

//        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (!dragging) return;
//            var parentForm = this.FindForm();
//            if (parentForm == null) return;
//            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
//            parentForm.Location = Point.Add(dragFormPoint, new Size(dif));
//        }

//        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left) dragging = false;
//        }
//    }
//}

using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

// DTO
using LMS.BUS.Dtos;

namespace LMS.GUI.ReportAdmin
{
    public partial class ucReportViewer : UserControl
    {
        public string ReportTitle { get; set; } = "Báo cáo";
        private readonly ReportViewer reportViewer;

        public ucReportViewer()
        {
            InitializeComponent();

            // Nếu control chưa kéo sẵn ReportViewer trên Designer, tạo runtime
            reportViewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
                ProcessingMode = ProcessingMode.Local
            };
            Controls.Add(reportViewer);
        }

        // ===== Helper: chọn RDLC =====
        private bool TrySetEmbedded(string embeddedResource)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(embeddedResource)) return false;
                reportViewer.LocalReport.ReportEmbeddedResource = embeddedResource;
                // ép load – nếu sai tên sẽ ném lỗi => rơi xuống ReportPath
                _ = reportViewer.LocalReport.ListRenderingExtensions();
                return true;
            }
            catch { return false; }
        }

        private void SetReport(string embeddedResource, string fileNameInOutput)
        {
            reportViewer.Reset();
            reportViewer.LocalReport.DataSources.Clear();

            // 1) Ưu tiên Embedded Resource (Build Action = Embedded Resource)
            if (TrySetEmbedded(embeddedResource)) return;

            // 2) Fallback: ReportPath (Copy to Output = Copy always)
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var p1 = Path.Combine(baseDir, fileNameInOutput);
            var p2 = Path.Combine(baseDir, "Reports", fileNameInOutput);

            if (File.Exists(p1)) reportViewer.LocalReport.ReportPath = p1;
            else if (File.Exists(p2)) reportViewer.LocalReport.ReportPath = p2;
            else
            {
                MessageBox.Show(
                    $"Không tìm thấy RDLC: {fileNameInOutput}\n" +
                    $"• Đặt Build Action = Embedded Resource với tên: {embeddedResource}\n" +
                    $"hoặc Copy file vào Output/Reports.",
                    "Thiếu RDLC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetCommonParameters(string dateRange)
        {
            try
            {
                var pars = new[]
                {
                    new ReportParameter("pReportTitle", ReportTitle ?? string.Empty),
                    new ReportParameter("pDateRange",   dateRange   ?? string.Empty)
                };
                reportViewer.LocalReport.SetParameters(pars);
            }
            catch { /* RDLC chưa khai báo params thì bỏ qua */ }
        }

        private void BindAndRefresh()
        {
            reportViewer.ZoomMode = ZoomMode.PageWidth;
            reportViewer.RefreshReport();
        }

        // ================== LOAD REPORTS ==================

        // Vận hành (rpt_Operations.rdlc)
        // DS_ShipmentStatus : List<ChartDataPoint>  (Label, Value)
        // DS_TopRoutes      : List<ChartDataPoint>
        // DS_Details        : List<ShipmentDetailDto1>
        public void LoadOperationsReport(
            List<ChartDataPoint> shipmentStatus,
            List<ChartDataPoint> topRoutes,
            List<ShipmentDetailDto1> details,
            string dateRange)
        {
            SetReport("LMS.GUI.ReportAdmin.rpt_Operations.rdlc", "rpt_Operations.rdlc");

            reportViewer.LocalReport.DataSources.Add(
                new ReportDataSource("DS_ShipmentStatus", shipmentStatus ?? new List<ChartDataPoint>()));
            reportViewer.LocalReport.DataSources.Add(
                new ReportDataSource("DS_TopRoutes", topRoutes ?? new List<ChartDataPoint>()));
            reportViewer.LocalReport.DataSources.Add(
                new ReportDataSource("DS_Details", details ?? new List<ShipmentDetailDto1>()));

            SetCommonParameters(dateRange);
            BindAndRefresh();
        }

        // Khách hàng (rpt_Customers.rdlc)
        // Chart:  DS_Customers (hoặc DS_Customer) : List<ChartDataPoint.TopCustomerDto>
        // Table:  DS_Details                      : List<CustomerOrderDetailDto>
        public void LoadCustomersReport(
            List<ChartDataPoint.TopCustomerDto> rows,
            List<CustomerOrderDetailDto> details,
            string dateRange)
        {
            SetReport("LMS.GUI.ReportAdmin.rpt_Customers.rdlc", "rpt_Customers.rdlc");

            var list1 = rows ?? new List<ChartDataPoint.TopCustomerDto>();
            // Nạp cả 2 tên để khớp mọi trường hợp đặt tên trong RDLC
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Customers", list1));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Customer", list1));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Details", details ?? new List<CustomerOrderDetailDto>()));

            SetCommonParameters(dateRange);
            BindAndRefresh();
        }

        // Tài xế (rpt_Drivers.rdlc)
        // Chart:  DS_Drivers (hoặc DS_Driver) : List<ChartDataPoint.TopDriverDto>
        // Table:  DS_Details                  : List<DriverShipmentDetailDto>
        public void LoadDriversReport(
            List<ChartDataPoint.TopDriverDto> rows,
            List<DriverShipmentDetailDto> details,
            string dateRange)
        {
            SetReport("LMS.GUI.ReportAdmin.rpt_Drivers.rdlc", "rpt_Drivers.rdlc");

            var list1 = rows ?? new List<ChartDataPoint.TopDriverDto>();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Drivers", list1));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Driver", list1));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Details", details ?? new List<DriverShipmentDetailDto>()));

            SetCommonParameters(dateRange);
            BindAndRefresh();
        }

        // Tổng quan (Order Status) — CẦN có rpt_Overview.rdlc
        // DS_Summary : List<ChartDataPoint> (Label, Value)
        // DS_Details : List<OrderStatusDetailDto>
        public void LoadOrderStatusReport(
            List<ChartDataPoint> summary,
            List<OrderStatusDetailDto> details,
            string dateRange)
        {
            SetReport("LMS.GUI.ReportAdmin.rpt_Overview.rdlc", "rpt_Overview.rdlc");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Summary", summary ?? new List<ChartDataPoint>()));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Details", details ?? new List<OrderStatusDetailDto>()));
            SetCommonParameters(dateRange);
            BindAndRefresh();
        }

        // Doanh thu — CẦN có rpt_Revenue.rdlc
        // DS_Summary : List<TimeSeriesDataPoint>
        // DS_Details : List<RevenueDetailDto>
        public void LoadRevenueReport(
            List<TimeSeriesDataPoint> summary,
            List<RevenueDetailDto> details,
            string dateRange)
        {
            SetReport("LMS.GUI.ReportAdmin.rpt_Revenue.rdlc", "rpt_Revenue.rdlc");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Summary", summary ?? new List<TimeSeriesDataPoint>()));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DS_Details", details ?? new List<RevenueDetailDto>()));
            SetCommonParameters(dateRange);
            BindAndRefresh();
        }
    }
}
