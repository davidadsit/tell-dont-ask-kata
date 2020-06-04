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

        private Product Product { get; }
        public string ProductName => Product.Name;
        public int Quantity { get; }
        public decimal SubTotal => Math.Round(Product.Price * Quantity, 2, MidpointRounding.AwayFromZero);
        public decimal Tax => Math.Round(Product.UnitTax * Quantity, 2, MidpointRounding.AwayFromZero);
        public decimal Total => SubTotal + Tax;
    
    }
}