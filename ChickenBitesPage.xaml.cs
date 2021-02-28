// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: PROCESS THE SELECTED OPTION FOR CHICKEN BITES PAGE
// ===============================
using Final.models;
using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Final
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChickenBitesPage : Page
    {
        private List<CheckBox> Sauces;
        private ShoppingCart Cart = ShoppingCart.Instance;
        public ChickenBitesPage()
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
                chkSauce15
            };
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> Sauces = new List<string>();
            int Pieces = Convert.ToInt32(
                (cbxPieces.SelectedItem as ComboBoxItem).Tag.ToString());

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
                await new MessageDialog("Please select at least two sauce for 20 Chicken Bites.", "Unable to place item").ShowAsync();
                return;
            }

            ChickenBite bites = new ChickenBite(Pieces, Sauces);

            var dlg = new ConfirmItemDialog(bites.PricedString());

            MainPage.ReplaceDialog(dlg, sender);

            try
            {
                var result = await dlg.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Cart.Items.Add(bites);
                    ContentFrame.Navigate(typeof(ChickenBitesPage));
                }
            }
            catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
        }
    }
}
