// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: PROCESS THE SELECTED OPTION FOR PIZZA PAGE
// ===============================
using Final.models;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Final
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PizzaPage : Page
    {
        private List<CheckBox> Toppings;
        private GlobalPropertiesAndDatabaseConnector globalProperties = GlobalPropertiesAndDatabaseConnector.Instance;
        private ShoppingCart Cart = ShoppingCart.Instance;

        public PizzaPage()
        {
            this.InitializeComponent();
            pizzaName.Text = globalProperties.SelectedPizzaName;
            Toppings = new List<CheckBox>
            {
                chkTopping1,
                chkTopping2,
                chkTopping3,
                chkTopping4,
                chkTopping5,
                chkTopping6,
                chkTopping7,
                chkTopping8,
                chkTopping9,
                chkTopping10,
                chkTopping11,
                chkTopping12,
                chkTopping13,
                chkTopping14,
                chkTopping15,
                chkTopping16,
                chkTopping17,
                chkTopping18,
                chkTopping19,
                chkTopping20,
                chkTopping21,
                chkTopping22,
                chkTopping23,
                chkTopping24,
                chkTopping25,
                chkTopping26,
                chkTopping27,
                chkTopping28,
                chkTopping29,
                chkTopping30,
                chkTopping31,
                chkTopping32,
                chkTopping33,
                chkTopping34,
                chkTopping35,
                chkTopping36,
                chkTopping37,
                chkTopping38,
                chkTopping39,
                chkTopping40,
                chkTopping41
            };
            btnVeggie_Click(this, new RoutedEventArgs());
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            List<Topping> Toppings = new List<Topping>();
            foreach (CheckBox topping in this.Toppings)
            {
                if ((bool)topping.IsChecked)
                    Toppings.Add(new Topping(topping.Content.ToString(), float.Parse(topping.Tag.ToString()), 1));
            }

            Pizza pizza = new Pizza(globalProperties.SelectedPizzaName, (cbxSize.SelectedItem as ComboBoxItem).Content.ToString(),
                (cbxDough.SelectedItem as ComboBoxItem).Content.ToString(), (cbxSauce.SelectedItem as ComboBoxItem).Content.ToString(),
                (cbxCheese.SelectedItem as ComboBoxItem).Content.ToString(), Toppings);

            var dlg = new ConfirmItemDialog(pizza.PricedString());

            MainPage.ReplaceDialog(dlg, sender);

            try
            {
                var result = await dlg.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Cart.Items.Add(pizza);
                    ContentFrame.Navigate(typeof(PizzaPage));
                }
            }
            catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
        }

        private void btnVeggie_Click(object sender, RoutedEventArgs e)
        {
            meatTopping.Visibility = Visibility.Collapsed;
            veggieTopping.Visibility = Visibility.Visible;
            cheeseTopping.Visibility = Visibility.Collapsed;
            btnVeggie.Background = new SolidColorBrush(Colors.Gray);
            btnMeat.Background = new SolidColorBrush(Colors.Black);
            btnCheese.Background = new SolidColorBrush(Colors.Black);
        }

        private void btnMeat_Click(object sender, RoutedEventArgs e)
        {
            meatTopping.Visibility = Visibility.Visible;
            veggieTopping.Visibility = Visibility.Collapsed;
            cheeseTopping.Visibility = Visibility.Collapsed;
            btnVeggie.Background = new SolidColorBrush(Colors.Black);
            btnMeat.Background = new SolidColorBrush(Colors.Gray);
            btnCheese.Background = new SolidColorBrush(Colors.Black);
        }

        private void btnCheese_Click(object sender, RoutedEventArgs e)
        {
            meatTopping.Visibility = Visibility.Collapsed;
            veggieTopping.Visibility = Visibility.Collapsed;
            cheeseTopping.Visibility = Visibility.Visible;
            btnVeggie.Background = new SolidColorBrush(Colors.Black);
            btnMeat.Background = new SolidColorBrush(Colors.Black);
            btnCheese.Background = new SolidColorBrush(Colors.Gray);
        }
    }
}
