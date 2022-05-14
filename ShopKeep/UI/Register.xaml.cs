using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ShopKeepDB.Misc;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }

        private async void RegistrationClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Username.Text) || String.IsNullOrWhiteSpace(Password.Password))
            {
                PopupMessage.Message("Registration failed - both username and password must be filled out.",
                                     "Okay");
                return;
            }


            RegistrationResults result =
                await ShopKeepDB.Operations.Credentials.Register.RegisterAsync(Username.Text, Password.Password);
            //await Registration.Register(Username.Text, Password.Password);
            switch (result)
            {
                case RegistrationResults.RegistrationFailure:
                    PopupMessage.Message("Registration failed. Your username might already be in use, or some other error occurred.", 
                                         "Okay");
                    return;
                case RegistrationResults.RegistrationSuccess:
                    Frame.Navigate(typeof(Login));
                    return;
                case RegistrationResults.DbError:
                    PopupMessage.Message("A database error occurred. Please, contact the administrator.",
                                         "Okay");
                    return;
                default:
                    PopupMessage.Message("Something entirely unexpected happened. Please, contact the administrator.",
                                         "Okay");
                    return;
            }

        }
    }
}
