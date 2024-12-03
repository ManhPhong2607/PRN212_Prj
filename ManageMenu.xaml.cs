using Microsoft.Win32;
using PRN212_Prj.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Menu = PRN212_Prj.Models.Menu;

namespace PRN212_Prj
{
    public partial class ManageMenu : Window
    {
        ManageStoreContext context = new ManageStoreContext();
        private const string ImageDirectory = @"D:\PRN212_Projet\PRN212_Prj\Image"; 

        public ManageMenu()
        {
            InitializeComponent();
            LoadCategories();  
            loadMenu();
        }
        private void LoadCategories()
        {
            var categories = context.Categories.ToList(); 
            cmbCateId.ItemsSource = categories; 
            cmbCateId.SelectedValuePath = "Id"; 
            cmbCateId.DisplayMemberPath = "Name";
        }

        private void loadMenu()
        {
            var listMenu = context.Menus.ToList();
            foreach (var menu in listMenu)
            {
                menu.Image = System.IO.Path.Combine(ImageDirectory, menu.Image);
            }
            lvMenu.ItemsSource = listMenu;
        }

        private void BtnAddMenu_Click(object sender, RoutedEventArgs e)
        {
            string imageName = SaveImageToDirectory(imgMenu.Tag as string);

            var newMenu = new Menu
            {
                Name = txtMenuName.Text,
                CateId = (int)cmbCateId.SelectedValue, 
                Price = decimal.Parse(txtMenuPrice.Text),
                Description = txtMenuDescription.Text,
                Image = imageName 
            };
            context.Menus.Add(newMenu);
            context.SaveChanges();
            loadMenu();
            ClearMenuFields();
        }

        private void BtnUpdateMenu_Click(object sender, RoutedEventArgs e)
        {
            if (lvMenu.SelectedItem is Menu selectedMenu)
            {
                var menu = context.Menus.Find(selectedMenu.Id);

                if (menu != null)
                {
                    menu.Name = txtMenuName.Text;
                    menu.CateId = (int)cmbCateId.SelectedValue; 
                    menu.Price = decimal.Parse(txtMenuPrice.Text);
                    menu.Description = txtMenuDescription.Text;
                    if (imgMenu.Tag is string imageFilePath && !string.IsNullOrEmpty(imageFilePath))
                    {
                        string imageName = System.IO.Path.GetFileName(imageFilePath);
                        string destinationPath = System.IO.Path.Combine(ImageDirectory, imageName);

                        if (!System.IO.File.Exists(destinationPath))
                        {
                            System.IO.File.Copy(imageFilePath, destinationPath, true);
                        }
                        menu.Image = imageName;
                    }

                    try
                    {
                        context.SaveChanges();
                        loadMenu();
                        ClearMenuFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating menu item: {ex.Message}");
                    }
                }
            }
        }

        private string SaveImageToDirectory(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            string fileName = System.IO.Path.GetFileName(filePath); 
            string targetPath = System.IO.Path.Combine(ImageDirectory, fileName);
            if (!File.Exists(targetPath)) 
            {
                File.Copy(filePath, targetPath, true);
            }

            return fileName; 
        }

        private void BtnBackMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void ClearMenuFields()
        {
            txtId.Clear();
            cmbCateId.SelectedIndex = -1;
            txtMenuName.Clear();
            txtMenuPrice.Clear();
            imgMenu.ClearValue(Image.SourceProperty);
            txtMenuDescription.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                imgMenu.Source = new BitmapImage(new Uri(filePath));
                imgMenu.Tag = filePath; 
            }
        }

        private void lvChange(object sender, SelectionChangedEventArgs e)
        {
            if (lvMenu.SelectedItem is Menu selectedMenu)
            {
                txtId.Text = selectedMenu.Id.ToString();
                cmbCateId.SelectedValue = selectedMenu.CateId; 
                txtMenuName.Text = selectedMenu.Name;
                txtMenuPrice.Text = selectedMenu.Price.ToString();
                txtMenuDescription.Text = selectedMenu.Description;

                string imagePath = System.IO.Path.Combine(ImageDirectory, selectedMenu.Image);
                imgMenu.Source = new BitmapImage(new Uri(imagePath));
                imgMenu.Tag = imagePath; 
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearMenuFields();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ManageCategory mc = new ManageCategory();
            mc.Show();
            this.Close();
        }
    }
}
