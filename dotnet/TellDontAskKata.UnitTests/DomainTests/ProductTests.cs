using NUnit.Framework;
using TellDontAskKata.Domain;

namespace TellDontAskKata.UnitTests.DomainTests
{
    public class ProductTests
    {
        [TestCase(100, 5, 5)]
        [TestCase(101, 5, 5.05)]
        [TestCase(1001, 5.4, 54.05)]
        [TestCase(1001, 5.5, 55.06)]
        public void UnitTax_is_price_times_tax_rate(decimal price, decimal taxPercentage, decimal expectedUnitTax)
        {
            var product = new Product("", price, new Category {TaxPercentage = taxPercentage});

            Assert.That(product.UnitTax, Is.EqualTo(expectedUnitTax));
        }
    }
}