using CommunityToolkit.Mvvm.ComponentModel;

namespace DTO
{
    public class PassportViewModel : ObservableObject
    {
        #region Private

        private Guid _id;
        private string _serialNumber;
        private string _issuer;
        private DateTime _dateOfIssue;

        #endregion Private

        #region Public

        public Guid Id 
        {
            get => _id;
            set => _id = value;
        }

        public string SerialNumber 
        {
            get => _serialNumber;
            set 
            {
                if (_serialNumber.Length < value.Length && value.Length >= 5 && Char.IsDigit(value[4])) 
                {
                    value.Insert(4, " ");
                }
                SetProperty(ref _serialNumber, value);
            }
        }

        public string Issuer 
        {
            get => _issuer;
            set => SetProperty(ref _issuer, value);
        }

        public DateTime DateOfIssue 
        {
            get => _dateOfIssue;
            set => SetProperty(ref _dateOfIssue, value);
        }

        #endregion Public


    }
}
