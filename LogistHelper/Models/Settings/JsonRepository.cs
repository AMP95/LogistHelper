using Shared;
using System.IO;
using System.Text.Json;

namespace LogistHelper.Models.Settings
{
    public abstract class JsonRepository<T> : ISettingsRepository<T> where T : class
    {
        private string _filePath;
        private T _settings;
        private ILogger _logger;

        protected JsonRepository(ILogger logger, string filePath)
        {
            _filePath = filePath;
            _logger = logger;

            Init();
        }

        protected abstract T GetDefault();

        private void Init()
        {
            if (!File.Exists(_filePath))
            {
                _settings = GetDefault();

                SaveSettings(_settings);
            }
        }

        public T GetSettings()
        {
            if (_settings == null)
            {
                try
                {
                    string str = File.ReadAllText(_filePath);
                    _settings = JsonSerializer.Deserialize<T>(str);
                }
                catch (Exception ex)
                {
                    _logger.Log(ex, ex.Message, LogLevel.Error);
                }

            }
            return _settings;
        }

        public void ResetSettings()
        {
            _settings = null;
        }

        public bool SaveSettings(T settings)
        {
            _settings = settings;
            try
            {
                using (var file = File.OpenWrite(_filePath))
                {
                    JsonSerializer.Serialize<T>(file, _settings);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
                return false;
            }
        }
    }
}
