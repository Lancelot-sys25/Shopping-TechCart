using Microsoft.EntityFrameworkCore;
using shoppingTechCart.Entities;
using shoppingTechCart.Services;
using System.Windows;
using System;
using System.Linq;

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
            LoadUserProfile();
        }

        private void LoadProducts()
        {
            lbProducts.ItemsSource = _db.Products
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

        private void LoadUserProfile()
        {
            txtFirstName.Text = _currentUser.FirstName;
            txtLastName.Text = _currentUser.LastName;
            txtPhone.Text = _currentUser.Phone;
            dpBirthday.SelectedDate = _currentUser.Birthday;

            // Gender combobox item selection
            if (_currentUser.Gender == true)
            {
                cbxGender.SelectedIndex = 0; // Male
            }
            else if (_currentUser.Gender == false)
            {
                cbxGender.SelectedIndex = 1; // Female
            }
            else
            {
                cbxGender.SelectedIndex = 2; // Other
            }
        }

        private void btnToggleCart_Click(object sender, RoutedEventArgs e)
        {
            if (cartBorder.Visibility == Visibility.Visible)
            {
                cartBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                cartBorder.Visibility = Visibility.Visible;
            }
        }

        private void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            DateTime? birthday = dpBirthday.SelectedDate;
            bool? gender = null;
            if (cbxGender.SelectedIndex == 0) gender = true;
            else if (cbxGender.SelectedIndex == 1) gender = false;

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Please enter First Name");
                return;
            }

            if (firstName.Length > 30)
            {
                MessageBox.Show("First name must be less than or equal to 30 characters");
                return;
            }

            if (lastName.Length > 50)
            {
                MessageBox.Show("Last name must be less than or equal to 50 characters");
                return;
            }

            if (phone.Length > 20)
            {
                MessageBox.Show("Phone number must be less than or equal to 20 characters");
                return;
            }

            try
            {
                // Reload user account from DB to make sure we edit tracked entity
                var userInDb = _db.Accounts.FirstOrDefault(a => a.Account1 == _currentUser.Account1);
                if (userInDb == null)
                {
                    MessageBox.Show("User account not found in database");
                    return;
                }

                userInDb.FirstName = firstName;
                userInDb.LastName = lastName;
                userInDb.Phone = phone;
                userInDb.Birthday = birthday;
                userInDb.Gender = gender;

                // Password update if new password is typed
                string newPassword = txtNewPassword.Password.Trim();
                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (newPassword.Length > 20)
                    {
                        MessageBox.Show("New password must be less than or equal to 20 characters");
                        return;
                    }
                    userInDb.Pass = PasswordHasher.HashPassword(newPassword);
                }

                _db.SaveChanges();

                // Update local session user object
                _currentUser = userInDb;
                txtNewPassword.Password = ""; // Clear password box

                MessageBox.Show("Profile updated successfully!");
                LoadUserProfile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile: " + ex.GetBaseException().Message);
            }
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (lbProducts.SelectedItem is not Product product)
            {
                MessageBox.Show("Please select product");
                return;
            }

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("Error adding to cart: " + ex.GetBaseException().Message);
            }
        }

        private void btnRemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (dgCart.SelectedItem is not CartItem cartItem)
            {
                MessageBox.Show("Please select cart item");
                return;
            }

            try
            {
                _db.CartItems.Remove(cartItem);
                _db.SaveChanges();
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing from cart: " + ex.GetBaseException().Message);
            }
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            try
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

                using var transaction = _db.Database.BeginTransaction();

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

                transaction.Commit();

                _currentOrderId = order.OrderId;

                MessageBox.Show($"Order created. Total: {total}");
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating order: " + ex.GetBaseException().Message);
            }
        }

        private void btnPayCash_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrderId == null)
            {
                MessageBox.Show("Please create order first");
                return;
            }

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("Error making payment: " + ex.GetBaseException().Message);
            }
        }
    }
}
