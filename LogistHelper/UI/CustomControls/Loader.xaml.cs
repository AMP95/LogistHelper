using System.Windows;
using System.Windows.Controls;

namespace LogistHelper.UI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для Loader.xaml
    /// </summary>
    public partial class Loader : UserControl
    {
        public bool IsInProcess
        {
            get { return (bool)GetValue(IsInProcessProperty); }
            set { SetValue(IsInProcessProperty, value); }
        }
        public static readonly DependencyProperty IsInProcessProperty =
            DependencyProperty.Register("IsInProcess", typeof(bool), typeof(Loader));


        public Loader()
        {
            InitializeComponent();
        }
    }
}
