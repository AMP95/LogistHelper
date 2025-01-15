using DTOs.Dtos;

namespace LogistHelper.ViewModels.Base.Interfaces
{
    public interface IMainListView<T> where T : IDto
    {
        IMainMenuPage<T> Parent { get; set; }
        Task Load();
    }

    public interface IMainEditView<T> where T : IDto
    {
        IMainMenuPage<T> Parent { get; set; }
        Task Load(Guid id);
    }
}
