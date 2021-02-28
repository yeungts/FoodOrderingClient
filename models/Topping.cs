// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: MODEL FOR TOPPING
// ===============================
namespace FoodOrderingClient.models
{
    class Topping
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public decimal Quantity { get; set; }

        public Topping(string Name, float Price, decimal Quantity)
        {
            this.Name = Name;
            this.Price = Price;
            this.Quantity = Quantity;
        }

        public float GetPrice()
        {
            return Price * (float)Quantity;
        }
    }
}
