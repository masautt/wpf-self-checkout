using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
namespace wpf-self-checkout
{
   // <summary>
   // Interaction logic for MainWindow.xaml
   // </summary>
    public partial class MainWindow : Window
    {
        public List<invItem> listOfInv = new List<invItem>();
        public List<invItem> listOfProduce = new List<invItem>();
        public List<invItem> listOfMeat = new List<invItem>();
        public List<invItem> listOfBeverages = new List<invItem>();
        public List<invItem> listOfDairy = new List<invItem>();
        public List<invItem> listOfOther = new List<invItem>();
        public List<invItem> listOfCartItems = new List<invItem>();
        public DirectoryInfo currDir = new DirectoryInfo(".");
        public bool employee;
        public string currDirStr = "";
        public DateTime today;
        public string todayStr = "";
        public double cartTotal = 0.0;
        public string comment = "";
        public const double taxRate = 0.09;
        public const double taxRateMult = 1.09;
        public double tax = 0.00;
        public double newTotal = 0.00;
        public string invFileName = "Inventory.txt";
        public double runningTotal = 0.00;
        public double taxedRunningTotal = 0.00;

        public MainWindow()
        {
            InitializeComponent();                  //Necessary Windows start function for XML
            getInventory(invFileName);              //Takes the name of the file and gathers all the data for the inventory
            setLocations();                         //Setting the locations after starting the program allows me to visualize each control on the designer without any overlap
            getCategories();                        //Loops through the new list of inventory and adds them to their categories
            printCategories();                      //Prints the inventory for the admin to view on the Console
            bindCategories();                       //Binds the category lists to the left list view in XML
            showStartMenu();                        //Shows the start menu
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(searchMenu.ItemsSource);      //Needed for the search menu
            view.Filter = UserFilter;                                                                               //Needed for the search menu
        }
        public void setLocations()
        {
            Canvas.SetLeft(inventoryTabs, 30);
            Canvas.SetTop(inventoryTabs, 200);
            Canvas.SetLeft(searchPanel, 550);
            Canvas.SetTop(searchPanel, 200);
            Canvas.SetTop(loginPanel, 346);
            Canvas.SetLeft(loginPanel, 628);
            Canvas.SetTop(cartPanel, 200);
            Canvas.SetLeft(cartPanel, 1070);
            Canvas.SetLeft(searchBtn, 491);
            Canvas.SetTop(searchBtn, 202);
            Canvas.SetLeft(invPanel, 30);
            Canvas.SetTop(invPanel, 200);
            Canvas.SetTop(newItemPanel, 200);
            Canvas.SetLeft(newItemPanel, 1070);

        }

        private void showStartMenu()
        {
            helpBtn.Visibility = Visibility.Hidden;
            inventoryTabs.Visibility = Visibility.Hidden;
            cartPanel.Visibility = Visibility.Hidden;
            CheckoutBtn.Visibility = Visibility.Hidden;
            startTxt.Visibility = Visibility.Visible;
            greenBorder.Visibility = Visibility.Visible;
            searchBtn.Visibility = Visibility.Hidden;
            searchPanel.Visibility = Visibility.Hidden;
            inventoryTabs.Visibility = Visibility.Hidden;
            invPanel.Visibility = Visibility.Hidden;
            loginPanel.Visibility = Visibility.Hidden;
            newItemPanel.Visibility = Visibility.Hidden;
            CheckoutBtn.IsEnabled = false;
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            showCheckout();
        }

        private void showCheckout()
        {
            printCategories();
            startTxt.Visibility = Visibility.Hidden;
            greenBorder.Visibility = Visibility.Hidden;
            CheckoutBtn.Visibility = Visibility.Visible;
            cartPanel.Visibility = Visibility.Visible;
            inventoryTabs.Visibility = Visibility.Visible;
            helpBtn.Visibility = Visibility.Visible;
            searchBtn.Visibility = Visibility.Visible;
            totalLabel.Content = "Total: $" + runningTotal.ToString();
            taxTotalLabel.Content = "Total+Tax: $" + taxedRunningTotal.ToString();
        }
    }
}
