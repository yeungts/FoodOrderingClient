// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 7 AUG 2020
// PURPOSE: PROCESS INPUT FOR FEEDBACK PAGE
// ===============================
using Final.models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Final
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedBackPage : Page
    {
        private GlobalPropertiesAndDatabaseConnector globalProperties = GlobalPropertiesAndDatabaseConnector.Instance;
        public FeedBackPage()
        {
            this.InitializeComponent();
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            errorMsg.Text = "";

            string name = txtName.Text;
            string email = txtEmail.Text;
            string content = txtFeedback.Text;

            if (name.Length == 0) 
            {
                errorMsg.Text = "Please enter your name.";
                return;
            }

            if (email.Length == 0)
            {
                errorMsg.Text = "Please enter your email address.";
                return;
            }

            if (!globalProperties.IsEmailValid(email))
            {
                errorMsg.Text = "Please enter a valid email address.";
                return;
            }

            if (content.Length == 0)
            {
                errorMsg.Text = "Please fill in your feedback.";
                return;
            }

            int result = globalProperties.SubmitFeedBack(name, email, content);

            ContentDialog dlg;
            if (result == 1)
                dlg = new ContentDialog {

                    Title = "Feedback sent!",
                    Content = "Your opinion is very important to us. We appreciate your feedback and will use it to evaluate changes and make improvements in our app.",
                    PrimaryButtonText = "OK"
                };
            else
                dlg = new ContentDialog { 
                    Title = "Feedback Failed to Send",
                    Content = "Encountered technical difficulities, please try again later.",
                    PrimaryButtonText = "OK"
                };

            MainPage.ReplaceDialog(dlg, sender);

            try
            {
                await dlg.ShowAsync();
            }
            catch (Exception) { /*The dialog didn't open, probably because another dialog is already open.*/ }
        }
    }
}
