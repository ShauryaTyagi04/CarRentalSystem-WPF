using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Car_Rental_System.Admin
{
    public partial class UpdateOrder : Window
    {
        private CabRentalServiceEntities db = new CabRentalServiceEntities();
        public UpdateOrder()
        {
            InitializeComponent();
            LoadIDs();
            LoadCustomerData();
            UpdateVehicleCount();
        }

        //Loading unique customer IDs with atleast one order into combo box.
        private void LoadIDs()
        {
            var customerIDsInOrders = db.Orders
                                .Select(o => o.CustomerID)
                                .Distinct()
                                .ToList();
            cmbCustomerID.ItemsSource = customerIDsInOrders;
        }

        //Loading data from customer table into data grid.
        //Customer ID is being fetched fromorder table before fetching the data.
        private void LoadCustomerData()
        {
            var customersWithOrders = db.Customer
                                        .Where(c => db.Orders.Any(o => o.CustomerID == c.CustomerID))
                                        .ToList();

            CustomerData.ItemsSource = customersWithOrders;

            var customerColumn = CustomerData.Columns.FirstOrDefault(c => c.Header.ToString() == "Customer");
            if (customerColumn != null)
            {
                CustomerData.Columns.Remove(customerColumn);
            }
        }

        //Vehicle count is being update if IsAvailable field is true.
        private void UpdateVehicleCount()
        {
            int availableVehicleCount = db.Vehicle.Count(v => v.IsAvailable == true);

            txtNumVehicles.Content = availableVehicleCount.ToString();
        }

        //Method to clear all the fields, will be used after form submission.
        private void ClearFormFields()
        {
            txtColour.Text = string.Empty;
            txtMake.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtRegistration.Text = string.Empty;
            txtFuel.Text = string.Empty;
            txtLicense_P.Text = string.Empty;
            txtName.Text = string.Empty;
            txtLicense_D.Text = string.Empty;
            txtSDate.Text = string.Empty;
            txtEdate.Text = string.Empty;

            checkboxInsurance.IsChecked = false;

            cmbCustomerID.SelectedIndex = -1;
            cmbOrderID.SelectedIndex = -1;
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

        //Redirection to home page.
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            AdminHome HomeWindow = new AdminHome();
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

        //Updating the order details using selected id from combo box.
        //Submission only possible after all validation checks are completed.
        //Vheicle and customer details cannot be changed and wont be saved after submission.
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCustomerID.SelectedItem == null)
            {
                MessageBox.Show("Please select a Csutomer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbOrderID.SelectedItem == null)
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

            int selectedOrderID = (int)cmbOrderID.SelectedItem;
            var order = db.Orders.FirstOrDefault(o => o.OrderID == selectedOrderID);

            int selectedVehicleID = order.VehicleID;
            var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == selectedVehicleID);

            double price = vehicle.Price;
            int makeYear = vehicle.Make;
            double vehiclePrice = vehicle.Price;
            bool hasInsurance = checkboxInsurance.IsChecked ?? false;

            double deposit = CalculateDeposit(price, makeYear);

            double totalRent = CalculateTotalRent(startDate, endDate, vehiclePrice, makeYear, hasInsurance);

            int selectedCustomerID = (int)cmbCustomerID.SelectedItem;

            if (cmbOrderID.SelectedItem != null)
            {

                if (order != null)
                {
                    order.DriverLicence = txtLicense_D.Text;
                    order.RentDate = startDate;
                    order.ReturnDate = endDate;
                    order.Deposit = deposit;
                    order.TotalRent = totalRent;
                    order.Insurance = checkboxInsurance.IsChecked ?? false;
                    order.CustomerID = selectedCustomerID;
                    order.VehicleID = selectedVehicleID;

                    db.SaveChanges();
                    MessageBox.Show("Order updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCustomerData();
                    UpdateVehicleCount();
                }
            }
        }

        //Deleting the order data using selected id from combo box.
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOrderID.SelectedItem != null)
            {
                int selectedID = (int)cmbOrderID.SelectedItem;
                var order = db.Orders.FirstOrDefault(o => o.OrderID == selectedID);

                int selectedVehicleID = order.VehicleID;
                var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == selectedVehicleID);

                if (order != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Are you sure you want to delete this Rental Order record?",
                        "Confirm deletion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        vehicle.IsAvailable = true;
                        db.Orders.Remove(order);
                        db.SaveChanges();
                        MessageBox.Show("Rental Order records deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateVehicleCount();
                        LoadCustomerData();
                        LoadIDs();
                        ClearFormFields();
                    }
                }
            }
        }

        //populating the UI fields after selection of a customer ID.
        //Order IDs are being populated in the combo box.
        private void cmbCustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomerID.SelectedItem != null)
            {
                int selectedID = (int)cmbCustomerID.SelectedItem;
                var customer = db.Customer.FirstOrDefault(o => o.CustomerID == selectedID);
                if (customer != null)
                {
                    txtName.Text = $"{customer.FirstName} {customer.LastName}";

                    var orders = db.Orders
                                    .Where(o => o.CustomerID == selectedID)
                                    .Select(o => new
                                    {
                                        o.OrderID,
                                        o.RentDate,
                                        o.ReturnDate,
                                        o.TotalRent,
                                        o.Deposit,
                                        o.DriverLicence
                                    })
                                    .ToList();

                    var orderIDs = orders.Select(o => o.OrderID).ToList();
                    cmbOrderID.ItemsSource = orderIDs;

                    OrderData.ItemsSource = orders;

                    cmbOrderID.SelectedIndex = -1;
                }
                else
                {
                    cmbOrderID.ItemsSource = null;
                }
            }
        }

        //populating the UI fields after selection of an order ID.
        //vehicle ID is being fethced from orders table to populate data from the respective table.
        private void cmbOrderID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomerID.SelectedItem != null)
            {
                int selectedOrderID = (int)cmbOrderID.SelectedItem;
                var order = db.Orders.FirstOrDefault(o => o.OrderID == selectedOrderID);

                if (order != null)
                {
                    int vehicleID = order.VehicleID;
                    var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == vehicleID);

                    txtColour.Text = vehicle.Colour.ToString();
                    txtLicense_P.Text = vehicle.License_Plate.ToString();
                    txtMake.Text = vehicle.Make.ToString();
                    txtModel.Text = vehicle.Model.ToString();
                    txtRegistration.Text = vehicle.Registration.ToString();
                    txtFuel.Text = vehicle.FuelType.ToString();

                    txtLicense_D.Text = order.DriverLicence.ToString();
                    txtSDate.Text = order.RentDate.ToString("dd-MM-yyyy");
                    txtEdate.Text = order.ReturnDate.ToString("dd-MM-yyyy");
                    checkboxInsurance.IsChecked = order.Insurance;
                }
            }
        }

    }
}
