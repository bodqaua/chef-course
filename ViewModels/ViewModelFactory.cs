using Chef.Models.Database;
using Chef.Pages;

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
    }
}
