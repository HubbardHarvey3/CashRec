using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CashRec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Initialize our donorDictionary
        Dictionary<Int32, string> donorDictionary = new Dictionary<int, string>();
        
        //store our PATH into a string variable
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+@"\test.csv";
        //Creating a donorList class with two objects Name and Number as opposed to donorDicitionary
        public class donorList
        {
            public Int32 donorNum { get; set; }
            public string donorName { get; set; }
        }
        //Initialize a new List using the donorList Class.
        List<donorList> donors = new List<donorList>();
        public MainWindow()
        {
            InitializeComponent();
            //Initializing the donorList Class and adding people hard code after program initializes
            
            //Reads CSV and adds the values to List
            using (StreamReader sr = new StreamReader(path))
            {
                //create a string variable per line read from the CSV
                string row;
                
                //While the row variable isn't null, split the row into an array to be added to the Key/Value Dictionary
                while ((row = sr.ReadLine()) != null)
                {
                    String[] strlist = row.Split(",");
                    //Check to ensure the first substring isn't empty
                    if (strlist[0] != "")
                    {
                        //Add the substrings to the dictionary
                        //Show create Try Catch in case the dictionary already contains the information being added.
                        donors.Add(new donorList() { donorNum = Convert.ToInt32(strlist[0]), donorName = strlist[1] });
                        Console.WriteLine($"{strlist[0]} : {strlist[1]} added to donors list.");
                    }
                }
                //Add the List to the DonorList Display Text Box for the User
                foreach (var item in donors)
                {
                    DonorListDisplay.Text += $"{ item.donorNum }: { item.donorName }" + Environment.NewLine;
                    DonorComboBox.Items.Add(item.donorNum.ToString() + " " + item.donorName);
                }

            }

            //Setting the source of the datagrid to our recently created donorList, donors
            DailyRecGrid.ItemsSource = donors;
            
            
        }
        //I don't know what these two below do:
        private void DonorListDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void DailyRecordInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //I don't know what these two above do:

        //Uses donors list
        private void SumbitNewDonor(object sender, RoutedEventArgs e)
        {
            //setup variables for the Input Textboxes
            string name = DonorNameInput.Text;
            string num = DonorNumInput.Text;
            //Add the information in the textboxes to our list.
            donors.Add(new donorList() { donorNum = (Convert.ToInt32(num)), donorName = name });
            //Display pop up confirming users submission
            MessageBox.Show($"Your new Donor {name} is saved!");
            //reset the text boxes so duplicates can't be added.
            DonorNameInput.Text = "";
            DonorNumInput.Text = "";
        }

        //Display reads from the csv file and adds what it reads to the donorDictionary.  
        //Display also writes the dictionary to the Donor List Output.
        public void Display(object sender, RoutedEventArgs e)
        {
            //Reset the DonorList Display
            DonorListDisplay.Text = "";
            //Loop through the list and add each element to the textbox
            foreach (var item in donors)
            {
                DonorListDisplay.Text += $"{item.donorNum}: {item.donorName}" + Environment.NewLine;
            }

        }
        //Uses donors list
        private void SaveDonorList(object sender, RoutedEventArgs e)
        {
            //Clean the CSV File of records
            File.WriteAllText(path, "");
            //loop through the donors list and write the records in the CSV file.
            foreach (var item in donors)
            {
                String csv = String.Join(Environment.NewLine, $"{item.donorNum}, {item.donorName}"+Environment.NewLine);
                File.AppendAllText(path, csv);
            }
        }

        private void DonorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
