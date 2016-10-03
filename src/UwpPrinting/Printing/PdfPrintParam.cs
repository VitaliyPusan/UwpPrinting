using System;

namespace UwpPrinting.Printing
{
    internal class PdfPrintParam
    {
        public string Header { get; set; }
        public Uri PdfUri { get; set; }
        public int QualityRatio { get; set; }
    }
}