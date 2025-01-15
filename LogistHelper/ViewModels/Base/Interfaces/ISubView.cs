using DTOs.Dtos;

namespace LogistHelper.ViewModels.Base.Interfaces
{
    public interface ISubListView<T> where T : IDto
    {
        ISubMenuPage<T> SubParent { get; set; }
        Guid MainId { get; set; }
        Task Load(Guid mainId);
    }

    public interface ISubEditView<T> where T : IDto
    {
        ISubMenuPage<T> SubParent { get; set; }
        Task Load(Guid subId, Guid mainId);
    }
}
