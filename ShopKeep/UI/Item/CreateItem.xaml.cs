using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Create;
using ShopKeepDB.Operations.Retrievals;

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
            InitializeComponent();
            PopulateItemTypes();
            foreach (var rarity in Constants.Rarities)
            {
                Rarities.Add(rarity);
            }
        }

        private async void PopulateItemTypes()
        {
            var contents = await TypeGetter.GetAllTypesAsync();
            contents.ForEach(type => Types.Add(type));
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void CreateClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ItemNameBox.Text) || ItemTypes.SelectedItems.Count == 0 || ItemRarityBox.SelectionBoxItem == null)
            {
                PopupMessage.Message("No item name entered, no types chosen or no rarity chosen.");
                return;
            }
            string itemName = ItemNameBox.Text;
            string itemRarity = string.IsNullOrWhiteSpace(ItemRarityBox.SelectionBoxItem.ToString()) ? "Common" : ItemRarityBox.SelectionBoxItem.ToString();
            int priceGold = (int) (ItemPriceGoldBox.Value >= 0 ? ItemPriceGoldBox.Value : 0);
            int priceSilver = (int)(ItemPriceSilverBox.Value >= 0 ? ItemPriceSilverBox.Value : 0);
            int priceCopper = (int)(ItemPriceCopperBox.Value >= 0 ? ItemPriceCopperBox.Value : 0);
            string description = ItemDescriptionBox.Text;

            if (priceGold == 0 && priceSilver == 0 && priceCopper == 0)
            {
                PopupMessage.Message("Price can't be set to 0.");
                return;
            }

            List<Type> types = new List<Type>();
            foreach (var type in ItemTypes.SelectedItems)
            {
                types.Add((Type)type);
            }

            bool result = await Task.Run(() => ItemCreator.CreateItemAsync(itemName, itemRarity,
                description, priceGold, priceSilver, priceCopper, types));
            if (!result)
            {
                PopupMessage.Message("Item creation failed.");
            }
            PopupMessage.Message("Item successfully created!", "Yay!");
        }
    }
}
