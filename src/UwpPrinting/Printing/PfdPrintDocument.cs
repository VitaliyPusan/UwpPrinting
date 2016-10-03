using System.Collections.Generic;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Printing;

namespace UwpPrinting.Printing
{
    internal class PfdPrintDocument
    {
        private readonly List<UIElement> _previewElements = new List<UIElement>();
        private readonly PrintDocument _printDocument;

        public PfdPrintDocument(IEnumerable<ImageSource> imageSources)
        {
            _printDocument = new PrintDocument();
            _printDocument.Paginate += OnPaginate;
            _printDocument.AddPages += OnAddPages;
            _printDocument.GetPreviewPage += OnGetPreviewPage;

            Source = _printDocument.DocumentSource;

            foreach (var source in imageSources)
            {
                _previewElements.Add(new Image {Source = source});
            }
        }

        public IPrintDocumentSource Source { get; }

        private void OnPaginate(object sender, PaginateEventArgs e)
        {
            _printDocument.SetPreviewPageCount(_previewElements.Count, PreviewPageCountType.Final);
        }

        private void OnAddPages(object sender, AddPagesEventArgs e)
        {
            var printDocument = (PrintDocument)sender;
            _previewElements.ForEach(printDocument.AddPage);
            printDocument.AddPagesComplete();
        }

        private void OnGetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            var printDocument = (PrintDocument)sender;
            printDocument.SetPreviewPage(e.PageNumber, _previewElements[e.PageNumber - 1]);
        }

        public void Dispose()
        {
            _printDocument.Paginate -= OnPaginate;
            _printDocument.AddPages -= OnAddPages;
            _printDocument.GetPreviewPage -= OnGetPreviewPage;
        }
    }
}