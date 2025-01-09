using System.Windows;
using System.Windows.Controls;

namespace LogistHelper.UI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomPasswordBox.xaml
    /// </summary>
    public partial class CustomPasswordBox : UserControl
    {
        public PasswordBox PasswordBox { get; private set; }

        public Style PasswordBoxStyle
        {
            get { return (Style)GetValue(PasswordBoxStyleProperty); }
            set { SetValue(PasswordBoxStyleProperty, value); }
        }
        public static readonly DependencyProperty PasswordBoxStyleProperty =
            DependencyProperty.Register("PasswordBoxStyle", typeof(Style), typeof(CustomPasswordBox));


        public Style ShowedPasswordBoxStyle
        {
            get { return (Style)GetValue(ShowedPasswordBoxStyleProperty); }
            set { SetValue(ShowedPasswordBoxStyleProperty, value); }
        }
        public static readonly DependencyProperty ShowedPasswordBoxStyleProperty =
            DependencyProperty.Register("ShowedPasswordBoxStyle", typeof(Style), typeof(CustomPasswordBox));


        public Style ShowButtonStyle
        {
            get { return (Style)GetValue(ShowButtonStyleProperty); }
            set { SetValue(ShowButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty ShowButtonStyleProperty =
            DependencyProperty.Register("ShowButtonStyle", typeof(Style), typeof(Style));

        public bool IsPasswordShowed
        {
            get { return (bool)GetValue(IsPasswordShowedProperty); }
            set { SetValue(IsPasswordShowedProperty, value); }
        }
        public static readonly DependencyProperty IsPasswordShowedProperty =
            DependencyProperty.Register("IsPasswordShowed", typeof(bool), typeof(CustomPasswordBox),
                new FrameworkPropertyMetadata(new PropertyChangedCallback((d, e) =>
                {
                    if (d is CustomPasswordBox custom)
                    {
                        if (custom.IsPasswordShowed)
                        {
                            custom.pwdBox.Visibility = Visibility.Hidden;
                            custom.showedPwd.Visibility = Visibility.Visible;
                            custom.showedPwd.Focus();
                        }
                        else
                        {
                            custom.pwdBox.Visibility = Visibility.Visible;
                            custom.showedPwd.Visibility = Visibility.Hidden;
                            custom.pwdBox.Focus();
                        }
                    }
                })));


        public CustomPasswordBox()
        {
            InitializeComponent();
            PasswordBox = pwdBox;
        }

        private void pwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ChangePassword(pwdBox.Password);
        }

        private void showedPwd_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangePassword(showedPwd.Text);
        }

        private void ChangePassword(string password)
        {
            if (!pwdBox.Password.Equals(password))
            {
                pwdBox.Password = password;
            }
            if (!showedPwd.Text.Equals(password))
            {
                showedPwd.Text = password;
            }
        }
    }
}
