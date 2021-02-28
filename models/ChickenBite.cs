// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: MODEL FOR CHICKEN BITE
// ===============================
using System;
using System.Collections.Generic;

namespace FoodOrderingClient.models
{
    class ChickenBite : Item
    {
        public int Pieces { get; set; }
        public List<string> Sauces { get; set; }
        private string SauceString, memberDiscount;

        public ChickenBite(int Pieces, List<string> Sauces)
        {
            this.Pieces = Pieces;
            this.Sauces = Sauces;
            base.SubTotal = (float)9.99;
            base.Instance = this;
            SauceString = "";

            if (Pieces == 20)
                base.SubTotal += (float)9.5;

            int count = 1;

            foreach (string sauce in Sauces)
            {
                SauceString += "\n\t  " + sauce;
                if ((Pieces == 10 && count > 1) || (Pieces == 20 && count > 2))
                    base.SubTotal += (float)0.99;
                count++;
            }
            base.Summary = String.Format("{0, -40}\n  Sauces:{1}", (Pieces + " Chicken Bites"), SauceString);
        }

        public override string PricedString() 
        {
            if (base.globalProperties.IsLoggedIn)
                memberDiscount = String.Format("\n{0, 40} {1, 5}", "Member 10% off: ", "$" + Math.Round(base.SubTotal * 0.1, 2));
            else
                memberDiscount = "";
            return String.Format("{0, -38} {1, 7}\n  Sauces:{2}{3}", (Pieces + " Chicken Bites"), base.strSubTotal, SauceString, memberDiscount); 
        }
    }
}
