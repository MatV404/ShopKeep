using System;
using Windows.UI.Popups;

namespace ShopKeep.UI
{
    internal static class PopupMessage
    {
        /// <summary>
        /// Generates a small popup message that displays the given dialogMessage and has a button to close the popup
        /// labeled with cancelLabel.
        /// </summary>
        /// <param name="dialogMessage"></param>
        /// <param name="cancelLabel"></param>
        public static async void Message(string dialogMessage, string cancelLabel = "Okay")
        {
            var messageDialog = new MessageDialog(dialogMessage);
            messageDialog.Commands.Add(new UICommand(cancelLabel));
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }
    }
}
