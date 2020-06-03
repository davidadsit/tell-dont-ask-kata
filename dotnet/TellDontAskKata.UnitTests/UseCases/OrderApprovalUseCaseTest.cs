using NUnit.Framework;
using TellDontAskKata.Domain;
using TellDontAskKata.Tests.Doubles;
using TellDontAskKata.UseCase;

namespace TellDontAskKata.UnitTests.UseCases
{
    public class OrderApprovalUseCaseTest
    {
        private TestOrderRepository orderRepository;

        private OrderApprovalUseCase useCase;

        [SetUp]
        public void SetUp()
        {
            orderRepository = new TestOrderRepository();
            useCase = new OrderApprovalUseCase(orderRepository);
        }

        [Test]
        public void ApprovedExistingOrder()
        {
            var initialOrder = new Order {Status = OrderStatus.Created, Id = 1};
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1, Approved = true};

            useCase.Run(request);

            Assert.AreEqual(orderRepository.SavedOrder.Status, OrderStatus.Approved);
        }

        [Test]
        public void CannotApproveRejectedOrderBy()
        {
            var initialOrder = new Order {Status = OrderStatus.Rejected, Id = 1};
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1, Approved = true};

            Assert.Throws<RejectedOrderCannotBeApprovedException>(() => useCase.Run(request));
        }

        [Test]
        public void CannotRejectApprovedOrder()
        {
            var initialOrder = new Order {Status = OrderStatus.Approved, Id = 1};
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1, Approved = false};

            Assert.Throws<ApprovedOrderCannotBeRejectedException>(() => useCase.Run(request));
        }

        [Test]
        public void RejectExistingOrder()
        {
            var initialOrder = new Order {Status = OrderStatus.Created, Id = 1};
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1, Approved = false};

            useCase.Run(request);

            Assert.AreEqual(orderRepository.SavedOrder.Status, OrderStatus.Rejected);
        }

        [Test]
        public void ShippedOrdersCannotBeApproved()
        {
            var initialOrder = new Order {Status = OrderStatus.Shipped, Id = 1};
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1, Approved = true};

            Assert.Throws<ShippedOrdersCannotBeChangedException>(() => useCase.Run(request));
        }

        [Test]
        public void ShippedOrdersCannotBeRejected()
        {
            var initialOrder = new Order {Status = OrderStatus.Shipped, Id = 1};
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1, Approved = false};

            Assert.Throws<ShippedOrdersCannotBeChangedException>(() => useCase.Run(request));
        }
    }
}