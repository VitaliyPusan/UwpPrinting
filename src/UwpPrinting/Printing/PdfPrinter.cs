using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Media;
using UwpPrinting.Helpers;

namespace UwpPrinting.Printing
{
    internal class PdfPrinter
    {
        private readonly PrintManager _manager;
        private PfdPrintDocument _document;
        private IEnumerable<ImageSource> _images;
        private PdfPrintParam _param;

        public PdfPrinter()
        {
            _manager = PrintManager.GetForCurrentView();
        }

        public async Task PrintAsync(PdfPrintParam param)
        {
            _param = param;
            _manager.PrintTaskRequested += OnPrintTaskRequested;

            _images = await PdfHelper.GetImagesFromPdfUrlAsync(param.PdfUri, param.QualityRatio);
            _document = new PfdPrintDocument(_images);

            await PrintManager.ShowPrintUIAsync();
        }

        private void OnPrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            var printTask = e.Request.CreatePrintTask(_param.Header, OnPrintTaskSourceRequested);
            printTask.Completed += OnCompleted;
        }

        private async void OnCompleted(PrintTask printTask, PrintTaskCompletedEventArgs e)
        {
            printTask.Completed -= OnCompleted;
            _images = null;

            await DispatcherHelper.RunAsync(() =>
            {
                _manager.PrintTaskRequested -= OnPrintTaskRequested;
                _document?.Dispose();
            });
        }

        private void OnPrintTaskSourceRequested(PrintTaskSourceRequestedArgs e)
        {
            e.SetSource(_document?.Source);
        }
    }
}