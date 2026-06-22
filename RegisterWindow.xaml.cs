using shoppingTechCart.Entities;
using shoppingTechCart.Services;
using System.Windows;

namespace shoppingTechCart
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string accountId = txtAccount.Text.Trim();
            string password = txtPassword.Password.Trim();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(accountId))
            {
                MessageBox.Show("Please enter account");
                return;
            }

            if (accountId.Length > 20)
            {
                MessageBox.Show("Account must be less than or equal to 20 characters");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter password");
                return;
            }

            if (password.Length > 20)
            {
                MessageBox.Show("Password must be less than or equal to 20 characters");
                return;
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Please enter first name");
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
                MessageBox.Show("Phone must be less than or equal to 20 characters");
                return;
            }

            try
            {
                using var _db = new ProductIntroContext();

                bool existed = _db.Accounts.Any(a => a.Account1 == accountId);
                if (existed)
                {
                    MessageBox.Show("Account already exists");
                    return;
                }

                Account account = new Account()
                {
                    Account1 = accountId,
                    Pass = PasswordHasher.HashPassword(password),
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                    IsUse = true,
                    RoleInSystem = 0
                };

                _db.Accounts.Add(account);
                int rows = _db.SaveChanges();

                MessageBox.Show($"Register successfully. Saved rows: {rows}");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Register failed: " + ex.GetBaseException().Message);
            }
        }
    }
}
