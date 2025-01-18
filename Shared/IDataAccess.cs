namespace Shared
{
    public interface IAccessResult<T>
    {
        string ErrorMessage { get; set; }
        bool IsSuccess { get; set; }
        T Result { get; set; }
    }

    public interface IDataAccess
    {
        Task<IAccessResult<T>> GetIdAsync<T>(Guid id);
        Task<IAccessResult<IEnumerable<T>>> GetFilteredAsync<T>(string propertyName, params string[] param);
        Task<IAccessResult<IEnumerable<T>>> GetRangeAsync<T>(int start, int end);
        Task<IAccessResult<bool>> AddAsync<T>(T value);
        Task<IAccessResult<bool>> UpdateAsync<T>(T value);
        Task<IAccessResult<bool>> UpdatePropertyAsync<T>(Guid id, params KeyValuePair<string, object>[] updates);
        Task<IAccessResult<bool>> DeleteAsync<T>(Guid id);
    }
}