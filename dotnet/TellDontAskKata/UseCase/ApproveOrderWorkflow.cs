using TellDontAskKata.Domain;
using TellDontAskKata.Exceptions;
using TellDontAskKata.Repository;

namespace TellDontAskKata.UseCase
{
    public class ApproveOrderWorkflow
    {
        private readonly IOrderRepository orderRepository;

        public ApproveOrderWorkflow(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public void Approve(OrderApprovalRequest request)
        {
            var order = orderRepository.GetById(request.OrderId);

            if (order.Status == OrderStatus.Shipped)
            {
                throw new ShippedOrdersCannotBeChangedException();
            }

            if (order.Status == OrderStatus.Rejected)
            {
                throw new RejectedOrderCannotBeApprovedException();
            }

            order.Approve();

            orderRepository.Save(order);
        }

        public void Reject(OrderApprovalRequest request)
        {
            var order = orderRepository.GetById(request.OrderId);

            if (order.Status == OrderStatus.Shipped)
            {
                throw new ShippedOrdersCannotBeChangedException();
            }

            if (order.Status == OrderStatus.Approved)
            {
                throw new ApprovedOrderCannotBeRejectedException();
            }

            order.Reject();

            orderRepository.Save(order);
        }
    }
}