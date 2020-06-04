using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Exceptions;

namespace TellDontAskKata.Entities
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

        public void Approve()
        {
            switch (Status)
            {
                case OrderStatus.Shipped:
                    throw new ShippedOrdersCannotBeChangedException();
                case OrderStatus.Rejected:
                    throw new RejectedOrderCannotBeApprovedException();
                case OrderStatus.Approved:
                case OrderStatus.Created:
                default:
                    Status = OrderStatus.Approved;
                    break;
            }
        }

        public void Reject()
        {
            switch (Status)
            {
                case OrderStatus.Shipped:
                    throw new ShippedOrdersCannotBeChangedException();
                case OrderStatus.Approved:
                    throw new ApprovedOrderCannotBeRejectedException();
                case OrderStatus.Rejected:
                case OrderStatus.Created:
                default:
                    Status = OrderStatus.Rejected;
                    break;
            }
        }

        public void Ship()
        {
            switch (Status)
            {
                case OrderStatus.Created:
                case OrderStatus.Rejected:
                    throw new OrderCannotBeShippedException();
                case OrderStatus.Shipped:
                    throw new OrderCannotBeShippedTwiceException();
                case OrderStatus.Approved:
                default:
                    Status = OrderStatus.Shipped;
                    break;
            }
        }
    }
}