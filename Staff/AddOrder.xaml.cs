using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Car_Rental_System.Staff
{
    public partial class AddOrder : Window
    {
        private CabRentalServiceEntities db = new CabRentalServiceEntities();
        public AddOrder()
        {
            InitializeComponent();
            LoadIDs();
            UpdateVehicleCount();
        }

        //Loading unique customer IDs with atleast one order into combo box.
        private void LoadIDs()
        {
            var customerIDs = db.Customer.Select(o => o.CustomerID).ToList();
            cmbCustomerID.ItemsSource = customerIDs;

            var vehicleIDs = db.Vehicle
                    .Where(o => o.IsAvailable == true && o.IsApproverd == true)
                    .Select(o => o.VehicleID)
                    .ToList();

            cmbVehicleID.ItemsSource = vehicleIDs;
        }

        //Vehcile count is being updated if IsAvailable field is true.
        private void UpdateVehicleCount()
        {
            int availableVehicleCount = db.Vehicle.Count(v => v.IsAvailable == true);

            txtNumVehicles.Content = availableVehicleCount.ToString();
        }

        //All fields are being cleared after submission.
        private void ClearFormFields()
        {
            txtColour.Text = string.Empty;
            txtMake.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtRegistration.Text = string.Empty;
            txtFuel.Text = string.Empty;
            txtLicense_P.Text = string.Empty;
            txtFName.Text = string.Empty;
            txtLName.Text = string.Empty;
            txtLicense_D.Text = string.Empty;
            txtSDate.Text = string.Empty;
            txtEdate.Text = string.Empty;

            checkboxInsurance.IsChecked = false;

            cmbCustomerID.SelectedIndex = -1;
            cmbVehicleID.SelectedIndex = -1;
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

        //Redirection to view order page.
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {

            ViewOrder ViewWindow = new ViewOrder();
            ViewWindow.Show();
            this.Close();
        }

        //populating the UI fields after selection of a customer ID.
        private void cmbCustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomerID.SelectedItem != null)
            {
                int selectedID = (int)cmbCustomerID.SelectedItem;
                var customer = db.Customer.FirstOrDefault(c => c.CustomerID== selectedID);

                if (customer != null)
                {
                    txtFName.Text = customer.FirstName.ToString();
                    txtLName.Text = customer.LastName.ToString();
                }
            }
        }

        //populating the UI fields after selection of a vehicle ID.
        private void cmbVehicleID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbVehicleID.SelectedItem != null)
            {
                int selectedID = (int)cmbVehicleID.SelectedItem;
                var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == selectedID);

                if (vehicle != null)
                {
                    txtColour.Text = vehicle.Colour.ToString();
                    txtLicense_P.Text = vehicle.License_Plate.ToString();
                    txtMake.Text = vehicle.Make.ToString();
                    txtModel.Text = vehicle.Model.ToString();
                    txtRegistration.Text = vehicle.Registration.ToString();
                    txtFuel.Text = vehicle.FuelType.ToString();
                }
            }
        }

        //Deposit is being calculated after reffering to price and make year of a vehicle.
        private double CalculateDeposit(double price, int makeYear)
        {
            double baseDepositPercentage = 0.10;
            double deposit = price * baseDepositPercentage;

            int currentYear = DateTime.Now.Year;

            int vehicleAge = currentYear - makeYear;

            if (vehicleAge < 5)
            {
                deposit += price * 0.05;
            }
            else if (vehicleAge > 10)
            {
                deposit -= price * 0.05;
            }

            return Math.Round(deposit, 2);
        }

        //Total rent is being calculated after reffering to rent date, return date, vehicle price, make year and insurance field.
        //depriciation factor has been used.
        //Insurance per day has been set to 15 pounds.
        //20 percent VAT has been added to the final rent.
        public double CalculateTotalRent(DateTime startDate, DateTime endDate, double vehiclePrice, int makeYear, bool hasInsurance)
        {
            TimeSpan rentalDuration = endDate - startDate;
            int numberOfDays = rentalDuration.Days;
            int currentYear = DateTime.Now.Year;
            int vehicleAge = currentYear - makeYear;

            double depreciationFactor = 0.01 * vehicleAge;

            double dailyRentalRate = (vehiclePrice * (1 - depreciationFactor)) / 52;

            double baseRentalCost = dailyRentalRate * numberOfDays;

            double insuranceCost = 0;
            if (hasInsurance)
            {
                double insuranceRatePerDay = 15.0;
                insuranceCost = insuranceRatePerDay * numberOfDays;
            }

            double subTotal = baseRentalCost + insuranceCost;
            double vat = subTotal * 0.20;

            double totalRent = subTotal + vat;

            return Math.Round(totalRent, 2); 
        }

        //creating a new order using selected id from combo box.
        //Submission only possible after all validation checks are completed.
        //Vheicle and customer details cannot be changed and wont be saved after submission.
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCustomerID.SelectedItem == null)
            {
                MessageBox.Show("Please select a Csutomer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbVehicleID.SelectedItem == null)
            {
                MessageBox.Show("Please select a Vehicle.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLicense_D.Text))
            { 
                MessageBox.Show("Please fill in all the required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime startDate;
            if (!DateTime.TryParseExact(txtSDate.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out startDate))
            {
                MessageBox.Show("Start Date must be in DD-MM-YYYY format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (startDate <= DateTime.Now)
            {
                MessageBox.Show("Booking date should be a future date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime endDate;
            if (!DateTime.TryParseExact(txtEdate.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out endDate))
            {
                MessageBox.Show("Return date must be in DD-MM-YYYY format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (endDate < startDate)
            {
                MessageBox.Show("Return date must be greater than booking date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectedVehicleID = (int)cmbVehicleID.SelectedItem;

            var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == selectedVehicleID);

            double price = vehicle.Price; 
            int makeYear = vehicle.Make;
            double vehiclePrice = vehicle.Price;
            bool hasInsurance = checkboxInsurance.IsChecked ?? false;

            double deposit = CalculateDeposit(price, makeYear);

            double totalRent = CalculateTotalRent(startDate, endDate, vehiclePrice, makeYear, hasInsurance);

            int selectedCustomerID = (int)cmbCustomerID.SelectedItem;

            var newOrder = new Orders
            {
                DriverLicence = txtLicense_D.Text,
                RentDate = startDate,
                ReturnDate = endDate,
                Deposit = deposit,
                TotalRent = totalRent,
                Insurance = checkboxInsurance.IsChecked ?? false,
                CustomerID = selectedCustomerID,
                VehicleID = selectedVehicleID
            };

            db.Orders.Add(newOrder);

            vehicle.IsAvailable = false;
            
            db.SaveChanges();

            MessageBox.Show("Rent Order created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateVehicleCount();

            LoadIDs();

            ClearFormFields();

        }
    }
}
