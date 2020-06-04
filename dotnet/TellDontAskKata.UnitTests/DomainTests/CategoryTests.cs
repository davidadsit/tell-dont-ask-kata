using NUnit.Framework;
using TellDontAskKata.Entities;

namespace TellDontAskKata.UnitTests.DomainTests
{
    public class CategoryTests
    {
        [TestCase(10, .1)]
        [TestCase(8, .08)]
        public void TaxRate_is_TaxPercentage_divided_by_100(int percentage, decimal rate)
        {
            var category = new Category("", percentage);

            Assert.That(category.TaxRate, Is.EqualTo(rate));
        }
    }
}