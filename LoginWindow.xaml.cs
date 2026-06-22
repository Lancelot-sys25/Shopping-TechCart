using shoppingTechCart.Entities;
using System.Windows;

namespace shoppingTechCart
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            using var db = new ProductIntroContext();

            var account = db.Accounts.FirstOrDefault(a =>
                a.Account1 == txtAccount.Text &&
                a.Pass == txtPassword.Password &&
                a.IsUse == true);

            if (account == null)
            {
                MessageBox.Show("Wrong account or password");
                return;
            }

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
