using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMS.BUS.Services; // Namespace của OrderService
using LMS.DAL.Models;   // Namespace của OrderStatus
using LiveCharts;             // Core LiveCharts library
using LiveCharts.Wpf;         // For PieSeries, etc.
using LiveCharts.WinForms;    // For the PieChart control itself

namespace LMS.GUI.ReportAdmin
{
    public partial class ucStatistics : UserControl
    {

        public ucStatistics()
        {
            InitializeComponent();
        }
    }
}
