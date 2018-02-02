using System;
using System.Windows;
using Microsoft.Reporting.WinForms;
using ReportingExample.RustiviaMetalsDataSetTableAdapters;

namespace ReportingExample
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isReportViewerLoaded;

        public MainWindow()
        {
            InitializeComponent();
            _reportViewer.Load += _reportViewer_Load;
        }

        private void _reportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                var reportDataSource1 = new ReportDataSource();
                var dataset = new RustiviaMetalsDataSet();

                dataset.BeginInit();

                reportDataSource1.Name = "WeighBridgeInfoesDataSet"; //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.WeighBridgeInfoes;
                _reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                _reportViewer.LocalReport.ReportEmbeddedResource = "ReportingExample.ReportSlip.rdlc";

                _reportViewer.Width = 80;
                _reportViewer.Height = 200;
                

                dataset.EndInit();

                //fill data into adventureWorksDataSet
                var weighBridgeInfoesTableAdapter = new WeighBridgeInfoesTableAdapter {ClearBeforeFill = true};
                weighBridgeInfoesTableAdapter.Fill(dataset.WeighBridgeInfoes);

                _reportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }
    }
}