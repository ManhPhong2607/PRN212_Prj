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

namespace PRN212_Prj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ManageTables_Click(object sender, RoutedEventArgs e)
        {
            ManageTables tablesWindow = new ManageTables();
            tablesWindow.Show();
            this.Hide();
        }

        private void ManageMenu_Click(object sender, RoutedEventArgs e)
        {
            ManageMenu menuWindow = new ManageMenu();
            menuWindow.Show();
            this.Hide();
        }

        private void ManageOrder_Click(object sender, RoutedEventArgs e)
        {
            ManageOrder ordersWindow = new ManageOrder();
            ordersWindow.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int roleId = (int)Application.Current.Properties["CurrentUserRoleId"];
            Home loginWindow = new Home(roleId);
            this.Hide();
            loginWindow.ShowDialog();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            CustomerInfo cs = new CustomerInfo();
            cs.Show();
            this.Hide();
        }

        private void ManageAccount_click(object sender, RoutedEventArgs e)
        {
            ManageAccount ma = new ManageAccount();
            ma.Show();
            this.Hide();
        }
    }
}