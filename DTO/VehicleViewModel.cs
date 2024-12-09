using CommunityToolkit.Mvvm.ComponentModel;

namespace DTO
{
    public class VehiclePartViewModel : ObservableObject
    {
        private string _model;
        private string _number;

        public Guid Id { get; set; }
        public Guid CarierId { get; set; }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
    }

    public class VehicleViewModel : ObservableObject
    {
        public Guid TruckId { get; set; }
        public Guid TrailerId { get; set; }
        public Guid CarierId { get; set; }
        public string Number { get; set; }
    }
}
