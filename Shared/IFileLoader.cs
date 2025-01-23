namespace Utilities
{
    public interface IFileLoader
    {
        Task<bool> DownloadFiles(string downloadPath, IEnumerable<Guid> fileGuids);
        Task<bool> UploadFiles(Guid entityID, IEnumerable<object> viewModels);
    }
}
