using CommunityToolkit.Mvvm.ComponentModel;

namespace DTO
{
    public class DriverViewModel : ObservableObject
    {
        #region Private

        private string _name;
        private string _familyName;
        private string _fatherName;
        private PassportViewModel _passport;
        private DateTime _birthDate;
        private VehicleViewModel _vehicle;
        private string _phones;

        #endregion Private

        public Guid Id { get; set; }
        public Guid CarierId { get; set; }

        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string FamilyName
        {
            get => _familyName;
            set => SetProperty(ref _familyName, value);
        }
        public string FatherName
        {
            get => _fatherName;
            set => SetProperty(ref _fatherName, value);
        }

        public DateTime DateOfBirth
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }
        public PassportViewModel Passport
        {
            get => _passport;
            set => SetProperty(ref _passport, value);
        }
        public VehicleViewModel Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }
        public string Phones
        {
            get => _phones;
            set => SetProperty(ref _phones, value);
        }
    }
}
