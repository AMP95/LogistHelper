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
                            search._canExecuteCommand = false;
                        }

                        if (search._canExecuteCommand)
                        {
                            search.loader.IsInProcess = true;

                            search.SearchCommand?.Execute(search.SearchText);

                            search.loader.IsInProcess = false;
                        }

                        search._canExecuteCommand = true;
                    }
                })));


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
                        search.popuplist.IsOpen = true;

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




        #endregion Depenencies

        private bool _canExecuteCommand;


        public SearchBox()
        {
            InitializeComponent();
        }

        private void searchText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SearchList != null && SearchList.Any()) 
            {
                popuplist.IsOpen = true;
            }
        }

        private void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                popuplist.IsOpen = false;
            }
            else if (e.Key == Key.Down) 
            {
                if (popuplist.IsOpen && SearchList != null && SearchList.Any()) 
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
            }
            else if (e.Key == Key.Up)
            {
                if (popuplist.IsOpen && SearchList != null && SearchList.Any())
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
        }

        private void ListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popuplist.IsOpen = false;
        }
    }
}
