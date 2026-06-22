using Microsoft.EntityFrameworkCore;
using shoppingTechCart.Entities;
using System.Windows;

namespace shoppingTechCart
{
    public partial class UserWindow : Window
    {
        private ProductIntroContext _db;
        private Account _currentUser;
        private int? _currentOrderId;

        public UserWindow(Account currentUser)
        {
            InitializeComponent();

            _db = new ProductIntroContext();
            _currentUser = currentUser;

            LoadProducts();
            LoadCart();
        }

        private void LoadProducts()
        {
            dgProducts.ItemsSource = _db.Products
                .Include(p => p.Type)
                .ToList();
        }

        private void LoadCart()
        {
            dgCart.ItemsSource = _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == _currentUser.Account1)
                .ToList();
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is not Product product)
            {
                MessageBox.Show("Please select product");
                return;
            }

            var cartItem = _db.CartItems.FirstOrDefault(c =>
                c.SessionId == _currentUser.Account1 &&
                c.ProductId == product.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += 1;
            }
            else
            {
                cartItem = new CartItem
                {
                    SessionId = _currentUser.Account1,
                    ProductId = product.ProductId,
                    Quantity = 1,
                    AddedAt = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }

            _db.SaveChanges();
            LoadCart();
        }

        private void btnRemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (dgCart.SelectedItem is not CartItem cartItem)
            {
                MessageBox.Show("Please select cart item");
                return;
            }

            _db.CartItems.Remove(cartItem);
            _db.SaveChanges();

            LoadCart();
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var cartItems = _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == _currentUser.Account1)
                .ToList();

            if (cartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty");
                return;
            }

            int total = cartItems.Sum(c =>
            {
                int price = c.Product.Price ?? 0;
                int discount = c.Product.Discount ?? 0;
                int quantity = c.Quantity ?? 1;

                return (price - discount) * quantity;
            });

            Order order = new Order
            {
                AccountId = _currentUser.Account1,
                CreateAt = DateTime.Now,
                TotalAmount = total,
                Status = "Pending"
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            foreach (var item in cartItems)
            {
                int price = item.Product.Price ?? 0;
                int discount = item.Product.Discount ?? 0;
                int quantity = item.Quantity ?? 1;

                OrderDetail detail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = quantity,
                    UnitPrice = price,
                    Discount = discount,
                    LineTotal = (price - discount) * quantity
                };

                _db.OrderDetails.Add(detail);
            }

            _db.CartItems.RemoveRange(cartItems);
            _db.SaveChanges();

            _currentOrderId = order.OrderId;

            MessageBox.Show($"Order created. Total: {total}");
            LoadCart();
        }

        private void btnPayCash_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrderId == null)
            {
                MessageBox.Show("Please create order first");
                return;
            }

            var order = _db.Orders.FirstOrDefault(o => o.OrderId == _currentOrderId);

            if (order == null)
            {
                MessageBox.Show("Order not found");
                return;
            }

            order.Status = "Paid";
            order.PaymentMethod = "Cash";
            order.PaidAt = DateTime.Now;

            _db.SaveChanges();

            MessageBox.Show("Payment successful. Order status is Paid");
        }
    }
}
