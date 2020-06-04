using System;

namespace TellDontAskKata.Entities
{
    public class Product
    {
        private readonly Category category;

        public Product(string name, decimal price, Category category)
        {
            Name = name;
            Price = price;
            this.category = category;
        }

        public string Name { get; }
        public decimal Price { get; }
        public decimal UnitTax => Math.Round(Price * category.TaxRate, 2, MidpointRounding.AwayFromZero);
        public decimal UnitTotal => Math.Round(Price + UnitTax, 2, MidpointRounding.AwayFromZero);
    }
}