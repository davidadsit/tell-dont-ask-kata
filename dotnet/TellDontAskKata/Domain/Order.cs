using System.Collections.Generic;
using System.Linq;

namespace TellDontAskKata.Domain
{
    public class Order
    {
        public Order(int id = -1)
        {
            Id = id;
        }

        public decimal Total => Items.Sum(i => i.SubTotal);
        public string Currency { get; } = "EUR";
        public List<LineItem> Items { get; } = new List<LineItem>();
        public decimal Tax => Items.Sum(i => i.Tax);
        public OrderStatus Status { get; private set; } = OrderStatus.Created;
        public int Id { get; }

        public void Ship()
        {
            Status = OrderStatus.Shipped;
        }

        public void Approve()
        {
            Status = OrderStatus.Approved;
        }

        public void Reject()
        {
            Status = OrderStatus.Rejected;
        }
    }
}