using HelpAPIs.Settings;
using LogistHelper.Models.Settings;
using Shared;

namespace LogistHelper.Models
{
    public class ApiJsonRepository : JsonRepository<ApiSettings>
    {
        public ApiJsonRepository(ILogger logger) : base(logger, "apiSettings.txt") { }

        protected override ApiSettings GetDefault()
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
