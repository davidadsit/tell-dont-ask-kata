using TellDontAskKata.Entities;
using TellDontAskKata.Service;

namespace TellDontAskKata.UnitTests.Doubles
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