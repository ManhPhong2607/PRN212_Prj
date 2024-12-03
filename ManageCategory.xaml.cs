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
    /// Interaction logic for ManageCategory.xaml
    /// </summary>
    public partial class ManageCategory : Window
    {
        public ManageCategory()
        {
            InitializeComponent();
            LoadCategories();
        }
        private void LoadCategories()
        {
            using (var db = new ManageStoreContext())
            {
                var categories = db.Categories.ToList();
                LvCategory.ItemsSource = categories;
            }
        }

        private void BtnAddMenu_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = txtCategory.Text;

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Please enter a category name.");
                return;
            }

            using (var db = new ManageStoreContext()) 
            {
                var newCategory = new Category
                {
                    Name = categoryName
                };

                db.Categories.Add(newCategory);
                db.SaveChanges();
                LoadCategories();
            }

            txtCategory.Clear();
            ManageMenu mm = new ManageMenu();
            mm.Show();
            this.Close();
        }
        private void BtnUpdateMenu_Click(object sender, RoutedEventArgs e)
        {
            if (LvCategory.SelectedItem is Category selectedCategory)
            {
                var updatedName = txtCategory.Text;

                if (string.IsNullOrEmpty(updatedName))
                {
                    MessageBox.Show("Please enter a category name.");
                    return;
                }

                using (var db = new ManageStoreContext())
                {
                    var categoryToUpdate = db.Categories.FirstOrDefault(c => c.Id == selectedCategory.Id);
                    if (categoryToUpdate != null)
                    {
                        categoryToUpdate.Name = updatedName;
                        db.SaveChanges();
                        LoadCategories();
                    }
                }

                txtCategory.Clear();
            }
            else
            {
                MessageBox.Show("Please select a category to update.");
            }
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtCategory.Clear();
            txtId.Clear();
        }
        private void BtnBackMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ManageMenu mm = new ManageMenu();
            mm.Show();
            this.Close();
        }
        private void lvChange(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LvCategory.SelectedItem is Category selectedCategory)
            {
                txtId.Text = selectedCategory.Id.ToString();
                txtCategory.Text = selectedCategory.Name;
            }
        }
    }
}
