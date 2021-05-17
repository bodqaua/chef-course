using Chef.Models.Database;
using Chef.Shared;
using Chef.ViewModels.Pages;
using Chef.ViewModels.Recipe;
using Chef.ViewModels.WarehouseAdd;
using Chef.ViewModels.WarehouseEdit;

namespace Chef.ViewModels
{
    public class ViewModelFactory
    {
        private readonly DatabaseContext databaseContext;
        private readonly ValidationController validationController;
        private readonly ProductService productService;
        private readonly RecipeService recipeService;
        public ViewModelFactory(DatabaseContext databaseContext,
                                ValidationController validationController,
                                ProductService productService,
                                RecipeService recipeService
            )
        {
            this.databaseContext = databaseContext;
            this.validationController = validationController;
            this.productService = productService;
            this.recipeService = recipeService;
        }

        public HomeViewModel createHomePage()
        {
            return new HomeViewModel(this);
        }
        public WarehouseViewModel createWarehousePage()
        {
            return new WarehouseViewModel(this.databaseContext, this);
        }

        public WarehouseAddViewModel createWarehouseAddPage()
        {
            return new WarehouseAddViewModel(this.validationController, this.productService, this);
        }

        public WarehouseEditViewModel createWarehouseEditPage()
        {
            return new WarehouseEditViewModel(this.validationController, this.productService, this);
        }

        public RecipeAdd createRecipeAddPage()
        {
            return new RecipeAdd(this.validationController, this.productService, this, this.recipeService);
        }

        public RecipeList createRecipeListPage()
        {
            return new RecipeList(this.recipeService, this);
        }

        public RecipeView createRecipeView(int recipeId)
        {
            return new RecipeView(recipeId, this.recipeService, this);
        }

        public RecipeEdit createRecipeEdit(int recipeId)
        {
            return new RecipeEdit(recipeId, this.recipeService, this.productService, this);
        }
    }
}
