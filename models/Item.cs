// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: ABSTRACT PARENT MODEL FOR ALL TYPES OF ITEMS
// ===============================
using System;

namespace Final.models
{
    abstract class Item
    {
        protected GlobalPropertiesAndDatabaseConnector globalProperties = GlobalPropertiesAndDatabaseConnector.Instance;
        public string Summary { get; set; }
        public string strSubTotal { get { return "$" + SubTotal; } }
        public float SubTotal { get; set; }
        public string CartPageSubTotal {
            get
            {
                if (globalProperties.IsLoggedIn)
                    return "$" + Math.Round(SubTotal * 0.9, 2);
                else
                    return "$" + SubTotal;
            }
        }
        public Object Instance { get; set; }

        public abstract string PricedString();
    }
}
