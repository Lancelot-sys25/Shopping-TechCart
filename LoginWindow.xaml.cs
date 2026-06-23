using shoppingTechCart.Entities;
using shoppingTechCart.Services;
using System.Windows;

namespace shoppingTechCart
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            // Code tạm để lấy mật khẩu đã mã hóa (Ví dụ muốn đặt mật khẩu là: abc)
            //string hashedPassword = Services.PasswordHasher.HashPassword("abc");
            //System.Diagnostics.Debug.WriteLine($"=== HASHED PASSWORD: {hashedPassword}");
            //System.Windows.MessageBox.Show(hashedPassword, "Copy chuỗi này nha bạn");
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            using var db = new ProductIntroContext();

            // normalize input (trim) to avoid accidental spaces
            string accountId = txtAccount.Text?.Trim() ?? string.Empty;
            string password = txtPassword.Password?.Trim() ?? string.Empty;

            // find account by id first (only active accounts)
            var account = db.Accounts.FirstOrDefault(a => a.Account1 == accountId && a.IsUse == true);

            if (account == null)
            {
                MessageBox.Show("Wrong account or password");
                return;
            }

            // verify hashed password
            if (!Services.PasswordHasher.VerifyPassword(password, account.Pass))
            {
                MessageBox.Show("Wrong account or password");
                return;
            }

            //if (password != account.Pass) // So sánh trực tiếp chữ thường

                //phân quyền 
                if (account.RoleInSystem == 1)
            {
                MainWindow mainWindow = new MainWindow(account);
                mainWindow.Show();
            }
            else
            {
                UserWindow userWindow = new UserWindow(account);
                userWindow.Show();
            }

            Close();
        }

        private void btnOpenRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}
