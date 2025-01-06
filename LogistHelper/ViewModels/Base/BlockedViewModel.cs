using CommunityToolkit.Mvvm.ComponentModel;

namespace LogistHelper.ViewModels.Base
{
    public class BlockedViewModel : ObservableObject
    {
        private bool _isBlock;
        private string _blockText;

        public bool IsBlock
        {
            get => _isBlock;
            set => SetProperty(ref _isBlock, value);
        }

        public string BlockText
        {
            get => _blockText;
            set => SetProperty(ref _blockText, value);
        }
    }
}
