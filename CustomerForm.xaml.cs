using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRN212_Prj
{
    /// <summary>
    /// Interaction logic for CustomerForm.xaml
    /// </summary>
    public partial class CustomerForm : Window
    {
        public string CustomerName { get; private set; }
        public string CustomerPhone { get; private set; }

        public CustomerForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            CustomerName = txtName.Text.Trim();
            CustomerPhone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(CustomerName) || string.IsNullOrEmpty(CustomerPhone))
            {
                MessageBox.Show("Vui lòng nhập tên và số điện thoại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
