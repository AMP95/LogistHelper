using LogistHelper.ViewModels.DataViewModels;

namespace LogistHelper.Models
{
    public class LoadPackage
    {
        public string SavePath { get; set; }
        public IEnumerable<FileViewModel> FileToLoad { get; set; }
    }
}
