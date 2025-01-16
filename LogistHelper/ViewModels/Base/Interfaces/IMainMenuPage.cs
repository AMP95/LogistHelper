using DTOs.Dtos;

namespace LogistHelper.ViewModels.Base
{
    public interface IMainMenuPage<T> where T : IDto
    {
        Task SwitchToList();
        Task SwitchToEdit(Guid id);
    }

    public interface IMainListView<T> where T : IDto
    {
        IMainMenuPage<T> MainParent { get; set; }
        Task Load();
    }

    public interface IMainEditView<T> where T : IDto
    {
        IMainMenuPage<T> MainParent { get; set; }
        Task Load(Guid id);
    }

    
}
