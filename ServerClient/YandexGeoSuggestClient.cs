using HelpAPIs.Settings;
using Models.Sugget;
using Shared;
using System.Net.Http.Json;
using Utilities;

namespace HelpAPIs
{
    public class YandexGeoSuggestClient : IDataSuggest<GeoSuggestItem>
    {
        private string _apiKey;
        private ILogger _logger;

        public YandexGeoSuggestClient(ISettingsRepository<ApiSettings> repository, ILogger logger)
        {
            _logger = logger;
            _apiKey = repository.GetSettings().YandexGeoSuggestApiKey;
        }

        public async Task<IEnumerable<GeoSuggestItem>> SuggestAsync(string searchString)
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
                                IEnumerable<GeoSuggestItem> results = geoResult.results.Where(r => !r.Address.Component.Any(c => c.Kind.Any(k => k == "ENTRANCE"))).
                                                                                          Select(r => new GeoSuggestItem()
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

            return Enumerable.Empty<GeoSuggestItem>();
        }
    }

    #region GeoClasses

    internal class GeoSuggestResponse
    {
        public GeoResponseResult[] results { get; set; }
    }

    internal class GeoResponseResult
    {
        public GeoTitle Title { get; set; }
        public GeoSubtitle Subtitle { get; set; }
        public List<string> Tags { get; set; }
        public GeoDistanse Distance { get; set; }
        public GeoAddress Address { get; set; }

        public string Uri { get; set; }
    }

    internal class GeoTitle
    {
        public string Text { get; set; }
        public List<GeoTiTleHl> Hl { get; set; }
    }

    internal class GeoTiTleHl
    {
        public int Begin { get; set; }
        public int End { get; set; }
    }

    internal class GeoSubtitle
    {
        public string Text { get; set; }
    }

    internal class GeoDistanse
    {
        public string Text { get; set; }
        public double Value { get; set; }
    }

    internal class GeoAddress
    {
        public string Formatted_address { get; set; }
        public List<GeoAddressComponent> Component { get; set; }
    }

    internal class GeoAddressComponent
    {
        public string Name { get; set; }
        public string[] Kind { get; set; }
    }

    #endregion GeoClasses
}
