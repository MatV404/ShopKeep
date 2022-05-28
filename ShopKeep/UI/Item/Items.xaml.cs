using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Delete;
using ShopKeepDB.Operations.Retrievals;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShopKeep.UI.Item
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Items : Page
    {
        public ObservableCollection<ShopKeepDB.Models.Item> AllItems { get; private set; } =
            new ObservableCollection<ShopKeepDB.Models.Item>();

        public ObservableCollection<Type> Types { get; private set; } =
            new ObservableCollection<Type>();

        public ObservableCollection<string> Rarities { get; private set; } =
            new ObservableCollection<string>();
        public Items()
        {
            InitializeComponent();
            PopulateAllItems();
            PopulateTypes();
            foreach (var rarity in Constants.Rarities)
            {
                Rarities.Add(rarity);
            }
        }

        private async void PopulateTypes()
        {
            var contents = await Task.Run(() => TypeGetter.GetAllTypesAsync());
            contents.ForEach(type => Types.Add(type));
        }

        private async void PopulateAllItems()
        {
            var contents = await Task.Run(() => ItemGetter.GetAllItemsAsync());
            contents.ForEach(item => AllItems.Add(item));
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void FilterItems(object sender, RoutedEventArgs e)
        {
            string itemName = ItemName.Text;
            Type itemType = (Type)ItemType.SelectionBoxItem;
            string itemRarity = ItemRarity.SelectionBoxItem?.ToString();
            int minGold = PriceMinGold.Value >= 0 ? (int)PriceMinGold.Value : 0;
            int maxGold = PriceMaxGold.Value >= 0 ? (int)PriceMaxGold.Value : int.MaxValue;
            int minSilver = PriceMinSilver.Value >= 0 ? (int)PriceMinSilver.Value : 0;
            int maxSilver = PriceMaxSilver.Value >= 0 ? (int)PriceMaxSilver.Value : int.MaxValue;
            int minCopper = PriceMinCopper.Value >= 0 ? (int)PriceMinCopper.Value : 0;
            int maxCopper = PriceMaxCopper.Value >= 0 ? (int)PriceMaxCopper.Value : int.MaxValue;
            var result = await Task.Run(() => ItemGetter.FilterItemsAsync(itemName, itemType, itemRarity,
                                                                                          minGold, maxGold, minSilver,
                                                                                          maxSilver, minCopper, maxCopper));
            AllItems.Clear();
            foreach (ShopKeepDB.Models.Item item in result)
            {
                AllItems.Add(item);
            }
        }

        private void ClearFilter(object sender, RoutedEventArgs e)
        {
            AllItems.Clear();
            ItemName.Text = "";
            ItemType.SelectedItem = null;
            ItemRarity.SelectedItem = null;
            PriceMinGold.Value = 0;
            PriceMaxGold.Value = 0;
            PriceMinSilver.Value = 0;
            PriceMaxSilver.Value = 0;
            PriceMaxCopper.Value = 0;
            PriceMinCopper.Value = 0;
            PopulateAllItems();
        }

        private async void DeleteFiltered(object sender, RoutedEventArgs e)
        {
            string itemName = ItemName.Text;
            Type itemType = (Type)ItemType.SelectionBoxItem;
            string itemRarity = ItemRarity.SelectionBoxItem?.ToString();
            int minGold = PriceMinGold.Value >= 0 ? (int)PriceMinGold.Value : 0;
            int maxGold = PriceMaxGold.Value >= 0 ? (int)PriceMaxGold.Value : int.MaxValue;
            int minSilver = PriceMinSilver.Value >= 0 ? (int)PriceMinSilver.Value : 0;
            int maxSilver = PriceMaxSilver.Value >= 0 ? (int)PriceMaxSilver.Value : int.MaxValue;
            int minCopper = PriceMinCopper.Value >= 0 ? (int)PriceMinCopper.Value : 0;
            int maxCopper = PriceMaxCopper.Value >= 0 ? (int)PriceMaxCopper.Value : int.MaxValue;
            var toDelete = await Task.Run(() => ItemGetter.FilterItemsAsync(itemName, itemType, itemRarity,
                minGold, maxGold, minSilver,
                maxSilver, minCopper, maxCopper));
            await Task.Run(() => ItemDestroyer.DeleteItems(toDelete));
            ClearFilter(null, null);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(ItemManagement), ItemView.SelectedItem);
        }
    }
}
