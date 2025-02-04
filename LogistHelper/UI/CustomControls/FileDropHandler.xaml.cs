using LogistHelper.Models;
using LogistHelper.Services;
using LogistHelper.ViewModels.DataViewModels;
using Microsoft.Win32;
using Shared;
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
                        handler.UpdateLoadAllowance();
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





        public int AllowableFileCount
        {
            get { return (int)GetValue(AllowableFileCountProperty); }
            set { SetValue(AllowableFileCountProperty, value); }
        }

        public static readonly DependencyProperty AllowableFileCountProperty =
            DependencyProperty.Register("AllowableFileCount", typeof(int), typeof(FileDropHandler), new PropertyMetadata(10));




        public List<string> AllowableFileExtensions
        {
            get { return (List<string>)GetValue(AllowableFileExtensionsProperty); }
            set { SetValue(AllowableFileExtensionsProperty, value); }
        }

        public static readonly DependencyProperty AllowableFileExtensionsProperty =
            DependencyProperty.Register("AllowableFileExtensions", typeof(List<string>), typeof(FileDropHandler), new PropertyMetadata(new List<string>() 
            {
                ".doc", ".docx", ".pdf", ".txt", ".png", ".jpg", ".jpeg", ".rar", ".zip"
            }));




        public int MaxFileSizeInMb
        {
            get { return (int)GetValue(MaxFileSizeInMbProperty); }
            set { SetValue(MaxFileSizeInMbProperty, value); }
        }
        public static readonly DependencyProperty MaxFileSizeInMbProperty =
            DependencyProperty.Register("MaxFileSizeInMb", typeof(int), typeof(FileDropHandler), new PropertyMetadata(10));




        public bool IsAllowToAddFiles
        {
            get { return (bool)GetValue(IsAllowToAddFilesProperty); }
            set { SetValue(IsAllowToAddFilesProperty, value); }
        }
        public static readonly DependencyProperty IsAllowToAddFilesProperty =
            DependencyProperty.Register("IsAllowToAddFiles", typeof(bool), typeof(FileDropHandler), new PropertyMetadata(true));



        private IMessageDialog _dialog;

        public FileDropHandler()
        {
            InitializeComponent();

            _dialog = (IMessageDialog)ContainerService.Services.GetService(typeof(IMessageDialog));
        }

        private void DownloadAllButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button) 
            { 
                OpenFolderDialog saveFile = new OpenFolderDialog();

                if (saveFile.ShowDialog() == true) 
                {
                    LoadPackage package = new LoadPackage()
                    {
                        SavePath = Path.GetDirectoryName(saveFile.FolderName),
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
                OpenFolderDialog saveFile = new OpenFolderDialog();

                if (saveFile.ShowDialog() == true)
                {
                    Guid guid = (Guid)button.Tag;

                    LoadPackage package = new LoadPackage()
                    {
                        SavePath = Path.GetDirectoryName(saveFile.FolderName),
                        FileToLoad = Files.Where(f => f.Id == guid).Select(f => f.Item).ToList()
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
                if (AllowableFileCount > 1)
                {
                    openFile.Multiselect = true;
                }
                string filter = string.Empty;

                foreach (string ext in AllowableFileExtensions) 
                {
                    filter += $"(*{ext}) | *{ext}|";
                }

                openFile.Filter = filter.Remove(filter.Length - 1);

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
            if (Files.Count() +  files.Count() > AllowableFileCount)
            {
                _dialog.ShowError($"Превышено максимальное количество файлов - {AllowableFileCount}");
                return;
            }

            if (Files != null)
            {
                foreach (var file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    long bytes = fileInfo.Length;

                    if (bytes > MaxFileSizeInMb * 1048576) 
                    {
                        _dialog.ShowError($"{fileInfo.Name}: Превышен максимальный размер файла - {MaxFileSizeInMb} Мб");
                        continue;
                    }

                    if (!AllowableFileExtensions.Any(e => e.ToLower() == fileInfo.Extension)) 
                    {
                        _dialog.ShowError($"{fileInfo.Name}: Недопустимое расширение файла");
                        continue;
                    }

                    Files.Add(new ListItem<FileViewModel>(new FileViewModel() { LocalFullFilePath = file }));
                }
            }
            
            UpdateLoadAllowance();
        }

        private void UpdateLoadAllowance() 
        {
            if (Files != null && Files.Count() >= AllowableFileCount)
            {
                IsAllowToAddFiles = false;
            }
            else
            {
                IsAllowToAddFiles = true;
            }
        }
    }
}
