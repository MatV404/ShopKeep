using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Create;
using ShopKeepDB.Operations.Retrievals;
using ShopKeepDB.StockGeneration;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Shop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateShop : Page
    {
        private readonly string[] _locales = Constants.ShopLocales;
        private readonly List<Type> _types = new List<Type>();
        public CreateShop()
        {
            InitializeComponent();
            PopulateTypes();
        }

        private async void PopulateTypes()
        {
            var types = await TypeGetter.GetAllTypesAsync();
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
            var shopType = (Type)ShopTypeBox.SelectedItem;
            string shopLocale = ShopLocaleBox.SelectedItem?.ToString();
            string shopDescription = ShopDescriptionBox.Text;
            bool generateStock = GenerateStockCheck.IsChecked ?? false;
            if (string.IsNullOrEmpty(shopName) || shopType == null || string.IsNullOrEmpty(shopLocale))
            {
                PopupMessage.Message("The only properties that can be empty are Shop Owner and Shop Description");
                return;
            }

            ShopKeepDB.Models.Shop result = await Task.Run(() => ShopCreator.CreateShopAsync(shopName, ownerName,
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
