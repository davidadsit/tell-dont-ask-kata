using NUnit.Framework;
using TellDontAskKata.Domain;

namespace TellDontAskKata.UnitTests.DomainTests
{
    public class LineItemTests
    {
        [TestCase(1, 1, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(5, 8, 40)]
        public void SubTotal_equals_the_quantity_times_the_cost(int quantity, decimal price, decimal expectedSubTotal)
        {
            var lineItem = LineItem.CreateWithProduct(price, 0, quantity);
            Assert.That(lineItem.SubTotal, Is.EqualTo(expectedSubTotal));
        }

        [TestCase(10, 1, 10, 1)]
        [TestCase(10, 2, 10, 2)]
        [TestCase(10, 1, 20, 2)]
        [TestCase(10, 5, 8, 4)]
        public void Tax_equals_the_quantity_times_the_cost_times_the_tax_percentage(int quantity, decimal price, int tax, decimal expectedTax)
        {
            var lineItem = LineItem.CreateWithProduct(price, tax, quantity);
            Assert.That(lineItem.Tax, Is.EqualTo(expectedTax));
        }
    }
}