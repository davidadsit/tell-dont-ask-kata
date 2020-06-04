using NUnit.Framework;
using TellDontAskKata.Entities;
using TellDontAskKata.Exceptions;
using TellDontAskKata.UnitTests.Doubles;
using TellDontAskKata.UseCase;

namespace TellDontAskKata.UnitTests.UseCases
{
    public class ShipOrderWorkflowTest
    {
        private TestOrderRepository orderRepository;
        private TestShipmentService shipmentService;
        private ShipOrderWorkflow shipOrderWorkflow;
        private Order initialOrder;

        [SetUp]
        public void SetUp()
        {
            orderRepository = new TestOrderRepository();
            shipmentService = new TestShipmentService();
            shipOrderWorkflow = new ShipOrderWorkflow(orderRepository, shipmentService);

            initialOrder = new Order (1);
        }

        [Test]
        public void CreatedOrdersCannotBeShipped()
        {
            orderRepository.AddOrder(initialOrder);

            Assert.Throws<OrderCannotBeShippedException>(() => shipOrderWorkflow.Ship(initialOrder.Id));
            Assert.Null(orderRepository.SavedOrder);
            Assert.Null(shipmentService.ShippedOrder);
        }

        [Test]
        public void RejectedOrdersCannotBeShipped()
        {
            initialOrder.Reject();
            orderRepository.AddOrder(initialOrder);

            Assert.Throws<OrderCannotBeShippedException>(() => shipOrderWorkflow.Ship(initialOrder.Id));
            Assert.Null(orderRepository.SavedOrder);
            Assert.Null(shipmentService.ShippedOrder);
        }

        [Test]
        public void ShipApprovedOrder()
        {
            initialOrder.Approve();
            orderRepository.AddOrder(initialOrder);

            shipOrderWorkflow.Ship(initialOrder.Id);

            Assert.AreEqual(OrderStatus.Shipped, orderRepository.SavedOrder.Status);
            Assert.AreEqual(shipmentService.ShippedOrder, initialOrder);
        }
    }
}