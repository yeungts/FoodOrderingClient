// ===============================
// AUTHOR: TSZ KIN YEUNG
// CREATE DATE: 5 AUG 2020
// PURPOSE: MODEL FOR DEFINING A TYPES OF PIZZA
// ===============================
namespace FoodOrderingClient.models
{
    class PizzaType
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public float Price { get; set; }

        public PizzaType(string Name, string Tag, float Price)
        {
            this.Name = Name;
            this.Tag = Tag;
            this.Price = Price;
        }
    }
}
