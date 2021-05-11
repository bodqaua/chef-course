using Chef.Models.Database;
using Chef.Pages;
using Chef.Shared;
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
    }
}
