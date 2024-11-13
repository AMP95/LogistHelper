using CommunityToolkit.Mvvm.ComponentModel;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class TractorViewModel : ObservableObject
    {
        private string _issuer;
        private string _number;
        private string _name;

        public string Issuer 
        { 
            get => _issuer;
            private set => SetProperty(ref _issuer, value);
        }

        public string Number 
        {
            get => _number;
            private set => SetProperty(ref _number, value);
        }

        public string Name 
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }
    }

    public class TrailerViewModel : ObservableObject
    {
        private string _issuer;
        private string _number;
        private string _name;

        public string Issuer
        {
            get => _issuer;
            private set => SetProperty(ref _issuer, value);
        }

        public string Number
        {
            get => _number;
            private set => SetProperty(ref _number, value);
        }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }
    }

    public class VehicleViewModel : ObservableObject
    {
        private TractorViewModel _tractor;
        private TrailerViewModel _trailer;


        public TrailerViewModel Trailer 
        {
            get => _trailer;
            set => SetProperty(ref _trailer, value);
        }

        public TractorViewModel Tractor
        {
            get => _tractor;
            set => SetProperty(ref _tractor, value);
        }
    }
}
