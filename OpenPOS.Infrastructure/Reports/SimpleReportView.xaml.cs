using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CodeReason.Reports;
using System.Windows.Xps.Packaging;

namespace OpenPOS.Infrastructure.Reports
{
    /// <summary>
    /// Interaction logic for SimpleReportView.xaml
    /// </summary>
    public partial class SimpleReportView : UserControl
    {
        private readonly XpsDocument _document;

        public SimpleReportView(XpsDocument document)
        {
            _document = document;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            documentViewer.Document = _document.GetFixedDocumentSequence();
        }
    }
}
