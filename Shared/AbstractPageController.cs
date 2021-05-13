using System.Windows;
using System.Windows.Controls;

namespace Chef.Shared
{
    public class AbstractPageController: Page
    {
        private Page self;
        protected void Redirect(Page page)
        {
            self.Content = new Frame { Content = page };
        }

        protected void Init(Page self)
        {
            this.self = self;
        }

        protected bool GetConfirmation(string title, string description, MessageBoxButton buttons)
        {
            return MessageBox.Show(title, description, buttons) == MessageBoxResult.Yes;
        }
    }
}
