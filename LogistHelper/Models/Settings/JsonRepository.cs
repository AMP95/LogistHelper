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

            string str = JsonSerializer.Serialize<Settings>(_settings);
            try
            {
                using (var file = File.Open(_fileName, FileMode.Truncate)) 
                {
                    using (StreamWriter writer = new StreamWriter(file)) 
                    {
                        writer.WriteLine(str);
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex) 
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
            }
        }
    }
}
