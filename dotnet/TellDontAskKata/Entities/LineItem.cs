using System;

namespace TellDontAskKata.Entities
{
    public class LineItem
    {
        public LineItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; }
        public int Quantity { get; }
        public decimal SubTotal => Math.Round(Product.UnitCost * Quantity, 2, MidpointRounding.AwayFromZero);
        public decimal Tax => Math.Round(Product.UnitTax * Quantity, 2, MidpointRounding.AwayFromZero);

        public static LineItem CreateWithProduct(decimal price, int tax, int quantity)
        {
            return new LineItem(new Product("", price, new Category("", tax)), quantity);
        }
    }
}