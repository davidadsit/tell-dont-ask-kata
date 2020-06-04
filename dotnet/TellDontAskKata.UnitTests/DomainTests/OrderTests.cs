using NUnit.Framework;
using TellDontAskKata.Domain;

namespace TellDontAskKata.UnitTests.DomainTests
{
    public class OrderTests
    {
        [Test]
        public void An_empty_order_has_a_tax_of_0()
        {
            var order = new Order();

            Assert.That(order.Tax, Is.EqualTo(0m));
        }

        [Test]
        public void An_empty_order_has_a_total_of_0()
        {
            var order = new Order();

            Assert.That(order.Total, Is.EqualTo(0m));
        }

        [Test]
        public void An_order_with_an_item_has_a_tax_equal_to_the_item_tax()
        {
            var order = new Order();
            order.Items.Add(LineItem.CreateWithProduct(11m, 10, 1));

            Assert.That(order.Tax, Is.EqualTo(1.1m));
        }

        [Test]
        public void An_order_with_an_item_has_a_total_equal_to_the_item_total()
        {
            var order = new Order();
            order.Items.Add(LineItem.CreateWithProduct(11m, 0, 1));

            Assert.That(order.Total, Is.EqualTo(11m));
        }

        [Test]
        public void An_order_with_items_has_a_tax_equal_to_the_sum_of_the_item_taxes()
        {
            var order = new Order();
            order.Items.Add(LineItem.CreateWithProduct(11m, 10, 1));
            order.Items.Add(LineItem.CreateWithProduct(6m, 20, 2));

            Assert.That(order.Tax, Is.EqualTo(3.5m));
        }

        [Test]
        public void An_order_with_items_has_a_total_equal_to_the_sum_of_the_item_totals()
        {
            var order = new Order();
            order.Items.Add(LineItem.CreateWithProduct(11m, 0, 1));
            order.Items.Add(LineItem.CreateWithProduct(6m, 0, 2));

            Assert.That(order.Total, Is.EqualTo(23m));
        }

        [Test]
        public void The_status_of_a_new_order_is_Created()
        {
            var order = new Order();

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Created));
        }

        [Test]
        public void The_status_of_a_rejected_order_is_Rejected()
        {
            var order = new Order();

            order.Reject();

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Rejected));
        }

        [Test]
        public void The_status_of_a_shipped_order_is_Shipped()
        {
            var order = new Order();

            order.Ship();

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Shipped));
        }

        [Test]
        public void The_status_of_an_approved_order_is_Approved()
        {
            var order = new Order();

            order.Approve();

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Approved));
        }
    }
}