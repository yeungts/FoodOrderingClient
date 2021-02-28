// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 9 AUG 2020
// PURPOSE: INDICATE HOW SHOULD THE APP INTERACT WITH THE DATABASE
// ===============================
namespace Final.models
{
    interface IDBFunctions
    {
        void FetchPizzaTypes();
        bool IsCredentialValid(string username, string passwordHash);
        bool IsEmailValid(string email);
        bool IsUserExist(string username);
        int RegisterNewUser(string username, string passwordHash);
        int PlaceOrder(string summary, double SubTotal, double Tax, double Total);
        void PlaceOrderedItem(int orderId, string summary);
        int SubmitFeedBack(string name, string user, string content);
    }
}
