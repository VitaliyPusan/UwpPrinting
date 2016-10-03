using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace UwpPrinting.Printing
{
    public static class DispatcherHelper
    {
        public static async Task RunAsync(DispatchedHandler handler)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
        }
    }
}