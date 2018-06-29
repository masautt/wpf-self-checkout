using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
namespace CPSC_362_Project
{
    /// <summary>
    /// This cs file is the backend for the cart. It handles the 2 events associated with the cart:
    ///     invListCart_dClick: When clicked the cart does 4 major things
    ///             1. Remove's the item visually from the cart
    ///             2. Updates the running total and taxed running total
    ///             3. Updates the quantity of the item that was added
    ///    Requirements:
    ///             a) An employee or manager must be logged in
    ///             b) Am 
    ///     CheckoutBtn_Click: When clicked the cart does 4 major things
    ///             1. Prints the Receipt in the Console
    ///             2. Updates the Inventory
    ///             3. Clears the cart
    ///             4. Resets the menu for the next customer
    ///     Requirements:
    ///             a) An employee or manager must be logged in
    ///             b) The cart must have at least one Item
    /// </summary>
    public partial class MainWindow : Window
    {
        public void CheckoutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (employee) MessageBox.Show("Press CTRL+O to Disble Elevated Permissions","You need to log out of evevated permissions before checkout!" );
            else if (!listOfCartItems.Any<invItem>()) MessageBox.Show("Double Click and Item To Add to the Cart", "You need to add items to your account before you checkout!");
            else
            {
                MessageBox.Show("Printing Receipt");        //Let the user know their receipt is being printed
                printList(listOfCartItems, true);           //Print the reciept
                updateInventory();                          //Update the inventory with new values
                listOfCartItems.Clear();                    //Clear out the cart
                showStartMenu();                            //Return to start menu
                bindCategories();
            }
        }

        public void invListCart_dClick(object sender, MouseButtonEventArgs e)
        {
            if (employee == true)
            {
                foreach (invItem tempItem in cartList.SelectedItems)
                {
                    runningTotal -= tempItem.invPrice;
                    totalLabel.Content = "Total: $" + runningTotal.ToString();
                    taxTotalLabel.Content = "Total+Tax: $" + Math.Round(taxRateMult * runningTotal, 2);
                    invItem listItem = new invItem();
                    listItem = getItemFromInv(tempItem, listOfInv);
                    listItem.invQuantity += 1;
                }
                

                int index = cartList.Items.IndexOf(cartList.SelectedItem);
                cartList.ItemsSource = null;
                listOfCartItems.Remove(listOfCartItems[index]);
                cartList.ItemsSource = listOfCartItems;
                
            }
            else
            {
                MessageBox.Show("You need elevated access to remove an item!");
            }
            if (listOfCartItems.Count == 0)
            {
                CheckoutBtn.IsEnabled = false;
            }

        }

    }
}
