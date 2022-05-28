using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Update;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangePassword : Page
    {

        private User _currentUser;
        public ChangePassword()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _currentUser = e.Parameter as User;
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void ChangePasswordClickAsync(object sender, RoutedEventArgs e)
        {
            string newPass = NewPasswordBox.Password;
            string verify = RepeatPasswordBox.Password;
            if (newPass != verify || string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(verify))
            {
                PopupMessage.Message("Empty password, or the password doesn't match the repeated password.");
                return;
            }

            if (!await Task.Run(() => UserUpdate.ChangeUserPassword(_currentUser, newPass)))
            {
                PopupMessage.Message("Password update failed.");
                return;
            }
            PopupMessage.Message("Password updated succesfully.");
        }
    }
}
