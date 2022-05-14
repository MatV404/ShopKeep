using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ShopKeep.UI.Shop;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.User
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserMenu : Page
    {

        private ShopKeepDB.Models.User _currentUser;

        public UserMenu()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            _currentUser = (ShopKeepDB.Models.User) arguments.Parameter;
        }

        private void ViewShopsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Shops), _currentUser);
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void ViewInventoryClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Inventory), new Tuple<ShopKeepDB.Models.User, bool>(_currentUser, false));
        }

        private void ChangePasswordClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChangePassword), _currentUser);
        }
    }
}
