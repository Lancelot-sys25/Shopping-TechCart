using Microsoft.EntityFrameworkCore;
using shoppingTechCart.Entities;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace shoppingTechCart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProductIntroContext _db;// khoi tao db
        private Account _currentUser;

        public MainWindow(Account currentUser)
        {
            InitializeComponent();
            _db = new();
            _currentUser = currentUser;

            LoadProduct();
            LoadCategories();
        }

        private void LoadProduct()
        {
            var Product = _db.Products.Include(x => x.Type).ToList();
            dgProducts.ItemsSource = Product;
        }

        private void LoadCategories()
        {
            var categories = _db.Categories.ToList();
            cbxType.ItemsSource = categories;

            cbxType.DisplayMemberPath = "CategoryName";
            cbxType.SelectedValuePath = "TypeId";
        }

        
        //READ
        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgProducts.SelectedItem is Product product)
            {
                txtId.Text = product.ProductId;
                txtName.Text = product.ProductName;
                txtPrice.Text = product.Price?.ToString();
                txtDiscount.Text = product.Discount?.ToString();
                txtUnit.Text = product.Unit;
                cbxType.SelectedValue = product.TypeId;
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter product name");
                return;
            }

            if (!int.TryParse(txtPrice.Text, out int price))
            {
                MessageBox.Show("Price must be a number");
                return;
            }

            if (!int.TryParse(txtDiscount.Text, out int discount))
            {
                MessageBox.Show("Discount must be a number");
                return;
            }

            if (cbxType.SelectedValue == null)
            {
                MessageBox.Show("Please select category");
                return;
            }
            Product product = new Product()
            {

                ProductId = txtId.Text,
                ProductName = txtName.Text,
                Price = int.Parse(txtPrice.Text),
                Discount = int.Parse(txtDiscount.Text),
                Unit = txtUnit.Text,
                TypeId = (int)(cbxType.SelectedValue),
                Account = _currentUser.Account1,
                PostedDate = DateTime.Now
            };

            var result = MessageBox.Show("Do you want to add the product?", "ADD",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                LoadProduct();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(dgProducts.SelectedItem is Product product)
            {
                product.ProductName = txtName.Text;
                product.Price = int.Parse(txtPrice.Text);
                product.Discount = int.Parse(txtDiscount.Text);
                product.Unit = txtUnit.Text;
                product.TypeId = (int)cbxType.SelectedValue;

                var result = MessageBox.Show("Do you want to update ", "UPDATE",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    _db.SaveChanges();
                    LoadProduct();

                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product product)
            {
                // Check if product is in any orders to prevent foreign key errors
                bool hasHistory = _db.OrderDetails.Any(od => od.ProductId == product.ProductId);
                if (hasHistory)
                {
                    MessageBox.Show("Cannot delete this product because it has purchase history in orders.", "Delete Blocked", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show("Do you want to delete? ", "DELETE",
                   MessageBoxButton.YesNo, MessageBoxImage.Question);
               
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Also remove associated cart items if any, to avoid FK issues there
                        var relatedCartItems = _db.CartItems.Where(c => c.ProductId == product.ProductId);
                        _db.CartItems.RemoveRange(relatedCartItems);

                        _db.Products.Remove(product);
                        _db.SaveChanges();
                        LoadProduct();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting product: " + ex.GetBaseException().Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
