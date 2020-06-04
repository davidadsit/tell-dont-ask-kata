using System.Collections.Generic;
using NUnit.Framework;
using TellDontAskKata.Entities;
using TellDontAskKata.Repository;
using TellDontAskKata.UnitTests.Doubles;
using TellDontAskKata.UseCase;

namespace TellDontAskKata.UnitTests.UseCases
{
    public class OrderCreationUseCaseTest
    {
        private Category food;
        private TestOrderRepository orderRepository;
        private IProductCatalog productCatalogue;
        private OrderCreationUseCase useCase;

        [SetUp]
        public void SetUp()
        {
            orderRepository = new TestOrderRepository();
            food = new Category {Name = "Food", TaxPercentage = new decimal(10)};
            productCatalogue = new InMemoryProductCatalog(
                new List<Product>
                {
                    new Product("salad", 3.56m, food), 
                    new Product("tomato", 4.65m, food)
                });
            useCase = new OrderCreationUseCase(orderRepository, productCatalogue);
        }

        [Test]
        public void SellMultipleItems()
        {
            var saladRequest = new SellItemRequest {ProductName = "salad", Quantity = 2};
            var tomatoRequest = new SellItemRequest {ProductName = "tomato", Quantity = 3};

            var request = new SellItemsRequest {Requests = new List<SellItemRequest> {saladRequest, tomatoRequest}};

            useCase.Run(request);

            var insertedOrder = orderRepository.SavedOrder;
            Assert.That(insertedOrder.Status, Is.EqualTo(OrderStatus.Created));
            Assert.That(insertedOrder.Total, Is.EqualTo(23.20m));
            Assert.That(insertedOrder.Tax, Is.EqualTo(2.13m));
            Assert.That(insertedOrder.Currency, Is.EqualTo("EUR"));
            Assert.That(insertedOrder.Items.Count, Is.EqualTo(2));

            Assert.That(insertedOrder.Items[0].Product.Name, Is.EqualTo("salad"));
            Assert.That(insertedOrder.Items[0].Product.Price, Is.EqualTo(3.56m));
            Assert.That(insertedOrder.Items[0].Quantity, Is.EqualTo(2));
            Assert.That(insertedOrder.Items[0].SubTotal, Is.EqualTo(7.84m));
            Assert.That(insertedOrder.Items[0].Tax, Is.EqualTo(0.72m));

            Assert.That(insertedOrder.Items[1].Product.Name, Is.EqualTo("tomato"));
            Assert.That(insertedOrder.Items[1].Product.Price, Is.EqualTo(4.65m));
            Assert.That(insertedOrder.Items[1].Quantity, Is.EqualTo(3));
            Assert.That(insertedOrder.Items[1].SubTotal, Is.EqualTo(15.36m));
            Assert.That(insertedOrder.Items[1].Tax, Is.EqualTo(1.41m));
        }

        [Test]
        public void UnknownProduct()
        {
            var request = new SellItemsRequest {Requests = new List<SellItemRequest>()};
            var unknownProductRequest = new SellItemRequest {ProductName = "unknown product"};
            request.Requests.Add(unknownProductRequest);

            Assert.Throws<UnknownProductException>(() => useCase.Run(request));
        }
    }
}