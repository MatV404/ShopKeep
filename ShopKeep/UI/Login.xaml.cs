using System;
using System.Security;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using ShopKeep.UI.Admin;
using ShopKeep.UI.User;
using ShopKeepDB.Misc;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void LoginSubmit(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Username.Text) || String.IsNullOrWhiteSpace(Password.Password))
            {
                PopupMessage.Message("Login failed - both username and password must be filled out.");
                return;
            }

            var loginResult =
                await ShopKeepDB.Operations.Credentials.Login.ValidateLoginAsync(Username.Text, Password.Password);
            switch (loginResult.Item1)
            {
                case LoginResults.Banned:
                    PopupMessage.Message("You have been banned. Please, contact an administrator.");
                    return;
                case LoginResults.Admin:
                    Frame.Navigate(typeof(AdminMenu), loginResult.Item2);
                    return;
                case LoginResults.User:
                    Frame.Navigate(typeof(RegularMenu), loginResult.Item2);
                    return;
                case LoginResults.Invalid:
                    PopupMessage.Message("Invalid credentials entered.");
                    return;
                case LoginResults.DbError:
                    PopupMessage.Message("Database error. Please, contact the administrator.");
                    return;
                default:
                    PopupMessage.Message("Something entirely unexpected happened. Please, contact the administrator.");
                    return;
            }
        }

        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }
    }
}
