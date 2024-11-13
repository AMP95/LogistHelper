using System.Windows;

namespace CustomDialog
{
    /// <summary>
    /// Логика взаимодействия для Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public Dialog()
        {
            InitializeComponent();
            Loaded += DialogWindow_Loaded;
        }

        private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            accept.Focus();
            if (this.DataContext is IWindowCloser closer)
            {
                closer.Close += new Action<bool?>((result) =>
                {
                    this.DialogResult = result;
                    Close();
                });
            }
        }
    }
}
