using TellDontAskKata.Repository;

namespace TellDontAskKata.Workflows
{
    public class OrderApprovalWorkflow
    {
        private readonly IOrderRepository orderRepository;

        public OrderApprovalWorkflow(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public void Approve(int orderId)
        {
            var order = orderRepository.GetById(orderId);
            order.Approve();
            orderRepository.Save(order);
        }

        public void Reject(int orderId)
        {
            var order = orderRepository.GetById(orderId);
            order.Reject();
            orderRepository.Save(order);
        }
    }
}