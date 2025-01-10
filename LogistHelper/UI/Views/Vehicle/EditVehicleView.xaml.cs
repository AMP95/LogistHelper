using System.Windows.Controls;

namespace LogistHelper.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для EditVehicleView.xaml
    /// </summary>
    public partial class EditVehicleView : UserControl
    {
        public EditVehicleView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox text) 
            { 
                text.CaretIndex = text.Text.Length;
            }
        }
    }
}
