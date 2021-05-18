using Chef.Models;
using Chef.Models.Database;
using Chef.Shared;
using System.Collections.Generic;

namespace Chef.ViewModels.Pages
{
    /// <summary>
    /// Interaction logic for RecipeEveningCounter.xaml
    /// </summary>
    public partial class RecipeEveningCounter : AbstractPageController
    {
        private readonly ViewModelFactory viewModelFactory;
        private readonly RecipeService recipeService;
        private readonly ProductService productService;
        private readonly ValidationController validationController;
        private List<RecipeEntity> recipes;
        //private readonly IngredientData ingredientData;
        public RecipeEveningCounter(
            RecipeService recipeService,
            ProductService productService,
            ViewModelFactory viewModelFactory,
            ValidationController validationController)
        {
            this.recipeService = recipeService;
            this.productService = productService;
            this.viewModelFactory = viewModelFactory;
            this.validationController = validationController;
            InitializeComponent();
            this.Init(this);
            this.InitBackNavigation(this.viewModelFactory.createHomePage(), BackPanel);

            this.InitRecipes();
        }

        private void InitRecipes()
        {
            this.recipes = this.recipeService.loadRecipes();
            RecipesSelector.SelectedValuePath = "Id";
            RecipesSelector.DisplayMemberPath = "Name";
            foreach (RecipeEntity recipe in recipes)
            {
                RecipesSelector.Items.Add(recipe);
            }
        }
    }
}
