using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ShopKeep.UI.Shop;
using ShopKeepDB.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Regular
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegularMenu : Page
    {

        private User CurrentUser;

        public RegularMenu()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            this.CurrentUser = (User) arguments.Parameter;
        }

        private void ViewShopsClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Shops));
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void ViewInventoryClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Inventory), new Tuple<User, bool>(CurrentUser, false));
        }
    }
}
