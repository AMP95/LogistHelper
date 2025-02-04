using CommunityToolkit.Mvvm.Input;
using DTOs.Dtos;
using LogistHelper.ViewModels.Base;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.Windows.Input;

namespace LogistHelper.ViewModels.Views
{

    public class EditTemplateViewModel : MainEditViewModel<TemplateDto>
    {
        #region Private

        private TeplateViewModel _template;

        #endregion Private

        #region Public



        #endregion Public

        public ICommand RemoveAdditionalCommand { get; set; }
        public ICommand AddAdditionalCommand { get; set; }

        public EditTemplateViewModel(IDataAccess dataAccess, 
                                     IViewModelFactory<TemplateDto> factory, 
                                     IMessageDialog dialog) : base(dataAccess, factory, dialog)
        {
            
            RemoveAdditionalCommand = new RelayCommand<AdditionalsViewModel>(async (additional) =>
            {
                _template.Additionals.Remove(additional);
            });

            AddAdditionalCommand = new RelayCommand(() => 
            {
                _template.Additionals.Add(new AdditionalsViewModel());
            });
        }

        public override async Task Load(Guid id)
        {
            await base.Load(id);

            _template = EditedViewModel as TeplateViewModel;
        }

        public override bool CheckSave()
        {
            if (string.IsNullOrWhiteSpace(_template.Name)) 
            {
                _dialog.ShowError($"Необходимо указать название шаблона");
                return false;
            }
            if (_template.Additionals.Any(a => string.IsNullOrWhiteSpace(a.Name) || string.IsNullOrWhiteSpace(a.Description))) 
            {
                _dialog.ShowError($"Необходимо полностью заполнить дополнительные требования");
                return false;
            }
            return true;
        }
    }
}
