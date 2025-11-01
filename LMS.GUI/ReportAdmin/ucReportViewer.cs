using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LMS.BUS.Dtos;                 // ChartDataPoint
using Microsoft.Reporting.WinForms; // ReportViewer

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

        // Nạp báo cáo "Tình trạng đơn hàng"
        //  -> summary: dữ liệu cho biểu đồ (DS_OrderStatus)
        //  -> details: dữ liệu cho bảng chi tiết (DS_OrderDetails)
        public void LoadOrderStatusReport(
            List<ChartDataPoint> summary,
            List<ChartDataPoint.OrderStatusDetailDto> details,
            string dateRange)
        {
            try
            {
                var dsSummary = new ReportDataSource("DS_OrderStatus", summary ?? new List<ChartDataPoint>());
                var dsDetails = new ReportDataSource("DS_OrderDetails", details ?? new List<ChartDataPoint.OrderStatusDetailDto>());

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(dsSummary);
                reportViewer1.LocalReport.DataSources.Add(dsDetails);

                // RDLC: đảm bảo Build Action = Embedded Resource
                string embedded = "LMS.GUI.ReportAdmin.rpt_OrderStatus.rdlc";
                reportViewer1.LocalReport.ReportEmbeddedResource = embedded;

                // RDLC hiện chưa khai báo ReportParameter -> không SetParameters

                reportViewer1.ShowToolBar = false;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo: " + ex.Message,
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
