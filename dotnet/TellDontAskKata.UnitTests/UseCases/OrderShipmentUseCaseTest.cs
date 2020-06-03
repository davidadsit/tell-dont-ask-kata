using NUnit.Framework;
using TellDontAskKata.Domain;
using TellDontAskKata.Tests.Doubles;
using TellDontAskKata.UseCase;

namespace TellDontAskKata.UnitTests.UseCases
{
    public class OrderShipmentUseCaseTest
    {
        private TestOrderRepository orderRepository;

        private TestShipmentService shipmentService;

        private OrderShipmentUseCase useCase;

        [SetUp]
        public void SetUp()
        {
            orderRepository = new TestOrderRepository();
            shipmentService = new TestShipmentService();
            useCase = new OrderShipmentUseCase(orderRepository, shipmentService);
        }

        [Test]
        public void CreatedOrdersCannotBeShipped()
        {
            var initialOrder = new Order {Id = 1, Status = OrderStatus.Created};
            orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest {OrderId = 1};

            Assert.Throws<OrderCannotBeShippedException>(() => useCase.Run(request));
            Assert.Null(orderRepository.SavedOrder);
            Assert.Null(shipmentService.ShippedOrder);
        }

        [Test]
        public void RejectedOrdersCannotBeShipped()
        {
            var initialOrder = new Order {Id = 1, Status = OrderStatus.Rejected};
            orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest {OrderId = 1};

            Assert.Throws<OrderCannotBeShippedException>(() => useCase.Run(request));
            Assert.Null(orderRepository.SavedOrder);
            Assert.Null(shipmentService.ShippedOrder);
        }

        [Test]
        public void ShipApprovedOrder()
        {
            var initialOrder = new Order {Id = 1, Status = OrderStatus.Approved};
            orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest {OrderId = 1};

            useCase.Run(request);

            Assert.AreEqual(OrderStatus.Shipped, orderRepository.SavedOrder.Status);
            Assert.AreEqual(shipmentService.ShippedOrder, initialOrder);
        }

        [Test]
        public void ShippedOrdersCannotBeShippedAgain()
        {
            var initialOrder = new Order {Id = 1, Status = OrderStatus.Shipped};
            orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest {OrderId = 1};

            Assert.Throws<OrderCannotBeShippedTwiceException>(() => useCase.Run(request));
            Assert.Null(orderRepository.SavedOrder);
            Assert.Null(shipmentService.ShippedOrder);
        }
    }
}