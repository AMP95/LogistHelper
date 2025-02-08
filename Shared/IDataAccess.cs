﻿namespace Shared
{
    public interface IAccessResult<T>
    {
        bool IsSuccess { get; set; }
        string ErrorMessage { get; set; }
        T Result { get; set; }
    }

    public interface IDataAccess
    {
        Task<IAccessResult<T>> GetIdAsync<T>(Guid id);
        Task<IAccessResult<IEnumerable<T>>> GetFilteredAsync<T>(string propertyName, params string[] param);
        Task<IAccessResult<IEnumerable<T>>> GetRangeAsync<T>(int start, int end);
        Task<IAccessResult<Guid>> AddAsync<T>(T value);
        Task<IAccessResult<bool>> UpdateAsync<T>(T value);
        Task<IAccessResult<bool>> UpdatePropertyAsync<T>(Guid id, params KeyValuePair<string, object>[] updates);
        Task<IAccessResult<bool>> DeleteAsync<T>(Guid id);
        Task<IAccessResult<Guid>> SendMultipartAsync(MultipartFormDataContent content);
        Task<IAccessResult<bool>> DownloadFileAsync(Guid fileId, string fullPath);
        Task<IAccessResult<bool>> UploadFileAsync<T>(Guid entityId, T fileDto, string fullFilePath);
        Task<IAccessResult<IEnumerable<T>>> GetRequiredPayments<T>();
        Task<IAccessResult<T>> Login<T>(T dto);
        Task Logout();
    }
}