using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Retrievals;
using Type = ShopKeepDB.Models.Type;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Shop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shops : Page
    {
        private readonly ObservableCollection<ShopKeepDB.Models.Shop> _shopCollection = new ObservableCollection<ShopKeepDB.Models.Shop>();
        private readonly string[] _locales = Constants.ShopLocales;
        private List<Type> _types;
        private User _currentUser;

        public Shops()
        {
            InitializeComponent();
            PopulateShopsAsync();
            PopulateTypesAsync();
        }

        private async void PopulateShopsAsync()
        {
            var shops = await Task.Run(() => ShopGetter.GetAllShopsAsync());
            foreach (var shop in shops)
            {
                _shopCollection.Add(shop);
            }
        }

        private async void PopulateTypesAsync()
        {
            _types = await Task.Run(() => TypeGetter.GetAllTypesAsync());
            if (_types == null)
            {
                PopupMessage.Message("Something went wrong with the database while trying to load Types.");
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            _currentUser = arguments.Parameter as User;
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void FilterShopsAsync(object sender, RoutedEventArgs e)
        {
            string name = ShopName.Text;
            string owner = OwnerName.Text;
            string locale = ShopLocale.SelectedItem?.ToString();
            int? typeId = ((Type)ShopType.SelectedItem)?.Id;
            List<ShopKeepDB.Models.Shop> filteredShops =
                await Task.Run(() => ShopGetter.FilterShopsAsync(name, owner, typeId, locale));
            _shopCollection.Clear();
            filteredShops.ForEach(shop => _shopCollection.Add(shop));
        }

        private void ClearFilters(object sender, RoutedEventArgs e)
        {
            ShopName.Text = "";
            OwnerName.Text = "";
            ShopLocale.SelectedItem = null;
            ShopType.SelectedItem = null;
            _shopCollection.Clear();
            PopulateShopsAsync();
        }

        private void OnShopSelection(object sender, SelectionChangedEventArgs e)
        {
            if (!_currentUser.IsAdmin)
            {
                Frame.Navigate(typeof(Shop),
                    new Tuple<ShopKeepDB.Models.Shop, User>((ShopKeepDB.Models.Shop)ShopsView.SelectedItem,
                        _currentUser));
            }
            else
            {
                Frame.Navigate(typeof(ShopAdmin), (ShopKeepDB.Models.Shop)ShopsView.SelectedItem);
            }
        }
    }
}
