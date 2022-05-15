using System;
using System.Collections.Generic;
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
using ShopKeep.UI.Item;
using ShopKeep.UI.Shop;
using ShopKeep.UI.UserUI;
using ShopKeepDB.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMenu : Page
    {
        private ShopKeepDB.Models.User _currentUser;

        public AdminMenu()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            _currentUser = arguments.Parameter as ShopKeepDB.Models.User;
        }


        private void ViewShopsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Shops), _currentUser);
        }

        private void CreateShopClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateShop));
        }

        private void DeleteShopsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DeleteShops));
        }

        private void ManageItemsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Items));
        }

        private void CreateItemClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateItem));
        }

        private void ManageUsers(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Users));
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }

        private void ChangePasswordClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChangePassword), _currentUser);
        }
    }
}
