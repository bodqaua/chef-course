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
        public WarehousePage createWarehousePage()
        {
            return new WarehousePage(this.databaseContext, this);
        }

        public EmptyPage createEmptyPage()
        {
            return new EmptyPage();
        }
    }
}
