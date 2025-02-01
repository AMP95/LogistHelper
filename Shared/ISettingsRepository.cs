namespace Shared
{
    public interface ISettingsRepository<T>
    {
        bool SaveSettings(T settings);
        T GetSettings();
        void ResetSettings();
    }
}
