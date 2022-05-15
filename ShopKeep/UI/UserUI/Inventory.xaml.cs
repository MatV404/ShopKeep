using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ShopKeepDB.Models;
using Type = ShopKeepDB.Models.Type;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.UserUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Inventory : Page
    {

        private User _currentUser;
        private ObservableCollection<UserItem> _inventoryItems = new ObservableCollection<UserItem>();
        private ObservableCollection<string> _rarities = new ObservableCollection<string>();
        private ObservableCollection<Type> _types = new ObservableCollection<Type>();
        public Inventory()
        {
            InitializeComponent();
            foreach (var rarity in ShopKeepDB.Misc.Constants.Rarities)
            {
                _rarities.Add(rarity);
            }
            PopulateTypesAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            Tuple<ShopKeepDB.Models.User, bool> args = (Tuple<ShopKeepDB.Models.User, bool>) arguments.Parameter;
            _currentUser = args.Item1;
            PopulateItemsAsync();
            if (!args.Item2)
            {
                GoldBox.Visibility = Visibility.Collapsed;
                SilverBox.Visibility = Visibility.Collapsed;
                CopperBox.Visibility = Visibility.Collapsed;
                ChangeBalanceButton.Visibility = Visibility.Collapsed;
                ItemAmountBox.Visibility = Visibility.Collapsed;
                RemoveItemButton.Visibility = Visibility.Collapsed;
                BanButton.Visibility = Visibility.Collapsed;
                UnbanButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void PopulateItemsAsync()
        {
            _inventoryItems.Clear();
            var items = await ShopKeepDB.Operations.Retrievals.UserItemGetter.GetUserItems(_currentUser.Name);
            foreach (var item in items)
            {
                _inventoryItems.Add(item);
            }
        }

        private async void PopulateTypesAsync()
        {
            var types = await ShopKeepDB.Operations.Retrievals.TypeGetter.GetAllTypesAsync();
            foreach (var type in types)
            {
                _types.Add(type);
            }
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void BanUser(object sender, RoutedEventArgs e)
        {
            var banResult = await ShopKeepDB.Operations.Update.UserUpdate.ChangeUserBanState(_currentUser, false);
            PopupMessage.Message(banResult ? "User banned." : "Ban failed.", "Okay");
        }

        private async void FilterItemsClickAsync(object sender, RoutedEventArgs e)
        {
            string itemName = string.IsNullOrWhiteSpace(ItemNameBox.Text) ? "" : ItemNameBox.Text;
            string itemRarity = string.IsNullOrWhiteSpace(ItemRarityBox.SelectionBoxItem?.ToString())
                ? null
                : ItemRarityBox.SelectionBoxItem.ToString();
            int? itemTypeId = ((Type) ItemTypeBox.SelectionBoxItem)?.Id;
            var results =
                await ShopKeepDB.Operations.Retrievals.UserItemGetter.FilterUserItems(_currentUser.Name, itemName,
                    itemRarity, itemTypeId);
            if (results == null)
            {
                PopupMessage.Message("Something went wrong with the database while filtering items.");
                return;
            }
            _inventoryItems.Clear();
            results.ForEach(item => _inventoryItems.Add(item));
        }

        private void ClearFilterClick(object sender, RoutedEventArgs e)
        {
            ItemNameBox.Text = "";
            ItemRarityBox.SelectedItem = null;
            ItemTypeBox.SelectedItem = null;
            _inventoryItems.Clear();
            PopulateItemsAsync();
        }

        private async void RemoveItemClickAsync(object sender, RoutedEventArgs e)
        {
            if (InventoryView.SelectedItems.Count == 0)
            {
                PopupMessage.Message("No items selected.");
                return;
            }
            int itemAmount = (int)(ItemAmountBox.Value >= 0 ? ItemAmountBox.Value : 0);
            List<UserItem> toDelete = new List<UserItem>();
            List<UserItem> toUpdate = new List<UserItem>();
            foreach (UserItem item in InventoryView.SelectedItems)
            {
                if (item.Amount - itemAmount <= 0 || itemAmount == 0)
                {
                    toDelete.Add(item);
                }
                else
                {
                    toUpdate.Add(item);
                }
            }

            if (toDelete.Count > 0)
            {
                var result = await ShopKeepDB.Operations.Delete.UserItemRemover.DeleteUserItemsAsync(toDelete);
                if (!result)
                {
                    PopupMessage.Message("An error occurred while trying to delete items from the inventory.");
                    return;
                }
            }

            var updateResult = true;
            if (toUpdate.Count > 0)
            {
                foreach (UserItem item in toUpdate)
                {
                    if (!await ShopKeepDB.Operations.Update.UserItemUpdate.ChangeUserItemAmountAsync(item,
                            item.Amount - itemAmount))
                    {
                        updateResult = false;
                    }
                }
                
            }

            PopulateItemsAsync();
            if (!updateResult)
            {
                PopupMessage.Message("One or more of the items were not updated due to a database issue.");
                return;
            }
            PopupMessage.Message("Item amounts updated or removed!");

        }

        /// <summary>
        /// Admin-only function. Changes user balance.
        /// </summary>
        private async void ChangeBalanceClick(object sender, RoutedEventArgs e)
        {
            int goldChange = (int)(GoldBox.Value >= 0 ? GoldBox.Value : _currentUser.Coins.Gold);
            int silverChange = (int)(SilverBox.Value >= 0 ? SilverBox.Value : _currentUser.Coins.Silver);
            int copperChange = (int)(CopperBox.Value >= 0 ? CopperBox.Value : _currentUser.Coins.Copper);

            var result = await ShopKeepDB.Operations.Update.CoinsUpdate.UpdateCoins(_currentUser.Coins, goldChange, 
                                                                                        silverChange, copperChange);
            if (result)
            {
                PopupMessage.Message($"{_currentUser.Name}'s balance changed.");
                GoldText.Text = goldChange.ToString();
                SilverText.Text = silverChange.ToString();
                CopperText.Text = copperChange.ToString();
                return;
            }
            PopupMessage.Message("Something went wrong, balance likely not changed.");
        }

        private async void UnbanUser(object sender, RoutedEventArgs e)
        {
            var unbanResult = await ShopKeepDB.Operations.Update.UserUpdate.ChangeUserBanState(_currentUser, true);
            PopupMessage.Message(unbanResult ? "User unbanned." : "Unban failed.", "Okay");
        }
    }
}
