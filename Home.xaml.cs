using Microsoft.VisualBasic.Logging;
using PRN212_Prj.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PRN212_Prj
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        ManageStoreContext context = new ManageStoreContext();
        private int? roleId;
        private const string ImageDirectory = @"D:\PRN212_Projet\PRN212_Prj\Image";

        public Home(int? roleID)
        {
            InitializeComponent();
            this.roleId = roleID; 
            LoadMenu();
            LoadTables();
            CheckUserRole();
        }

        private void CheckUserRole()
        {
            btnManage.Visibility = (roleId == 1) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadTables()
        {
            var listTable = context.Tables.Select(t => new
            {
                Id = t.Id,
                Name = t.Name,
                Status = t.Status,
            }).ToList();
            lvTables.ItemsSource = listTable;
        }

        private void LoadMenu()
        {
            var listMenu = context.Menus.Select(m => new
            {
                Name = m.Name,
                CateId = m.CateId,
                Price = m.Price,
                Image = System.IO.Path.Combine(ImageDirectory, m.Image), 
                Description = m.Description,
            }).ToList();

            lvMenu.ItemsSource = listMenu;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            this.Close();
            loginWindow.ShowDialog();
        }

        private void lvTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvTables.SelectedItem == null) return;

            var selectedTable = (dynamic)lvTables.SelectedItem;
            int tableId = selectedTable.Id;

            OrderWindow orderWindow = new OrderWindow(tableId);
            orderWindow.ShowDialog();

            lvTables.SelectedItem = null; 
            LoadTables();
        }

        private void History_click(object sender, RoutedEventArgs e)
        {
            OrderHistory oh = new OrderHistory();
            oh.Show();
        }

        private void Logout_click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login lg = new Login();
            lg.Show();
        }
    }
}
