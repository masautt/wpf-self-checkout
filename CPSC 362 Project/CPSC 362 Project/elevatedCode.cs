using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
namespace CPSC_362_Project
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This cs file is mainly responsible for all functions of elevated permissions
        ///    (1) activateElevated:        OnKeyDown Event that the employee/manager uses to use the elevated permissions
        ///             1. Ctrl+I: Login Panel becomes visible
        ///             2. Ctrl+O: Remove's all elevated permissions
        ///             3. Return: Calls the login button event or addnewItem button event (not necessary but nice)
        ///    (2) loginButton_Click:        Logs the employee/manager into the system and activates certain events
        ///             1. Employee: If the user correctly enters the employee's username and password then the employee bool becomes true and allows the user to delete cart items
        ///             2. Manager:  If the user correctly enters the manager's username and password then the inventory panel appears along with the addItemPanel
        ///          Requirements:
        ///             (a) The username and password for each must be correct
        ///             (b) The boxes must be filled in in order for the button to be pressed
        ///    (3) addItemBtn_Click: Button event that basically adds the item to the inventory
        ///             1. Checks to make sure all dialog boxes are filled in
        ///             2. Creates a new InvItem object
        ///             3. Checks for no duplicates of the object already in the database
        ///             4. Adds the Item to the Inventory
        ///             5. Visually updates all of the inventory
        ///          Requirements:
        ///             (a) All dialog boxes must be filled in
        ///             (b) The name of the new product must not equal the name of an existing product
        ///             (c) The ID of the new product must not equal the ID of an existing product
        ///     (4) checkForDuplicate:      Helper function that makes sure the id or name does not exist in an existing item
        ///     (5) helpBtn_Click:          Basic button event that alters the user an employee is on their way
        ///     (6) loginExitBtn_Click/addITemExtBtn_Click/invListExit_Click:   Basic close button for each panel
        ///     (7) invListRemove_dClick:   Function that removes an item from the total inventory
        ///             1. Checks to make sure the cart is empty in case the item removed is already in the cart
        ///             2. Prompts the user with a Yes/No Dialgo box to make sure the user is sure they want to remove the item
        ///             3. Removes the item from the inventory
        ///             4. Updates the inventory panel and the tab control
        ///         Requriements
        ///             (a) The cart must be empty
        ///             (b) The manager must select yes in the dialog box
        /// </summary>
        public void activateElevated(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))   //At any time, a manager or employee is able to access the login menu but pressing Ctrl I
            {
                cartList.Background = Brushes.White;
                searchPanel.Visibility = Visibility.Hidden;
                loginPanel.Visibility = Visibility.Visible;
                FocusManager.SetFocusedElement(this, usernameTextBox);
                Keyboard.Focus(usernameTextBox);
            }

            if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))     //At any time, a manager or employee is able to access the login menu but pressing Ctrl I
            {
                employee = false;
                helpBtn.Content = "HELP";
                updateInventory();
            }
            if (e.Key == Key.Return)
            {
                if (loginPanel.Visibility == Visibility.Visible) loginButton_Click(sender, e);
                else if (newItemPanel.Visibility == Visibility.Visible) addItemBtn_Click(sender, e);

            }
        }

        public void loginButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (usernameTextBox.Text == "" || passwordTextBox.Password.Length == 0)
            {
                MessageBox.Show("Please Provide Log In Details", "NOTICE!");
            }
            else
            {
                
                if (usernameTextBox.Text == "employee" && passwordTextBox.Password == "iamemployee")
                {
                    loginPanel.Visibility = Visibility.Hidden;
                    employee = true;
                    usernameTextBox.Text = "";
                    passwordTextBox.Password = "";
                    helpBtn.Content = "Employee";
                }
                else if (usernameTextBox.Text == "manager" && passwordTextBox.Password == "iammanager")
                {
                    
                    invLabel.Content = "Inventory as of " + today.ToString();
                    loginPanel.Visibility = Visibility.Hidden;
                    invPanel.Visibility = Visibility.Visible;
                    
                    newItemPanel.Visibility = Visibility.Visible;
                    employee = true;
                    usernameTextBox.Text = "";
                    passwordTextBox.Password = "";
                    searchBox.Text = "";
                    helpBtn.Content = "Manager";
                }
                else
                {
                    MessageBox.Show("Username or Password is Incorrect");
                }
            }
        }

        public void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (newItemNameTextBox.Text == "" || newItemPriceTextBox.Text == "" || newItemQuantityTextBox.Text == "" || newItemIdTextBox.Text == "" || newItemTypeComboBox.SelectedIndex <0)
            {
                MessageBox.Show("One or more boxes are left unfilled!");
            }
            else
            {
                string tempType = newItemTypeComboBox.SelectedValue.ToString();
                tempType = tempType.Replace("System.Windows.Controls.ComboBoxItem: ", "");
                invItem tempItem = new invItem();
                tempItem.invName = newItemNameTextBox.Text;
                tempItem.invId = Convert.ToInt32(newItemIdTextBox.Text);
                tempItem.invQuantity = Convert.ToInt32(newItemQuantityTextBox.Text);
                tempItem.invType = tempType;
                tempItem.invPrice = Convert.ToDouble(newItemPriceTextBox.Text);
                tempItem.invStrPrice = "$" + tempItem.invPrice.ToString();
                if (checkForDuplicate(tempItem, listOfInv) == true)
                {
                    MessageBox.Show("ItemName and/or Id Already Used");
                }
                else
                {
                    MessageBox.Show(tempItem.invName + " has been added to the inventory with the id " + tempItem.invId.ToString());
                    listOfInv.Add(tempItem);
                    listOfInv = listOfInv.OrderBy(invItem=>invItem.invId).ToList();
                    CollectionViewSource.GetDefaultView(invList.ItemsSource).Refresh();
                    clearAddItemPanel();
                }
            }
        }

        public bool checkForDuplicate(invItem tempItem, List<invItem> tempList)
        {
            foreach(invItem listItem in tempList)
            {
                if (tempItem.invId == listItem.invId) return true;
                if (tempItem.invName == listItem.invName) return true;
            }
            return false;
        }

        private void helpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!employee) MessageBox.Show("Press Okay To Cancel", "An Employee Has Been Alerted");
        }

        private void loginExitBtn_Click(object sender, RoutedEventArgs e)
        {
            loginPanel.Visibility = Visibility.Hidden;
            usernameTextBox.Text = "";
            passwordTextBox.Password = "";
        }

        private void addItemExtBtn_Click(object sender, RoutedEventArgs e)
        {
            newItemPanel.Visibility = Visibility.Hidden;
            clearAddItemPanel();
        }

        private void invListExit_Click(object sender, RoutedEventArgs e)
        {
            invPanel.Visibility = Visibility.Hidden;
        }

        private void invListRemove_dClick(object sender, RoutedEventArgs e)
        {
            if (listOfCartItems.Count >= 1) MessageBox.Show("The cart must be empty before deleting an item from the inventory");
            else
            {
                int index = invList.Items.IndexOf(invList.SelectedItem);

                MessageBoxResult dr = MessageBox.Show("Are you sure you want to remove " + listOfInv[index].invName, "WARNING!", MessageBoxButton.YesNo);
                if (dr == MessageBoxResult.Yes)
                {

                    invList.ItemsSource = null;
                    listOfInv.Remove(listOfInv[index]);
                    invList.ItemsSource = listOfInv;
                    CollectionViewSource.GetDefaultView(invList.ItemsSource).Refresh();
                }
            }
        }

        public void clearAddItemPanel()
        {
            newItemNameTextBox.Text = "";
            newItemIdTextBox.Text = "";
            newItemPriceTextBox.Text = "";
            newItemQuantityTextBox.Text = "";
            newItemTypeComboBox.SelectedIndex = -1;
        }
    }
}
