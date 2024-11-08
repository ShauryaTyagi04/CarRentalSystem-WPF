using Car_Rental_System.Admin;
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

namespace Car_Rental_System.Staff
{
    public partial class ViewCustomer : Window
    {
        CabRentalServiceEntities db = new CabRentalServiceEntities();

        public ViewCustomer()
        {
            InitializeComponent();
            LoadCustomerData();
            LoadCustomerIDs();
        }

        //Loading data from customer table into data grid.
        private void LoadCustomerData()
        {
            var customers = db.Customer.ToList();
            CustomerData.ItemsSource = customers;
        }

        //Loading customer IDs into combo box.
        private void LoadCustomerIDs()
        {
            var customerIDs = db.Customer.Select(c => c.CustomerID).ToList();
            cmbCustomerID.ItemsSource = customerIDs;
        }

        //populating the UI fields after selection of an ID.
        private void cmbCustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomerID.SelectedItem != null)
            {
                int selectedID = (int)cmbCustomerID.SelectedItem;
                var customer = db.Customer.FirstOrDefault(c => c.CustomerID == selectedID);

                if (customer != null)
                {
                    txtFName.Text = customer.FirstName;
                    txtLName.Text = customer.LastName;
                    txtDOB.Text = customer.DOB.ToString("dd-MM-yyyy");
                    txtNumber.Text = customer.ContactNumber;
                    txtAltNumber.Text = customer.AlternateContactNumber;
                    txtEmail.Text = customer.EmailAddress;
                    txtAddress.Text = customer.Address;
                    txtPost.Text = customer.PostCode;
                    txtAccount.Text = customer.AccountReference;
                    txtIsApproved.Content = customer.IsApproved ? "Approved" : "Not Approved";
                }
            }
        }

        //populating the UI fields after selection of a row.
        private void CustomerData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerData.SelectedItem != null)
            {
                Customer selectedCustomer = (Customer)CustomerData.SelectedItem;

                cmbCustomerID.SelectedItem = selectedCustomer.CustomerID;
                txtFName.Text = selectedCustomer.FirstName;
                txtLName.Text = selectedCustomer.LastName;
                txtDOB.Text = selectedCustomer.DOB.ToString("dd-MM-yyyy");
                txtNumber.Text = selectedCustomer.ContactNumber;
                txtAltNumber.Text = selectedCustomer.AlternateContactNumber;
                txtEmail.Text = selectedCustomer.EmailAddress;
                txtAddress.Text = selectedCustomer.Address;
                txtPost.Text = selectedCustomer.PostCode;
                txtAccount.Text = selectedCustomer.AccountReference;
                txtIsApproved.Content = selectedCustomer.IsApproved ? "Approved" : "Not Approved";
            }
        }

        //Method to clear all the fields, will be used after form submission.
        private void ClearForm()
        {
            txtFName.Clear();
            txtLName.Clear();
            txtDOB.Clear();
            txtNumber.Clear();
            txtAltNumber.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtPost.Clear();
            txtAccount.Clear();
            txtIsApproved.Content = "";
            cmbCustomerID.SelectedIndex = -1;
            CustomerData.SelectedItem = -1;
        }

        //Redirection to home page.
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

            StaffHome HomeWindow = new StaffHome();
            HomeWindow.Show();
            this.Close();
        }

        //Redirection to login page.
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }

        // Updating the customer details using selected id from combo box.
        //Submission only possible after all validation checks are completed.
        private void Submit_Click(object sender, RoutedEventArgs e)
        {

            DateTime dob;
            if (!DateTime.TryParseExact(txtDOB.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dob))
            {
                MessageBox.Show("Date of Birth must be in DD-MM-YYYY format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbCustomerID.SelectedItem != null)
            {
                int selectedID = (int)cmbCustomerID.SelectedItem;
                var customer = db.Customer.FirstOrDefault(c => c.CustomerID == selectedID);
                if (customer != null)
                {

                    customer.FirstName = txtFName.Text;
                    customer.LastName = txtLName.Text;
                    customer.DOB = dob;
                    customer.ContactNumber = txtNumber.Text;
                    customer.AlternateContactNumber = txtAltNumber.Text;
                    customer.EmailAddress = txtEmail.Text;
                    customer.Address = txtAddress.Text;
                    customer.PostCode = txtPost.Text;
                    customer.AccountReference = txtAccount.Text;
                    customer.IsApproved = txtIsApproved.Content.ToString() == "Approved";

                    db.SaveChanges();
                    MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCustomerData();
                }
            }
        }

        //Deleting the vehichle data using selected id from combo box.
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCustomerID.SelectedItem != null)
            {
                int selectedID = (int)cmbCustomerID.SelectedItem;
                var customer = db.Customer.FirstOrDefault(c => c.CustomerID == selectedID);

                if (customer != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Are you sure you want to delete this customer's record?",
                        "Confirm deletion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        db.Customer.Remove(customer);
                        db.SaveChanges();
                        MessageBox.Show("Customer records deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCustomerData();
                        LoadCustomerIDs();
                        ClearForm();
                    }
                }
            }
        }
    }
}
