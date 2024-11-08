using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Car_Rental_System
{
    public partial class Signup : Window
    {
        CabRentalServiceEntities db = new CabRentalServiceEntities();

        public Signup()
        {
            InitializeComponent();
        }
        //Redirection link if user already have an account.
        private void LoginRedirect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        //New Employee will be created.
        //The new employee won't be able to sign in, as the IsApproved field has been passed as flase.
        //A message will be visible after successfull sign up, implying that admin will have to approve the sign up.
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            string first_name = txtFName.Text;
            string last_name = txtLName.Text;
            string user = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string confirm_password = txtCPassword.Text;
            string role = cmbRole.Text;

            if (string.IsNullOrWhiteSpace(first_name) ||
                string.IsNullOrWhiteSpace(last_name) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirm_password) ||
                string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirm_password)
            {
                MessageBox.Show("Passwords do not match.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var existingEmail = db.Employee
                         .FirstOrDefault(emp => emp.EmailAddress == email);

            if (existingEmail != null)
            {
                MessageBox.Show("Email address already registered. Please use a different one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var existingUser = db.Employee
                         .FirstOrDefault(emp => emp.UserName == user);

            if (existingUser != null)
            {
                MessageBox.Show("Username already exists. Please use a different one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Employee newEmployee = new Employee()
            {
                FirstName = first_name,
                LastName = last_name,
                UserName = user,
                EmailAddress = email,
                Password = password,
                Role = role.ToLower() == "staff" ? "staff" : "manager",
                isApproved = false
            };

            db.Employee.Add(newEmployee);
            db.SaveChanges();

            MessageBox.Show("Registration successful. Contact an admin to get your profile approved", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);

            Login loginWindow = new Login();
            loginWindow.Show();

            this.Close();
        }

    }
}
