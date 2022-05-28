using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ShopKeepDB.Misc;
using ShopKeepDB.Operations.Delete;
using ShopKeepDB.Operations.Retrievals;
using ShopKeepDB.Operations.Update;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeep.UI.Item
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///
    public sealed partial class ItemManagement : Page
    {
        private ShopKeepDB.Models.Item _examinedItem;
        private readonly ObservableCollection<Type> _examinedTypes = new ObservableCollection<Type>();

        public ObservableCollection<string> Rarities { get; } = new ObservableCollection<string>();
        public ItemManagement()
        {
            InitializeComponent();
            foreach (var rarity in Constants.Rarities)
            {
                Rarities.Add(rarity);
            }
        }

        private async void PopulateExaminedTypes()
        {
            var types = await TypeGetter.GetAllTypesByIdAsync(
                                _examinedItem.ItemTypes.Select(itemType => itemType.TypeId).ToList());

            foreach (var type in types)
            {
                _examinedTypes.Add(type);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs arguments)
        {
            base.OnNavigatedTo(arguments);
            _examinedItem = arguments.Parameter as ShopKeepDB.Models.Item;
            PopulateExaminedTypes();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void EditItemAsync(object sender, RoutedEventArgs e)
        {
            string itemName = string.IsNullOrWhiteSpace(ItemName.Text) ? _examinedItem.Name : ItemName.Text;
            string itemRarity = ItemRarity.SelectionBoxItem == null
                ? _examinedItem.Rarity
                : ItemRarity.SelectionBoxItem.ToString();
            string itemDescription = string.IsNullOrWhiteSpace(ItemDescription.Text) 
                ? _examinedItem.Description 
                : ItemDescription.Text;
            int goldPrice = (int)(GoldPrice.Value >= 0 ? GoldPrice.Value : 0);
            int silverPrice = (int)(SilverPrice.Value >= 0 ? SilverPrice.Value : 0);
            int copperPrice = (int)(CopperPrice.Value >= 0 ? CopperPrice.Value : 0);
            
            if (goldPrice == 0 && silverPrice == 0 && copperPrice == 0)
            {
                PopupMessage.Message("Price can't be set to 0!");
                return;
            }
            var item = await Task.Run(() => ItemUpdate.UpdateItemAsync(_examinedItem, itemName, itemRarity,
                                                                          itemDescription, goldPrice, silverPrice, copperPrice));
            CopperPriceText.Text = item.BaseItemPrice.Copper.ToString();
            SilverPriceText.Text = item.BaseItemPrice.Silver.ToString();
            GoldPriceText.Text = item.BaseItemPrice.Gold.ToString();
            RarityText.Text = item.Rarity;
            DescriptionText.Text = item.Description;
        }

        private async void RemoveTypesFromItemAsync(object sender, RoutedEventArgs e)
        {
            if (Types.SelectedItems.Count == Types.Items.Count)
            {
                PopupMessage.Message("You can't remove all the types!", "Okay...");
                return;
            }

            List<int> selectedTypeIds = new List<int>();
            foreach (Type item in Types.SelectedItems)
            {
                selectedTypeIds.Add(item.Id);
            }

            var result = await Task.Run(() => ItemTypeRemover.RemoveItemTypes(_examinedItem.Id, selectedTypeIds));
            if (!result)
            {
                PopupMessage.Message("Something went wrong while removing types.", "What a shame.");
            }

            foreach (Type item in Types.SelectedItems)
            {
                _examinedTypes.Remove(item);
            }
        }
    }
}
