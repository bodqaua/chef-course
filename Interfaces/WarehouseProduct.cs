using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef.Interfaces
{
    public interface IWarehouseProduct
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}
