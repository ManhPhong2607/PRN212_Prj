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
    /// Interaction logic for ViewDetailOrder.xaml
    /// </summary>
    public partial class ViewDetailOrder : Window
    {
        private ManageStoreContext context = new ManageStoreContext();
        private int orderId;

        public ViewDetailOrder(int orderId)
        {
            InitializeComponent();
            LoadOrderDetails(orderId);
        }
        private void LoadOrderDetails(int orderId)
        {
            var orderDetails = context.OrderDetails
                            .Where(od => od.OrderId == orderId).Include(o => o.Menu)
                            .ToList();

            dgOrderDetails.ItemsSource = orderDetails;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
