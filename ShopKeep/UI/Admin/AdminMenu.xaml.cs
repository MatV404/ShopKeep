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
using ShopKeep.UI.Regular;
using ShopKeep.UI.Shop;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMenu : Page
    {
        public AdminMenu()
        {
            this.InitializeComponent();
        }

        private void ViewShopsClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Shops));
        }

        private void CreateShopClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateShop));
        }

        private void DeleteShopsClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DeleteShops));
        }

        private void ManageItemsClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Items));
        }

        private void CreateItemClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateItem));
        }

        private void ManageUsers(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Regulars));
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }
    }
}
