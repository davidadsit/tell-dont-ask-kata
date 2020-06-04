using System;
using System.Collections.Generic;
using TellDontAskKata.Entities;
using TellDontAskKata.Repository;

namespace TellDontAskKata.UseCase
{
    public class OrderCreationUseCase
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductCatalog productCatalog;

        public OrderCreationUseCase(IOrderRepository orderRepository,
            IProductCatalog productCatalog)
        {
            this.orderRepository = orderRepository;
            this.productCatalog = productCatalog;
        }

        public void Run(SellItemsRequest request)
        {
            var order = new Order();

            foreach (var itemRequest in request.Requests)
            {
                var product = productCatalog.GetByName(itemRequest.ProductName);
                order.AddLineItem(product, itemRequest.Quantity);
            }
            orderRepository.Save(order);
        }
    }
}