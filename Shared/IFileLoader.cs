namespace Utilities
{
    public interface IFileLoader
    {
        Task<bool> DownloadFiles(string downloadPath, IEnumerable<object> filesData);
        Task<bool> DownloadFile(string downloadPath, object filesData);
        Task<bool> UploadFiles(Guid entityID, IEnumerable<object> viewModels);
    }
}
