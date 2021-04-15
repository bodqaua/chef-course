using Chef.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chef.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }

        public void prepare()
        {
            this.Name = this.Name.Trim();
            this.Price = Convert.ToDouble(this.Price);
            this.Quantity = Convert.ToDouble(this.Quantity);
        }
    }
}
