using System.Windows;

namespace Car_Rental_System.Staff
{
    public partial class StaffHome : Window
    {
        public StaffHome()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

            StaffHome HomeWindow = new StaffHome();
            HomeWindow.Show();
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }

        private void CustomerIcon_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer CustomerWindow = new AddCustomer();
            CustomerWindow.Show();
            this.Close();
        }

        private void VehicleIcon_Click(object sender, RoutedEventArgs e)
        {
            AddVehicle VehicleWindow = new AddVehicle();
            VehicleWindow.Show();
            this.Close();
        }

        private void OrderIcon_Click(object sender, RoutedEventArgs e)
        {
            AddOrder OrderWindow = new AddOrder();
            OrderWindow.Show();
            this.Close();
        }

        private void DealerIcon_Click(object sender, RoutedEventArgs e)
        {
            AddDealer DealerWindow = new AddDealer();
            DealerWindow.Show();
            this.Close();
        }
    }
}
