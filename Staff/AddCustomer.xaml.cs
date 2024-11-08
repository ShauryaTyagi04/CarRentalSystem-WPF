using System;
using System.Linq;
using System.Windows;

namespace Car_Rental_System.Staff
{
    public partial class AddCustomer : Window
    {
        //Database entity being called in an object.
        private CabRentalServiceEntities db = new CabRentalServiceEntities();
        public AddCustomer()
        {
            InitializeComponent();
        }

        //Redirection to Home Screen.
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

            StaffHome HomeWindow = new StaffHome();
            HomeWindow.Show();
            this.Close();
        }

        //Redirection to Login Screen.
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }

        //Redirection to View Customers.
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {

            ViewCustomer ViewWindow = new ViewCustomer();
            ViewWindow.Show();
            this.Close();
        }

        //New customer is being created.
        //Submission only possible after all validation checks are completed.
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            string email = txtEmail.Text;
            string acc = txtAccount.Text;

            if (IsFormValid())
            {

                DateTime dob;
                if (!DateTime.TryParseExact(txtDOB.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dob))
                {
                    MessageBox.Show("Date of Birth must be in DD-MM-YYYY format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (dob > DateTime.Now)
                {
                    MessageBox.Show("Date of Birth cannot be a future date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var existingEmail = db.Customer
                         .FirstOrDefault(c => c.EmailAddress == email);

                if (existingEmail != null)
                {
                    MessageBox.Show("Email address already exists. Please use a different one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var existingAcc = db.Customer
                         .FirstOrDefault(c => c.AccountReference == acc);

                if (existingAcc != null)
                {
                    MessageBox.Show("Account number already registered. Please use a different one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newCustomer = new Customer
                {
                    FirstName = txtFName.Text,
                    LastName = txtLName.Text,
                    DOB = dob,
                    ContactNumber = txtNumber.Text,
                    AlternateContactNumber = txtAltNumber.Text,
                    EmailAddress = txtEmail.Text,
                    Address = txtAddress.Text,
                    PostCode = txtPost.Text,
                    AccountReference = txtAccount.Text,
                    IsApproved = false
                };

                db.Customer.Add(newCustomer);
                db.SaveChanges();

                MessageBox.Show("Customer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();
            }
        }

        //Method to check if fields are empty or not.
        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(txtFName.Text) ||
                string.IsNullOrWhiteSpace(txtLName.Text) ||
                string.IsNullOrWhiteSpace(txtDOB.Text) ||
                string.IsNullOrWhiteSpace(txtNumber.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAccount.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        //Method to clear all fields.
        private void ClearForm()
        {
            txtFName.Text = "";
            txtLName.Text = "";
            txtDOB.Text = "";
            txtNumber.Text = "";
            txtAltNumber.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            txtPost.Text = "";
            txtAccount.Text = "";
        }
    }
}
