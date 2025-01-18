using Shared;

namespace HelpAPIs
{
    internal class AccessResult<T> : IAccessResult<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Result { get; set; }
    }
}
