using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Chef.Models.Database
{
    public class RecipeService
    {
        private readonly DatabaseContext databaseContext;
        public RecipeService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public void createRecipe(RecipeEntity recipe)
        {
            this.databaseContext.Recipes.Add(recipe);
            this.databaseContext.SaveChanges();
        }

        public void deleteRecipe(int id)
        {
            //RecipeEntity recipe = this.databaseContext.Recipes
            //    .Where(r => r.Id== id)
            //    .SingleOrDefault();

            RecipeEntity recipe = this.databaseContext.Recipes
                .Where(r => r.Id == id)
                .Include(x => x.Images)
                .Include(x => x.Ingredients)
                .SingleOrDefault();
            if (recipe == null)
            {
                return;
            }
            this.databaseContext.Remove(recipe);
            this.databaseContext.SaveChanges();
        }
    }
}
