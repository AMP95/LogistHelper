namespace LogistHelper.Models.Settings
{
    public class Settings
    {
        public string ServerUri { get; set; }
        public string DaDataApiKey { get; set; }
        public string DaDataSecretKey { get; set; }
        public string YandexGeoSuggestApiKey { get; set; }

        public List<CarModelSearch> TruckModels { get; set; }
        public List<CarModelSearch> TrailerModels { get; set; }
    }
}
