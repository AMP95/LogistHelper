using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogistHelper.UI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {

        #region Depenencies

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchBox),
                new FrameworkPropertyMetadata(new PropertyChangedCallback((d,e) => 
                {
                    if (d is SearchBox search) 
                    {
                        if (string.IsNullOrWhiteSpace(search.SearchText))
                        {
                            search.watermarkText.Visibility = Visibility.Visible;
                        }
                        else 
                        {
                            search.watermarkText.Visibility = Visibility.Hidden;
                        }

                        if (search.IsDynamicSearch)
                        {
                            if (string.IsNullOrWhiteSpace(search.SearchText))
                            {
                                search._canExecuteCommand = false;
                            }

                            if (search._canExecuteCommand)
                            {
                                search.SearchCommand?.Execute(search.SearchText);

                                search.popuplist.IsOpen = true;
                            }

                            search._canExecuteCommand = true;
                        }
                    }
                })));



        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(SearchBox));




        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchBox));


        public IEnumerable<object> SearchList
        {
            get { return (IEnumerable<object>)GetValue(SearchListProperty); }
            set { SetValue(SearchListProperty, value); }
        }
        public static readonly DependencyProperty SearchListProperty =
            DependencyProperty.Register("SearchList", typeof(IEnumerable<object>), typeof(SearchBox),
                new FrameworkPropertyMetadata(new PropertyChangedCallback((d, e) =>
                {
                    if (d is SearchBox search)
                    {
                        if (search.SearchList != null && search.SearchList.Any())
                        {
                            search.emptyText.Visibility = Visibility.Hidden;
                        }
                        else 
                        {
                            search.emptyText.Visibility = Visibility.Visible;
                        }
                    }
                })));

        public object SelectedSearch
        {
            get { return (object)GetValue(SelectedSearchProperty); }
            set { SetValue(SelectedSearchProperty, value); }
        }
        public static readonly DependencyProperty SelectedSearchProperty =
            DependencyProperty.Register("SelectedSearch", typeof(object), typeof(SearchBox),
                new FrameworkPropertyMetadata(new PropertyChangedCallback((d,e) => 
                {
                    if (d is SearchBox search)
                    {
                        search._canExecuteCommand = false;

                        if (search.SelectedSearch != null)
                        {
                            Type type = search.SelectedSearch.GetType();

                            PropertyInfo pinfo = type.GetProperty(search.DisplayMemberPath);

                            if (pinfo != null)
                            {
                                object value = pinfo.GetValue(search.SelectedSearch);
                                search.SearchText = value == null ? string.Empty : value.ToString();
                            }
                            else 
                            {
                                search.SearchText = search.SelectedSearch.ToString();
                            }
                        }
                    }
                })));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(SearchBox));



        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(SearchBox));




        public bool IsDynamicSearch
        {
            get { return (bool)GetValue(IsDynamicSearchProperty); }
            set { SetValue(IsDynamicSearchProperty, value); }
        }
        public static readonly DependencyProperty IsDynamicSearchProperty =
            DependencyProperty.Register("IsDynamicSearch", typeof(bool), typeof(SearchBox));



        #endregion Depenencies

        private bool _canExecuteCommand;


        public SearchBox()
        {
            InitializeComponent();
        }

        private void searchText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(SearchList != null && SearchList.Any())
            { 
                popuplist.IsOpen = true;
            }
        }

        private void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (popuplist.IsOpen)
                {
                    popuplist.IsOpen = false;
                    SearchList = null;
                }
                else
                {
                    SearchCommand?.Execute(SearchText);
                    popuplist.IsOpen = true;
                }
            }
            else if (e.Key == Key.Down && popuplist.IsOpen && SearchList != null && SearchList.Any())
            {
                if (SelectedSearch == null)
                {
                    SelectedSearch = SearchList.First();
                }
                else
                {
                    int index = Array.IndexOf(SearchList.ToArray(), SelectedSearch);

                    if (index == SearchList.Count() - 1)
                    {
                        SelectedSearch = null;
                    }
                    else
                    {
                        SelectedSearch = SearchList.ElementAt(index + 1);
                    }
                }
            }
            else if (e.Key == Key.Up && popuplist.IsOpen && SearchList != null && SearchList.Any())
            {
                if (SelectedSearch == null)
                {
                    SelectedSearch = SearchList.Last();
                }
                else
                {
                    int index = Array.IndexOf(SearchList.ToArray(), SelectedSearch);

                    if (index == 0)
                    {
                        SelectedSearch = null;
                    }
                    else
                    {
                        SelectedSearch = SearchList.ElementAt(index - 1);
                    }
                }
            }
        }

        private void ListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popuplist.IsOpen = false;
            SearchList = null;
        }

        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popuplist.IsOpen = true;
        }
    }
}
