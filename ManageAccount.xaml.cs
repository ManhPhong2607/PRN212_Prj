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
    /// Interaction logic for ManageAccount.xaml
    /// </summary>
    public partial class ManageAccount : Window
    {
        ManageStoreContext context = new ManageStoreContext();
        public ManageAccount()
        {
            InitializeComponent();
            LoadAccounts();
            LoadRole();
        }
        private void LoadAccounts()
        {
            var accountList = context.Accounts.Include(a => a.Role)
                .ToList();

            lvMenu.ItemsSource = accountList;

        }
        private void LoadRole()
        {
            var accountList = context.Roles
                .ToList();

            cbRole.ItemsSource = accountList;
            cbRole.SelectedValuePath = "Id";
            cbRole.DisplayMemberPath = "Name";
        }

        private void lvChange(object sender, SelectionChangedEventArgs e)
        {
            if (lvMenu.SelectedItem is Account item)
            {
                txtId.Text = item.Id.ToString();
                cbRole.SelectedValue = item.RoleId;
                txtUsername.Text = item.Username;
                txtPassword.Text = item.Password;
                txtFullname.Text = item.FullName;
                txtPhone.Text = item.Phone;
                if (item.Status == "Active")
                {
                    cmbStatus.SelectedItem = cmbStatus.Items.Cast<ComboBoxItem>()
                        .FirstOrDefault(i => i.Content.ToString() == "Active");
                }
                else if (item.Status == "InActive")
                {
                    cmbStatus.SelectedItem = cmbStatus.Items.Cast<ComboBoxItem>()
                        .FirstOrDefault(i => i.Content.ToString() == "InActive");
                }
                
            }
        }

        private void BtnAddMenu_Click(object sender, RoutedEventArgs e)
        {
            var newStatus = (cmbStatus.SelectedItem as ComboBoxItem).Content.ToString();
            var newAcc = new Account
            {
                Username = txtUsername.Text,
                Password = txtPassword.Text,
                Phone = txtPhone.Text,
                FullName = txtFullname.Text,
                RoleId = (int)cbRole.SelectedValue,
                Status = newStatus
            };
            context.Accounts.Add(newAcc);
            context.SaveChanges();
            LoadAccounts();
            ClearFields();


        }
        private void ClearFields()
        {
            txtId.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            txtFullname.Clear();
            cbRole.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }
        private void BtnUpdateMenu_Click(object sender, RoutedEventArgs e)
        {
            if (lvMenu.SelectedItem is Account selectedMenu)
            {
                var menu = context.Accounts.Find(selectedMenu.Id);

                if (menu != null)
                {
                    menu.Username = txtUsername.Text;
                    menu.Password = txtPassword.Text;
                    menu.Phone = txtPhone.Text;
                    menu.FullName = txtFullname.Text;
                    if (cbRole.SelectedValue != null)
                    {
                        menu.RoleId = (int)cbRole.SelectedValue;
                    }
                    else
                    {
                        MessageBox.Show("Please select a role.");
                        return;
                    }

                    string selectedStatus = cmbStatus.Text;
                    menu.Status = selectedStatus;

                    try
                    {
                        context.SaveChanges();
                        LoadAccounts();
                        ClearFields();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating account: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an account to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void BtnBackMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow m = new MainWindow();
            m.Show();
        }
    }
}
