using System;

namespace TellDontAskKata.Entities
{
    public class Product
    {
        public Product(string name, decimal price, Category category)
        {
            Name = name;
            Price = price;
            Category = category;
        }

        public string Name { get; }
        public decimal Price { get; }
        public Category Category { get; }
        public decimal UnitTax => Math.Round(Price * Category.TaxPercentage / 100, 2, MidpointRounding.AwayFromZero);
        public decimal UnitCost => Math.Round(Price + UnitTax, 2, MidpointRounding.AwayFromZero);
    }
}