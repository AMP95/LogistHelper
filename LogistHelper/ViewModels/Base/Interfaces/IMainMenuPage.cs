using DTOs.Dtos;

namespace LogistHelper.ViewModels.Base
{
    public interface IMainMenuPage<T> where T : IDto
    {
        Task SwitchToList();
        Task SwitchToEdit(Guid id);
    }
}
