using System;
using System.Collections.Generic;
using System.IO;
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
        //Dictionary that holds everything from the Donor Class
        public static Dictionary<int, string> Dlist = new Dictionary<int, string>();
        //List that takes entries from Dlist and helps to spit them into CSV File
        public List<Transaction> TransactList = new List<Transaction>();
        //Filepath for CSV File which will record entries made on the transaction grid
        string csvFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TestCSV.csv";
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


        //*************EVENT HANDLERS BELOW for Donor List Tab
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
            DonorListOutputA.Text = line;
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



        //*************EVENT HANDLERS ABOVE for Donor List Tab


        //Event Handlers for Transaction Class
        private void ListTransactions(object sender, RoutedEventArgs e)
        {
            //set the Itemsource to the new list TransactList
            TransactionDataGrid.ItemsSource = TransactList;
            //Use a foreach to add Dlist dicitionary to Transactlist on the Transaction Class
            foreach (var item in Dlist)
            {
                //only adds if the Dlist.Key is valid
                if (item.Key == Convert.ToInt32(DonorNumTransactionInput.Text))
                {
                    TransactList.Add(new Transaction { date = TransactionDateInput.Text, donorNum = item.Key, name = item.Value, amount = Convert.ToDouble(TransactionAmountInput.Text) });
                    //must refresh the DataGrid to include each row.
                    TransactionDataGrid.Items.Refresh();
                }

            }

        }

        private void ExportList(object sender, RoutedEventArgs e)
        {
            //start by adding the column headers to the string
            string csv = "DATE,Donor Number,Last Name, First Name, Donated Amount";
            //add each item from the TransactList to the string, ensuring to start a new line after before each entry
            foreach (var item in TransactList)
            {
                csv += "\r\n";
                csv += $"{item.date},{item.donorNum},{item.name},${item.amount}";
            }
            //Found this on Microsofts website https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-write-to-a-text-file
            //Uses Streamwriter to APPEND the string in CSV to the existing file.
            using (StreamWriter file =
                new StreamWriter(csvFile, true))
            {
                file.WriteLine("\r\n" + csv);
            }
        }

        private void DeleteRow(object sender, RoutedEventArgs e)
        {

            foreach (var item in TransactList)
            {
                if (item.donorNum == Convert.ToInt32(DonorNumTransactionInput.Text) && item.amount == Convert.ToDouble(TransactionAmountInput.Text))
                {
                    TransactList.Remove(item);
                    break;
                }
            }
            TransactionDataGrid.Items.Refresh();
        }

        private void total(object sender, RoutedEventArgs e)
        {
            double total = 0;
            foreach (var item in TransactList)
            {
                total += item.amount;
            }
            MessageBox.Show("The Total is:$" + total);
        }
    }

    //Stop user from entering unused DonorNumber
    //Print Donor List functionality
}
