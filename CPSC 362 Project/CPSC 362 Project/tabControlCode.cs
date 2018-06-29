using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace CPSC_362_Project
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This cs file is mainly responsible for the backend behind the tabcontrol of all categories
        ///    (1) addItemToCart: When an item in the tab control is clicked the following occurs
        ///             1. The item is added to the Virtual Cart
        ///             2. The virtual cart's running total is incremented by the item price
        ///             3. The quantity of the item in the inventory is decremented
        ///        Requirements:
        ///             a) The quantity of the item must be >= 0
        ///    (2) bindCategories: Startup function that just binds all the items in the inventory to the individual categories
        ///    (3) getItemFromInv: Helper function that scans the invList for the selected item
        /// </summary>
        public void invList_dClick(object sender, MouseButtonEventArgs e)
        {
            addItemtoCart(searchMenu);
        }

        public void invListOther_dClick(object sender, MouseButtonEventArgs e)
        {
            addItemtoCart(invListOther);
        }

        public void invListDairy_dClick(object sender, MouseButtonEventArgs e)
        {
            addItemtoCart(invListDairy);
        }

        public void invListBeverages_dClick(object sender, MouseButtonEventArgs e)
        {
            addItemtoCart(invListBeverages);
        }

        public void invListMeat_dClick(object sender, MouseButtonEventArgs e)
        {
            addItemtoCart(invListMeats);
        }

        public void invListProduce_dClick(object sender, MouseButtonEventArgs e)
        {
            addItemtoCart(invListProduce);
        }
        public void bindCategories()
        {
            ScrollViewer.SetHorizontalScrollBarVisibility(inventoryTabs, ScrollBarVisibility.Hidden);
            invListProduce.ItemsSource = listOfProduce;
            invListBeverages.ItemsSource = listOfBeverages;
            invListOther.ItemsSource = listOfOther;
            invListDairy.ItemsSource = listOfDairy;
            invListMeats.ItemsSource = listOfMeat;
            searchMenu.ItemsSource = listOfInv;
            invList.ItemsSource = listOfInv;
            cartList.ItemsSource = listOfCartItems;
        }

        public invItem getItemFromInv(invItem selectedItem, List<invItem> selectedList)
        {
            foreach (invItem tempItem in selectedList)
            {
                if (tempItem.invId == selectedItem.invId)
                    return tempItem;
            }
            return selectedItem;
        }

        public void addItemtoCart(ListView listViewName)
        {
            CheckoutBtn.IsEnabled = true;
            foreach (invItem listItem in listViewName.SelectedItems)
            {
                invItem tempItem = new invItem();
                tempItem = getItemFromInv(listItem, listOfInv);
                if (tempItem.invQuantity == 0)
                {
                    MessageBox.Show("We are all out of " + tempItem.invName);
                }
                else
                {
                    listOfCartItems.Add(listItem);
                    CollectionViewSource.GetDefaultView(cartList.ItemsSource).Refresh();
                    runningTotal += listItem.invPrice;
                    runningTotal = Math.Round(runningTotal, 2);
                    taxedRunningTotal = taxRateMult * runningTotal;
                    taxedRunningTotal = Math.Round(taxedRunningTotal, 2);
                    totalLabel.Content = "Total: $" + runningTotal.ToString();
                    taxTotalLabel.Content = "Total+Tax: $" + taxedRunningTotal.ToString();
                    tempItem.invQuantity -= 1;
                }
            }
        }
    }
}
