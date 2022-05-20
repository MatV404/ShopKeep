using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ShopKeepDB.Misc;
using System.Threading.Tasks;

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
            this.BackButton.IsEnabled = false;
            this.RegisterButton.IsEnabled = false;
            string registerName = Username.Text;
            string registerPass = Password.Password;
            if (String.IsNullOrWhiteSpace(registerName) || String.IsNullOrWhiteSpace(registerPass))
            {
                PopupMessage.Message("Registration failed - both username and password must be filled out.",
                                     "Okay");
                return;
            }


            RegistrationResults result =
                await Task.Run(() => ShopKeepDB.Operations.Credentials.Register.RegisterAsync(registerName, registerPass));
            this.BackButton.IsEnabled = true;
            this.RegisterButton.IsEnabled = true;
            switch (result)
            {
                case RegistrationResults.RegistrationFailure:
                    PopupMessage.Message("Registration failed. Your username might already be in use, or some other error occurred.", 
                                         "Okay");
                    return;
                case RegistrationResults.RegistrationSuccess:
                    PopupMessage.Message("Your registration was successful!");
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
