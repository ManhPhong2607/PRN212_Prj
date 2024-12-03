using Microsoft.EntityFrameworkCore;
using PRN212_Prj.Models;
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
    /// Interaction logic for OrderHistory.xaml
    /// </summary>
    public partial class OrderHistory : Window
    {
        private ManageStoreContext context = new ManageStoreContext();
        public OrderHistory()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            int accountID = (int)Application.Current.Properties["CurrentUserId"];

            var listOrder = context.Orders
                .Include(o => o.Account)
                .Include(o => o.Customer)
                .Include(o => o.Table)
                .Include(o => o.OrderDetails)
                .Include(o => o.Payments)
                .Where(o => o.AccountId == accountID) 
                .ToList();

            lvOrders.ItemsSource = listOrder;
        }
        private void BtnBackMainWindow_Click(object sender, RoutedEventArgs e)
        {
            int accountID = (int)Application.Current.Properties["CurrentUserRoleId"];
            this.Close();
            Home mainWindow = new Home(accountID);
            mainWindow.Show();
        }

        private void Btn_ViewOrder(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag is int orderId)
            {
                ViewDetailOrder orderDetailsWindow = new ViewDetailOrder(orderId);
                orderDetailsWindow.Show();
            }
        }
    }
}
