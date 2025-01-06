using Shared;
using System.IO;
using System.Text.Json;

namespace LogistHelper.Models.Settings
{
    public class JsonRepository : ISettingsRepository<Settings>
    {
        private string _fileName = "settings.txt";
        private Settings _settings;
        private ILogger _logger;

        public JsonRepository(ILogger logger)
        {
            _logger = logger;
            InitSettings();
        }

        private void InitSettings() 
        {
            if (!File.Exists(_fileName)) 
            {
                Settings settings = new Settings()
                {
                    ServerUri = "https://localhost:7081/api",
                    DaDataApiKey = "00475e8fb9e3d1877e8b9e0d5d5f269c2a5a7f90",
                    DaDataSecretKey = "a9bf357e95073eff9a20f62532fb0db1ebfa7bc3"
                };

                SaveSettings(settings);    
            }
        }

        public Settings GetSettings()
        {
            if (_settings == null) 
            {
                try 
                {
                    string str = File.ReadAllText(_fileName);
                    _settings = JsonSerializer.Deserialize<Settings>(str);
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

        public void SaveSettings(Settings settings)
        {
            _settings = settings;
            try
            {
                using (var file = File.OpenWrite(_fileName)) 
                {
                    JsonSerializer.Serialize<Settings>(file, _settings);
                }
            }
            catch (Exception ex) 
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
            }
        }
    }
}
