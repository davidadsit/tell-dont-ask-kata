using System.Collections.Generic;
using NUnit.Framework;
using TellDontAskKata.Domain;
using TellDontAskKata.Repository;
using TellDontAskKata.Tests.Doubles;
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
                new List<Product> {new Product {Category = food, Name = "salad", Price = new decimal(3.56)}, new Product {Category = food, Name = "tomato", Price = new decimal(4.65)}});
            useCase = new OrderCreationUseCase(orderRepository, productCatalogue);
        }

        [Test]
        public void SellMultipleItems()
        {
            var saladRequest = new SellItemRequest {ProductName = "salad", Quantity = 2};

            var tomatoRequest = new SellItemRequest {ProductName = "tomato", Quantity = 3};

            var request =
                new SellItemsRequest {Requests = new List<SellItemRequest> {saladRequest, tomatoRequest}};

            useCase.Run(request);

            var insertedOrder = orderRepository.SavedOrder;
            Assert.AreEqual(OrderStatus.Created, insertedOrder.Status);
            Assert.AreEqual(23.20m, insertedOrder.Total);
            Assert.AreEqual(2.13m, insertedOrder.Tax);
            Assert.AreEqual("EUR", insertedOrder.Currency);
            Assert.AreEqual(2, insertedOrder.Items.Count);
            Assert.AreEqual("salad", insertedOrder.Items[0].Product.Name);
            Assert.AreEqual(3.56m, insertedOrder.Items[0].Product.Price);
            Assert.AreEqual(2, insertedOrder.Items[0].Quantity);
            Assert.AreEqual(7.84m, insertedOrder.Items[0].TaxedAmount);
            Assert.AreEqual(0.72m, insertedOrder.Items[0].Tax);
            Assert.AreEqual("tomato", insertedOrder.Items[1].Product.Name);
            Assert.AreEqual(4.65m, insertedOrder.Items[1].Product.Price);
            Assert.AreEqual(3, insertedOrder.Items[1].Quantity);
            Assert.AreEqual(15.36m, insertedOrder.Items[1].TaxedAmount);
            Assert.AreEqual(1.41m, insertedOrder.Items[1].Tax);
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