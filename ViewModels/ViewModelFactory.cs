using Chef.Models.Database;
using Chef.Pages;
using Chef.Shared;
using Chef.ViewModels.WarehouseAdd;

namespace Chef.ViewModels
{
    public class ViewModelFactory
    {
        private DatabaseContext databaseContext;
        private ValidationController validationController;
        public ViewModelFactory(DatabaseContext databaseContext,
                                ValidationController validationController)
        {
            this.databaseContext = databaseContext;
            this.validationController = validationController;
        }
        public WarehouseViewModel createWarehousePage()
        {
            return new WarehouseViewModel(this.databaseContext, this);
        }

        public WarehouseAddViewModel createWarehouseAddPage()
        {
            return new WarehouseAddViewModel(this.validationController, this.databaseContext, this);
        }
    }
}
