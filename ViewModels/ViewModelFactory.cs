using Chef.Models.Database;
using Chef.Pages;
using Chef.ViewModels.WarehouseAdd;

namespace Chef.ViewModels
{
    public class ViewModelFactory
    {
        private DatabaseContext databaseContext;
        public ViewModelFactory(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public WarehouseViewModel createWarehousePage()
        {
            return new WarehouseViewModel(this.databaseContext, this);
        }

        public WarehouseAddViewModel createWarehouseAddPage()
        {
            return new WarehouseAddViewModel(this.databaseContext, this);
        }
    }
}
