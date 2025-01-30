using Shared;

namespace Utilities
{
    public interface IAuthDialog<T>
    {
        bool ShowPasswordChange(T user, IDataAccess access, IHashService hashService);
    }
}
