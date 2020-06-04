using System.Collections.Generic;
using TellDontAskKata.Entities;
using TellDontAskKata.Repository;

namespace TellDontAskKata.UseCase
{
    public class CreateOrderWorkflow
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductCatalog productCatalog;

        public CreateOrderWorkflow(IOrderRepository orderRepository, IProductCatalog productCatalog)
        {
            this.orderRepository = orderRepository;
            this.productCatalog = productCatalog;
        }

        public void FromItems(IEnumerable<RequestedItem> items)
        {
            var order = new Order();

            foreach (var item in items)
            {
                var product = productCatalog.GetByName(item.ProductName);
                order.AddLineItem(product, item.Quantity);
            }

            orderRepository.Save(order);
        }
    }
}