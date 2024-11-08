using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Car_Rental_System.Staff
{
    public partial class AddVehicle : Window
    {
        private CabRentalServiceEntities db = new CabRentalServiceEntities();

        public AddVehicle()
        {
            InitializeComponent();
            LoadDealerIDs();
        }

        //Loading IDs from dealer table into data grid.
        private void LoadDealerIDs()
        {
            var dealerIDs = db.Dealer
                .Where(o => o.IsApproved == true)
                .Select(o => o.DealerID)
                .ToList();

            cmbDealerID.ItemsSource = dealerIDs;
        }

        //Method to chekc if fields are empty or not.
        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(txtColour.Text) ||
                string.IsNullOrWhiteSpace(txtCouncil.Text) ||
                string.IsNullOrWhiteSpace(txtMilage.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtLicenseP.Text) ||
                string.IsNullOrWhiteSpace(txtMake.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtSeats.Text) ||
                string.IsNullOrWhiteSpace(txtRegistration.Text))
            {
                MessageBox.Show("Please fill in all the required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        //Method to clear all fields.
        private void ClearFormFields()
        {
            txtColour.Text = string.Empty;
            txtCouncil.Text = string.Empty;
            txtMilage.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtLicenseP.Text = string.Empty;
            txtMake.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtRegistration.Text = string.Empty;
            txtSeats.Text = string.Empty;
            txtAltFuel.Text = string.Empty;

            cmbFuel.SelectedIndex = -1;
            cmbWheelDrive.SelectedIndex = -1;
            cmbDealerID.SelectedIndex = -1;
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

        //Redirection to view vehicle page.
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {

            ViewVehicle ViewWindow = new ViewVehicle();
            ViewWindow.Show();
            this.Close();
        }

        //Newvehicle is being added
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                if (cmbDealerID.SelectedItem == null)
                {
                    MessageBox.Show("Please select a Policy Number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string fuelType;
                if (!string.IsNullOrEmpty(txtAltFuel.Text))
                {
                    fuelType = txtAltFuel.Text;
                }
                else
                {
                    if (cmbFuel.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a Fuel Type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    fuelType = (cmbFuel.SelectedItem as ComboBoxItem).Content.ToString();
                }

                if (cmbWheelDrive.SelectedItem == null)
                {
                    MessageBox.Show("Please select a Wheel Drive.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string wheelDrive = (cmbWheelDrive.SelectedItem as ComboBoxItem).Content.ToString();

                if (!double.TryParse(txtPrice.Text, out double price))
                {
                    MessageBox.Show("Please enter a valid Price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtMake.Text, out int make))
                {
                    MessageBox.Show("Please enter a valid Year of Make.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtMilage.Text, out int mileage))
                {
                    MessageBox.Show("Please enter a valid Milage.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtSeats.Text, out int seats))
                {
                    MessageBox.Show("Please enter a valid Number of Seats.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var existingRegistration = db.Vehicle.FirstOrDefault(v => v.Registration == txtRegistration.Text);
                var existingLicensePlate = db.Vehicle.FirstOrDefault(v => v.License_Plate == txtLicenseP.Text);

                if (existingRegistration != null)
                {
                    MessageBox.Show("The Registration already exists. Please enter a unique Registration.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (existingLicensePlate != null)
                {
                    MessageBox.Show("The License Plate already exists. Please enter a unique License Plate.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int selectedPolicyID = (int)cmbDealerID.SelectedItem;

                var newVehicleInsurance = new Vehicle
                {
                    Colour = txtColour.Text,
                    Make = make,
                    License_Plate = txtLicenseP.Text,
                    Council = txtCouncil.Text,
                    Model = txtModel.Text,
                    Registration = txtRegistration.Text,
                    Mileage = mileage,
                    Price = price,
                    FuelType = fuelType,       
                    Wheel_Drive = wheelDrive,
                    Seats = seats,
                    IsApproverd = false,
                    DealerID = selectedPolicyID,
                    IsAvailable = true,
                };


                db.Vehicle.Add(newVehicleInsurance);
                db.SaveChanges();

                MessageBox.Show("Car insurance added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearFormFields();
            }
        }

    }
}
