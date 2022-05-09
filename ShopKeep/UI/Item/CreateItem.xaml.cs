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
using ShopKeep.UI.Admin;
using ShopKeepDB.Misc;
using ShopKeepDB.Operations;
using Type = ShopKeepDB.Models.Type;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Item
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateItem : Page
    {
        public ObservableCollection<Type> Types { get; set; } = new ObservableCollection<Type>();
        public ObservableCollection<string> Rarities { get; set; } = new ObservableCollection<string>();
        public CreateItem()
        {
            this.InitializeComponent();
            PopulateItemTypes();
            foreach (var rarity in Constants.Rarities)
            {
                Rarities.Add(rarity);
            }
        }

        private async void PopulateItemTypes()
        {
            var contents = await ShopKeepDB.Operations.Retrievals.TypeGetter.GetAllTypesAsync();
            contents.ForEach(type => Types.Add(type));
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void CreateClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ItemNameBox.Text) || ItemTypes.SelectedItems.Count == 0)
            {
                PopupMessage.Message("No item name entered or no types chosen.", "Okay");
                return;
            }
            string itemName = ItemNameBox.Text;
            string itemRarity = string.IsNullOrWhiteSpace(ItemRarityBox.SelectionBoxItem.ToString()) ? "Common" : ItemRarityBox.SelectionBoxItem.ToString();
            int priceGold = string.IsNullOrWhiteSpace(ItemPriceGoldBox.Text) ? 0 : int.Parse(ItemPriceGoldBox.Text);
            int priceSilver = string.IsNullOrWhiteSpace(ItemPriceSilverBox.Text) ? 0 : int.Parse(ItemPriceSilverBox.Text);
            int priceCopper = string.IsNullOrWhiteSpace(ItemPriceCopperBox.Text) ? 0 : (int.Parse(ItemPriceCopperBox.Text));
            if (priceGold < 0 || priceSilver < 0 || priceCopper < 0)
            {
                PopupMessage.Message("Price can't be less than 0.", "Okay");
                return;
            }

            if (priceGold == 0 && priceSilver == 0 && priceCopper == 0)
            {
                PopupMessage.Message("Price can't be 0.", "Okay");
                return;
            }
            
            List<Type> types = new List<Type>();
            foreach (var type in ItemTypes.SelectedItems)
            {
                types.Add((Type) type);
            }

            bool result = await ShopKeepDB.Operations.Create.ItemCreator.CreateItemAsync(itemName, itemRarity,
                this.ItemDescriptionBox.Text, priceGold, priceSilver, priceCopper, types);
            if (!result)
            {
                PopupMessage.Message("Item creation failed.", "Okay");
            }
            PopupMessage.Message("Item successfully created!", "Yay!");
        }
    }
}
