using System.Linq;
using System.Windows;
using Car_Rental_System.Admin;
using Car_Rental_System.Staff;

namespace Car_Rental_System
{
    public partial class Login : Window
    {
        CabRentalServiceEntities db = new CabRentalServiceEntities();
        public Login()
        {
            InitializeComponent();
        }

        //Logs in the user if username and password match.
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string user = txtEmail.Text;
            string password = txtPassword.Text;

            var employee = db.Employee
                             .FirstOrDefault(emp => emp.UserName == user && emp.Password == password);

            if (employee != null)
            {
                if (employee.isApproved)
                {
                    MessageBox.Show("Successfully logged in.", "Login Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (employee.Role == "staff")
                    {
                        StaffHome staffHomeWindow = new StaffHome();
                        staffHomeWindow.Show();
                    }
                    else if (employee.Role == "manager")
                    {
                        AdminHome adminHomeWindow = new AdminHome();
                        adminHomeWindow.Show();
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Your account is not approved. Please contact the administrator.", "Account Not Approved", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Redirection link if user does not have an account.
        private void SignUpRedirect_MouseDown(object sender, RoutedEventArgs e)
        {
            Signup signupWindow = new Signup();
            signupWindow.Show();
            this.Close();
        }
    }
}
