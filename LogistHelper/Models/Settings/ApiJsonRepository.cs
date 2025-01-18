using HelpAPIs.Settings;
using LogistHelper.Models.Settings;
using Shared;
using System.IO;
using System.Text.Json;

namespace LogistHelper.Models
{
    public class ApiJsonRepository : JsonRepository<ApiSettings>
    {
        public ApiJsonRepository(ILogger logger) : base(logger, "apiSettings.txt") { }

        protected override ApiSettings GetDegault()
        {
           return new ApiSettings()
           {
               ServerUri = AppSettings.Default.DefaultServerUri,
               DaDataApiKey = AppSettings.Default.DefaultDaDataKey,
               YandexGeoSuggestApiKey = AppSettings.Default.DefaultYandexKey,
           };
        }
    }
}
