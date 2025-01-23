using DTOs;
using HelpAPIs.Settings;
using Newtonsoft.Json;
using Shared;
using System.Net.Http.Json;

namespace HelpAPIs
{
    internal enum RequestStatus
    {
        Created,
        InProgress,
        Done,
        Unknown
    }

    public class ServerClient : IDataAccess
    {
        private string _url;
        private string _token;

        public ServerClient(ISettingsRepository<ApiSettings> settings)
        {
            _url = settings.GetSettings().ServerUri;
        }

        private async Task<IAccessResult<T>> GetRequest<T>(string route)
        {
            return await SendAsync<T>(HttpMethod.Get, $"{_url}/{route}");
        }

        public async Task<IAccessResult<T>> GetIdAsync<T>(Guid id)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await GetRequest<Guid>($"Get/{route}/id/{id}");
            return await GetResult<T>(result);
        }
                          
        public async Task<IAccessResult<IEnumerable<T>>> GetFilteredAsync<T>(string propertyName, params string[] param)                                                        
        {
            string route = GetRoute(typeof(T));

            string uri = $"Get/{route}/filter/{propertyName}";

            if (param.Any())
            {
                uri += "?param=" + string.Join("&param=", param);
            }

            IAccessResult<Guid> result = await GetRequest<Guid>(uri);
            return await GetResult<IEnumerable<T>>(result);
        }
                          
        public async Task<IAccessResult<IEnumerable<T>>> GetRangeAsync<T>(int start, int end)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await GetRequest<Guid>($"Get/{route}/range/{start}/{end}");
            return await GetResult<IEnumerable<T>>(result);
        }
                          
        public async Task<IAccessResult<Guid>> AddAsync<T>(T value)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await SendAsync<Guid>(HttpMethod.Post, $"{_url}/Add/{route}", JsonConvert.SerializeObject(value));
            return await GetResult<Guid>(result);
        }
                          
        public async Task<IAccessResult<bool>> UpdateAsync<T>(T value)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await SendAsync<Guid>(HttpMethod.Put, $"{_url}/Update/{route}", JsonConvert.SerializeObject(value));
            return await GetResult<bool>(result);
        }
                          
        public async Task<IAccessResult<bool>> UpdatePropertyAsync<T>(Guid id, params KeyValuePair<string, object>[] updates)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await SendAsync<Guid>(HttpMethod.Patch, $"{_url}/Patch/{route}/{id}", JsonConvert.SerializeObject(updates));
            return await GetResult<bool>(result);

        }
                          
        public async Task<IAccessResult<bool>> DeleteAsync<T>(Guid id)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await SendAsync<Guid>(HttpMethod.Delete, $"{_url}/Delete/{route}/{id}");
            return await GetResult<bool>(result);
        }

        private string GetRoute(Type type)
        {
            string typeName = type.Name;

            switch (typeName)
            {
                case nameof(VehicleDto): return "vehicle";
                case nameof(DriverDto): return "driver";
                case nameof(CarrierDto): return "carrier";
                case nameof(ClientDto): return "client";
                case nameof(RoutePointDto): return "route";
                case nameof(ContractDto): return "contract";
                case nameof(DocumentDto): return "document";
                case nameof(PaymentDto): return "payment";
                default: return string.Empty;
            }
        }

        private async Task<IAccessResult<T>> SendAsync<T>(HttpMethod method, string route, string jObject = null)
        {
            AccessResult<T> result = new AccessResult<T>();

            try
            {
                using (HttpClientHandler clientHandler = new HttpClientHandler()
                { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } })
                {
                    using (HttpClient client = new HttpClient(clientHandler))
                    {
                        using (HttpRequestMessage message = new HttpRequestMessage(method, route))
                        {
                            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                            if (jObject != null)
                            {
                                message.Content = new StringContent(jObject, encoding: System.Text.Encoding.UTF8, "application/json");
                            }

                            var response = await client.SendAsync(message);
                            if (response.IsSuccessStatusCode)
                            {
                                result.IsSuccess = true;
                                result.Result = await response.Content.ReadFromJsonAsync<T>();
                            }
                            else
                            {
                                result.IsSuccess = false;
                                string errorMessage = await response.Content.ReadAsStringAsync();
                                if (string.IsNullOrWhiteSpace(errorMessage))
                                {
                                    result.ErrorMessage = response.ReasonPhrase;
                                }
                                else
                                {
                                    result.ErrorMessage = errorMessage;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        private async Task<IAccessResult<T>> GetResult<T>(IAccessResult<Guid> guidResult)
        {
            IAccessResult<T> result = new AccessResult<T>();

            if (guidResult.IsSuccess)
            {
                IAccessResult<RequestStatus> statusResult = await GetRequest<RequestStatus>($"Result/status/{guidResult.Result}");

                while (statusResult.IsSuccess && statusResult.Result != RequestStatus.Done)
                {
                    statusResult = await GetRequest<RequestStatus>($"Result/status/{guidResult.Result}");
                }

                if (statusResult.IsSuccess)
                {
                    result = await GetRequest<T>($"Result/{guidResult.Result}");
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = statusResult.ErrorMessage;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.ErrorMessage = guidResult.ErrorMessage;
            }

            return result;
        }
    }
}
