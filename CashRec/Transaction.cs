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
    
    public class Transaction
    {
        
        public int donorNum { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public double amount { get; set; }


    }
}
