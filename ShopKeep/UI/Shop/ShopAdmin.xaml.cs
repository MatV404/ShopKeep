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
        private ShopKeepDB.Models.Shop CurrentShop;
        private User CurrentUser;
        private ObservableCollection<ShopStock> CurrentShopStock = new ObservableCollection<ShopStock>();

        private ObservableCollection<ShopKeepDB.Models.Item> FoundItems =
            new ObservableCollection<ShopKeepDB.Models.Item>();

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            Tuple<ShopKeepDB.Models.Shop, User> args = (Tuple<ShopKeepDB.Models.Shop, User>)arguments.Parameter;
            CurrentShop = args.Item1;
            CurrentUser = args.Item2;
            PopulateRelevantCollections();
        }

        private async void PopulateRelevantCollections()
        {
            await PopulateShopStockAsync();
        }

        private async Task PopulateShopStockAsync()
        {
            var stock = await ShopKeepDB.Operations.Retrievals.ShopStockGetter.GetShopStock(CurrentShop.Id);
            stock.ForEach(stockItem => CurrentShopStock.Add(stockItem));
        }
        
        private void BackToShops(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void FindItemIdClick(object sender, RoutedEventArgs e)
        {
            if (!FindItemPopup.IsOpen)
            {
                FindItemPopup.IsOpen = true;
            }
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            if (FindItemPopup.IsOpen)
            {
                FindItemPopup.IsOpen = false;
                FoundItems.Clear();
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
                FoundItems.Clear();
                result.ForEach(item => FoundItems.Add(item));
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

            if (await ShopKeepDB.Operations.Retrievals.ShopStockGetter.ShopStockExistsAsync(CurrentShop.Id,
                    itemId))
            {
                PopupMessage.Message("Item is already in stock.");
                return;
            }


            ShopStock newStock = await
                ShopKeepDB.Operations.Create.ShopStockCreator.CreateShopStock(CurrentShop.Id, itemId, amount, goldPrice, silverPrice,
                    copperPrice);

            if (newStock == null)
            {
                PopupMessage.Message("Something went wrong with the database while trying to add the item into the shop.");
            }
            newStock.Item = queryResult;
            CurrentShopStock.Add(newStock);
            return;

        }

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

                CurrentShopStock.Remove(selected);
                AddToBuyButton.IsEnabled = true;
                return;
            }

            result = await ShopKeepDB.Operations.Update.ShopStockUpdate.ChangeShopStockAmountAsync(selected,
                selected.Amount - amount);
            if (!result)
            {
                PopupMessage.Message("A database error occurred while changing the item stock amount.");
            }
            AddToBuyButton.IsEnabled = true;
        }
        

        private async void DeleteShopClick(object sender, RoutedEventArgs e)
        {
            var deleted = await ShopDestroyer.DeleteShopAsync(CurrentShop);
            if (deleted)
            {
                Frame.GoBack();
                return;
            }
            PopupMessage.Message("Failed to delete shop.");
        }
        private async Task<bool> RemoveAllStock()
        {
            if (!await ShopStockRemover.RemoveShopStockAsync(CurrentShopStock.ToList()))
            {
                PopupMessage.Message("Couldn't remove shop stock. Aborting.");
                return false;
            }
            CurrentShopStock.Clear();
            return true;
        }

        private async void RegenerateStockAsync(object sender, RoutedEventArgs e)
        {
            StockGeneration.IsEnabled = false;
            if (!await RemoveAllStock())
            {
                StockGeneration.IsEnabled = true;
                return;
            }

            StockGenerationWrapper generation = new StockGenerationWrapper(CurrentShop);
            if (!await generation.GenerateStockAsync())
            {
                PopupMessage.Message("Stock generation failed.");
                StockGeneration.IsEnabled = true;
                return;
            }
            await PopulateShopStockAsync();
            StockGeneration.IsEnabled = true;
        }

        private async void TransferStockToTextAsync(object sender, RoutedEventArgs e)
        {
            StockToTextButton.IsEnabled = false;
            string result = await StockWriter.StockToFile(CurrentShop, CurrentShopStock.ToList());
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
    }
}
