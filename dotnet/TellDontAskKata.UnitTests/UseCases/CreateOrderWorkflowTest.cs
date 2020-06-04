using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TellDontAskKata.Entities;
using TellDontAskKata.Repository;
using TellDontAskKata.UnitTests.Doubles;
using TellDontAskKata.UseCase;

namespace TellDontAskKata.UnitTests.UseCases
{
    public class CreateOrderWorkflowTest
    {
        private CreateOrderWorkflow createOrderWorkflow;
        private Category food;
        private TestOrderRepository orderRepository;
        private IProductCatalog productCatalogue;

        [SetUp]
        public void SetUp()
        {
            orderRepository = new TestOrderRepository();
            food = new Category("Food", 10m);
            var products = new List<Product> {new Product("salad", 3.56m, food), new Product("tomato", 4.65m, food)};
            productCatalogue = new InMemoryProductCatalog(products);
            createOrderWorkflow = new CreateOrderWorkflow(orderRepository, productCatalogue);
        }

        [Test]
        public void An_order_can_be_created_from_items()
        {
            var salad = new RequestedItem {ProductName = "salad", Quantity = 2};
            var tomato = new RequestedItem {ProductName = "tomato", Quantity = 3};

            createOrderWorkflow.FromItems(new[] {salad, tomato});

            var savedOrder = orderRepository.SavedOrder;
            Assert.That(savedOrder.Status, Is.EqualTo(OrderStatus.Created));
            Assert.That(savedOrder.Items.Count, Is.EqualTo(2));
            Assert.That(savedOrder.Items.Any(i=>i.ProductName == salad.ProductName && i.Quantity == salad.Quantity));
            Assert.That(savedOrder.Items.Any(i=>i.ProductName == tomato.ProductName && i.Quantity == tomato.Quantity));
        }

        [Test]
        public void UnknownProduct()
        {
            Assert.Throws<UnknownProductException>(() => createOrderWorkflow.FromItems(new[] {new RequestedItem {ProductName = "unknown product"}}));
        }
    }
}