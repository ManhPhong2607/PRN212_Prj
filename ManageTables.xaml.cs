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
using Table = PRN212_Prj.Models.Table;

namespace PRN212_Prj
{
    /// <summary>
    /// Interaction logic for ManageTables.xaml
    /// </summary>
    public partial class ManageTables : Window
    {

        ManageStoreContext context = new ManageStoreContext();
        public ManageTables()
        {
            InitializeComponent();
            LoadTables();
        }

        private void LoadTables()
        {
            var listTable = context.Tables.ToList();
            lvTables.ItemsSource = listTable;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var newName = txtName.Text;
            var newStatus = (cmbStatus.SelectedItem as ComboBoxItem).Content.ToString();

            Table newTable = new Table
            {
                Name = newName,
                Status = newStatus
            };
            context.Tables.Add(newTable);
            context.SaveChanges();
            LoadTables();
            ClearTableFields();
        }



        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvTables.SelectedItem != null)
            {
                var selectedTable = lvTables.SelectedItem as Table;
                if (selectedTable != null)
                {
                    selectedTable.Name = txtName.Text;  
                    var selectedStatus = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (!string.IsNullOrEmpty(selectedStatus))
                    {
                        selectedTable.Status = selectedStatus; 
                    }
                    else
                    {
                        MessageBox.Show("Please select a valid status.");
                        return;
                    }

                    try
                    {
                        context.SaveChanges();
                        LoadTables();
                        txtId.Clear();
                        txtName.Clear();
                        cmbStatus.SelectedIndex = -1; 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating table: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a table to update.");
            }
        }


        private void ClearTableFields()
        {
            txtId.Clear();
            txtName.Clear();
            cmbStatus.SelectedIndex = -1;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 

            MainWindow mainWindow = new MainWindow(); 
            mainWindow.Show(); 
        }

        private void BtnClear_click(object sender, RoutedEventArgs e)
        {
            ClearTableFields();

        }
        private void lvChange(object sender, SelectionChangedEventArgs e)
        {
            if (lvTables.SelectedItem != null)
            {
                var item = lvTables.SelectedItem as Table;
                if (item != null)
                {
                    txtId.Text = item.Id.ToString();
                    txtName.Text = item.Name;
                    if (item.Status == "Available")
                    {
                        cmbStatus.SelectedItem = cmbStatus.Items.Cast<ComboBoxItem>()
                            .FirstOrDefault(i => i.Content.ToString() == "Available");
                    }
                    else if (item.Status == "Occupied")
                    {
                        cmbStatus.SelectedItem = cmbStatus.Items.Cast<ComboBoxItem>()
                            .FirstOrDefault(i => i.Content.ToString() == "Occupied");
                    }
                    else if (item.Status == "Not Yet")
                    {
                        cmbStatus.SelectedItem = cmbStatus.Items.Cast<ComboBoxItem>()
                            .FirstOrDefault(i => i.Content.ToString() == "Not Yet");
                    }
                }
            }
        }

    }
}
