using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Globalization;

namespace CashRec
{
    
    public class Transaction : Window
    {
        //List that takes entries from Dlist and helps to spit them into CSV File
        public static List<Transaction> TransactList = new List<Transaction>();
        public int donorNum { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public decimal amountCheck { get; set; }
        public decimal amountCash { get; set; }

        
       
    }
}
