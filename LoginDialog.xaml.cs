// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 6 AUG 2020
// PURPOSE: PROCESS THE INPUT AND SELECTED OPTION FOR LOGIN DIALOG
// ===============================
using Final.models;
using System;
using System.Security.Cryptography;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Final
{
    public sealed partial class LoginDialog : ContentDialog
    {
        private GlobalPropertiesAndDatabaseConnector globalProperties = GlobalPropertiesAndDatabaseConnector.Instance;

        public LoginDialog()
        {
            this.InitializeComponent();
        }

        private async void LoginDialog_LoginClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string user = userid.Text;
            string pwd = password.Password.ToString();

            // connect to db and check if user exist and check if password matches;
            using (SHA256 sha256hash = SHA256.Create())
            {
                string hash = globalProperties.GetHash(sha256hash, pwd);
                if (globalProperties.IsCredentialValid(user, hash))
                {
                    errorMsg.Visibility = Visibility.Collapsed;
                    globalProperties.IsLoggedIn = true;
                    globalProperties.LoggedInUser = user;

                    var dlg = new MessageDialog($"Welcome back! {user}");
                    dlg.Commands.Add(new UICommand("Continue", null));
                    dlg.CancelCommandIndex = 0;
                    await dlg.ShowAsync();
                }
                else
                {
                    errorMsg.Visibility = Visibility.Visible;
                    globalProperties.IsLoggedIn = false;
                }
            }

        }

        private void LoginDialog_CancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var registerDialog = new RegisterDialog();

            MainPage.ReplaceDialog(registerDialog);

            try
            {
                ContentDialogResult registerResult;
                do
                {
                    registerResult = await registerDialog.ShowAsync();
                } while ((registerDialog.IsUserExist || !registerDialog.IsPasswordVaild) && registerResult == ContentDialogResult.Primary);
            }

            catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
        }
    }
}
