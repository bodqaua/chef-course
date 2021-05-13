using Chef.Models;
using Chef.Models.Database;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Chef.ViewModels.Recipe
{
    /// <summary>
    /// Interaction logic for RecipeList.xaml
    /// </summary>
    public partial class RecipeList : Page
    {
        private readonly RecipeService recipeService;
        private readonly ViewModelFactory viewModelFactory;

        private List<RecipeEntity> recipes = new List<RecipeEntity>();

        public RecipeList(RecipeService recipeService,
                          ViewModelFactory viewModelFactory)
        {
            this.recipeService = recipeService;
            this.viewModelFactory = viewModelFactory;
            InitializeComponent();
            this.LoadRecipes();
        }

        private void LoadRecipes()
        {
            this.recipes = this.recipeService.loadRecipes();
            RecipeListElement.ItemsSource = this.recipes;
        }

        private void deleteRecipe(object sender, RoutedEventArgs e)
        {
            Button element = (Button)sender;
            RecipeEntity recipe = (RecipeEntity)element.Tag;
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Уверенны что хотите удалить рецепт " + recipe.Name,
                "Внимание!", 
                MessageBoxButton.YesNo
            );

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.recipeService.deleteRecipe(recipe.Id);
                this.LoadRecipes();
            }
        }

        private void viewRecipe(object sender, EventArgs args)
        {
            Button element = (Button)sender;
            if (element.Tag == null)
            {
                return;
            }
            int recipeId = Convert.ToInt32(element.Tag);
            this.Content = new Frame { Content = this.viewModelFactory.createRecipeView(recipeId) };
        }

        private void EditRecipe(object sender, EventArgs args)
        {
            Button element = (Button)sender;
            if (element.Tag == null)
            {
                return;
            }
            int recipeId = Convert.ToInt32(element.Tag);
            this.Content = new Frame { Content = this.viewModelFactory.createRecipeEdit(recipeId) };
        }
    }
}
