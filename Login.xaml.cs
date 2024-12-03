using Microsoft.VisualBasic.ApplicationServices;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            using (var context = new ManageStoreContext())
            {
                var acc = context.Accounts
                    .Where(a => a.Username == username && a.Password == password)
                    .FirstOrDefault();
                if (acc != null)
                {
                    if (acc.Status == "Active")  
                    {
                        int? roleId = acc.RoleId;
                        if (roleId.HasValue)
                        {
                            Application.Current.Properties["CurrentUserId"] = acc.Id;
                            Application.Current.Properties["CurrentUserRoleId"] = acc.RoleId;

                            Home lg = new Home(roleId.Value);
                            lg.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your account is inactive. Please contact the admin to enable your account.",
                                        "Access Denied",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



    }
}
