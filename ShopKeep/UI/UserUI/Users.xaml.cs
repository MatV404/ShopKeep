using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Retrievals;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.UserUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Users : Page
    {
        public ObservableCollection<User> UserList { get; set; } = new ObservableCollection<User>();

        public Users()
        {
            InitializeComponent();
            PopulateUsers();
        }

        private async void PopulateUsers()
        {
            var usersList = await Task.Run(() => UserGetter.GetAllRegularUsers());
            if (usersList == null)
            {
                PopupMessage.Message("A database error occurred. Please, refresh the page.", "Okay...");
                return;
            }
            foreach (var user in usersList)
            {
                UserList.Add(user);
            }
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void ClearFilter(object sender, RoutedEventArgs e)
        {
            UserList.Clear();
            UserName.Text = "";
            PopulateUsers();
        }

        private async void FilterUsers(object sender, RoutedEventArgs e)
        {
            string userNameFilter = string.IsNullOrWhiteSpace(UserName.Text) ? "" : UserName.Text;

            List<User> foundUsers = await Task.Run(() => UserGetter.FilterUsers(userNameFilter));
            if (foundUsers == null)
            {
                PopupMessage.Message("A database error occurred.", "Aww...");
                return;
            }
            UserList.Clear();
            foreach (User user in foundUsers)
            {
                if (!user.IsAdmin)
                {
                    UserList.Add(user);
                }
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(Inventory), new Tuple<User, bool>((User)UserView.SelectedItem, true));
        }
    }
}
