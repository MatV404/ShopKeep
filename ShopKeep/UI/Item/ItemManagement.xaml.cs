using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ShopKeep.UI.Admin;
using ShopKeepDB.Misc;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Item
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///
    public sealed partial class ItemManagement : Page
    {
        public ShopKeepDB.Models.Item ExaminedItem { get; private set; }
        public List<ShopKeepDB.Models.Type> ExaminedTypes { get; private set; }

        public ObservableCollection<string> Rarities { get; private set; } = new ObservableCollection<string>();
        public ItemManagement()
        {
            this.InitializeComponent();
            foreach (var rarity in Constants.Rarities)
            {
                Rarities.Add(rarity);
            }
        }

        private async void PopulateExaminedTypes()
        {
            this.ExaminedTypes =
                await ShopKeepDB.Operations.Retrievals.TypeGetter.GetAllTypesByIdAsync(
                    ExaminedItem.ItemTypes.Select(itemType => itemType.TypeId).ToList());
        }

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            this.ExaminedItem = arguments.Parameter as ShopKeepDB.Models.Item;
            PopulateExaminedTypes();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        //ToDo: Figure out how to refresh the textbox stuff.
        private async void EditItemAsync(object sender, RoutedEventArgs e)
        {
            string itemName = String.IsNullOrWhiteSpace(this.ItemName.Text) ? ExaminedItem.Name : this.ItemName.Text;
            string itemRarity = ItemRarity.SelectionBoxItem == null
                ? ExaminedItem.Rarity
                : ItemRarity.SelectionBoxItem.ToString();
            string itemDescription = String.IsNullOrWhiteSpace(this.ItemDescription.Text) ? ExaminedItem.Description : this.ItemDescription.Text;
            int goldPrice = this.GoldPrice.Value >= 0 
                ? (int) this.GoldPrice.Value 
                : ExaminedItem.BaseItemPrice.Gold;
            int silverPrice = this.SilverPrice.Value >= 0
                ? (int) this.SilverPrice.Value
                : ExaminedItem.BaseItemPrice.Silver;
            int copperPrice = this.CopperPrice.Value >= 0
                ? (int) this.CopperPrice.Value
                : ExaminedItem.BaseItemPrice.Copper;
            var item = await ShopKeepDB.Operations.Update.ItemUpdate.UpdateItemAsync(ExaminedItem, itemName, itemRarity, 
                                                                          itemDescription, goldPrice, silverPrice, copperPrice);
        }

        private async void RemoveTypesFromItemAsync(object sender, RoutedEventArgs e)
        {
            if (Types.SelectedItems.Count == Types.Items.Count)
            {
                PopupMessage.Message("You can't remove all the types!", "Okay...");
                return;
            }

            List<int> selectedTypeIds = new List<int>();
            foreach (ShopKeepDB.Models.Type item in Types.SelectedItems)
            {
                selectedTypeIds.Add(item.Id);
            }

            var result = await ShopKeepDB.Operations.Delete.ItemTypeRemover.RemoveItemTypes(ExaminedItem.Id, selectedTypeIds);
            if (!result)
            {
                PopupMessage.Message("Something went wrong while removing types.", "What a shame.");
            }
        }
    }
}
