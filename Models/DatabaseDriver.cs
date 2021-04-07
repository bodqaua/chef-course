using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Chef.Interfaces;

namespace Chef.Models
{
    public class DatabaseDriver : IDatabaseDriver
    {
        public DatabaseDriver()
        {
        }

        public string init()
        {
            return "DI is working!";
        }
    }
}
