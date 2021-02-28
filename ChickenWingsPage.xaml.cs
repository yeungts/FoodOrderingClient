// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: PROCESS THE SELECTED OPTION FOR CHICKEN WING PAGE
// ===============================
using FoodOrderingClient.models;
using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodOrderingClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChickenWingsPage : Page
    {
        private List<CheckBox> Sauces;
        private ShoppingCart Cart = ShoppingCart.Instance;

        public ChickenWingsPage()
        {
            this.InitializeComponent();
            Sauces = new List<CheckBox>
            {
                chkSauce1,
                chkSauce2,
                chkSauce3,
                chkSauce4,
                chkSauce5,
                chkSauce6,
                chkSauce7,
                chkSauce8,
                chkSauce9,
                chkSauce10,
                chkSauce11,
                chkSauce12,
                chkSauce13,
                chkSauce14,
                chkSauce15,
                chkSauce16,
                chkSauce17,
                chkSauce18,
                chkSauce19,
                chkSauce20
            };
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> Sauces = new List<string>();
            int Pieces = Convert.ToInt32((cbxPieces.SelectedItem as ComboBoxItem).Tag.ToString());

            foreach (CheckBox sauce in this.Sauces)
            {
                if ((bool)sauce.IsChecked)
                    Sauces.Add(sauce.Content.ToString());
            }

            if (Sauces.Count == 0)
            {
                await new MessageDialog("Please select at least one sauce.", "Unable to place item").ShowAsync();
                return;
            }
            else if (Pieces == 20 && Sauces.Count == 1)
            {
                await new MessageDialog("Please select at least two sauce for 20 Chicken Wings.", "Unable to place item").ShowAsync();
                return;
            }

            ChickenWing wings = new ChickenWing(Pieces,
                (cbxClassicBreaded.SelectedItem as ComboBoxItem).Content.ToString(), Sauces);

            var dlg = new ConfirmItemDialog(wings.PricedString());

            MainPage.ReplaceDialog(dlg, sender);

            try
            {
                var result = await dlg.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Cart.Items.Add(wings);
                    ContentFrame.Navigate(typeof(ChickenWingsPage));
                }
            }
            catch (Exception) {}
        }
    }
}
