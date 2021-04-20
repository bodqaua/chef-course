using Chef.Models.Database;
using Chef.Pages;
using Chef.Shared;
using Chef.ViewModels.WarehouseAdd;

namespace Chef.ViewModels
{
    public class ViewModelFactory
    {
        private DatabaseContext databaseContext;
        private AbstractController abstractController;
        public ViewModelFactory(DatabaseContext databaseContext,
                                AbstractController abstractController)
        {
            this.databaseContext = databaseContext;
            this.abstractController = abstractController;
        }
        public WarehouseViewModel createWarehousePage()
        {
            return new WarehouseViewModel(this.databaseContext, this);
        }

        public WarehouseAddViewModel createWarehouseAddPage()
        {
            return new WarehouseAddViewModel(this.abstractController, this.databaseContext, this);
        }
    }
}
