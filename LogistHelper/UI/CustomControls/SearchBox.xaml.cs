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
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchBox));


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
                            search.listToggle.Visibility = Visibility.Visible;
                            search.listToggle.IsChecked = true;
                        }
                        else
                        {
                            search.listToggle.Visibility = Visibility.Collapsed;
                            search.listToggle.IsChecked = false;
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
                        if (search.SelectedSearch != null)
                        {
                            search.listToggle.IsChecked = false;

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
        public SearchBox()
        {
            InitializeComponent();
        }

        private void searchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                SearchCommand.Execute(null);
            }
        }
    }
}
