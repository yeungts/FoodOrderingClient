// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: MODEL FOR CHICKEN BITES
// ===============================
using System.Collections.Generic;

namespace Final.models
{
    class ShoppingCart
    {
        private static readonly ShoppingCart instance = new ShoppingCart();
        public static ShoppingCart Instance { get { return instance; } }
        public List<Item> Items { get; set; }

        static ShoppingCart() { }

        private ShoppingCart() 
        {
            Items = new List<Item>();
        }
    }
}
