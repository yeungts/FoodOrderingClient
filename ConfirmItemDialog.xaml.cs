// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 7 AUG 2020
// PURPOSE: THIS DIALOG WILL ASK IF USER CONFIRM ABOUT ADDING AN ITEM INTO THE CART
// ===============================
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Final
{
    public sealed partial class ConfirmItemDialog : ContentDialog
    {
        public ConfirmItemDialog()
        {
            this.InitializeComponent();
        }

        public ConfirmItemDialog(string Summary)
        {
            this.InitializeComponent();
            lblSummary.Text = Summary;
        }

        private void ConfirmItemDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ConfirmItemDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
