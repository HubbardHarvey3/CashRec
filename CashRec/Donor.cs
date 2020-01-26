using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace CashRec
{
    public class Donor
    {
        //FilePath for JSON doc
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\text.txt";

        //Initialize the Dictionary


        //Properties of the Donor Class
        public int number { get; set; }
        public string fullName { get; set; }

        //Donor Constructor
        public Donor(int _number, string _lastName, string _firstName)
        {
            this.number = _number;
            this.fullName = $"{_lastName}, {_firstName}";
        }
        //Write the Donor List to the console and overwrite the JSON file on the desktop

        public static Dictionary<int, string> temp = new Dictionary<int, string>();
        public static void savetoDlist()
        {

            //print dictionary to console

            //reorder the dictionary before printing it
            //In this foreach, sort the existing Dlist and add to temp dictionary
            foreach (KeyValuePair<int, string> donor in MainWindow.Dlist.OrderBy(key => key.Key))
            {
                temp.Add(donor.Key, donor.Value);
            }
            //with the data added to temp, clear everything in Dlist
            MainWindow.Dlist.Clear();
            //with Dlist clean, add back the sorted dictionary from the temp dictionary
            foreach (KeyValuePair<int, string> item in temp)
            {
                MainWindow.Dlist.Add(item.Key, item.Value);
            }
            //we are done with Temp, clear it out for use next time
            temp.Clear();


            //serialize the dictionary
            string jsonA = JsonConvert.SerializeObject(MainWindow.Dlist, Formatting.Indented);

            //create JSON file
            File.WriteAllText(Donor.path, jsonA);
        }

        //Read an existing JSON file and add to the dictionary
        public static void readDlist()
        {
            //Read all text in the Text.txt file and deserialize it.  
            var json = JsonConvert.DeserializeObject(File.ReadAllText(path));
            //take the deserialized data and add it to the Dlist Dictionary
            Dictionary<int, string> Temp = JsonConvert.DeserializeObject<Dictionary<int, string>>(json.ToString());
            //Add the deserialized data to our Dlist dictionary
            foreach (var item in Temp)
            {
                MainWindow.Dlist.Add(item.Key, item.Value);
            }
        }

        public static void addDlist(string Dnum, string firstName, string lastName)
        {
            //add donor to dictionary
            //Learning Try Catch Exemptions
            try
            {
                MainWindow.Dlist.Add(Convert.ToInt32(Dnum), $"{lastName}, {firstName}");
                savetoDlist();
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Donor # already in use");
            }

        }

        public static void removeDlist(string num)
        {
            MainWindow.Dlist.Remove(Convert.ToInt32(num));
            savetoDlist();
        }

    }
}
