using System.Windows;
using System.Windows.Controls;

namespace CustomDialog
{
    /// <summary>
    /// Логика взаимодействия для MessageView.xaml
    /// </summary>
    public partial class MessageView : UserControl
    {
        public MessageView()
        {
            InitializeComponent();

            Loaded += MessageView_Loaded;
            accept.Focus();
        }

        private void MessageView_Loaded(object sender, RoutedEventArgs e)
        {
            accept.Focus();
            Loaded -= MessageView_Loaded;
        }
    }
}
