using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;

namespace PLS__C_sharp
{
    internal class Patient
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public DateOnly Birthday { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string BloodGroup { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
    }
}
