using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using UwpPrinting.Printing;

namespace UwpPrinting
{
    public sealed partial class MainPage
    {
        private readonly PdfPrinter _printer = new PdfPrinter();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Path.IsEnabled = false;
                Quality.IsEnabled = false;
                PrintButton.IsEnabled = false;
                ProgressRing.IsActive = true;
                
                var sw = new Stopwatch();
                sw.Start();

                var param = new PdfPrintParam
                {
                    Header = "PDF Printing",
                    PdfUri = new Uri(Path.Text),
                    QualityRatio = int.Parse(Quality.Text)
                };

                await _printer.PrintAsync(param);
                sw.Stop();

                ElapsedList.Items?.Add($"Ratio X{Quality.Text} Elapsed: {sw.Elapsed}");
            }
            finally
            {
                Path.IsEnabled = true;
                Quality.IsEnabled = true;
                PrintButton.IsEnabled = true;
                ProgressRing.IsActive = false;
            }
        }
    }
}