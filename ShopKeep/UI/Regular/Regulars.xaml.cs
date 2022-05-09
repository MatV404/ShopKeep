using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Controls;
using ShopKeep.UI.Admin;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Retrievals;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Regular
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Regulars : Page
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public Regulars()
        {
            this.InitializeComponent();
            PopulateUsers();
        }

        private async void PopulateUsers()
        {
            var usersList = await ShopKeepDB.Operations.Retrievals.UserGetter.GetAllRegularUsers();
            if (usersList == null)
            {
                PopupMessage.Message("A database error occurred. Please, refresh the page.", "Okay...");
                return;
            }
            foreach (var user in usersList)
            {
                Users.Add(user);
            }
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void ClearFilter(object sender, RoutedEventArgs e)
        {
            Users.Clear();
            UserName.Text = "";
            UserId.Text = "";
            PopulateUsers();
        }

        private async void FilterUsers(object sender, RoutedEventArgs e)
        {
            string userNameFilter = string.IsNullOrWhiteSpace(UserName.Text) ? "" : UserName.Text;
            int userId = (int)(UserId.Value >= 0 ? UserId.Value : -1);
            List<User> foundUsers = await UserGetter.FilterUsers(userId, userNameFilter);
            if (foundUsers == null)
            {
                PopupMessage.Message("A database error occurred.", "Aww...");
                return;
            }
            Users.Clear();
            foreach (User user in foundUsers)
            {
                Users.Add(user);
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Frame.Navigate(typeof(Inventory), new Tuple<User, bool>((User) UserView.SelectedItem, true));
        }
    }
}
