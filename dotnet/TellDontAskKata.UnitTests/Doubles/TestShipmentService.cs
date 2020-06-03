using TellDontAskKata.Domain;
using TellDontAskKata.Service;

namespace TellDontAskKata.Tests.Doubles
{
    public class TestShipmentService : IShipmentService
    {
        public Order ShippedOrder { get; set; }

        public void Ship(Order order)
        {
            ShippedOrder = order;
        }
    }
}