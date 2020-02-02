using System;
using System.Collections.Generic;
using System.Text;

namespace CashRec
{
    class Balancing
    {
        public decimal checkAmountBal { get; set; }
        public decimal cashAmountBal { get; set; }

        public static List<Balancing> balancingList = new List<Balancing>();
    }
}
