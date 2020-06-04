using System.Linq;
using NUnit.Framework;
using TellDontAskKata.Entities;
using TellDontAskKata.Exceptions;

namespace TellDontAskKata.UnitTests.DomainTests
{
    public class OrderTests
    {
        private Order order;
        private Product product1, product2;

        [SetUp]
        public void Setup()
        {
            order = new Order();
            product1 = new Product("product1", 11m, new Category("category-1", 10m));
            product2 = new Product("product2", 6m, new Category("category-2", 20m));
        }

        [Test]
        public void AddLineItem_adds_the_item()
        {
            order.AddLineItem(product1, 4);

            Assert.That(order.Items.Any(i => i.ProductName == product1.Name && i.Quantity == 4));
        }

        [Test]
        public void An_empty_order_has_a_tax_of_0()
        {
            Assert.That(order.Tax, Is.EqualTo(0m));
        }

        [Test]
        public void An_empty_order_has_a_total_of_0()
        {
            Assert.That(order.SubTotal, Is.EqualTo(0m));
        }

        [Test]
        public void An_order_with_an_item_has_a_tax_equal_to_the_item_tax()
        {
            order.AddLineItem(product1, 1);

            Assert.That(order.Tax, Is.EqualTo(1.1m));
        }

        [Test]
        public void An_order_with_an_item_has_a_subtotal_equal_to_the_sum_of_line_item_subtotals()
        {
            order.AddLineItem(product1, 1);

            Assert.That(order.SubTotal, Is.EqualTo(11m));
        }
        
        [Test]
        public void An_order_with_an_item_has_a_total_equal_to_the_subtotal_plus_tax()
        {
            order.AddLineItem(product1, 1);

            Assert.That(order.Total, Is.EqualTo(12.1m));
        }

        [Test]
        public void An_order_with_items_has_a_tax_equal_to_the_sum_of_the_item_taxes()
        {
            order.AddLineItem(product1, 1);
            order.AddLineItem(product2, 2);

            Assert.That(order.Tax, Is.EqualTo(3.5m));
        }

        [Test]
        public void Adding_an_item_multiple_times_increases_the_quantity()
        {
            order.AddLineItem(product1, 1);
            order.AddLineItem(product1, 2);
            order.AddLineItem(product1, 3);

            Assert.That(order.Items.Count, Is.EqualTo(1));
            Assert.That(order.Items.Any(i => i.ProductName == product1.Name && i.Quantity == 6));
        }

        [Test]
        public void An_order_with_items_has_a_subtotal_equal_to_the_sum_of_the_item_totals()
        {
            order.AddLineItem(product1, 1);
            order.AddLineItem(product2, 2);

            Assert.That(order.SubTotal, Is.EqualTo(23m));
        }

        [Test]
        public void An_order_with_items_has_a_total_equal_to_the_subtotal_plus_tax()
        {
            order.AddLineItem(product1, 1);
            order.AddLineItem(product2, 2);

            Assert.That(order.Total, Is.EqualTo(26.5m));
        }

        [Test]
        public void Cannot_approve_a_rejected_order()
        {
            order.Reject();

            Assert.Throws<RejectedOrderCannotBeApprovedException>(() => order.Approve());
        }

        [Test]
        public void Cannot_approve_a_shipped_order()
        {
            order.Approve();
            order.Ship();

            Assert.Throws<ShippedOrdersCannotBeChangedException>(() => order.Approve());
        }

        [Test]
        public void Cannot_reject_a_shipped_order()
        {
            order.Approve();
            order.Ship();

            Assert.Throws<ShippedOrdersCannotBeChangedException>(() => order.Reject());
        }

        [Test]
        public void Cannot_reject_an_approved_order()
        {
            order.Approve();

            Assert.Throws<ApprovedOrderCannotBeRejectedException>(() => order.Reject());
        }

        [Test]
        public void Cannot_ship_a_rejected_order()
        {
            order.Reject();

            Assert.Throws<OrderCannotBeShippedException>(() => order.Ship());
        }

        [Test]
        public void Cannot_ship_a_shipped_order()
        {
            order.Approve();
            order.Ship();

            Assert.Throws<OrderCannotBeShippedTwiceException>(() => order.Ship());
        }

        [Test]
        public void The_status_of_a_new_order_is_Created()
        {
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Created));
        }

        [Test]
        public void The_status_of_a_rejected_order_is_Rejected()
        {
            order.Reject();

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Rejected));
        }

        [Test]
        public void The_status_of_a_shipped_order_is_Shipped()
        {
            order.Approve();
            order.Ship();

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Shipped));
        }

        [Test]
        public void The_status_of_an_approved_order_is_Approved()
        {
            order.Approve();

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Approved));
        }
    }
}