using System.Windows;
using System.Windows.Controls;

namespace LogistHelper.UI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomDatePicker.xaml
    /// </summary>
    public partial class CustomDatePicker : UserControl
    {
        public string DateRangeString
        {
            get { return (string)GetValue(DateRangeStringProperty); }
            set { SetValue(DateRangeStringProperty, value); }
        }
        public static readonly DependencyProperty DateRangeStringProperty =
            DependencyProperty.Register("DateRangeString", typeof(string), typeof(CustomDatePicker));



        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(CustomDatePicker));

        

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }
        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime), typeof(CustomDatePicker));






        public CustomDatePicker()
        {
            InitializeComponent();
        }

        private void WatermarkTextbox_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox text) 
            { 
                calendarPopup.IsOpen = true;
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Calendar calendar) 
            {
                OnRangeChanged(calendar.SelectedDates);
            }
        }

        private void OnRangeChanged(SelectedDatesCollection dates)
        {
            string rangeString = string.Empty;

            if (dates != null && dates.Any())
            {
                DateTime start = dates.FirstOrDefault();
                DateTime end = dates.LastOrDefault();

                rangeString = $"{start.ToString("dd.MM.yyyy")} - {end.ToString("dd.MM.yyyy")}";
            }
            DateRangeString = rangeString;
        }
    }
}
