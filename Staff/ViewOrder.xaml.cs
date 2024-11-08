using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Car_Rental_System.Staff
{
    public partial class ViewOrder : Window
    {
        private CabRentalServiceEntities db = new CabRentalServiceEntities();
        public ViewOrder()
        {
            InitializeComponent();
            LoadIDs();
            LoadCustomerData();
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

        //Redirection to home page.
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            StaffHome HomeWindow = new StaffHome();
            HomeWindow.Show();
            this.Close();
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
                    txtName.Content = $"{customer.FirstName} {customer.LastName}";
                    txtNumber.Content = customer.ContactNumber.ToString();

                    var orders = db.Orders
                                    .Where(o => o.CustomerID == selectedID)
                                    .Select(o => new
                                    {
                                        o.OrderID,
                                        o.RentDate,
                                        o.ReturnDate,
                                        o.TotalRent,
                                        o.Deposit,
                                        o.DriverLicence,
                                        o.Insurance
                                    })
                                    .ToList();

                    var orderIDs = orders.Select(o => o.OrderID).ToList();
                    cmbOrderID.ItemsSource = orderIDs;

                    OrderData.ItemsSource = orders;

                }
            }
        }

        //populating the UI fields after selection of an order ID.
        //vehicle ID is being fethced from orders table to populate data from the respective table.
        private void cmbOrderID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomerID.SelectedItem != null)
            {
                int selectedCustomerID = (int)cmbCustomerID.SelectedItem;
                int selectedOrderID = (int)cmbOrderID.SelectedItem;
                var order = db.Orders.FirstOrDefault(o => o.OrderID == selectedOrderID);
              
                if (order != null) 
                {
                    int vehicleID = order.VehicleID;
                    var vehicle = db.Vehicle.FirstOrDefault(v => v.VehicleID == vehicleID);

                    txtColour.Content = vehicle.Colour.ToString();
                    txtLicenseP.Content = vehicle.License_Plate.ToString();
                    txtMake.Content = vehicle.Make.ToString();
                    txtModel.Content = vehicle.Model.ToString();
                    txtRegistration.Content = vehicle.Registration.ToString();
                    txtSeats.Content = vehicle.Seats.ToString();
                    txtFuel.Content = vehicle.FuelType.ToString(); 

                    txtLicenseD.Content = order.DriverLicence.ToString();
                    txtSDate.Content = order.RentDate.ToString("dd-MM-yyyy");
                    txtEDate.Content = order.ReturnDate.ToString("dd-MM-yyyy");
                    txtDeposit.Content = order.Deposit.ToString();
                    txtTotal.Content = order.TotalRent.ToString();
                    checkboxInsurance.IsChecked = order.Insurance;
                }
            }
        }

        private void CustomerData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void OrderData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //Redirection to login page.
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }
    }
}
