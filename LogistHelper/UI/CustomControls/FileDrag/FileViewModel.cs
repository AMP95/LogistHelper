using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;

namespace LogistHelper.UI.CustomControls.FileDrag
{
    public class FileViewModel : ObservableObject
    {
        public Guid Id { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public BitmapImage Thumbnail { get; set; }
    }
}
