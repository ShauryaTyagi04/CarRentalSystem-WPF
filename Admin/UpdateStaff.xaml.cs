using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Car_Rental_System.Admin
{
    public partial class UpdateStaff : Window
    {
        CabRentalServiceEntities db = new CabRentalServiceEntities();

        public UpdateStaff()
        {
            InitializeComponent();
            LoadEmployeeIDs();
            LoadRoles();
            LoadEmployeeData();
        }

        //Loading data from employee table into data grid.
        private void LoadEmployeeData()
        {
            var employees = db.Employee.ToList();
            EmployeeData.ItemsSource = employees;
        }

        //Loading employee IDs into combo box.
        private void LoadEmployeeIDs()
        {
            var employeeIDs = db.Employee.Select(emp => emp.EmployeeID).ToList();
            Employee_ID.ItemsSource = employeeIDs;
        }
        //Loading roles into combo box.
        private void LoadRoles()
        {
            cmbRole.Items.Add("staff");
            cmbRole.Items.Add("manager");
        }

        //populating the UI fields after selection of an ID.
        private void ID_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Employee_ID.SelectedItem != null)
            {
                int selectedID = (int)Employee_ID.SelectedItem;
                var employee = db.Employee.FirstOrDefault(emp => emp.EmployeeID == selectedID);

                if (employee != null)
                {
                    txtFName.Text = employee.FirstName;
                    txtLName.Text = employee.LastName;
                    txtEmail.Text = employee.EmailAddress;
                    txtPassword.Text = employee.Password;
                    txtUsername.Text = employee.UserName;
                    cmbRole.SelectedItem = employee.Role;
                    txtIsApproved.Content = employee.isApproved ? "Approved" : "Not Approved";
                }
            }

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

        //Changing the IsAPProved field to true.
        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Employee_ID.SelectedItem != null)
            {
                int selectedID = (int)Employee_ID.SelectedItem;
                var employee = db.Employee.FirstOrDefault(emp => emp.EmployeeID == selectedID);

                if (employee != null)
                {
                    employee.isApproved = true;
                    db.SaveChanges();
                    MessageBox.Show("Employee application approved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtIsApproved.Content = "Approved";
                    LoadEmployeeData();
                }
            }
        }

        // Updating the staff details using selected id from combo box.
        //Submission only possible after all validation checks are completed.
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Employee_ID.SelectedItem != null)
            {
                int selectedID = (int)Employee_ID.SelectedItem;
                var employee = db.Employee.FirstOrDefault(emp => emp.EmployeeID == selectedID);

                if (employee != null)
                {
                    employee.FirstName = txtFName.Text;
                    employee.LastName = txtLName.Text;
                    employee.EmailAddress = txtEmail.Text;
                    employee.UserName = txtUsername.Text;
                    employee.Role = cmbRole.Text;
                    employee.isApproved = true;

                    db.SaveChanges();
                    MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadEmployeeData();
                }
            }
        }

        //Deleting the staff data using selected id from combo box.
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (Employee_ID.SelectedItem != null)
            {
                int selectedID = (int)Employee_ID.SelectedItem;
                var employee = db.Employee.FirstOrDefault(emp => emp.EmployeeID == selectedID);

                if (employee != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Are you sure you want to reject this employee application?",
                        "Confirm Rejection",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        db.Employee.Remove(employee);
                        db.SaveChanges();
                        MessageBox.Show("Employee application rejected successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadEmployeeIDs();
                        LoadEmployeeData();
                        ClearFields();
                    }
                }
            }
        }

        //Method to clear all the fields, will be used after form submission.
        private void ClearFields()
        {
            txtFName.Text = string.Empty;
            txtLName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUsername.Text = string.Empty;
            cmbRole.SelectedIndex = -1;
            Employee_ID.SelectedIndex = -1;
        }

        //populating the UI fields after selection of a row.
        private void EmployeeData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeData.SelectedItem != null)
            {
                Employee selectedEmployee = (Employee)EmployeeData.SelectedItem;

                Employee_ID.SelectedItem = selectedEmployee.EmployeeID;
                txtFName.Text = selectedEmployee.FirstName;
                txtLName.Text = selectedEmployee.LastName;
                txtEmail.Text = selectedEmployee.EmailAddress;
                txtPassword.Text = selectedEmployee.Password;
                txtUsername.Text = selectedEmployee.UserName;
                cmbRole.SelectedItem = selectedEmployee.Role;
                txtIsApproved.Content = selectedEmployee.isApproved ? "Approved" : "Not Approved";
            }
        }
    }
}
