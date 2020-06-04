using NUnit.Framework;
using TellDontAskKata.Entities;

namespace TellDontAskKata.UnitTests.DomainTests
{
    public class LineItemTests
    {
        private static LineItem CreateWithProduct(decimal price, int tax, int quantity)
        {
            return new LineItem(new Product("", price, new Category("", tax)), quantity);
        }

        [TestCase(1, 1, 0, 1)]
        [TestCase(2, 1, 0, 2)]
        [TestCase(1, 2, 0, 2)]
        [TestCase(5, 8, 0, 40)]
        public void SubTotal_equals_the_quantity_times_the_cost(int quantity, decimal price, int tax, decimal expectedSubTotal)
        {
            var lineItem = CreateWithProduct(price, tax, quantity);
            Assert.That(lineItem.SubTotal, Is.EqualTo(expectedSubTotal));
        }

        [TestCase(1, 2, 3)]
        [TestCase(5, 8, 13)]
        [TestCase(8, -5, 3)]
        public void Increase_quantity_updates_the_quantity(int initialQuantity, int increase, int expected)
        {
            var lineItem = CreateWithProduct(10, 0, initialQuantity);
            lineItem.IncreaseQuantityBy(increase);

            Assert.That(lineItem.Quantity, Is.EqualTo(expected));
        }
        
        [TestCase(5, -8, 0)]
        public void Quantity_cannot_be_less_than_zero(int initialQuantity, int increase, int expected)
        {
            var lineItem = CreateWithProduct(10, 0, initialQuantity);
            lineItem.IncreaseQuantityBy(increase);

            Assert.That(lineItem.Quantity, Is.EqualTo(expected));
        }

        [TestCase(10, 1, 10, 1)]
        [TestCase(10, 2, 10, 2)]
        [TestCase(10, 1, 20, 2)]
        [TestCase(10, 5, 8, 4)]
        public void Tax_equals_the_quantity_times_the_cost_times_the_tax_percentage(int quantity, decimal price, int tax, decimal expectedTax)
        {
            var lineItem = CreateWithProduct(price, tax, quantity);
            Assert.That(lineItem.Tax, Is.EqualTo(expectedTax));
        }

        [TestCase(10, 1, 10)]
        [TestCase(10, 2, 10)]
        [TestCase(10, 1, 20)]
        [TestCase(10, 5, 8)]
        public void Total_equals_the_subtotal_plus_tax(int quantity, decimal price, int tax)
        {
            var lineItem = CreateWithProduct(price, tax, quantity);
            Assert.That(lineItem.Total, Is.EqualTo(lineItem.SubTotal + lineItem.Tax));
        }
    }
}