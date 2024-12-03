using Microsoft.EntityFrameworkCore;
using PRN212_Prj.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace PRN212_Prj
{
    public partial class OrderWindow : Window
    {
        public int TableId { get; private set; }
        private ManageStoreContext context = new ManageStoreContext();
        private ObservableCollection<CustomerOrder> customerOrders;

        public OrderWindow(int tableId)
        {
            InitializeComponent();
            TableId = tableId;
            this.Title = $"Order for Table {TableId}";
            LoadOrderDetails(TableId);
            LoadMenuItems();
        }
        private void LoadMenuItems()
        {
            var menuItems = context.Menus.ToList();
            cbMenuItems.ItemsSource = menuItems;
        }

        private void LoadOrderDetails(int tableId)
        {
            var orders = context.CustomerOrders
                        .Where(order => order.TableId == tableId)
                        .Include(order => order.Menu) 
                        .ToList();
            customerOrders = new ObservableCollection<CustomerOrder>(orders);
            lvOrderDetails.ItemsSource = customerOrders;

            UpdateTotalAmount();

        }

        private void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (var order in customerOrders)
            {
                if (order.Menu != null && order.Quantity.HasValue)
                {
                    totalAmount += order.Total;
                }
            }

            txtTotalAmount.Text = totalAmount.ToString("N3");
        }
        private decimal CalculateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (var order in customerOrders)
            {
                if (order.Menu != null && order.Quantity.HasValue)
                {
                    totalAmount += order.Total;
                }
            }
            return totalAmount;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbMenuItems.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int menuId = (int)cbMenuItems.SelectedValue;
            var existingOrder = context.CustomerOrders
                .FirstOrDefault(o => o.MenuId == menuId && o.TableId == TableId);
            if (existingOrder != null)
            {
                existingOrder.Quantity += quantity;
            }
            else
            {
                var newOrder = new CustomerOrder
                {
                    MenuId = menuId,
                    TableId = TableId,
                    Quantity = quantity
                };
                context.CustomerOrders.Add(newOrder);
            }
            var table = context.Tables.FirstOrDefault(t => t.Id == TableId);
            if (table != null)
            {
                table.Status = "Occupied";
            }
            context.SaveChanges();
            LoadOrderDetails(TableId);
            txtQuantity.Text = "1";
            cbMenuItems.SelectedIndex = -1;
        }

        private void Button_Click_DeleteOrder(object sender, RoutedEventArgs e)
        {
            if (lvOrderDetails.SelectedItem is CustomerOrder selectedOrder)
            {
                var orderToDelete = context.CustomerOrders.Find(selectedOrder.Id);
                if (orderToDelete != null)
                {
                    context.CustomerOrders.Remove(orderToDelete);
                    context.SaveChanges();
                }
                customerOrders.Remove(selectedOrder);
                UpdateTotalAmount();
                bool hasOrders = context.CustomerOrders.Any(order => order.TableId == TableId);
                if (!hasOrders)
                {
                    var table = context.Tables.FirstOrDefault(t => t.Id == TableId);
                    if (table != null)
                    {
                        table.Status = "Available";
                        context.SaveChanges();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một món để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var customerForm = new CustomerForm();
            if (customerForm.ShowDialog() == true)
            {
                string phone = customerForm.CustomerPhone;

                using (var context = new ManageStoreContext())
                {
                    var customer = context.Customers.FirstOrDefault(c => c.Phone == phone);
                    if (customer == null)
                    {
                        customer = new Customer
                        {
                            Name = customerForm.CustomerName,
                            Phone = phone
                        };
                        context.Customers.Add(customer);
                        context.SaveChanges(); 
                    }
                    int customerId = customer.Id;
                    //tinh tong tien
                    decimal totalAmount = CalculateTotalAmount();
                    int accountID = (int)Application.Current.Properties["CurrentUserId"];

                    var newOrder = new Order
                    {
                        CustomerId = customerId,
                        TableId = TableId,
                        OrderDate = DateTime.Now,
                        Total = totalAmount,
                        AccountId = accountID
                    };
                    context.Orders.Add(newOrder);
                    context.SaveChanges(); 
                    int orderId = newOrder.OrderId;

                    var customerOrders = context.CustomerOrders
                        .Where(co => co.TableId == TableId)
                        .Include(co => co.Menu) 
                        .ToList();
                    foreach (var order in customerOrders)
                    {
                        if (order.MenuId.HasValue)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderId = orderId,
                                MenuId = order.MenuId.Value,
                                Price = order.Menu.Price ?? 0,
                                Quantity = order.Quantity ?? 0
                            };
                            context.OrderDetails.Add(orderDetail);
                        }
                    }

                    context.CustomerOrders.RemoveRange(customerOrders);

                    var table = context.Tables.FirstOrDefault(t => t.Id == TableId);
                    if (table != null)
                    {
                        table.Status = "Available"; 
                    }
                    context.SaveChanges();

                    MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadOrderDetails(TableId); 
                }
            }
        }
    }
}
