// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: ACT AS A CENTRAL DATA REPO WHERE DATA CAN BE UNIFIED AND CONSTANT
//          AS WELL AS THE CONNECTOR BETWEEN THE DB AND THE APP
// ===============================
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FoodOrderingClient.models
{
    class GlobalPropertiesAndDatabaseConnector : IDBFunctions
    {
        private static readonly GlobalPropertiesAndDatabaseConnector instance = new GlobalPropertiesAndDatabaseConnector();
        private string connectionString = "Server = Kendrick-T440s; Database = PapaDariosPizza; User ID = PapaDario; PASSWORD = 12345";
        public static GlobalPropertiesAndDatabaseConnector Instance { get { return instance; } }
        public List<PizzaType> PizzaTypes { get; set; } = new List<PizzaType>();
        public string SelectedPizzaName { get; set; }
        public string LoggedInUser { get; set; } = "guest";
        public bool IsLoggedIn { get; set; }
        public ShoppingCart FoodOrder { get; set; }
        static GlobalPropertiesAndDatabaseConnector() { }
        private GlobalPropertiesAndDatabaseConnector() { }

        // Fetch the available types of Pizza
        public void FetchPizzaTypes()
        {
            string query = "SELECT Type, Tag, BasePrice FROM PizzaType;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PizzaTypes.Add(new PizzaType(reader.GetString(0), reader.GetString(1), (float)reader.GetDecimal(2)));
                }
            }
        }

        // Check if the input password matches the password associate to the username
        public bool IsCredentialValid(string username, string passwordHash)
        {
            string query = "SELECT UserName FROM Member WHERE UPPER(UserName) = UPPER(@Username) AND PasswordHash = @PasswordHash";
            bool isFound = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("Username", username);
                cmd.Parameters.AddWithValue("PasswordHash", passwordHash);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    isFound = true;
            }

            return isFound;
        }

        // Check if the provided input matches the email's format
        public bool IsEmailValid(string email)
        {
            char[] emailArr = email.ToCharArray();

            Stack<char> stack = new Stack<char>();

            int? firstLocation = null;
            int? lastLocation = null;

            stack.Push('.');
            stack.Push('@');

            for (int i = 0; i < emailArr.Length && stack.Count() != 0; i++)
            {
                if (emailArr[i] == stack.Peek())
                {
                    if (firstLocation == null)
                        firstLocation = i;
                    if (firstLocation == i || firstLocation != i - 1)
                    {
                        stack.Pop();
                        if (firstLocation != i)
                            lastLocation = i;
                    }
                }
            }

            if (lastLocation != null && lastLocation == emailArr.Length - 1)
                return false;
            else
                return stack.Count() == 0;
        }

        // Check if the username has been registered or not
        public bool IsUserExist(string username)
        {
            string query = "SELECT UserName FROM Member WHERE UPPER(UserName) = UPPER(@Username)";
            bool isFound = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("Username", username);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    isFound = true;
            }

            return isFound;
        }

        // Insert new user's information into database
        public int RegisterNewUser(string username, string passwordHash)
        {
            string query = "INSERT INTO Member (UserName, PasswordHash) VALUES (@UserName, @PasswordHash);";
            int result;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("UserName", username);
                cmd.Parameters.AddWithValue("PasswordHash", passwordHash);
                conn.Open();

                result = cmd.ExecuteNonQuery();
            }

            return result;
        }

        // Insert the order into the database
        public int PlaceOrder(string summary, double SubTotal, double Tax, double Total)
        {
            string query = $"INSERT INTO FoodOrder (Orderer, Summary, SubTotal, Tax, Total) VALUES ('{LoggedInUser}', @Summary, {SubTotal}, {Tax}, {Total});" +
                $"SELECT SCOPE_IDENTITY();";
            int primaryKey;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("Summary", summary);
                conn.Open();

                primaryKey = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return primaryKey;
        }

        // Insert each item or an order into the database
        public void PlaceOrderedItem(int orderId, string summary)
        {
            string query = $"INSERT INTO OrderedItem (Order_ID, Summary) VALUES ({orderId}, '{summary}');";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // Insert user feedback into the database
        public int SubmitFeedBack(string name, string user, string content)
        {
            string query = $"INSERT INTO FeedBack (CusName, CusEmail, FeedBack_Content) VALUES (@Name, @User, @Content);";
            int result;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("Name", name);
                cmd.Parameters.AddWithValue("User", user);
                cmd.Parameters.AddWithValue("Content", content);
                conn.Open();

                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        // Comes from MS documentation
        // ref https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=netcore-3.1
        public string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

    }

}
