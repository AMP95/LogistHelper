using CommunityToolkit.Mvvm.ComponentModel;
using DTOs.Dtos;
using System.ComponentModel;

namespace LogistHelper.ViewModels.Base
{
    public class DataViewModel<TDto> : ObservableObject where TDto : IDto, IDataErrorInfo
    {
        private int _counter;

        protected TDto _dto;

        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
        }

        public Guid Id
        {
            get => _dto.Id;
        }

        #region Validation

        public string this[string columnName] => _dto[columnName];

        public string Error => _dto.Error;

        #endregion Validation

        public DataViewModel(TDto dto)
        {
            _dto = dto;
        }
        public DataViewModel(TDto dto, int counter)
        {
            _dto = dto;
            Counter = counter;
        }
        public DataViewModel()
        {
            
        }

        public virtual TDto GetDto()
        {
            return _dto;
        }
    }

    public interface IViewModelFactory<TDto> where TDto : IDto 
    {
        public DataViewModel<TDto> GetViewModel(TDto dto, int number);
        public DataViewModel<TDto> GetViewModel(TDto dto);
        public DataViewModel<TDto> GetViewModel();
    }
}
