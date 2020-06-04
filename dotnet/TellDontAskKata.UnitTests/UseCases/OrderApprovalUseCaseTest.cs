using NUnit.Framework;
using TellDontAskKata.Domain;
using TellDontAskKata.Tests.Doubles;
using TellDontAskKata.UseCase;

namespace TellDontAskKata.UnitTests.UseCases
{
    public class OrderApprovalUseCaseTest
    {
        private ApproveOrderWorkflow approveOrderWorkflow;
        private TestOrderRepository orderRepository;
        private Order initialOrder;

        [SetUp]
        public void SetUp()
        {
            orderRepository = new TestOrderRepository();
            approveOrderWorkflow = new ApproveOrderWorkflow(orderRepository);

            initialOrder = new Order(1);
        }

        [Test]
        public void ApprovedExistingOrder()
        {
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1};

            approveOrderWorkflow.Approve(request);

            Assert.AreEqual(orderRepository.SavedOrder.Status, OrderStatus.Approved);
        }

        [Test]
        public void CannotApproveRejectedOrderBy()
        {
            initialOrder.Reject();
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1};

            Assert.Throws<RejectedOrderCannotBeApprovedException>(() => approveOrderWorkflow.Approve(request));
        }

        [Test]
        public void CannotRejectApprovedOrder()
        {
            initialOrder.Approve();
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1};

            Assert.Throws<ApprovedOrderCannotBeRejectedException>(() => approveOrderWorkflow.Reject(request));
        }

        [Test]
        public void RejectExistingOrder()
        {
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1};

            approveOrderWorkflow.Reject(request);

            Assert.AreEqual(orderRepository.SavedOrder.Status, OrderStatus.Rejected);
        }

        [Test]
        public void ShippedOrdersCannotBeApproved()
        {
            initialOrder.Ship();
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1};

            Assert.Throws<ShippedOrdersCannotBeChangedException>(() => approveOrderWorkflow.Approve(request));
        }

        [Test]
        public void ShippedOrdersCannotBeRejected()
        {
            initialOrder.Ship();
            orderRepository.AddOrder(initialOrder);
            var request = new OrderApprovalRequest {OrderId = 1};

            Assert.Throws<ShippedOrdersCannotBeChangedException>(() => approveOrderWorkflow.Reject(request));
        }
    }
}