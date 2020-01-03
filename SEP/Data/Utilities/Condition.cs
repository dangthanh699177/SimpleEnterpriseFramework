using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.Data.Utilities
{
    public class Condition
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Condition(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
