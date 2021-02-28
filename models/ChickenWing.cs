// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: MODEL FOR CHICKEN WING
// ===============================
using System;
using System.Collections.Generic;

namespace Final.models
{
    class ChickenWing : Item
    {
        public int Pieces { get; set; }
        public string Type { get; set; }
        public List<string> Sauces { get; set; }
        private string SauceString, memberDiscount;

        public ChickenWing(int Pieces, string Type, List<string> Sauces)
        {
            this.Pieces = Pieces;
            this.Type = Type;
            this.Sauces = Sauces;
            base.SubTotal = (float)6.29;
            base.Instance = this;
            SauceString = "";

            switch (Pieces)
            {
                case 10:
                    base.SubTotal += (float)3.7;
                    break;
                case 20:
                    base.SubTotal += (float)13.2;
                    break;
            }

            int count = 1;

            foreach (string sauce in Sauces)
            {
                SauceString += "\n\t  " + sauce;
                if (count > 1)
                    base.SubTotal += (float)0.99;
                count++;
            }

            base.Summary = String.Format("{0, -40}\n  Sauces:{1}", (Pieces + " " + Type + " Wings"), SauceString);
        }

        public override string PricedString() 
        {
            if (base.globalProperties.IsLoggedIn)
                memberDiscount = String.Format("\n{0, 40} {1, 5}", "Member 10% off: ", "$" + Math.Round(base.SubTotal * 0.1, 2));
            else
                memberDiscount = "";
            return String.Format("{0, -38} {1, 7}\n  Sauces:{2}{3}", (Pieces + " " + Type + " Wings"), base.strSubTotal, SauceString, memberDiscount); 
        }
    }
}
