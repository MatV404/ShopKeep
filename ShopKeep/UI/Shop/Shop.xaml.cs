using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class Shop : Page
    {
        private ShopKeepDB.Models.Shop _currentShop;
        private User _currentUser;
        private ObservableCollection<ShopStock> _currentShopStock = new ObservableCollection<ShopStock>();
        private ObservableCollection<BuyStock> _buyStock = new ObservableCollection<BuyStock>();
        private ObservableCollection<UserItem> _userItems = new ObservableCollection<UserItem>();
        private ObservableCollection<SaleItem> _saleItems = new ObservableCollection<SaleItem>();
        private CoinTracker _coinLossTracker = new CoinTracker();
        private CoinTracker _coinGainTracker = new CoinTracker();

        public Shop()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            Tuple<ShopKeepDB.Models.Shop, User> args = (Tuple<ShopKeepDB.Models.Shop, User>)arguments.Parameter;
            _currentShop = args.Item1;
            _currentUser = args.Item2;
            this.DataContext = this;
            PopulateRelevantCollections();
        }

        private async void PopulateRelevantCollections()
        {
            await PopulateUserInventory();
            await PopulateShopStockAsync();
        }

        private async Task PopulateShopStockAsync()
        {
            var stock = await ShopKeepDB.Operations.Retrievals.ShopStockGetter.GetShopStock(_currentShop.Id);
            stock.ForEach(stockItem => _currentShopStock.Add(stockItem));
        }

        private async Task PopulateUserInventory()
        {
            var inventory = await ShopKeepDB.Operations.Retrievals.UserItemGetter.GetUserItems(_currentUser.Name);
            inventory.ForEach(item => _userItems.Add(item));
        }

        private void BackToShops(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void AddToBuyList(object sender, RoutedEventArgs e)
        {
            ShopStock selected = ShopStock.SelectedItem as ShopStock;
            int amount = (int) BuyAmount.Value;
            if (selected == null || amount <= 0)
            {
                PopupMessage.Message("No item selected, or an invalid amount entered.");
                return;
            }

            int inBuyAmount = 0;
            var existingItem =
                _buyStock.FirstOrDefault(buyStock => buyStock.OriginalShopStock.ItemId == selected.ItemId);
            if (existingItem != null)
            {
                _buyStock.Remove(existingItem);
                _coinLossTracker.UpdateValues(existingItem.TotalPriceGold * -1,
                                              existingItem.TotalPriceSilver * -1,
                                              existingItem.TotalPriceCopper * -1);
                inBuyAmount = existingItem.Amount;
            }
            amount = (amount + inBuyAmount > selected.Amount) ? selected.Amount : amount + inBuyAmount;
            BuyStock toBuy = new BuyStock(selected, amount);
            _buyStock.Add(toBuy);
            _coinLossTracker.UpdateValues(toBuy.TotalPriceGold, toBuy.TotalPriceSilver, toBuy.TotalPriceCopper);
            UpdateLossText();
        }

        private async void BuyItemsAsync(object sender, RoutedEventArgs e)
        {
            if (_buyStock.Count == 0)
            {
                return;
            }

            double userCoin = _currentUser.Coins.Gold + (double) _currentUser.Coins.Silver / 10 +
                              (double) _currentUser.Coins.Copper / 100;
            double coinLoss = _coinLossTracker.Gold + (double) _coinLossTracker.Silver / 10 +
                              (double) _coinLossTracker.Copper / 100;

            if (userCoin - coinLoss < 0)
            {
                PopupMessage.Message("Not enough coin to purchase all of this!");
                return;
            }

            ShopPurchaseHandler handler = new ShopPurchaseHandler(_currentUser, _buyStock, _coinLossTracker);
            var buyResult = await handler.HandlePurchaseAsync();
            if (buyResult == BulkTransactionResult.Failure)
            {
                PopupMessage.Message("Purchase failed.");
                _buyStock.Clear();
                return;
            }

            if (buyResult == BulkTransactionResult.PartialSuccess)
            {
                PopupMessage.Message("Some of the items were not purchased.");
                _buyStock.Clear();
            }

            if (!await CoinsControl.UpdateUserCoinsAsync(_currentUser, _coinLossTracker.Gold * -1,
                    _coinLossTracker.Silver * -1, _coinLossTracker.Copper * -1))
            {
                PopupMessage.Message("Coin update failed. :-( Please, contact a system administrator.");
            }
            this.GoldBalance.Text = _currentUser.Coins.Gold.ToString();
            this.SilverBalance.Text = _currentUser.Coins.Silver.ToString();
            this.CopperBalance.Text = _currentUser.Coins.Copper.ToString();
            this.GoldLoss.Text = "0";
            this.SilverLoss.Text = "0";
            this.CopperLoss.Text = "0";
            _buyStock.Clear();
            _userItems.Clear();
            _currentShopStock.Clear();
            PopulateRelevantCollections();
        }

        private void UpdateGainText()
        {
            this.GoldGain.Text = _coinGainTracker.Gold.ToString();
            this.SilverGain.Text = _coinGainTracker.Silver.ToString();
            this.CopperGain.Text = _coinGainTracker.Copper.ToString();
        }

        private void UpdateLossText()
        {
            this.GoldLoss.Text = _coinLossTracker.Gold.ToString();
            this.SilverLoss.Text = _coinLossTracker.Silver.ToString();
            this.CopperLoss.Text = _coinLossTracker.Copper.ToString();
        }

        private void AddToSaleItems(object sender, RoutedEventArgs e)
        {
            UserItem item = InventoryView.SelectedItem as UserItem;
            int amount = (int) SellAmount.Value;
            if (item == null || amount <= 0)
            {
                PopupMessage.Message("No item selected or invalid amount entered.");
                return;
            }

            int inSellAmount = 0;
            var existingItem = _saleItems.FirstOrDefault(saleItem => saleItem.OriginalUserItem.ItemId == item.ItemId);
            if (existingItem != null)
            {
                _saleItems.Remove(existingItem);
                _coinGainTracker.UpdateValues(existingItem.TotalPriceGold * -1,
                                              existingItem.TotalPriceSilver * -1,
                                              existingItem.TotalPriceCopper * -1);
                inSellAmount = existingItem.Amount;
            }
            amount = (amount + inSellAmount > item.Amount) ? item.Amount : amount + inSellAmount;
            SaleItem toSell = new SaleItem(item, amount);
            _saleItems.Add(toSell);
            DataContext = _coinGainTracker;
            _coinGainTracker.UpdateValues(toSell.TotalPriceGold, toSell.TotalPriceSilver, toSell.TotalPriceCopper);
            UpdateGainText();
        }

        /// <summary>
        /// This method handles the sale of items from user inventory to the shop.
        /// </summary>
        private async void SellItemsAsync(object sender, RoutedEventArgs e)
        {
            if (_saleItems.Count == 0)
            {
                return;
            }
            ShopSaleHandler handler = new ShopSaleHandler(_currentShop, _saleItems, _coinGainTracker);
            var sellResult = await handler.HandleSaleAsync();
            if (sellResult == BulkTransactionResult.Failure)
            {
                PopupMessage.Message("Sale failed.");
                _saleItems.Clear();
                return;
            }

            if (sellResult == BulkTransactionResult.PartialSuccess)
            {
                PopupMessage.Message("Some of the items were not sold.");
                _saleItems.Clear();
                return;
            }

            if (!await CoinsControl.UpdateUserCoinsAsync(_currentUser, _coinGainTracker.Gold, _coinGainTracker.Silver,
                    _coinGainTracker.Copper))
            {
                PopupMessage.Message("Coin update failed. Please, contact a system administrator.");
            }

            this.GoldBalance.Text = _currentUser.Coins.Gold.ToString();
            this.SilverBalance.Text = _currentUser.Coins.Silver.ToString();
            this.CopperBalance.Text = _currentUser.Coins.Copper.ToString();
            this.GoldGain.Text = "0";
            this.SilverGain.Text = "0";
            this.CopperGain.Text = "0";
            _saleItems.Clear();
            _userItems.Clear();
            _currentShopStock.Clear();
            PopulateRelevantCollections();
        }

        private void RemoveFromPurchase(object sender, SelectionChangedEventArgs e)
        {
            if (PurchaseView.SelectedItem is BuyStock stock)
            {
                _coinLossTracker.UpdateValues(stock.TotalPriceGold * -1,
                                              stock.TotalPriceSilver * -1,
                                              stock.TotalPriceCopper * -1);
                _buyStock.Remove(stock);
                UpdateLossText();
            }
        }

        private void RemoveFromSale(object sender, SelectionChangedEventArgs e)
        {
            if (SaleView.SelectedItem is SaleItem item)
            {
                _coinGainTracker.UpdateValues(item.TotalPriceGold * -1,
                                              item.TotalPriceSilver * -1,
                                              item.TotalPriceCopper * -1);
                _saleItems.Remove(item);
                UpdateGainText();
            }
        }
    }
}
