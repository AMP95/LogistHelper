using DTOs.Dtos;
using LogistHelper.ViewModels.DataViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Shared;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using Utilities;

namespace LogistHelper.Models
{
    public class FileManager : IFileLoader
    {
        private IDataAccess _access;
        private ILogger _logger;
        public FileManager(IDataAccess dataAccess, ILogger logger)
        {
            _access = dataAccess;
            _logger = logger;
        }

        public async Task<bool> DownloadFiles(string downloadPath, IEnumerable<Guid> fileGuids)
        {
            return await Task.Run<bool>(async () => 
            {
                bool result = false;
                foreach (var fileGuid in fileGuids)
                {
                    IAccessResult<FileDto> loadResult = await _access.GetIdAsync<FileDto>(fileGuid);
                    if (loadResult.IsSuccess)
                    {
                        FileDto dto = loadResult.Result;

                        string fullPath = Path.Combine(downloadPath, dto.FileNameWithExtencion);

                        try
                        {
                            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                            {
                                await dto.File.CopyToAsync(fileStream);
                            }
                            result |= true;
                        }
                        catch (Exception ex)
                        {
                            _logger.Log(ex, ex.Message, LogLevel.Error);
                        }
                    }
                }
                return result;
            });
            
        }

        public async Task<bool> UploadFiles(Guid entityID, IEnumerable<object> viewModels)
        {
            IEnumerable<FileViewModel> files = viewModels.Cast<FileViewModel>();

            bool result = false;

            foreach (var fileViewModel in files) 
            {
                FileDto fileDto = fileViewModel.GetDto();

                try
                {
                    using (MultipartFormDataContent content = new MultipartFormDataContent
                    {
                        // file
                        { new StreamContent(File.OpenRead(fileViewModel.LocalFullFilePath)), "FileToUpload", fileDto.FileNameWithExtencion },

                        // payload
                        { new StringContent(fileDto.Id.ToString()), nameof(fileDto.Id) },
                        { new StringContent(fileDto.FileNameWithExtencion), nameof(fileDto.FileNameWithExtencion) },
                        { new StringContent(fileDto.Catalog), nameof(fileDto.Catalog) },
                        { new StringContent(fileDto.DtoType.Name), nameof(fileDto.DtoType) },
                        { new StringContent(fileDto.DtoId.ToString()), nameof(fileDto.DtoId) },
                    }) 
                    {
                        IAccessResult<Guid> addResult = await _access.AddMultipartAsync(content);

                        if (addResult.IsSuccess)
                        {
                            fileViewModel.Id = addResult.Result;
                            result |= true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(ex, ex.Message, LogLevel.Error);
                    continue;
                }
            }

            return result;
        }
    }
}
