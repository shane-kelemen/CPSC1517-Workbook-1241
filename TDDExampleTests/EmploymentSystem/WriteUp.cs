using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem
{
    public class WriteUp
    {
        string _violation;
        public string Violation 
        {
            get
            {
                return _violation;
            } 

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The violation details are required!");
                }

                _violation = value;
            }
        }

        public string SupervisorName { get; set; }
        public string Resolution { get; set; }

        public WriteUp(string v, string s, string r)
        {
            Violation = v;
            SupervisorName = s;
            Resolution = r;
        }
    }
}
