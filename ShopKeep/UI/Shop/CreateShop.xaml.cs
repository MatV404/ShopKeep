using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ShopKeep.UI.Admin;
using ShopKeepDB.StockGeneration;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Shop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateShop : Page
    {
        private readonly string[] _locales = ShopKeepDB.Misc.Constants.ShopLocales;
        private List<ShopKeepDB.Models.Type> _types = new List<ShopKeepDB.Models.Type>();
        public CreateShop()
        {
            InitializeComponent();
            PopulateTypes();
        }

        private async void PopulateTypes()
        {
            var types = await Task.Run(() => ShopKeepDB.Operations.Retrievals.TypeGetter.GetAllTypesAsync());
            if (_types == null)
            {
                PopupMessage.Message("Something went wrong with the database while trying to retrieve all shop types. Please, refresh the page.");
            }

            types.ForEach(type => _types.Add(type));
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void CreateShopClickAsync(object sender, RoutedEventArgs e)
        {
            string shopName = ShopNameBox.Text;
            string ownerName = ShopOwnerBox.Text;
            var shopType = (ShopKeepDB.Models.Type) ShopTypeBox.SelectedItem;
            string shopLocale = ShopLocaleBox.SelectedItem?.ToString();
            string shopDescription = ShopDescriptionBox.Text;
            bool generateStock = GenerateStockCheck.IsChecked ?? false;
            if (string.IsNullOrEmpty(shopName) || shopType == null || string.IsNullOrEmpty(shopLocale))
            {
                PopupMessage.Message("The only properties that can be empty are Shop Owner and Shop Description");
                return;
            }

            ShopKeepDB.Models.Shop result = await Task.Run(() => ShopKeepDB.Operations.Create.ShopCreator.CreateShopAsync(shopName, ownerName,
                shopType.Id, shopLocale, shopDescription));
            if (result == null)
            {
                PopupMessage.Message("Something went wrong with the database while creating the shop. Please, contact the administrator.");
                return;
            }
            result.Type = shopType;
            if (generateStock)
            {
                StockGenerationWrapper generation = new StockGenerationWrapper(result);
                if (!await Task.Run(() => generation.GenerateStockAsync()))
                {
                    PopupMessage.Message("Some of the items were not generated properly.");
                }
            }
            PopupMessage.Message("Shop successfully created!");
        }
    }
}
