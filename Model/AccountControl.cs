using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Model
{
    public class AccountControl
    {
        public int Account { get; set; } // primary key, Auto Increment
        public string Id { get; set; } 
        public string Password { get; set; } 
        public DateTime Date { get; set; } 
        public string Authority { get; set; } // admin, user, guest
    }
}
