using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataObjects
{
    public class Security
    {

        public string UserName { get; set; }
        public int NumberOfAttempts { get; set; }
        public DateTime TimeOfLastAttempt { get; set; }
        public bool Lock { get; set; }
        public string SecurityQuestion1 { get; set; }
        public int SecurityQuestion2 { get; set; }


    }
}
