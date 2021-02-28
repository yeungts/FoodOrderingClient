// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 7 AUG 2020
// PURPOSE: DISPLAY THE ITEMS IN CART, ALLOW USER TO MODIFY, DELETE, OR CHECK OUT
// ===============================
using FoodOrderingClient.models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodOrderingClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CartPage : Page
    {
        private GlobalPropertiesAndDatabaseConnector globalProperties = GlobalPropertiesAndDatabaseConnector.Instance;
        private ShoppingCart Cart = ShoppingCart.Instance;
        private float SubTotal = 0;
        private float Tax = 0;

        public CartPage()
        {
            this.InitializeComponent();

            // Display different content when there are no items in cart
            if (Cart.Items.Count == 0)
            {
                lblNoItem.Visibility = Visibility.Visible;
                lblItems.Visibility = Visibility.Collapsed;
            }
            else if (Cart.Items.Count != 0)
            {
                lblNoItem.Visibility = Visibility.Collapsed;
                lblItems.Visibility = Visibility.Visible;

                ItemList.ItemsSource = Cart.Items;
            }

            foreach (Item item in Cart.Items)
                SubTotal += item.SubTotal;

            if (globalProperties.IsLoggedIn)
                SubTotal = (float)(SubTotal * 0.9);

            Tax = (float)Math.Round((0.13 * (double)SubTotal), 2);

            lblSubTotal.Text = "$" + Math.Round(SubTotal, 2);
            lblTax.Text = "$" + Tax;
            lblTotal.Text = "$" + Math.Round((SubTotal + Tax), 2);
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string ItemSummary = (sender as Button).Tag.ToString();

            foreach (Item item in Cart.Items)
                if (item.Summary.Equals(ItemSummary))
                {
                    var dlg = new ContentDialog
                    {
                        Title = "Remove Item",
                        Content = "Confirm removing:\n" + item.Summary,
                        PrimaryButtonText = "Remove",
                        CloseButtonText = "Cancel"
                    };

                    MainPage.ReplaceDialog(dlg, sender);

                    try
                    {
                        var result = await dlg.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            Cart.Items.Remove(item);
                            ContentFrame.Navigate(typeof(CartPage));
                        }
                        break;
                    }
                    catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
                }
        }

        // Display receipt and ask for payment method
        private async void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            string summary = "";
            SubTotal = 0;
            btnRefresh_Click(this, new RoutedEventArgs());

            // Abort action when cart is empty
            if (Cart.Items.Count == 0)
            {
                var dlgEmpty = new ContentDialog
                {
                    Title = "Unable to checkout",
                    Content = "There are no item placed in cart",
                    PrimaryButtonText = "OK"
                };

                MainPage.ReplaceDialog(dlgEmpty, sender);

                try
                {
                    await dlgEmpty.ShowAsync();
                    return;
                }
                catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
            }

            // generate receipt
            string receipt = "\n\t       Papa Dario's Pizza";
            receipt += "\n\tOrdered at: " + DateTime.Now.ToString();
            receipt += "\n----------------------------------------------\n\n";

            foreach (Item item in Cart.Items)
            {
                receipt += item.PricedString() + "\n\n";
                SubTotal += item.SubTotal;
                summary += item.Summary + "\n";
            }

            if (globalProperties.IsLoggedIn)
                SubTotal = (float)(SubTotal * 0.9);

            Tax = (float)Math.Round((0.13 * (double)SubTotal), 2);

            receipt += "----------------------------------------------\n";
            receipt += String.Format("{0, 38} {1, 7}\n", "Subtotal:", "$" + Math.Round(SubTotal, 2));
            receipt += String.Format("{0, 38} {1, 7}\n", "Tax:", "$" + Math.Round(Tax, 2));
            receipt += String.Format("{0, 38} {1, 7}\n", "Total:", "$" + Math.Round((SubTotal + Tax), 2));
            receipt += "\n\n  Thank you for purchasing from Papa Dario's!";

            var dlg = new ReceiptDialog(receipt);

            MainPage.ReplaceDialog(dlg, sender);

            try
            {
                var result = await dlg.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    int orderId = globalProperties.PlaceOrder(summary, Math.Round(SubTotal, 2), Math.Round(Tax, 2), Math.Round((SubTotal + Tax), 2));
                    foreach (Item item in Cart.Items)
                        globalProperties.PlaceOrderedItem(orderId, item.Summary);
                    Cart.Items.Clear();

                    var dlgSuccess = new ContentDialog
                    {
                        Title = "Order Placed",
                        Content = "Thank you for ordering from Papa Dario's!",
                        PrimaryButtonText = "Close"
                    };

                    MainPage.ReplaceDialog(dlgSuccess, sender);

                    await dlgSuccess.ShowAsync();
                    ContentFrame.Navigate(typeof(CartPage));
                }
                return;
            }
            catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
        }

        // Refresh this page
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(CartPage));
        }

        // Prompt user if they really wish to empty cart, only empty cart when user confirm
        private async void btnEmptyCart_Click(object sender, RoutedEventArgs e)
        {
            if (ItemList.Items.Count == 0)
            {
                var dlgNotEmpty = new ContentDialog
                {
                    Title = "Unable To Empty Cart",
                    Content = "Cart is already empty.",
                    PrimaryButtonText = "OK"
                };

                MainPage.ReplaceDialog(dlgNotEmpty, sender);

                try
                {
                    await dlgNotEmpty.ShowAsync();
                    return;
                }
                catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
            }

            var dlgEmpty = new ContentDialog
            {
                Title = "Empty Cart",
                Content = "Are you sure about emptying the cart?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            MainPage.ReplaceDialog(dlgEmpty, sender);

            try
            {
                var result = await dlgEmpty.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Cart.Items.Clear();
                    ContentFrame.Navigate(typeof(CartPage));
                }
                return;
            }
            catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
        }
    }
}
