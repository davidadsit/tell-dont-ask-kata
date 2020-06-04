using NUnit.Framework;
using TellDontAskKata.Entities;
using TellDontAskKata.Exceptions;
using TellDontAskKata.UnitTests.Doubles;
using TellDontAskKata.UseCase;

namespace TellDontAskKata.UnitTests.UseCases
{
    public class OrderApprovalUseCaseTest
    {
        private Order initialOrder;
        private OrderApprovalWorkflow orderApprovalWorkflow;
        private TestOrderRepository orderRepository;

        [SetUp]
        public void SetUp()
        {
            orderRepository = new TestOrderRepository();
            orderApprovalWorkflow = new OrderApprovalWorkflow(orderRepository);

            initialOrder = new Order(1);
        }

        [Test]
        public void ApprovedExistingOrder()
        {
            orderRepository.AddOrder(initialOrder);

            orderApprovalWorkflow.Approve(initialOrder.Id);

            Assert.AreEqual(orderRepository.SavedOrder.Status, OrderStatus.Approved);
        }

        [Test]
        public void CannotApproveRejectedOrderBy()
        {
            initialOrder.Reject();
            orderRepository.AddOrder(initialOrder);

            Assert.Throws<RejectedOrderCannotBeApprovedException>(() => orderApprovalWorkflow.Approve(initialOrder.Id));
        }

        [Test]
        public void CannotRejectApprovedOrder()
        {
            initialOrder.Approve();
            orderRepository.AddOrder(initialOrder);

            Assert.Throws<ApprovedOrderCannotBeRejectedException>(() => orderApprovalWorkflow.Reject(initialOrder.Id));
        }

        [Test]
        public void RejectExistingOrder()
        {
            orderRepository.AddOrder(initialOrder);

            orderApprovalWorkflow.Reject(initialOrder.Id);

            Assert.AreEqual(orderRepository.SavedOrder.Status, OrderStatus.Rejected);
        }

    }
}