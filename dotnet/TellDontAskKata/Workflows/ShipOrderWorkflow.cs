using TellDontAskKata.Repository;
using TellDontAskKata.Service;

namespace TellDontAskKata.Workflows
{
    public class ShipOrderWorkflow
    {
        private readonly IOrderRepository orderRepository;
        private readonly IShipmentService shipmentService;

        public ShipOrderWorkflow(IOrderRepository orderRepository, IShipmentService shipmentService)
        {
            this.orderRepository = orderRepository;
            this.shipmentService = shipmentService;
        }

        public void Ship(int orderId)
        {
            var order = orderRepository.GetById(orderId);

            order.Ship();
            
            shipmentService.Ship(order);
            orderRepository.Save(order);
        }
    }
}