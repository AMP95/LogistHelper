using LogistHelper.UI.CustomControls.FileDrag;
using Microsoft.WindowsAPICodePack.Shell;
using Spire.Pdf;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LogistHelper.UI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для FileDropHandler.xaml
    /// </summary>
    public partial class FileDropHandler : UserControl
    {
        public IEnumerable<FileViewModel> Files
        {
            get { return (IEnumerable<FileViewModel>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }

        public static readonly DependencyProperty FilesProperty =
            DependencyProperty.Register("Files", typeof(IEnumerable<FileViewModel>), typeof(FileDropHandler),
                new FrameworkPropertyMetadata(new PropertyChangedCallback((d,e) => 
                {
                    if (d is FileDropHandler handler) 
                    { 
                        
                    }
                })));




        public FileDropHandler()
        {
            InitializeComponent();
        }

        public BitmapImage GetThumbnail(FileViewModel file)
        {
            switch (file.Extension)
            {
                case ".PDF": return GetPDFThumbnail(file.FullPath);
                case ".PNG":
                case ".BMP":
                case ".JPEG":
                case ".JPG": return GetImageThumbnail(file.FullPath);
                default: return GetApplicationIcon(file.FullPath);
            }
        }

        private BitmapImage GetPDFThumbnail(string fileName)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);
            using (Stream ms = doc.SaveAsImage(0))
            {
                ms.Position = 0;
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = ms;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.DecodePixelWidth = 150;
                bmi.EndInit();
                return bmi;
            }
        }

        private BitmapImage GetImageThumbnail(string fileName)
        {
            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.UriSource = new Uri(fileName);
            bmi.DecodePixelWidth = 150;
            bmi.EndInit();
            return bmi;
        }
        private BitmapImage GetApplicationIcon(string fileName)
        {
            BitmapSource bms = ShellFile.FromFilePath(fileName).Thumbnail.BitmapSource;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream ms = new MemoryStream();
            BitmapImage bmi = new BitmapImage();
            encoder.Frames.Add(BitmapFrame.Create(bms));
            encoder.Save(ms);
            ms.Position = 0;
            bmi.BeginInit();
            bmi.StreamSource = new MemoryStream(ms.ToArray());
            bmi.DecodePixelWidth = 150;
            bmi.EndInit();
            return bmi;
        }
    }
}
