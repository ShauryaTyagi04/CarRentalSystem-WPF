using System.Windows;

namespace Car_Rental_System.Admin
{
    public partial class AdminHome : Window
    {
        public AdminHome()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

            AdminHome HomeWindow = new AdminHome();
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
            UpdateCustomer CustomerWindow = new UpdateCustomer();
            CustomerWindow.Show();
            this.Close();
        }

        private void StaffIcon_Click(object sender, RoutedEventArgs e)
        {
            AddStaff StaffWindow = new AddStaff();
            StaffWindow.Show();
            this.Close();
        }

        private void VehicleIcon_Click(object sender, RoutedEventArgs e)
        {
            UpdateVehicle VehicleWindow = new UpdateVehicle();
            VehicleWindow.Show();
            this.Close();
        }

        private void OrderIcon_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrder OrderWindow = new UpdateOrder();
            OrderWindow.Show();
            this.Close();
        }

        private void DealerIcon_Click(object sender, RoutedEventArgs e)
        {
            UpdateDealer DealerWindow = new UpdateDealer();
            DealerWindow.Show();
            this.Close();
        }
    }
}
