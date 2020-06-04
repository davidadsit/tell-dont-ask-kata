using System;

namespace TellDontAskKata.Domain
{
    public class Product
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public Category Category { get; set; }
        public decimal UnitTax =>  Math.Round(Price * Category.TaxPercentage / 100, 2, MidpointRounding.AwayFromZero);
        public decimal UnitCost => Math.Round(Price + UnitTax, 2, MidpointRounding.AwayFromZero);
    }
}
