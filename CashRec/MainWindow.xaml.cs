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


        //Event Handlers for Transaction Page
        public void ListTransactions(object sender, RoutedEventArgs e)
        {

            //set the Itemsource to the new list TransactList
            TransactionDataGrid.ItemsSource = Transaction.TransactList;
            //BalancingDataGrid.ItemsSource = Transaction.TransactList;
            //Use a foreach to add Dlist dicitionary to Transactlist on the Transaction Class
            foreach (var item in Dlist)
            {
                //only adds if the Dlist.Key is valid
                if (item.Key == Convert.ToInt32(DonorNumTransactionInput.Text))
                {
                    //Check if nothing is entered in Cash/Check input boxes and if nothing then change the amounts to 0.
                    if (TransactionAmountCashInput.Text == "")
                    {
                        TransactionAmountCashInput.Text = "0";
                    }
                    else if (TransactionAmountCheckInput.Text == "")
                    {
                        TransactionAmountCheckInput.Text = "0";
                    }
                    Transaction.TransactList.Add(new Transaction
                    {
                        date = TransactionDateInput.Text,
                        donorNum = item.Key,
                        name = item.Value,
                        amountCheck = Convert.ToDecimal(TransactionAmountCheckInput.Text),
                        amountCash = Convert.ToDecimal(TransactionAmountCashInput.Text)
                    });
                    //must refresh the DataGrid to include each row.
                    TransactionDataGrid.Items.Refresh();
                    //BalancingDataGrid.Items.Refresh();
                }

            }

        }

        private void ExportList(object sender, RoutedEventArgs e)
        {
            //start by adding the column headers to the string
            string csv = "DATE,Donor Number,Last Name, First Name, Check Amount, Cash Amount";
            //add each item from the TransactList to the string, ensuring to start a new line after before each entry
            foreach (var item in Transaction.TransactList)
            {
                csv += "\r\n";
                csv += $"{item.date},{item.donorNum},{item.name},${item.amountCheck},${item.amountCash}";

            }
            //Found this on Microsofts website https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-write-to-a-text-file
            //Uses Streamwriter to APPEND the string in CSV to the existing file.
            using (StreamWriter file =
                new StreamWriter(csvFile, true))
            {
                file.WriteLine(csv);
            }
        }

        private void DeleteRow(object sender, RoutedEventArgs e)
        {

            foreach (var item in Transaction.TransactList)
            {
                if (
                    item.donorNum == Convert.ToInt32(DonorNumTransactionInput.Text) &&
                    item.amountCheck == Convert.ToDecimal(TransactionAmountCheckInput.Text) &&
                    item.amountCash == Convert.ToDecimal(TransactionAmountCashInput.Text)
                    )
                {
                    Transaction.TransactList.Remove(item);
                    break;
                }
            }
            TransactionDataGrid.Items.Refresh();
        }

        private void total(object sender, RoutedEventArgs e)
        {
            decimal totalCheck = 0;
            decimal totalCash = 0;
            decimal totalA = 0;
            foreach (var item in Transaction.TransactList)
            {
                totalCheck += item.amountCheck;
                totalCash += item.amountCash;
            }
            totalCash = Math.Round(totalCash, 2);
            totalCheck = Math.Round(totalCheck, 2);
            totalA += totalCheck;
            totalA += totalCash;
            MessageBox.Show($"The total Check Amount:${totalCheck} {Environment.NewLine}Total Cash Amount:${totalCash} {Environment.NewLine}Total Amount:${totalA}");
        }
        
        //Event Handlers for Transaction Page
        
        //Event Handlers for Balancing Page
        //public int checkCount = 0;
        private void InsertCheckAmount(object sender, RoutedEventArgs e)
        {
            //set the Itemsource to the new list TransactList
            BalancingDataGrid.ItemsSource = Balancing.balancingList;
            //BalancingDataGrid.ItemsSource = Transaction.TransactList;
            //Use a foreach to add Dlist dicitionary to Transactlist on the Transaction Class
            Balancing.balancingList.Add(new Balancing
            {
                checkAmountBal = Math.Round(Convert.ToDecimal(BalanceCheckAmount.Text), 2)
            });
            //foreach (var item in Balancing.balancingList)
            //{
                BalancingDataGrid.Items.Refresh();
            //}
            //checkCount += 1;
            BalanceCheckCount.Text = Balancing.balancingList.Count.ToString();
        }
        
        private void DeleteCheckAmount(object sender, RoutedEventArgs e)
        {
            foreach (var item in Balancing.balancingList)
            {
                if (item.checkAmountBal == Convert.ToDecimal(BalanceCheckAmount.Text))
                {
                    Balancing.balancingList.Remove(item);
                    break;
                }
                
            }
            BalancingDataGrid.Items.Refresh();
            BalanceCheckCount.Text = Balancing.balancingList.Count.ToString();
        }

        private void BalancePennyInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalancePennyInput.Text) * .01M;
                totalstr = total.ToString();
                BalancePennyOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {

                Console.WriteLine("error");
                BalancePennyInput.Text = "0";
                total = Convert.ToDecimal(BalancePennyInput.Text) * .01M;
                totalstr = total.ToString();
                BalancePennyOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceNickelInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceNickelInput.Text) * .05M;
                totalstr = total.ToString();
                BalanceNickelOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceNickelInput.Text = "0";
                total = Convert.ToDecimal(BalanceNickelInput.Text) * .05M;
                totalstr = total.ToString();
                BalanceNickelOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceDimeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceDimeInput.Text) * .1M;
                totalstr = total.ToString();
                BalanceDimeOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceDimeInput.Text = "0";
                total = Convert.ToDecimal(BalanceDimeInput.Text) * .1M;
                totalstr = total.ToString();
                BalanceDimeOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceQuarterInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceQuarterInput.Text) * .25M;
                totalstr = total.ToString();
                BalanceQuarterOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceQuarterInput.Text = "0";
                total = Convert.ToDecimal(BalanceQuarterInput.Text) * .25M;
                totalstr = total.ToString();
                BalanceQuarterOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceDollarInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceDollarInput.Text) * 1M;
                totalstr = total.ToString();
                BalanceDollarOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceDollarInput.Text = "0";
                total = Convert.ToDecimal(BalanceDollarInput.Text) * 1M;
                totalstr = total.ToString();
                BalanceDollarOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceFiveInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceFiveInput.Text) * 5M;
                totalstr = total.ToString();
                BalanceFiveOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceFiveInput.Text = "0";
                total = Convert.ToDecimal(BalanceFiveInput.Text) * 5M;
                totalstr = total.ToString();
                BalanceFiveOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceTenInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceTenInput.Text) * 10M;
                totalstr = total.ToString();
                BalanceTenOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceTenInput.Text = "0";
                total = Convert.ToDecimal(BalanceTenInput.Text) * 10M;
                totalstr = total.ToString();
                BalanceTenOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceTwentyInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceTwentyInput.Text) * 20M;
                totalstr = total.ToString();
                BalanceTwentyOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceTwentyInput.Text = "0";
                total = Convert.ToDecimal(BalanceTwentyInput.Text) * 20M;
                totalstr = total.ToString();
                BalanceTwentyOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceFiftyInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceFiftyInput.Text) * 50M;
                totalstr = total.ToString();
                BalanceFiftyOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceFiftyInput.Text = "0";
                total = Convert.ToDecimal(BalanceFiftyInput.Text) * 50M;
                totalstr = total.ToString();
                BalanceFiftyOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }

        private void BalanceHundredInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal total = 0;
            string totalstr;
            try
            {
                total = Convert.ToDecimal(BalanceHundredInput.Text) * 100M;
                totalstr = total.ToString();
                BalanceHundredOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                BalanceHundredInput.Text = "0";
                total = Convert.ToDecimal(BalanceHundredInput.Text) * 100M;
                totalstr = total.ToString();
                BalanceHundredOutput.Text = totalstr;
                Console.WriteLine(totalstr);
            }
        }
    }

    //TODO
    
    //work on cash balancing
    //Stop user from entering unused DonorNumber
    //Print Donor List functionality
}
