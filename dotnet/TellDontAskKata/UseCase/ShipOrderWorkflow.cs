using TellDontAskKata.Entities;
using TellDontAskKata.Exceptions;
using TellDontAskKata.Repository;
using TellDontAskKata.Service;

namespace TellDontAskKata.UseCase
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