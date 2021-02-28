// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: LOAD THE CORRESOPNDING PAGE UPON USER SELECTION
// ===============================
using Final.models;
using System;
using System.Collections.Generic;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Final
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AppWindow appWindow;
        private GlobalPropertiesAndDatabaseConnector globalProperties = GlobalPropertiesAndDatabaseConnector.Instance;

        // Track open app windoes
        public static Dictionary<UIContext, AppWindow> AppWindows { get; set; }
        = new Dictionary<UIContext, AppWindow>();
        public static ContentDialog CurrentDialog { get; set; } = null;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "home")
                {
                    NavView.SelectedItem = item;
                    break;
                }
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item = args.SelectedItem as NavigationViewItem;

            ContentFrame.Margin = new Thickness(0, 0, 0, 0);
            switch (item.Tag.ToString())
            {
                case "home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case "order":
                    ContentFrame.Navigate(typeof(OrderPage));
                    break;
                case "about":
                    ContentFrame.Navigate(typeof(InfoPage));
                    break;
                case "cart":
                    ContentFrame.Margin = new Thickness(24, 12, 0, 0);
                    ContentFrame.Navigate(typeof(CartPage));
                    break;
            }
        }

        private async void Login()
        {
            LoginDialog loginDialog = new LoginDialog();

            ReplaceDialog(loginDialog);

            try
            {
                ContentDialogResult result;
                do
                {
                    result = await loginDialog.ShowAsync();
                    if (globalProperties.IsLoggedIn)
                    {
                        loginItem.Visibility = Visibility.Collapsed;
                        logoutItem.Visibility = Visibility.Visible;
                    }
                } while (!globalProperties.IsLoggedIn && result == ContentDialogResult.Primary);
            }
            catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }

        }

        private async void Logout()
        {
            var dlg = new ContentDialog 
            {
                Title = "Logout",
                Content = "Confirm Logout?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            ReplaceDialog(dlg);

            var result = await dlg.ShowAsync();

            try
            {
                if (result == ContentDialogResult.Primary)
                {
                    globalProperties.IsLoggedIn = false;
                    globalProperties.LoggedInUser = "guest";
                    loginItem.Visibility = Visibility.Visible;
                    logoutItem.Visibility = Visibility.Collapsed;

                    await new MessageDialog("You have successfully logged out!").ShowAsync();
                }
            }
            catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        private void loginItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Login();
        }

        private void logoutItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Logout();
        }

        private void ShoppingCart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NewWindow("Shopping Cart", typeof(CartPage));
        }

        private void FeedBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NewWindow("Feedback", typeof(FeedBackPage));
        }

        // Obtain from MS docs
        // https://docs.microsoft.com/en-us/windows/uwp/design/layout/app-window
        private async void NewWindow(String Title, Type Page)
        {
            appWindow = await AppWindow.TryCreateAsync();
            appWindow.Title = Title;
            Frame appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(Page);

            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);

            AppWindows.Add(appWindowContentFrame.UIContext, appWindow);

            appWindow.Closed += delegate
            {
                MainPage.AppWindows.Remove(appWindowContentFrame.UIContext);
                appWindowContentFrame.Content = null;
                appWindow = null;
            };

            await appWindow.TryShowAsync();
        }

        // Obtain from MS docs
        // https://docs.microsoft.com/en-us/windows/uwp/design/layout/app-window
        public static void ReplaceDialog(ContentDialog dlg)
        {
            if (MainPage.CurrentDialog != null)
                MainPage.CurrentDialog.Hide();
            MainPage.CurrentDialog = dlg;
        }

        public static void ReplaceDialog(ContentDialog dlg, object sender)
        {
            if (MainPage.CurrentDialog != null)
                MainPage.CurrentDialog.Hide();
            MainPage.CurrentDialog = dlg;

            // Set the root of the dialog to the current view
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
                dlg.XamlRoot = ((Button)sender).XamlRoot;
        }
    }
}
