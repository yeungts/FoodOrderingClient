// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: LOAD AVAILABLE ITEM OPTIONS INTO THE UI AND HANDLE USER INTERACTION
// ===============================
using FoodOrderingClient.models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodOrderingClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {

        private GlobalPropertiesAndDatabaseConnector globalProperties = GlobalPropertiesAndDatabaseConnector.Instance;

        public OrderPage()
        {
            this.InitializeComponent();

            List<NavigationViewItem> Items = new List<NavigationViewItem>();

            foreach (PizzaType pizzaType in globalProperties.PizzaTypes)
            {
                Items.Add(new NavigationViewItem()
                {
                    Content = pizzaType.Name,
                    Tag = pizzaType.Tag
                });

            }

            Items.Add(new NavigationViewItem()
            {
                Content = "Chicken That Comes Apart",
                Tag = "chickenBits"
            });

            Items.Add(new NavigationViewItem()
            {
                Content = "Fries and sides",
                Tag = "sides"
            });

            Items.Add(new NavigationViewItem()
            {
                Content = "Sauces",
                Tag = "sauces"
            });

            Items.Add(new NavigationViewItem()
            {
                Content = "Salad",
                Tag = "salad"
            });

            Items.Add(new NavigationViewItem()
            {
                Content = "Drinks",
                Tag = "drinks"
            });

            NavView.MenuItemsSource = Items;
        }

        // Load the page of selected option into the frame
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item = args.SelectedItem as NavigationViewItem;

            bool itemFound = false;

            foreach(PizzaType pizzaType in globalProperties.PizzaTypes)
            {
                if (item.Tag.ToString().Equals(pizzaType.Tag))
                {
                    globalProperties.SelectedPizzaName = pizzaType.Name;
                    ContentFrame.Margin = new Thickness(24, 12, 0, 0);
                    ContentFrame.Navigate(typeof(PizzaPage));
                    itemFound = true;
                    break;
                }
            }

            if (itemFound) { return; }

            switch (item.Tag.ToString())
            {
                case "chickenBits":
                    ContentFrame.Margin = new Thickness(0, 0, 0, 0);
                    ContentFrame.Navigate(typeof(ChickenBits));
                    break;
                case "sides":
                case "sauces":
                case "salad":
                case "drinks":
                    ContentFrame.Margin = new Thickness(24, 12, 0, 0);
                    ContentFrame.Navigate(typeof(UnderConstruction));
                    break;
            }
        }
    }
}
