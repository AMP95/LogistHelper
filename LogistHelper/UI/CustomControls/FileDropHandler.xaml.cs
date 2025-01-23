using LogistHelper.Models;
using LogistHelper.ViewModels.DataViewModels;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogistHelper.UI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для FileDropHandler.xaml
    /// </summary>
    public partial class FileDropHandler : UserControl
    {
        public ObservableCollection<ListItem<FileViewModel>> Files
        {
            get { return (ObservableCollection<ListItem<FileViewModel>>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }

        public static readonly DependencyProperty FilesProperty =
            DependencyProperty.Register("Files", typeof(ObservableCollection<ListItem<FileViewModel>>), typeof(FileDropHandler),
                new FrameworkPropertyMetadata(new PropertyChangedCallback((d,e) => 
                {
                    if (d is FileDropHandler handler) 
                    {
                        if (e.OldValue != null) 
                        { 
                            ((ObservableCollection<ListItem<FileViewModel>>)e.OldValue).CollectionChanged -= handler.Files_CollectionChanged;
                        }

                        if (handler.Files != null)
                        {
                            handler.Files.CollectionChanged += handler.Files_CollectionChanged;
                        }
                    }
                })));

        

        public ICommand DownloadCommand
        {
            get { return (ICommand)GetValue(DownloadCommandProperty); }
            set { SetValue(DownloadCommandProperty, value); }
        }

        public static readonly DependencyProperty DownloadCommandProperty =
            DependencyProperty.Register("DownloadCommand", typeof(ICommand), typeof(FileDropHandler));




        public ICommand RemoveCommand
        {
            get { return (ICommand)GetValue(RemoveCommandProperty); }
            set { SetValue(RemoveCommandProperty, value); }
        }
        public static readonly DependencyProperty RemoveCommandProperty =
            DependencyProperty.Register("RemoveCommand", typeof(ICommand), typeof(FileDropHandler));



        public FileDropHandler()
        {
            InitializeComponent();
        }

        private void DownloadAllButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button) 
            { 
                SaveFileDialog saveFile = new SaveFileDialog();

                if (saveFile.ShowDialog() == true) 
                {
                    LoadPackage package = new LoadPackage()
                    {
                        SavePath = Path.GetDirectoryName(saveFile.FileName),
                        FileToLoad = Files.Where(f => f.Item.Id != Guid.Empty).Select(f => f.Item)
                    };

                    DownloadCommand.Execute(package);
                }
            }
        }

        private void DownloadOneButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                SaveFileDialog saveFile = new SaveFileDialog();

                if (saveFile.ShowDialog() == true)
                {
                    Guid guid = (Guid)button.Tag;

                    LoadPackage package = new LoadPackage()
                    {
                        SavePath = Path.GetDirectoryName(saveFile.FileName),
                        FileToLoad = Files.Where(f => f.Item.Id != guid).Select(f => f.Item)
                    };

                    DownloadCommand.Execute(package);
                }
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button) 
            { 
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Multiselect = true;

                if (openFile.ShowDialog() == true) 
                {
                    FillFiles(openFile.FileNames);
                }
            }
        }

        private void items_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                FillFiles(files);
            }
        }


        private void FillFiles(string[] files) 
        {
            if (Files != null)
            {
                foreach (var file in files)
                {
                    Files.Add(new ListItem<FileViewModel>(new FileViewModel() { LocalFullFilePath = file }));
                }
            }
        }

        private void UpdateCrossGrid() 
        {
            if (Files.Any()) 
            { 
                crossGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                crossGrid.Visibility= Visibility.Visible;
            }
        }

        private void Files_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCrossGrid();
        }

    }
}
