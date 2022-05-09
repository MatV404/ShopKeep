using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ShopKeep.UI
{
    internal static class PopupMessage
    {
        public static async void Message(string dialogMessage, string cancelLabel = "Okay")
        {
            var messageDialog = new MessageDialog(dialogMessage);
            messageDialog.Commands.Add(new UICommand(cancelLabel));
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
    }
}
