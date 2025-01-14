using System.Windows.Controls;
using System.Windows.Data;

namespace LogistHelper.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
        }

        private void searchText_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is TextBox text && e.Key == System.Windows.Input.Key.Enter) 
            {
                BindingExpression be = text.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
                searchButton.Command?.Execute(null);
            }
        }
    }
}
