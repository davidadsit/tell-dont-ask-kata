using System;

namespace TellDontAskKata.Domain
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
            return new LineItem(new Product {Category = new Category {TaxPercentage = tax}, Price = price}, quantity);
        }
    }
}