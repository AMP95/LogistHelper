using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Models.SubClasses;

namespace DTO
{
    public class ContractViewModel : ObservableObject
    {
        #region Private

        private short _number;
        private DateTime _creationDate;
        private ContractStatus _status;
        private RoutePoint _loadingPoint;
        private RoutePoint _unloadingPoint;
        private float _weight;
        private float _volume;
        private CarrierViewModel _carrier;
        private DriverViewModel _driver;
        private VehiclePartViewModel _truck;
        private VehiclePartViewModel _trailer;
        private float _payment;
        private float _prepayment;
        private PaymentPriority _payPriority;
        private RecievingType _paymentCondition;
        private BookingDataViewModel _bookingData;

        #endregion Private

        public Guid Id { get; set; }
        public short Number 
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
        public DateTime CreationDate
        {
            get => _creationDate;
            set => SetProperty(ref _creationDate, value);
        }
        public ContractStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        public RoutePoint LoadingPoint
        {
            get => _loadingPoint;
            set => SetProperty(ref _loadingPoint, value);
        }
        public RoutePoint UnloadingPoint
        {
            get => _unloadingPoint;
            set => SetProperty(ref _unloadingPoint, value);
        }
        public float Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }
        public float Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value);
        }
        public CarrierViewModel Carrier
        {
            get => _carrier;
            set => SetProperty(ref _carrier, value);
        }
        public DriverViewModel Driver
        {
            get => _driver;
            set => SetProperty(ref _driver, value);
        }
        public VehiclePartViewModel Truck
        {
            get => _truck;
            set => SetProperty(ref _truck, value);
        }
        public VehiclePartViewModel Trailer
        {
            get => _trailer;
            set => SetProperty(ref _trailer, value);
        }
        public float Payment
        {
            get => _payment;
            set => SetProperty(ref _payment, value);
        }
        public float Prepayment
        {
            get => _prepayment;
            set => SetProperty(ref _prepayment, value);
        }
        public PaymentPriority PayPriority
        {
            get => _payPriority;
            set => SetProperty(ref _payPriority, value);
        }
        public RecievingType PaymentCondition
        {
            get => _paymentCondition;
            set => SetProperty(ref _paymentCondition, value);
        }
        public BookingDataViewModel BookingData
        {
            get => _bookingData;
            set => SetProperty(ref _bookingData, value);
        }
    }
}
