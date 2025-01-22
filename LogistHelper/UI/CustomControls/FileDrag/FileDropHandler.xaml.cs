using LogistHelper.UI.CustomControls.FileDrag;
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
        public ObservableCollection<FileViewModel> Files
        {
            get { return (ObservableCollection<FileViewModel>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }

        public static readonly DependencyProperty FilesProperty =
            DependencyProperty.Register("Files", typeof(ObservableCollection<FileViewModel>), typeof(FileDropHandler));


        public ICommand DownloadCommand
        {
            get { return (ICommand)GetValue(DownloadCommandProperty); }
            set { SetValue(DownloadCommandProperty, value); }
        }

        public static readonly DependencyProperty DownloadCommandProperty =
            DependencyProperty.Register("DownloadCommand", typeof(ICommand), typeof(FileDropHandler));



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
                        FileToLoad = Files.Where(f => f.Id != Guid.Empty)
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
                        FileToLoad = Files.Where(f => f.Id != guid)
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
                    Files.Add(new FileViewModel()
                    {
                        FullPath = file,
                        Name = Path.GetFileNameWithoutExtension(file),
                        Extension = Path.GetExtension(file)
                    });
                }
            }
        }
        
    }
}
