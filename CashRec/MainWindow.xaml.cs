using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CashRec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            //Reads the JSON file and adds the entries to the DList Dictionary
            Donor.readDlist();
        }
        //I don't know what these two below do:
        private void DonorListDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void DailyRecordInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void writeDlist(object sender, RoutedEventArgs e)
        {
            Donor.writeDlist();
        }

        //Submit Button Event Handler.  Grabs the various inputs and writes them to the JSON File
        private void SubmitDonor(object sender, RoutedEventArgs e)
        {
            string Dnum = DonorNumInput.Text;
            string FirstName = DonorFirstNameInput.Text;
            string LastName = DonorLastNameInput.Text;
            Donor.addDlist(Dnum, FirstName, LastName);
            Donor.writeDlist();

            DonorNumInput.Text = "";
            DonorFirstNameInput.Text = "";
            DonorLastNameInput.Text = "";
        }
        


    }

}
