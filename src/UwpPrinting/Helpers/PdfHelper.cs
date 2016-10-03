using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UwpPrinting.Helpers
{
    internal static class PdfHelper
    {
        public static async Task<IEnumerable<ImageSource>> GetImagesFromPdfUrlAsync(Uri url, int qalityRatio = 1)
        {
            var result = new List<ImageSource>();

            try
            {
                var reference = RandomAccessStreamReference.CreateFromUri(url);

                using (var stream = await reference.OpenReadAsync())
                {
                    var document = await PdfDocument.LoadFromStreamAsync(stream);

                    for (uint i = 0; i < document.PageCount; i++)
                    {
                        using (var page = document.GetPage(i))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var randomStream = memoryStream.AsRandomAccessStream())
                                {
                                    var image = new BitmapImage();

                                    var options = new PdfPageRenderOptions
                                    {
                                        DestinationWidth = (uint)page.Size.Width * (uint)qalityRatio,
                                        DestinationHeight = (uint)page.Size.Height * (uint)qalityRatio
                                    };

                                    await page.RenderToStreamAsync(randomStream, options);
                                    image.SetSource(randomStream);
                                    result.Add(image);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: Unable to convert PDF to Image. " + ex.Message);
            }

            return result;
        }
    }
}