using System.Windows.Controls;
using System.Windows.Input;

namespace LogistHelper.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для EnterPage.xaml
    /// </summary>
    public partial class EnterPage : UserControl
    {
        public EnterPage()
        {
            InitializeComponent();
        }

        private void passboxPasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                enterButton.Command?.Execute(passboxPasswordBox.PasswordBox);
            }
        }
    }
}
