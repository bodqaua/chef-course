using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            for (int i = 0; i < recipe.Images.Count; i++)
            {
                recipe.Images[i].Id = 0;
            }
            this.databaseContext.Recipes.Add(recipe);
            this.databaseContext.SaveChanges();
        }

        public void deleteRecipe(int id)
        {
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

        public List<RecipeEntity> loadRecipes()
        {
            return this.databaseContext.Recipes
                .Where(r => true)
                .Include(x => x.Images)
                .Include(x => x.Ingredients)
                .ToList();
        }

        public RecipeEntity getRecipeById(int recipeId)
        {
            return this.databaseContext.Recipes
                .Where(r => r.Id == recipeId)
                .Include(r => r.Ingredients)
                .Include(r => r.Images)
                .SingleOrDefault();
        }

        public void UpdateRecipe(RecipeEntity recipe)
        {
            RecipeEntity entity = this.databaseContext.Recipes.Where(r => r.Id == recipe.Id).FirstOrDefault();
            entity.Name = recipe.Name;
            entity.Calories = recipe.Calories;
            entity.Proteins= recipe.Proteins;
            entity.Fats = recipe.Fats;
            entity.Carbohydrate= recipe.Carbohydrate;
            entity.Description= recipe.Description;
            entity.Images= recipe.Images;
            entity.Ingredients = recipe.Ingredients;

            this.save();
        }

        private void save()
        {
            this.databaseContext.SaveChanges();
        }
    }
}
