using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ShopKeep.Misc;
using ShopKeepDB.Controls;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Delete;
using ShopKeepDB.Operations.Update;
using ShopKeepDB.StockGeneration;
using ShopKeepDB.TransactionMisc;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Shop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShopAdmin : Page
    {
        private ShopKeepDB.Models.Shop _currentShop;
        private User _currentUser;
        private ObservableCollection<ShopStock> _currentShopStock = new ObservableCollection<ShopStock>();

        private ObservableCollection<ShopKeepDB.Models.Item> _foundItems =
            new ObservableCollection<ShopKeepDB.Models.Item>();

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            var (shop, user) = (Tuple<ShopKeepDB.Models.Shop, User>)arguments.Parameter;
            _currentShop = shop;
            _currentUser = user;
            PopulateRelevantCollections();
        }

        private async void PopulateRelevantCollections()
        {
            await PopulateShopStockAsync();
        }

        private async Task PopulateShopStockAsync()
        {
            var stock = await ShopKeepDB.Operations.Retrievals.ShopStockGetter.GetShopStock(_currentShop.Id);
            stock.ForEach(stockItem => _currentShopStock.Add(stockItem));
        }
        
        private void BackToShops(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindItemIdClick(object sender, RoutedEventArgs e)
        {
            if (!FindItemPopup.IsOpen)
            {
                FindItemPopup.IsOpen = true;
            }
        }

        /// <summary>
        /// Closes the item ID find popup.
        /// </summary>
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            if (FindItemPopup.IsOpen)
            {
                FindItemPopup.IsOpen = false;
                _foundItems.Clear();
                FinderName.Text = "";
            }
        }

        private async void FindItemIdAsync(object sender, RoutedEventArgs e)
        {
            string itemName = FinderName.Text;
            if (!string.IsNullOrWhiteSpace(itemName))
            {
                var result = await
                    ShopKeepDB.Operations.Retrievals.ItemGetter.FilterItemsAsync(itemName, null, null, int.MinValue, int.MaxValue,
                        int.MinValue, int.MaxValue, int.MinValue, int.MaxValue);
                _foundItems.Clear();
                result.ForEach(item => _foundItems.Add(item));
            }
        }

        private async void AddItemToShopAsync(object sender, RoutedEventArgs e)
        {
            int itemId = (int)ItemIdBox.Value;
            if (itemId <= 0)
            {
                PopupMessage.Message("Invalid item id entered.");
                return;
            }

            int amount = (int)AmountItemBox.Value;
            if (amount <= 0)
            {
                PopupMessage.Message("Invalid amount entered.");
                return;
            }

            int goldPrice = (int)GoldPriceItemBox.Value;
            int silverPrice = (int)SilverPriceItemBox.Value;
            int copperPrice = (int)CopperPriceItemBox.Value;

            if (goldPrice < 0 || silverPrice < 0 || copperPrice < 0)
            {
                PopupMessage.Message("Invalid input: One of the prices is less than 0.");
                return;
            }

            if (goldPrice == 0 && silverPrice == 0 && copperPrice == 0)
            {
                PopupMessage.Message("The price can't be 0!");
                return;
            }

            ShopKeepDB.Models.Item queryResult = await ShopKeepDB.Operations.Retrievals.ItemGetter.RetrieveItem(itemId);

            if (queryResult == null)
            {
                PopupMessage.Message("Either no item with matching Id exists, or a database error occurred.");
                return;
            }

            if (await ShopKeepDB.Operations.Retrievals.ShopStockGetter.ShopStockExistsAsync(_currentShop.Id,
                    itemId))
            {
                PopupMessage.Message("Item is already in stock.");
                return;
            }


            ShopStock newStock = await
                ShopKeepDB.Operations.Create.ShopStockCreator.CreateShopStock(_currentShop.Id, itemId, amount, goldPrice, silverPrice,
                    copperPrice);

            if (newStock == null)
            {
                PopupMessage.Message("Something went wrong with the database while trying to add the item into the shop.");
                return;
            }
            newStock.Item = queryResult;
            _currentShopStock.Add(newStock);
        }

        /// <summary>
        /// As the name suggests, this method removes selected stock from the shop.
        /// </summary>
        private async void RemoveFromStockAsync(object sender, RoutedEventArgs e)
        {
            var selected = ShopStock.SelectedItem as ShopStock;
            var amount = (int)BuyAmount.Value;
            if (selected == null)
            {
                PopupMessage.Message("No item selected.");
                return;
            }
            AddToBuyButton.IsEnabled = false;
            bool result;
            if (selected.Amount <= amount || amount <= 0)
            {
                result = await ShopStockRemover.RemoveShopStockAsync(selected);
                if (!result)
                {
                    PopupMessage.Message("A database error occurred while removing the item from stock.");
                }

                _currentShopStock.Remove(selected);
                AddToBuyButton.IsEnabled = true;
                return;
            }

            result = await ShopStockUpdate.ChangeShopStockAmountAsync(selected,
                selected.Amount - amount);
            if (!result)
            {
                PopupMessage.Message("A database error occurred while changing the item stock amount.");
            }
            AddToBuyButton.IsEnabled = true;
        }
        
        /// <summary>
        /// Deletes the entire shop.
        /// </summary>
        private async void DeleteShopClick(object sender, RoutedEventArgs e)
        {
            var deleted = await ShopDestroyer.DeleteShopAsync(_currentShop);
            if (deleted)
            {
                Frame.GoBack();
                return;
            }
            PopupMessage.Message("Failed to delete shop.");
        }
        private async Task<bool> RemoveAllStock()
        {
            if (!await ShopStockRemover.RemoveShopStockAsync(_currentShopStock.ToList()))
            {
                PopupMessage.Message("Couldn't remove shop stock. Aborting.");
                return false;
            }
            _currentShopStock.Clear();
            return true;
        }

        /// <summary>
        /// Deletes the old shop stock and generates new shop stock.
        /// </summary>
        private async void RegenerateStockAsync(object sender, RoutedEventArgs e)
        {
            StockGeneration.IsEnabled = false;
            if (!await RemoveAllStock())
            {
                StockGeneration.IsEnabled = true;
                return;
            }

            StockGenerationWrapper generation = new StockGenerationWrapper(_currentShop);
            if (!await generation.GenerateStockAsync())
            {
                PopupMessage.Message("Stock generation failed.");
                StockGeneration.IsEnabled = true;
                return;
            }
            await PopulateShopStockAsync();
            StockGeneration.IsEnabled = true;
        }

        /// <summary>
        /// Prints all stock into the file named after the given shop.
        /// </summary>
        private async void TransferStockToTextAsync(object sender, RoutedEventArgs e)
        {
            StockToTextButton.IsEnabled = false;
            string result = await StockWriter.StockToFile(_currentShop, _currentShopStock.ToList());
            if (result != "")
            {
                PopupMessage.Message($"Stock written to {result} successfully!");
            }
            else
            {
                PopupMessage.Message("Stock write failed.");
            }
            StockToTextButton.IsEnabled = true;
        }
        public ShopAdmin()
        {
            InitializeComponent();
        }

        private async void ChangeStockPriceAsync(object sender, RoutedEventArgs e)
        {
            var selected = ShopStock.SelectedItem as ShopStock;
            var newGoldPrice = (int) NewGoldPriceBox.Value;
            var newSilverPrice = (int) NewSilverPriceBox.Value;
            var newCopperPrice = (int) NewCopperPriceBox.Value;
            if (selected == null)
            {
                PopupMessage.Message("No item selected.");
                return;
            }

            if (newGoldPrice < 0 || newSilverPrice < 0 || newCopperPrice < 0)
            {
                PopupMessage.Message("Can't set a negative price!");
                return;
            }

            if (newGoldPrice == 0 && newSilverPrice == 0 && newCopperPrice == 0)
            {
                PopupMessage.Message("Price can't be 0!");
            }

            if (!await ShopStockPriceUpdate.UpdateShopStockPriceAsync(selected.ShopStockPrice, newGoldPrice, newSilverPrice, newCopperPrice))
            {
                PopupMessage.Message("Failed to update price due to a database error.");
                return;
            }

            _currentShopStock.Remove(selected);
            _currentShopStock.Add(selected);


        }
    }
}
