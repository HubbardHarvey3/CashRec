using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CashRec
{
    public class Donor
    {
        //FilePath for JSON doc
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\text.txt";

        //Initialize the Dictionary
        public static Dictionary<int, string> Dlist = new Dictionary<int, string>();

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
        public static void writeDlist()
        {

            //print dictionary to console

            //reorder the dictionary before printing it
            //In this foreach, sort the existing Dlist and add to temp dictionary
            foreach (KeyValuePair<int, string> donor in Dlist.OrderBy(key => key.Key))
            {
                temp.Add(donor.Key, donor.Value);
            }
            //with the data added to temp, clear everything in Dlist
            Dlist.Clear();
            //with Dlist clean, add back the sorted dictionary from the temp dictionary
            foreach (KeyValuePair<int, string> item in temp)
            {
                Dlist.Add(item.Key, item.Value);
            }
            //we are done with Temp, clear it out for use next time
            temp.Clear();

            Console.WriteLine("The below will be added to the JSON file");
            Console.WriteLine("******************************************");
            foreach (var item in Donor.Dlist)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
            Console.WriteLine("******************************************");
            //serialize the dictionary
            string jsonA = JsonConvert.SerializeObject(Donor.Dlist, Formatting.Indented);

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
            Console.WriteLine("The below was read from the JSON File");
            Console.WriteLine("******************************************");
            //Dictionary<int, string> json = JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText(path));
            foreach (var item in Temp)
            {
                Console.WriteLine(item.Key + " " + item.Value);
                Donor.Dlist.Add(item.Key, item.Value);

            }
            Console.WriteLine("******************************************");

        }

        public static void addDlist(string Dnum, string firstName, string lastName)
        {
            //add donor to dictionary
            Dlist.Add(Convert.ToInt32(Dnum), $"{lastName}, {firstName}");
            writeDlist();
        }

        public static void removeDlist()
        {
            Console.WriteLine("Do you want to remove someone? Y/N");
            string answer = Console.ReadLine();
            answer = answer.ToLower();

            if (answer == "y")
            {
                Console.WriteLine("Please enter their donor number:");
                string num = Console.ReadLine();
                Dlist.Remove(Convert.ToInt32(num));
                writeDlist();
            }
        }

    }
}
