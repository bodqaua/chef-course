using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chef.Shared
{
    public class AbstractPageController : Page
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

        protected void InitBackNavigation(Page page, DockPanel control)
        {
            var bc = new BrushConverter();
            Button button = new Button();
            button.Content = "Назад";
            button.Background = (Brush)bc.ConvertFrom("#FF263238");
            button.Margin = new Thickness(12);
            button.Padding = new Thickness(24, 6, 24, 6);
            button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            button.Cursor = Cursors.Hand;
            button.Click += (sender, e) => { this.Redirect(page); };
            control.Children.Add(button);
        }
    }
}
