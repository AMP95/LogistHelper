using Dadata;
using Dadata.Model;
using HelpAPIs.Settings;
using Models.Suggest;
using Shared;
using Utilities;

namespace HelpAPIs
{
    public class DaDataClient : IDataSuggest<CompanySuggestItem>, IDataSuggest<FmsSuggestItem>
    {
        private ILogger _logger;
        private string _apiKey;

        public DaDataClient(ILogger logger, ISettingsRepository<ApiSettings> repository)
        {
            _logger = logger;
            _apiKey = repository.GetSettings().DaDataApiKey;
        }

        public async Task<IEnumerable<CompanySuggestItem>> SuggestAsync(string searchString)
        {
            try
            {
                SuggestClientAsync api = new SuggestClientAsync(_apiKey);
                var response = await api.SuggestParty(searchString);
                return response.suggestions.Select(s => new CompanySuggestItem() { Name = s.value, Inn = s.data.inn, Kpp = s.data.kpp, Address = s.data.address.value });
            }
            catch (Exception ex)
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
            }

            return Enumerable.Empty<CompanySuggestItem>();
        }


        async Task<IEnumerable<FmsSuggestItem>> IDataSuggest<FmsSuggestItem>.SuggestAsync(string searchString)
        {
            try
            {
                OutwardClientAsync api = new OutwardClientAsync(_apiKey);
                var response = await api.Suggest<FmsUnit>(searchString);
                return response.suggestions.Select(s => new FmsSuggestItem() { Code = s.data.code, Name = s.data.name });
            }
            catch (Exception ex)
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
            }

            return Enumerable.Empty<FmsSuggestItem>();
        }
    }
}
