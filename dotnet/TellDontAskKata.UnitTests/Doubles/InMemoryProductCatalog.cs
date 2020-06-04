﻿using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Entities;
using TellDontAskKata.Repository;

namespace TellDontAskKata.UnitTests.Doubles
{
    public class InMemoryProductCatalog : IProductCatalog
    {
        private readonly IEnumerable<Product> products;

        public InMemoryProductCatalog(IEnumerable<Product> products)
        {
            this.products = products;
        }

        public Product GetByName(string name)
        {
            return products.SingleOrDefault(item => item.Name.Equals(name));
        }
    }
}