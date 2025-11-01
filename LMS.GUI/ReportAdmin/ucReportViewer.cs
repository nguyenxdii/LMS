using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LMS.BUS.Dtos; // Cần DTO ChartDataPoint
using Microsoft.Reporting.WinForms; // Thư viện ReportViewer

namespace LMS.GUI.ReportAdmin
{
    /// <summary>
    /// Một UserControl tùy chỉnh chứa ReportViewer và các nút xuất file.
    /// </summary>
    public partial class ucReportViewer : UserControl
    {
        public ucReportViewer()
        {
            InitializeComponent();

            // Kết nối sự kiện với các hàm (methods) ở bên dưới
            btnExportPdf.Click += btnExportPdf_Click;
            btnExportExcel.Click += btnExportExcel_Click;
        }


        public void LoadOrderStatusReport(List<ChartDataPoint> data, string dateRange)
        {
            try
            {
                // 1. Tên DataSet bên trong file .rdlc (ví dụ: DataSet_OrderStatus)
                ReportDataSource rds = new ReportDataSource("DataSet_OrderStatus", data);

                // Giả định ReportViewer của bạn tên là 'reportViewer1'
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(rds);

                // 2. Đường dẫn (embedded) đến tệp .rdlc
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "LMS.GUI.ReportAdmin.rpt_OrderStatus.rdlc";

                // 3. Truyền tham số
                ReportParameter param = new ReportParameter("pDateRange", dateRange);
                this.reportViewer1.LocalReport.SetParameters(new[] { param });

                // 4. Tắt thanh công cụ mặc định của ReportViewer (vì ta dùng nút tùy chỉnh)
                this.reportViewer1.ShowToolBar = false;

                // 5. Tải lại báo cáo
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // <--- HÀM BỊ THIẾU 1 (Gây ra lỗi trong ảnh) ---
        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            ExportReport("PDF", "PDF files (*.pdf)|*.pdf", "pdf");
        }

        // <--- HÀM BỊ THIẾU 2 (Sẽ gây lỗi nếu không thêm) ---
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Dùng EXCELOPENXML để xuất ra file .xlsx (Excel 2007+), có thể chỉnh sửa được.
            ExportReport("EXCELOPENXML", "Excel files (*.xlsx)|*.xlsx", "xlsx");
        }

        /// <summary>
        /// Hàm chung để render (kết xuất) và lưu báo cáo ra file
        /// </summary>
        private void ExportReport(string format, string filter, string extension)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, filenameExtension;

                // Render báo cáo ra một mảng byte[]
                byte[] bytes = this.reportViewer1.LocalReport.Render(
                    format,
                    null,
                    out mimeType,
                    out encoding,
                    out filenameExtension,
                    out streamids,
                    out warnings);

                // Mở hộp thoại lưu file
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = filter;
                    sfd.Title = $"Lưu báo cáo dưới dạng {extension.ToUpper()}";
                    sfd.FileName = $"BaoCao_DonHang_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Ghi mảng byte[] ra file
                        File.WriteAllBytes(sfd.FileName, bytes);
                        MessageBox.Show("Xuất báo cáo thành công!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}