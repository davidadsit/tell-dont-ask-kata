using NUnit.Framework;
using TellDontAskKata.Entities;
using TellDontAskKata.Exceptions;
using TellDontAskKata.UnitTests.Doubles;
using TellDontAskKata.UseCase;

namespace TellDontAskKata.UnitTests.UseCases
{
    public class OrderShipmentUseCaseTest
    {
        private TestOrderRepository orderRepository;

        private TestShipmentService shipmentService;

        private OrderShipmentUseCase useCase;
        private Order initialOrder;

        [SetUp]
        public void SetUp()
        {
            orderRepository = new TestOrderRepository();
            shipmentService = new TestShipmentService();
            useCase = new OrderShipmentUseCase(orderRepository, shipmentService);

            initialOrder = new Order (1);
        }

        [Test]
        public void CreatedOrdersCannotBeShipped()
        {
            orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest {OrderId = 1};

            Assert.Throws<OrderCannotBeShippedException>(() => useCase.Run(request));
            Assert.Null(orderRepository.SavedOrder);
            Assert.Null(shipmentService.ShippedOrder);
        }

        [Test]
        public void RejectedOrdersCannotBeShipped()
        {
            initialOrder.Reject();
            orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest {OrderId = 1};

            Assert.Throws<OrderCannotBeShippedException>(() => useCase.Run(request));
            Assert.Null(orderRepository.SavedOrder);
            Assert.Null(shipmentService.ShippedOrder);
        }

        [Test]
        public void ShipApprovedOrder()
        {
            initialOrder.Approve();
            orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest {OrderId = 1};

            useCase.Run(request);

            Assert.AreEqual(OrderStatus.Shipped, orderRepository.SavedOrder.Status);
            Assert.AreEqual(shipmentService.ShippedOrder, initialOrder);
        }

        [Test]
        public void ShippedOrdersCannotBeShippedAgain()
        {
            initialOrder.Ship();
            orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest {OrderId = 1};

            Assert.Throws<OrderCannotBeShippedTwiceException>(() => useCase.Run(request));
            Assert.Null(orderRepository.SavedOrder);
            Assert.Null(shipmentService.ShippedOrder);
        }
    }
}