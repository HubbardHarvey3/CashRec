using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace CashRec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<int, string> Dlist = new Dictionary<int, string>();

        public MainWindow()
        {

            InitializeComponent();
            //Reads the JSON file and adds the entries to the DList Dictionary
            Donor.readDlist();
            //Writes the dictionary to the textbox
            writeDlist();

        }
        //I don't know what these two below do:
        private void DonorListDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void DailyRecordInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        //*************EVENT HANDLERS BELOW
        public void writeDlist([Optional] object sender, [Optional] RoutedEventArgs e)
        {
            Donor.savetoDlist();
            //Add each dictionary pair to an empty string and then display that string to the output box.
            string line = "";
            foreach (var item in Dlist)
            {
                line += $"{item.Key}: {item.Value} \n";

            }
            DonorListOutput.Text = line;
        }

        //Submit Button Event Handler.  Grabs the various inputs and writes them to the JSON File
        private void SubmitDonor(object sender, RoutedEventArgs e)
        {
            string Dnum = DonorNumInput.Text;
            string FirstName = DonorFirstNameInput.Text;
            string LastName = DonorLastNameInput.Text;
            Donor.addDlist(Dnum, FirstName, LastName);
            Donor.savetoDlist();

            DonorNumInput.Text = "";
            DonorFirstNameInput.Text = "";
            DonorLastNameInput.Text = "";
            writeDlist();
        }

        private void DeleteDonorNum(object sender, RoutedEventArgs e)
        {
            string Donornum = DonorNumInput.Text;
            Donor.removeDlist(Donornum);
            writeDlist();
            DonorNumInput.Text = "";
        }

        //*************EVENT HANDLERS ABOVE
    }

}
