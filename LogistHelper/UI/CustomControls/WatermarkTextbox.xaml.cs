using System.Windows;
using System.Windows.Controls;

namespace LogistHelper.UI.CustomControls
{
    public partial class WatermarkTextbox : UserControl
    {
        public TextBox TextBox
        {
            get { return (TextBox)GetValue(TextBoxProperty); }
            set { SetValue(TextBoxProperty, value); }
        }

        public static readonly DependencyProperty TextBoxProperty =
            DependencyProperty.Register("TextBox", typeof(TextBox), typeof(WatermarkTextbox),
                new FrameworkPropertyMetadata(new PropertyChangedCallback((d,e) => 
                {
                    if (d is WatermarkTextbox water) 
                    {
                        if (water.TextBox != null)
                        {
                            water.TextBox.TextChanged += water.TextBox_TextChanged;

                            water.inputGrid.Children.Add(water.TextBox);

                            Grid.SetColumn(water.TextBox, 0);
                            Grid.SetRow(water.TextBox, 0);
                        }
                    }
                })));

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox box) 
            { 
                box.CaretIndex = box.Text.Length;

                if (box.Text.Length > 0) 
                { 
                    watermark.Visibility = Visibility.Hidden;
                }
                else
                {
                    watermark.Visibility = Visibility.Visible;
                }
            }
        }





        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkTextbox));



        public WatermarkTextbox()
        {
            InitializeComponent();
        }
    }
}
