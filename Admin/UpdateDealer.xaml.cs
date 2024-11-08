using Car_Rental_System.Staff;
using System.Linq;
using System.Security.Principal;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Remoting.Messaging;

namespace Car_Rental_System.Admin
{
    public partial class UpdateDealer : Window
    {
        CabRentalServiceEntities db = new CabRentalServiceEntities();

        public UpdateDealer()
        {
            InitializeComponent();
            LoadDealerData();
            LoadDealerIDs();
        }

        //Loading data from Dealer table into data grid.
        private void LoadDealerData()
        {
            var dealers = db.Dealer.ToList();
            DealerData.ItemsSource = dealers;
        }

        //Loading Dealer IDs into combo box.
        private void LoadDealerIDs()
        {
            var dealerIDs = db.Dealer.Select(d => d.DealerID).ToList();
            cmbDealerID.ItemsSource = dealerIDs;
        }

        //populating the UI fields after selection of an ID.
        private void cmbDealerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDealerID.SelectedItem != null)
            {
                int selectedID = (int)cmbDealerID.SelectedItem;
                var dealer = db.Dealer.FirstOrDefault(d => d.DealerID == selectedID);

                if (dealer != null)
                {
                    txtFName.Text = dealer.FirstName;
                    txtLName.Text = dealer.LastName;
                    txtDate.Text = dealer.DateJoined.ToString("dd-MM-yyyy");
                    txtNumber.Text = dealer.ContactNumber;
                    txtEmail.Text = dealer.EmailAddress;
                    txtAddress.Text = dealer.Address;
                    txtPost.Text = dealer.PostCode;
                    txtIsApproved.Content = (dealer.IsApproved ?? false) ? "Approved" : "Not Approved";

                    int vehicleCount = db.Vehicle.Count(v => v.DealerID == selectedID);
                    txtVehicles.Content = vehicleCount;
                }
            }
        }

        //populating the UI fields after selection of row.
        private void DealerData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DealerData.SelectedItem != null)
            {
                Dealer selectedDealer = (Dealer)DealerData.SelectedItem;

                cmbDealerID.SelectedItem = selectedDealer.DealerID;
                txtFName.Text = selectedDealer.FirstName;
                txtLName.Text = selectedDealer.LastName;
                txtDate.Text = selectedDealer.DateJoined.ToString("dd-MM-yyyy");
                txtNumber.Text = selectedDealer.ContactNumber;
                txtEmail.Text = selectedDealer.EmailAddress;
                txtAddress.Text = selectedDealer.Address;
                txtPost.Text = selectedDealer.PostCode;
                txtIsApproved.Content = (selectedDealer.IsApproved ?? false) ? "Approved" : "Not Approved";

                int vehicleCount = db.Vehicle.Count(v => v.DealerID == selectedDealer.DealerID);
                txtVehicles.Content = vehicleCount;
            }
        }

        //Method to clear all the fields, will be used after form submission.
        private void ClearForm()
        {
            txtFName.Clear();
            txtLName.Clear();
            txtDate.Clear();
            txtNumber.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtPost.Clear();
            txtIsApproved.Content = "";
            cmbDealerID.SelectedIndex = -1;
            DealerData.SelectedItem = -1;
        }

        //Redirection to home page.
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

            StaffHome HomeWindow = new StaffHome();
            HomeWindow.Show();
            this.Close();
        }

        //Redirection to Login page.
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }

        // Updating the dealer details using selected id from combo box.
        //Submission only possible after all validation checks are completed.
        private void Update_Click(object sender, RoutedEventArgs e)
        {

            DateTime dob;
            if (!DateTime.TryParseExact(txtDate.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dob))
            {
                MessageBox.Show("Date of Birth must be in DD-MM-YYYY format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbDealerID.SelectedItem != null)
            {
                int selectedID = (int)cmbDealerID.SelectedItem;
                var dealer = db.Dealer.FirstOrDefault(d => d.DealerID == selectedID);
                if (dealer != null)
                {

                    dealer.FirstName = txtFName.Text;
                    dealer.LastName = txtLName.Text;
                    dealer.DateJoined = dob;
                    dealer.ContactNumber = txtNumber.Text;
                    dealer.EmailAddress = txtEmail.Text;
                    dealer.Address = txtAddress.Text;
                    dealer.PostCode = txtPost.Text;
                    dealer.IsApproved = txtIsApproved.Content.ToString() == "Approved";

                    db.SaveChanges();
                    MessageBox.Show("Dealer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDealerData();
                }
            }
        }

        //Deleting the dealer data using selected id from combo box.
        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDealerID.SelectedItem != null)
            {
                int selectedID = (int)cmbDealerID.SelectedItem;
                var dealer = db.Dealer.FirstOrDefault(d => d.DealerID == selectedID);

                if (dealer != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Are you sure you want to delete this dealer's record?",
                        "Confirm deletion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        db.Dealer.Remove(dealer);
                        db.SaveChanges();
                        MessageBox.Show("Dealer records deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadDealerData();
                        LoadDealerIDs();
                        ClearForm();
                    }
                }
            }
        }

        //Changing the IsAPProved field to true.
        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDealerID.SelectedItem != null)
            {
                int selectedID = (int)cmbDealerID.SelectedItem;
                var dealer = db.Dealer.FirstOrDefault(d => d.DealerID == selectedID);

                if (dealer != null)
                {
                    dealer.IsApproved = true;
                    db.SaveChanges();
                    MessageBox.Show("Dealer application approved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtIsApproved.Content = "Approved";
                    LoadDealerData();
                }
            }
        }
    }
}
