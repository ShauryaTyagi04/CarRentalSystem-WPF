using Car_Rental_System.Staff;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Car_Rental_System.Admin
{
    public partial class UpdateVehicle : Window
    {
        CabRentalServiceEntities db = new CabRentalServiceEntities();
        public UpdateVehicle()
        {
            InitializeComponent();
            LoadVehicleData();
            LoadVehicleIDs();
        }

        //Loading data from vehicle table into data grid.
        private void LoadVehicleData()
        {
            var vehicle = db.Vehicle.ToList();
            VehicleData.ItemsSource = vehicle;
            var vehicleColumn = VehicleData.Columns.FirstOrDefault(v => v.Header.ToString() == "Vehicle");
            if (vehicleColumn != null)
            {
                VehicleData.Columns.Remove(vehicleColumn);
            }
        }

        //Loading IDs into combo box.
        private void LoadVehicleIDs()
        {
            var VehicleIDs = db.Vehicle.Select(c => c.VehicleID).ToList();
            cmbVehicleID.ItemsSource = VehicleIDs;
        }

        //Method to clear all the fields, will be used after form submission.
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
            txtDealerID.Content = string.Empty;
            txtAltFuel.Text = string.Empty;

            cmbFuel.SelectedIndex = -1;
            cmbWheelDrive.SelectedIndex = -1;
            cmbVehicleID.SelectedIndex = -1;
        }


        //Redirection to home page.
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

            StaffHome HomeWindow = new StaffHome();
            HomeWindow.Show();
            this.Close();
        }

        //Redirection to Login screen.
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }

        // Updating the vehicle details using selected id from combo box.
        //Submission only possible after all validation checks are completed.
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVehicleID.SelectedItem == null)
            {
                MessageBox.Show("Please select a Vehicle ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (!int.TryParse(txtDealerID.Content.ToString(), out int dealerID))
            {
                MessageBox.Show("Exception Error.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbVehicleID.SelectedItem != null)
            {
                int selectedID = (int)cmbVehicleID.SelectedItem;
                var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == selectedID);
                if (vehicle != null)
                {
                    vehicle.Colour = txtColour.Text;
                    vehicle.Make = make;
                    vehicle.License_Plate = txtLicenseP.Text;
                    vehicle.Council = txtCouncil.Text;
                    vehicle.Model = txtModel.Text;
                    vehicle.Registration = txtRegistration.Text;
                    vehicle.Mileage = mileage;
                    vehicle.Price = price;
                    vehicle.FuelType = fuelType;
                    vehicle.Wheel_Drive = wheelDrive;
                    vehicle.Seats = seats;
                    vehicle.IsApproverd = false;
                    vehicle.DealerID = dealerID;

                    db.SaveChanges();
                    MessageBox.Show("Vehicle updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadVehicleData();
                }
            }
        }

        //Deleting the vehichle data using selected id from combo box.
        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVehicleID.SelectedItem != null)
            {
                int selectedID = (int)cmbVehicleID.SelectedItem;
                var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == selectedID);

                if (vehicle != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Are you sure you want to delete this Vehicle record?",
                        "Confirm deletion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        db.Vehicle.Remove(vehicle);
                        db.SaveChanges();
                        MessageBox.Show("Vehicle records deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadVehicleData();
                        LoadVehicleIDs();
                        ClearFormFields();
                    }
                }
            }
        }

        //Changing the IsAPProved field to true.
        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVehicleID.SelectedItem != null)
            {
                int selectedID = (int)cmbVehicleID.SelectedItem;
                var vehicle = db.Vehicle.FirstOrDefault(c => c.VehicleID == selectedID);

                if (vehicle != null)
                {
                    vehicle.IsApproverd = true;
                    db.SaveChanges();
                    MessageBox.Show("Vehicle application approved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtIsApproved.Content = "Approved";
                    LoadVehicleData();
                }
            }
        }

        //populating the UI fields after selection of an ID.
        private void cmbVehicleID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbVehicleID.SelectedItem != null)
            {
                int selectedID = (int)cmbVehicleID.SelectedItem;
                var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == selectedID);

                if (vehicle != null)
                {
                    txtColour.Text = vehicle.Colour.ToString();
                    txtCouncil.Text = vehicle.Council.ToString();
                    txtLicenseP.Text = vehicle.License_Plate.ToString();
                    txtMake.Text = vehicle.Make.ToString();
                    txtModel.Text = vehicle.Model.ToString();
                    txtRegistration.Text = vehicle.Registration.ToString();
                    txtSeats.Text = vehicle.Seats.ToString();
                    txtMilage.Text = vehicle.Mileage.ToString();
                    txtPrice.Text = vehicle.Price.ToString();


                    txtDealerID.Content = vehicle.DealerID.ToString();
                    txtIsApproved.Content = vehicle.IsApproverd ? "Approved" : "Not Approved";

                    if (cmbFuel.Items.Cast<ComboBoxItem>().Any(item => item.Content.ToString() == vehicle.FuelType))
                    {
                        cmbFuel.SelectedItem = cmbFuel.Items.Cast<ComboBoxItem>()
                            .First(item => item.Content.ToString() == vehicle.FuelType);
                        cmbFuel.IsEnabled = true;
                        txtAltFuel.Clear();
                    }
                    else
                    {
                        txtAltFuel.Text = vehicle.FuelType;
                    }

                    if (cmbWheelDrive.Items.Cast<ComboBoxItem>().Any(item => item.Content.ToString() == vehicle.Wheel_Drive))
                    {
                        cmbWheelDrive.SelectedItem = cmbWheelDrive.Items.Cast<ComboBoxItem>()
                            .First(item => item.Content.ToString() == vehicle.Wheel_Drive);
                    }
                }
            }
        }

        //populating the UI fields after selection of a row.
        private void VehicleData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VehicleData.SelectedItem != null)
            {
                Vehicle vehicle = (Vehicle)VehicleData.SelectedItem;

                txtColour.Text = vehicle.Colour.ToString();
                txtCouncil.Text = vehicle.Council.ToString();
                txtLicenseP.Text = vehicle.License_Plate.ToString();
                txtMake.Text = vehicle.Make.ToString();
                txtModel.Text = vehicle.Model.ToString();
                txtRegistration.Text = vehicle.Registration.ToString();
                txtSeats.Text = vehicle.Seats.ToString();
                txtMilage.Text = vehicle.Mileage.ToString();
                txtPrice.Text = vehicle.Price.ToString();


                txtDealerID.Content = vehicle.DealerID.ToString();
                txtIsApproved.Content = vehicle.IsApproverd ? "Approved" : "Not Approved";

                if (cmbFuel.Items.Cast<ComboBoxItem>().Any(item => item.Content.ToString() == vehicle.FuelType))
                {
                    cmbFuel.SelectedItem = cmbFuel.Items.Cast<ComboBoxItem>()
                        .First(item => item.Content.ToString() == vehicle.FuelType);
                    cmbFuel.IsEnabled = true;
                    txtAltFuel.Clear();
                }
                else
                {
                    txtAltFuel.Text = vehicle.FuelType;
                }

                if (cmbWheelDrive.Items.Cast<ComboBoxItem>().Any(item => item.Content.ToString() == vehicle.Wheel_Drive))
                {
                    cmbWheelDrive.SelectedItem = cmbWheelDrive.Items.Cast<ComboBoxItem>()
                        .First(item => item.Content.ToString() == vehicle.Wheel_Drive);
                }

                cmbVehicleID.SelectedItem = vehicle.VehicleID;
            }
        }
    }
}
