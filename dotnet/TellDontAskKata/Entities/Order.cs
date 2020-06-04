using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Exceptions;

namespace TellDontAskKata.Entities
{
    public class Order
    {
        private readonly List<LineItem> lineItems;

        public Order(int id = -1)
        {
            Id = id;
            lineItems = new List<LineItem>();
        }

        public decimal SubTotal => Items.Sum(i => i.SubTotal);
        public decimal Tax => Items.Sum(i => i.Tax);
        public decimal Total => SubTotal + Tax;
        public string Currency { get; } = "EUR";
        public IReadOnlyList<LineItem> Items => lineItems.AsReadOnly();
        public OrderStatus Status { get; private set; } = OrderStatus.Created;
        public int Id { get; }

        public void AddLineItem(Product product, int quantity)
        {
            if (product == null) throw new UnknownProductException();

            var lineItem = lineItems.FirstOrDefault(i=>i.ProductName == product.Name);
            if (lineItem == null)
            {
                lineItems.Add(new LineItem(product, quantity));
            }
            else
            {
                lineItem.IncreaseQuantityBy(quantity);
            }
        }

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