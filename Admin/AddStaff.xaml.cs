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

namespace Car_Rental_System.Admin
{
    public partial class AddStaff : Window
    {
        CabRentalServiceEntities db = new CabRentalServiceEntities();

        public AddStaff()
        {
            InitializeComponent();
        }

        //New Employee is being added.
        //Submission only possible after all validation checks are completed.
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            string first_name = txtFName.Text;
            string last_name = txtLName.Text;
            string user = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string confirm_password = txtCPassword.Text;
            string role = cmbRole.Text;

            if (string.IsNullOrWhiteSpace(first_name) ||
                string.IsNullOrWhiteSpace(last_name) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirm_password) ||
                string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirm_password)
            {
                MessageBox.Show("Passwords do not match.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var existingEmail = db.Employee
                          .FirstOrDefault(emp => emp.EmailAddress == email);

            if (existingEmail != null)
            {
                MessageBox.Show("Email address already registered. Please use a different one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var existingUser = db.Employee
                         .FirstOrDefault(emp => emp.UserName == user);

            if (existingUser != null)
            {
                MessageBox.Show("Username already exists. Please use a different one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Employee newEmployee = new Employee()
            {
                FirstName = first_name,
                LastName = last_name,
                UserName = user,
                EmailAddress = email,
                Password = password,
                Role = role.ToLower() == "staff" ? "staff" : "manager",
                isApproved = true
            };

            db.Employee.Add(newEmployee);
            db.SaveChanges();

            MessageBox.Show("Registration successful", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearFields();

        }

        //Redirection to home page.
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

            AdminHome HomeWindow = new AdminHome();
            HomeWindow.Show();
            this.Close();
        }

        //Redirection to view employees page.
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {

            UpdateStaff ViewWindow = new UpdateStaff();
            ViewWindow.Show();
            this.Close();
        }

        //Redirection to login page.
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }

        //Method to clear all fields.
        private void ClearFields()
        {
            txtFName.Text = string.Empty;
            txtLName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtCPassword.Text = string.Empty;
            cmbRole.SelectedIndex = -1;
        }
    }
}
