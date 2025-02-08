using DTOs;
using DTOs.Dtos;
using HelpAPIs.Settings;
using Newtonsoft.Json;
using Shared;
using System.Net.Http.Json;

namespace HelpAPIs
{
    internal enum Status
    {
        Created,
        InProgress,
        Done
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
            return await SendStringAsync<T>(HttpMethod.Get, $"{_url}/{route}");
        }

        public async Task<IAccessResult<T>> GetIdAsync<T>(Guid id)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await GetRequest<Guid>($"{route}/{id}");
            return await GetResult<T>(result);
        }
                          
        public async Task<IAccessResult<IEnumerable<T>>> GetFilteredAsync<T>(string propertyName, params string[] param)                                                        
        {
            string route = GetRoute(typeof(T));

            string uri = $"{route}/filter/{propertyName}";

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
            IAccessResult<Guid> result = await GetRequest<Guid>($"{route}/range/{start}/{end}");
            return await GetResult<IEnumerable<T>>(result);
        }
                          
        public async Task<IAccessResult<Guid>> AddAsync<T>(T value)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await SendStringAsync<Guid>(HttpMethod.Post, $"{_url}/{route}", JsonConvert.SerializeObject(value));
            return await GetResult<Guid>(result);
        }
                          
        public async Task<IAccessResult<bool>> UpdateAsync<T>(T value)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> guidResult = await SendStringAsync<Guid>(HttpMethod.Put, $"{_url}/{route}", JsonConvert.SerializeObject(value));
            IAccessResult<object> result = await GetResult<object>(guidResult);

            bool.TryParse(result.Result.ToString(), out bool boolResult);

            return new AccessResult<bool>() { IsSuccess = result.IsSuccess, ErrorMessage = result.ErrorMessage, Result = boolResult };
        }

        public async Task<IAccessResult<bool>> UpdatePropertyAsync<T>(Guid id, params KeyValuePair<string, object>[] updates)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await SendStringAsync<Guid>(HttpMethod.Patch, $"{_url}/{route}/{id}", JsonConvert.SerializeObject(updates));
            return await GetResult<bool>(result);

        }
                          
        public async Task<IAccessResult<bool>> DeleteAsync<T>(Guid id)
        {
            string route = GetRoute(typeof(T));
            IAccessResult<Guid> result = await SendStringAsync<Guid>(HttpMethod.Delete, $"{_url}/{route}/{id}");
            return await GetResult<bool>(result);
        }

        private string GetRoute(Type type)
        {
            string typeName = type.Name;

            switch (typeName)
            {
                case nameof(VehicleDto): return "Vehicle";
                case nameof(DriverDto): return "Driver";
                case nameof(CarrierDto): return "Carrier";
                case nameof(CompanyDto): return "Company";
                case nameof(ContractDto): return "Contract";
                case nameof(DocumentDto): return "Document";
                case nameof(PaymentDto): return "Payment";
                case nameof(FileDto): return "File";
                case nameof(TemplateDto): return "Template";
                case nameof(LogistDto): return "Logist";
                default: return string.Empty;
            }
        }

        private async Task<IAccessResult<T>> SendStringAsync<T>(HttpMethod method, string route, string jObject = null)
        {
            AccessResult<T> result = new AccessResult<T>()
            {
                IsSuccess = false
            };

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

                            HttpResponseMessage response = await client.SendAsync(message);

                            if (response.IsSuccessStatusCode)
                            {
                                result = await response.Content.ReadFromJsonAsync<AccessResult<T>>();
                            }
                            else
                            {
                                result.ErrorMessage = response.ReasonPhrase;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public async Task<IAccessResult<Guid>> SendMultipartAsync(MultipartFormDataContent content)
        {
            IAccessResult<Guid> result = new AccessResult<Guid>()
            {
                IsSuccess = false,
                ErrorMessage = "Только для файлов"
            };

            try
            {
                using (HttpClientHandler clientHandler = new HttpClientHandler()
                { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } })
                {
                    using (HttpClient client = new HttpClient(clientHandler))
                    {
                        using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"{_url}/File"))
                        {
                            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                            message.Content = content;

                            var response = await client.SendAsync(message);

                            if (response.IsSuccessStatusCode)
                            {
                                result = await response.Content.ReadFromJsonAsync<AccessResult<Guid>>();
                            }
                            else
                            {
                                result.ErrorMessage = response.ReasonPhrase;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private async Task<IAccessResult<T>> GetResult<T>(IAccessResult<Guid> guidResult)
        {
            IAccessResult<T> result = new AccessResult<T>()
            {
                IsSuccess = false
            };

            if (guidResult.IsSuccess)
            {
                IAccessResult<Status> statusResult = await GetRequest<Status>($"Result/status/{guidResult.Result}");

                while (statusResult.IsSuccess && statusResult.Result != Status.Done)
                {
                    statusResult = await GetRequest<Status>($"Result/status/{guidResult.Result}");
                }

                if (statusResult.IsSuccess)
                {
                    result = await GetRequest<T>($"Result/{guidResult.Result}");
                }
                else
                {
                    result.ErrorMessage = statusResult.ErrorMessage;
                }
            }
            else
            {
                result.ErrorMessage = guidResult.ErrorMessage;
            }

            return result;
        }

        public async Task<IAccessResult<bool>> DownloadFileAsync(Guid fileId, string fullPath)
        {
            IAccessResult<bool> result = new AccessResult<bool>()
            {
                IsSuccess = false,
                ErrorMessage = "Ошибка загрузки"
            };
            IAccessResult<Guid> guidResult = await GetRequest<Guid>($"File/download/{fileId}");

            if (guidResult.IsSuccess)
            {
                IAccessResult<Status> statusResult = await GetRequest<Status>($"Result/status/{guidResult.Result}");

                while (statusResult.IsSuccess && statusResult.Result != Status.Done)
                {
                    statusResult = await GetRequest<Status>($"Result/status/{guidResult.Result}");
                }

                if (statusResult.IsSuccess)
                {
                    try
                    {
                        using (HttpClientHandler clientHandler = new HttpClientHandler()
                        { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } })
                        {
                            using (HttpClient client = new HttpClient(clientHandler))
                            {
                                using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"{_url}/Result/file/{guidResult.Result}"))
                                {
                                    message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                                    var response = await client.SendAsync(message);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        using (FileStream fs = File.OpenWrite(fullPath))
                                        {
                                            await response.Content.CopyToAsync(fs);
                                        }
                                        result.IsSuccess = true;
                                        result.Result = true;
                                    }
                                    else
                                    {
                                        result.IsSuccess = false;
                                        result.ErrorMessage = await response.Content.ReadAsStringAsync();
                                        if (string.IsNullOrWhiteSpace(result.ErrorMessage))
                                        {
                                            result.ErrorMessage = response.ReasonPhrase;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.Message;
                    }
                }
            }

            return result;
        }

        public async Task<IAccessResult<bool>> UploadFileAsync<T>(Guid entityId, T file, string fullFilePath) 
        {
            FileDto fileDto = file as FileDto;

            IAccessResult<Guid> addResult = new AccessResult<Guid>()
            {
                IsSuccess = false,
                ErrorMessage = "Ошибка отправки файла"
            };

            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(File.OpenRead(fullFilePath)), "File", fileDto.FileNameWithExtencion);

                content.Add(new StringContent(fileDto.FileNameWithExtencion), "FileDto.FileNameWithExtencion");
                content.Add(new StringContent(fileDto.Catalog), "FileDto.Catalog");
                content.Add(new StringContent(fileDto.DtoType), "FileDto.DtoType");
                content.Add(new StringContent(fileDto.DtoId.ToString()), "FileDto.DtoId");

                addResult = await SendMultipartAsync(content);
            }

            return await GetResult<bool>(addResult);
        }

        public async Task<IAccessResult<IEnumerable<T>>> GetRequiredPayments<T>()
        {
            IAccessResult<Guid> result = await GetRequest<Guid>($"Contract/payment");
            return await GetResult<IEnumerable<T>>(result);
        }

        public async Task<IAccessResult<T>> Login<T>(T dto)
        {
            IAccessResult<Guid> result = await SendStringAsync<Guid>(HttpMethod.Post, $"{_url}/Logist/validate", JsonConvert.SerializeObject(dto));
            IAccessResult<object[]> loginResult = await GetResult<object[]>(result);

            if (loginResult.IsSuccess && loginResult.Result.Any()) 
            {
                _token = loginResult.Result[0].ToString();

                T returnDto = (T)JsonConvert.DeserializeObject(loginResult.Result[1].ToString(), typeof(T));

                return new AccessResult<T>() 
                { 
                    IsSuccess = true,
                    Result = returnDto,
                };
            }

            return new AccessResult<T>()
            {
                IsSuccess = false,
                ErrorMessage = loginResult.ErrorMessage
            }; 
        }

        public Task Logout()
        {
            _token = string.Empty;
            return Task.CompletedTask;
        }
    }
}
