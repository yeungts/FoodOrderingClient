// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: LOAD THE CORRESOPNDING PAGE UPON USER SELECTION (CHICKEN SPECIFC)
// ===============================
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodOrderingClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChickenBits : Page
    {
        public ChickenBits()
        {
            this.InitializeComponent();

            List<NavigationViewItem> Items = new List<NavigationViewItem>();

            Items.Add(new NavigationViewItem()
            {
                Content = "Chicken Wings",
                Tag = "wings"
            });

            Items.Add(new NavigationViewItem()
            {
                Content = "Chicken Bites",
                Tag = "bites"
            });

            NavView.MenuItemsSource = Items;
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item = args.SelectedItem as NavigationViewItem;

            switch (item.Tag.ToString())
            {
                case "wings":
                    ContentFrame.Navigate(typeof(ChickenWingsPage));
                    break;
                case "bites":
                    ContentFrame.Navigate(typeof(ChickenBitesPage));
                    break;
            }
        }
    }
}
