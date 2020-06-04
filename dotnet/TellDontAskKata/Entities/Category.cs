namespace TellDontAskKata.Entities
{
    public class Category
    {
        public Category(string name, decimal taxPercentage)
        {
            Name = name;
            TaxRate = taxPercentage / 100;
        }

        public string Name { get; }
        public decimal TaxRate { get; }
    }
}
