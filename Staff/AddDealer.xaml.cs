using System;
using System.Linq;
using System.Windows;


namespace Car_Rental_System.Staff
{

    public partial class AddDealer : Window
    {
        private CabRentalServiceEntities db = new CabRentalServiceEntities();
        public AddDealer()
        {
            InitializeComponent();
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

        //Redirection to view dealer page.
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {

            ViewDealer ViewWindow = new ViewDealer();
            ViewWindow.Show();
            this.Close();
        }

        //New dealer is being created.
        //Submission only possible after all validation checks are completed.
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            string email = txtEmail.Text;

            if (IsFormValid())
            {

                DateTime dob;
                if (!DateTime.TryParseExact(txtDate.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dob))
                {
                    MessageBox.Show("Date of Birth must be in DD-MM-YYYY format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (dob <= DateTime.Now)
                {
                    MessageBox.Show("Joining Date should be be a future or present date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var existingEmail = db.Dealer
                         .FirstOrDefault(d => d.EmailAddress == email);

                if (existingEmail != null)
                {
                    MessageBox.Show("Email address already exists. Please use a different one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                var newDealer = new Dealer
                {
                    FirstName = txtFName.Text,
                    LastName = txtLName.Text,
                    DateJoined = dob,
                    ContactNumber = txtNumber.Text,
                    EmailAddress = txtEmail.Text,
                    Address = txtAddress.Text,
                    PostCode = txtPost.Text,
                    
                    IsApproved = false
                };

                db.Dealer.Add(newDealer);
                db.SaveChanges();

                MessageBox.Show("Dealer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();
            }
        }

        //Method to check if the fields are empty or not. 
        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(txtFName.Text) ||
                string.IsNullOrWhiteSpace(txtLName.Text) ||
                string.IsNullOrWhiteSpace(txtNumber.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtPost.Text))
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
            txtDate.Text = "";
            txtNumber.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            txtPost.Text = "";
        }
    }
}
