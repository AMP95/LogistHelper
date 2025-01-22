using CommunityToolkit.Mvvm.ComponentModel;

namespace LogistHelper.UI.CustomControls.FileDrag
{
    public class FileViewModel : ObservableObject
    {
        public Guid Id { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
    }
}
