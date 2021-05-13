using Chef.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef.Shared
{
    public class IngredientItem : Ingredient
    {
        public string ListId { get; set; }
        public bool Action { get; set; }

        public static List<Ingredient> PrepareIngredients(List<IngredientItem> ingredients)
        {
            List<Ingredient> dbIngredients = new List<Ingredient>();
            foreach (IngredientItem item in ingredients)
            {
                dbIngredients.Add(
                    new Ingredient { Name = item.Name, Quantity = item.Quantity }
                );
            }
            return dbIngredients;
        }

    }

}
