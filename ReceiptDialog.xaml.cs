// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 9 AUG 2020
// PURPOSE: LOAD INFORMATION INTO THE RECEIPT DIALOG
// ===============================
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Final
{
    public sealed partial class ReceiptDialog : ContentDialog
    {
        public ReceiptDialog()
        {
            this.InitializeComponent();
        }

        public ReceiptDialog(string receipt)
        {
            this.InitializeComponent();
            lblReceipt.Text = receipt;
        }

        private void ReceiptDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ReceiptDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
