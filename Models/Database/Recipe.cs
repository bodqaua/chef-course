using Chef.Models.Entities;
using System.Collections.Generic;

namespace Chef.Models
{
    public class RecipeEntity
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
