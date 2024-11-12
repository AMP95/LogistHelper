namespace Shared.Logger
{
    public interface ISettingsRepository<T>
    {
        void SaveSettings(T settings);
        T GetSettings();
        void ResetSettings();
    }
}
