namespace Utilities
{
    public interface IFileLoader<T>
    {
        Task<bool> DownloadFiles(string downloadPath, IEnumerable<T> filesData);
        Task<bool> DownloadFile(string downloadPath, T filesData);
        Task<bool> UploadFiles(Guid entityID, IEnumerable<T> filesData);
        Task<bool> UploadFile(Guid entityID, T fileData);
    }
}
