using DTOs;
using DTOs.Dtos;
using LogistHelper.ViewModels.Base;

namespace LogistHelper.ViewModels.DataViewModels
{
    public class LogistViewModel : DataViewModel<LogistDto>
    {
        public string Name 
        { 
            get => _dto.Name;
            set 
            { 
                _dto.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Login
        {
            get => _dto.Login;
            set
            {
                _dto.Login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get => _dto.Password;
            set
            {
                _dto.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public LogistRole Role
        {
            get => _dto.Role;
            set
            {
                _dto.Role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public bool IsExpired
        {
            get => _dto.IsExpired;
            set
            {
                _dto.IsExpired = value;
                OnPropertyChanged(nameof(IsExpired));
            }
        }

        public PasswordState PasswordState
        {
            get => _dto.PasswordState;
            set
            {
                _dto.PasswordState = value;
                OnPropertyChanged(nameof(PasswordState));
            }
        }


        public LogistViewModel(LogistDto dto, int counter) : base(dto, counter) { }
        public LogistViewModel(LogistDto dto) : this(dto, 0) { }
        public LogistViewModel() : base() { }

        protected override void DefaultInit()
        {
            _dto = new LogistDto();
        }
    }


    public class LogistViewModelFactory : IViewModelFactory<LogistDto>
    {
        public DataViewModel<LogistDto> GetViewModel(LogistDto dto, int number)
        {
            return new LogistViewModel(dto, number);
        }

        public DataViewModel<LogistDto> GetViewModel(LogistDto dto)
        {
            return new LogistViewModel(dto);
        }

        public DataViewModel<LogistDto> GetViewModel()
        {
            return new LogistViewModel();
        }
    }
}
