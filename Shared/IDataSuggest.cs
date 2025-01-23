namespace Utilities
{
    public interface IDataSuggest<T> 
    {
        Task<IEnumerable<T>> SuggestAsync(string searchString);
    }
}
