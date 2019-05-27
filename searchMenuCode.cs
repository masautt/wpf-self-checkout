using System;
using System.Windows;
using System.Windows.Data;
namespace wpf-self-checkout
{
    public partial class MainWindow : Window
    {
       // <summary>
       // This cs file is the search menu in the center of the screen
       //     searchBtn_Click:            Basic button that shows the search panel
       //     UserFilter:                 Code I found online that is responsible for updating the inventory after a character is inputted
       //     textFilter_TextChanged:     Helper function for the basic code that updates the inventory
       // </summary>
        public void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (searchPanel.Visibility == Visibility.Visible) searchPanel.Visibility = Visibility.Hidden;
            else { searchPanel.Visibility = Visibility.Visible; }
        }
        public bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(searchBox.Text))
                return true;
            else
                return ((item as invItem).invName.IndexOf(searchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        public void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(searchMenu.ItemsSource).Refresh();
        }
    }
}
