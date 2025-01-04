using Newtonsoft.Json;
using System.Net.Http.Json;
using DTOs;

namespace ServerClient
{
    public enum RequestStatus
    {
        Created,
        InProgress,
        Done,
        Unknown
    }
    public class ApiClient
    {
        private string _url;
        private string _token;

        public ApiClient(string url)
        {
            _url = url;
        }

        private async Task<RequestResult<T>> GetRequest<T>(string route)
        {
            return await SendAsync<T>(HttpMethod.Get, $"{_url}/Get/{route}");
        }

        public async Task<RequestResult<IEnumerable<T>>> Get<T>(string property = null, object value = null)
        {
            string route = GetRoute(typeof(T));
            RequestResult<Guid> result = await GetRequest<Guid>($"GetF/{route}/{property}/{value}");
            return await GetResult<IEnumerable<T>>(result);
        }

        public async Task<RequestResult<T>> Get<T>(int id)
        {
            string route = GetRoute(typeof(T));
            RequestResult<Guid> result = await GetRequest<Guid>($"Get/{route}/{id}");
            return await GetResult<T>(result);
        }

        public async Task<RequestResult<bool>> Add<T>(T value)
        {
            string route = GetRoute(typeof(T));
            RequestResult<Guid> result = await SendAsync<Guid>(HttpMethod.Post, $"{_url}/Add/{route}", JsonConvert.SerializeObject(value));
            return await GetResult<bool>(result);
        }

        public async Task<RequestResult<bool>> Update<T>(T value)
        {
            string route = GetRoute(typeof(T));
            RequestResult<Guid> result = await SendAsync<Guid>(HttpMethod.Put, $"{_url}/Update/{route}", JsonConvert.SerializeObject(value));
            return await GetResult<bool>(result);
        }

        public async Task<RequestResult<bool>> Delete<T>(int id)
        {
            string route = GetRoute(typeof(T));
            RequestResult<Guid> result = await SendAsync<Guid>(HttpMethod.Delete, $"{_url}/Delete/{route}/{id}");
            return await GetResult<bool>(result);
        }

        private string GetRoute(Type type)
        {
            string typeName = type.Name;

            switch (typeName)
            {
                case nameof(TruckDto): return "truck";
                case nameof(TrailerDto): return "trailer";
                case nameof(DriverDto): return "driver";
                case nameof(CarrierDto): return "carrier";
                case nameof(CompanyDto): return "company";
                case nameof(RoutePointDto): return "route";
                case nameof(ContractDto): return "contract";
                case nameof(DocumentDto): return "document";
                default: return string.Empty;
            }
        }

        private async Task<RequestResult<T>> SendAsync<T>(HttpMethod method, string route, string jObject = null)
        {
            RequestResult<T> result = new RequestResult<T>();

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

        private async Task<RequestResult<T>> GetResult<T>(RequestResult<Guid> guidResult)
        {
            RequestResult<T> result = new RequestResult<T>();

            if (guidResult.IsSuccess)
            {
                RequestResult<RequestStatus> statusResult = await GetRequest<RequestStatus>($"Result/status/{guidResult.Result}");

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
