using CommunityToolkit.Mvvm.ComponentModel;
using Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace LogistHelper.Services
{
    public class GeoSuggestResult : ObservableObject
    {
        private string _location;
        private string _fullAddress;

        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
        public string FullAddress
        {
            get => _fullAddress;
            set => SetProperty(ref _fullAddress, value);
        }
    }
    public static class GeoSuggestService
    {
        private static string _apiKey;
        private static ILogger _logger;
        static GeoSuggestService()
        {
            _apiKey = ContainerService.SettingsRepository.GetSettings().YandexGeoSuggestApiKey;
            _logger = ContainerService.Logger;
        }

        public static async Task<IEnumerable<GeoSuggestResult>> GetSuggestions(string searchString)
        {
            try
            {
                using (HttpClientHandler clientHandler = new HttpClientHandler()
                { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } })
                {
                    using (HttpClient client = new HttpClient(clientHandler))
                    {
                        string route = $"https://suggest-maps.yandex.ru/v1/suggest?apikey={_apiKey}&text={searchString}&lang=ru&results=10&print_address=1";

                        using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, route))
                        {
                            var response = await client.SendAsync(message);

                            if (response.IsSuccessStatusCode)
                            {
                                GeoSuggestResponse geoResult = await response.Content.ReadFromJsonAsync<GeoSuggestResponse>();
                                IEnumerable<GeoSuggestResult> results = geoResult.results.Where(r => !r.Address.Component.Any(c => c.Kind.Any(k => k == "ENTRANCE"))).
                                                                                          Select(r => new GeoSuggestResult()
                                                                                          {
                                                                                              FullAddress = r.Address.Formatted_address,
                                                                                              Location = r.Address.Component.FirstOrDefault(c => c.Kind.Any(k => k == "LOCALITY"))?.Name
                                                                                          });
                                return results;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
            }

            return Enumerable.Empty<GeoSuggestResult>();
        }
    }

    #region GeoClasses

    public class GeoSuggestResponse
    {
        public GeoResponseResult[] results { get; set; }
    }

    public class GeoResponseResult
    {
        public GeoTitle Title { get; set; }
        public GeoSubtitle Subtitle { get; set; }
        public List<string> Tags { get; set; }
        public GeoDistanse Distance { get; set; }
        public GeoAddress Address { get; set; }

        public string Uri { get; set; }
    }

    public class GeoTitle
    {
        public string Text { get; set; }
        public List<GeoTiTleHl> Hl { get; set; }
    }

    public class GeoTiTleHl
    {
        public int Begin { get; set; }
        public int End { get; set; }
    }

    public class GeoSubtitle
    {
        public string Text { get; set; }
    }

    public class GeoDistanse
    {
        public string Text { get; set; }
        public double Value { get; set; }
    }

    public class GeoAddress
    {
        public string Formatted_address { get; set; }
        public List<GeoAddressComponent> Component { get; set; }
    }

    public class GeoAddressComponent
    {
        public string Name { get; set; }
        public string[] Kind { get; set; }
    }

    #endregion GeoClasses
}
