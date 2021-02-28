// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 6 AUG 2020
// PURPOSE: PROCESS THE INPUT AND SELECTED OPTION FOR REGISTRATION DIALOG
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
    public sealed partial class RegisterDialog : ContentDialog
    {
        private GlobalPropertiesAndDatabaseConnector globalProperties = GlobalPropertiesAndDatabaseConnector.Instance;
        public bool IsPasswordVaild { get; private set; }
        public bool IsUserExist { get; private set; }

        public RegisterDialog()
        {
            this.InitializeComponent();
        }

        private async void RegisterDialog_RegisterClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string userId = userid.Text;
            string pwd = password.Password;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Verify if the input email is in valid formay
                if (globalProperties.IsEmailValid(userId))
                {
                    // Verify if the input password is at least 8 character long
                    if (pwd.Length >= 8)
                    {
                        string hash = globalProperties.GetHash(sha256Hash, pwd);

                        // Verify if both password matches
                        if (globalProperties.VerifyHash(sha256Hash, passwordCheck.Password, hash))
                        {
                            IsPasswordVaild = true;
                            IsUserExist = globalProperties.IsUserExist(userId);

                            // Register user if username does not exist
                            if (IsUserExist)
                            {
                                errorMsg.Text = "User already exists, try again.";
                                errorMsg.Visibility = Visibility.Visible;
                                return;
                            }
                            else
                            {
                                // register the user in db
                                int result = globalProperties.RegisterNewUser(userId, hash);

                                MessageDialog dlg;
                                if (result == 1)
                                    dlg = new MessageDialog($"Successfully registered! Welcome to Papa Dario's family! You can login as '{userId}' now!");
                                else 
                                    dlg = new MessageDialog("Encountered technical difficulities, please try again later.");

                                dlg.Commands.Add(new UICommand("Continue", null));
                                dlg.CancelCommandIndex = 0;
                                await dlg.ShowAsync();
                            }
                        }
                        else
                        {
                            errorMsg.Text = "Password doesn't match, try again.";
                            errorMsg.Visibility = Visibility.Visible;
                            IsPasswordVaild = false;
                            return;
                        }
                    }
                    else
                    {
                        errorMsg.Text = "Password must be at least 8-digits long, try again.";
                        errorMsg.Visibility = Visibility.Visible;
                        IsPasswordVaild = false;
                        return;
                    }
                }
                else
                {
                    errorMsg.Text = "E-mail address is not valid, try again.";
                    errorMsg.Visibility = Visibility.Visible;
                }
            }
        }

        private void RegisterDialog_CancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

    }
}
