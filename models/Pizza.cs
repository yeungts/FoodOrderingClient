// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: MODEL FOR PIZZA
// ===============================
using System;
using System.Collections.Generic;

namespace Final.models
{
    class Pizza : Item
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string Dough { get; set; }
        public string BaseSauce { get; set; }
        public string BaseCheese { get; set; }
        public List<Topping> Toppings { get; set; }
        private string ToppingString,  memberDiscount;

        public Pizza(string Name, string Size, string Dough, string BaseSauce, string BaseCheese, List<Topping> Toppings)
        {
            this.Name = Name;
            this.Size = Size;
            this.Dough = Dough;
            this.BaseSauce = BaseSauce;
            this.BaseCheese = BaseCheese;
            this.Toppings = Toppings;
            base.Instance = this;

            foreach (PizzaType pizzaType in base.globalProperties.PizzaTypes)
            {
                if (pizzaType.Name == this.Name)
                {
                    base.SubTotal = pizzaType.Price;
                    break;
                }
            }

            switch (Size)
            {
                case "Medium":
                    base.SubTotal += 2;
                    break;
                case "Large":
                    base.SubTotal += 4;
                    break;
            }

            if (Toppings.Count != 0)
            {
                ToppingString = "\n  Toppings:";
                foreach (Topping topping in Toppings)
                {
                    ToppingString += "\n\t  " + topping.Name;
                    base.SubTotal += topping.Price;
                }
            }
            base.Summary = String.Format("{0} {1}:\n\t{2}\n\t{3}\n\t{4}{5}", Size, Name, Dough, BaseSauce, BaseCheese, ToppingString);
        }

        public override string PricedString() 
        {
            if (base.globalProperties.IsLoggedIn)
                memberDiscount = String.Format("\n{0, 40} {1, 5}", "Member 10% off: ", "$" + Math.Round(base.SubTotal * 0.1, 2));
            else
                memberDiscount = "";
            return String.Format("{0, -38} {1, 7}\n\t{2}\n\t{3}\n\t{4}{5}{6}",
            (Size + " " + Name), base.strSubTotal, Dough, BaseSauce, BaseCheese, ToppingString, memberDiscount); 
        }
    }
}
