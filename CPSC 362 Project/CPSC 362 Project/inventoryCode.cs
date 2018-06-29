using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
namespace CPSC_362_Project
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This cs file is mainly responsible for the backend behind the implementation of the inventory
        ///    (1) getInventory:        Main function that is responsible for building the base inventory
        ///         1. Open up the inventory file in the debug folder
        ///         2. Parse through the data
        ///         3. Create and invItem that gets added to the total list of Inventory
        ///    (2) getCategories:       helper functon that is responsible for building the subcategory lists
        ///         1.  Clears out the previous lists just in case
        ///         2.  Scans through the listOfInv and makes a copy of each item and determines which categorical list to put it into
        ///    (3) printCategories:     Helper function that calls printList for all the categories
        ///    (4) printList:           Main function that prints the entire contents of a categoryList into the Console
        ///    (5) updateInventory:     Called after every checkout, this main function allows the inventory to be ready for the next checkout
        ///         1.  Creates a new file of the inventory and sets it as the default for the next update to the inventory
        ///         2.  Gets the current date and adds it to the file
        /// </summary>
        public void getInventory(string filename)
        {
            listOfInv.Clear();                                                  
            string line;                                                        
            StreamReader file = new StreamReader(currDir + "/" + filename);     
            while ((line = file.ReadLine()) != null)                            
            {
                string[] words = line.Split(',');                               
                invItem tempItem = new invItem();                               
                tempItem.invName = words[0];                                    
                tempItem.invType = words[1];                                    
                tempItem.invId = Convert.ToInt32(words[2]);                     
                tempItem.invQuantity = Convert.ToInt32(words[3]);               
                tempItem.invPrice = Convert.ToDouble(words[4]);                 
                tempItem.invStrPrice = "$" + Convert.ToString(words[4]);        
                listOfInv.Add(tempItem);                                        
            }
        }

        public void getCategories()
        {
            listOfBeverages.Clear();                            
            listOfDairy.Clear();
            listOfMeat.Clear();
            listOfProduce.Clear();
            listOfOther.Clear();
            foreach (invItem tempItem in listOfInv)
            {
                switch (tempItem.invType)                       
                {
                    case "PRODUCE":
                        listOfProduce.Add(tempItem);
                        break;
                    case "DAIRY":
                        listOfDairy.Add(tempItem);
                        break;
                    case "BEVERAGES":
                        listOfBeverages.Add(tempItem);
                        break;
                    case "OTHER":
                        listOfOther.Add(tempItem);
                        break;
                    case "MEAT":
                        listOfMeat.Add(tempItem);
                        break;
                }
            }
        }

        public void printCategories()         
        {
            printList(listOfDairy, false);
            printList(listOfMeat, false);
            printList(listOfProduce, false);
            printList(listOfBeverages, false);
            printList(listOfOther, false);
        }

        public void printList(List<invItem> myList, bool isCart)
        {
            today = DateTime.Now;              
            cartTotal = 0.0;                   
            tax = 0.00;                         
            newTotal = 0.00;                    
            if (!isCart)                                                                                            
            {
                comment = "";                                                                                       
                Console.WriteLine("\t|------------------------------------------------------------------|");
                Console.WriteLine("\t|{0, -24} {1,40} |", myList[0].invType, today.ToString());                     
                Console.WriteLine("\t|------------------------------------------------------------------|");
                Console.WriteLine("\t|  ID  |        PRODUCT       |TYPE|  #  | PRICE |     COMMENTS    |");
                Console.WriteLine("\t|------------------------------------------------------------------|");
                foreach (invItem item in myList)
                {
                    if (item.invQuantity <= 10) comment = "Quantity   Low!";
                    if (item.invQuantity >= 100) comment = "Quantity  High!";
                    if (item.invQuantity <= 0) comment = "NONE LEFT!";
                    Console.WriteLine(String.Format("\t|{0, -5} | {1,-20} | {2,2} | {3,3} | {4,4} | {5,-15} |", item.invId, item.invName, item.invType[0], item.invQuantity, item.invStrPrice, comment));
                    comment = "";
                }
                Console.WriteLine("\t|------------------------------------------------------------------|");
            }
            else
            {
                Console.WriteLine("\n\n\t|---------------------------------------------|");
                Console.WriteLine("\t|         Dunder Mifflin Paper Company        |");
                Console.WriteLine("\t|            Scranton, Pennsylvania           |");
                Console.WriteLine("\t|---------------------------------------------|");
                Console.WriteLine("\t|                                             |");
                Console.WriteLine("\t|{0,44} |", today.ToString());
                Console.WriteLine("\t|---------------------------------------------|");
                foreach (invItem item in myList)
                {
                    cartTotal += item.invPrice;
                    Console.WriteLine(String.Format("\t| {0, -44}|\n\t|    #{1,-5} {2,24}\t{3,4} |", item.invType, item.invId, item.invName, item.invStrPrice));
                }
                tax = cartTotal * taxRate;
                tax = Math.Round(tax, 2);
                newTotal = cartTotal + tax;
                Console.WriteLine("\t|                                             |");
                Console.WriteLine("\t|---------------------------------------------|");
                Console.WriteLine("\t|                                             |");
                Console.WriteLine("\t|{0,33}:   {1,7} |", "Subtotal", "$" + cartTotal.ToString());
                Console.WriteLine("\t|{0,33}:   {1,7} |", "9% State Tax", "$" + tax.ToString());
                Console.WriteLine("\t|---------------------------------------------|");
                Console.WriteLine("\t|{0,33}:   {1,7} |", "Total", "$" + newTotal.ToString());
                Console.WriteLine("\t|                                             |");
                Console.WriteLine("\t|                                             |");
                Console.WriteLine("\t|       Thank you for shopping with us!       |");
                Console.WriteLine("\t|                                             |");
                Console.WriteLine("\t|             9  am  -  7  pm  M-F            |");
                Console.WriteLine("\t|             9  am  -  5  pm  Sat            |");
                Console.WriteLine("\t|             11 am  -  5  pm  Sun            |");
                Console.WriteLine("\t|                                             |");
                Console.WriteLine("\t|---------------------------------------------|");

            }

        }

        public void updateInventory()
        {
            currDir = new DirectoryInfo(".");
            currDirStr = currDir.ToString();
            today = DateTime.Now;
            runningTotal = 0.00;
            taxedRunningTotal = 0.00;
            todayStr = today.ToString();
            todayStr = todayStr.Replace("/", "-");
            todayStr = todayStr.Replace(":", "-");
            invFileName = "Inventory as of " + todayStr + ".txt";
            using (StreamWriter file = File.CreateText(System.IO.Path.Combine(currDirStr, invFileName)))
            {
                foreach (invItem tempInvItem in listOfInv)
                {
                    file.WriteLine("{0},{1},{2},{3},{4}", tempInvItem.invName, tempInvItem.invType, tempInvItem.invId, tempInvItem.invQuantity, tempInvItem.invPrice);
                }
            }
            getCategories();
        }


    }
}
