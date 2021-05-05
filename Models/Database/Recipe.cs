using Chef.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef.Models.Database
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrate { get; set; }
        public List<DbImage> Images { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }   

   
}
