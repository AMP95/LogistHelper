using DTOs.Dtos;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.IO;
using System.Net.Http;
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
                    IAccessResult<bool> loadResult = await _access.DownloadFileAsync(fileGuid, downloadPath);
                    if (loadResult.IsSuccess)
                    {
                        result |= true;
                    }
                }
                return result;
            });
            
        }
        public async Task<bool> DownloadFile(string downloadFullPath, Guid fileId)
        {
            return await Task.Run<bool>(async () =>
            {
                IAccessResult<bool> loadResult = await _access.DownloadFileAsync(fileGuid, downloadFullPath);
                if (loadResult.IsSuccess)
                {
                    result |= true;
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
                    using (MultipartFormDataContent content = new MultipartFormDataContent()) 
                    {
                        content.Add(new StreamContent(File.OpenRead(fileViewModel.LocalFullFilePath)), "File", fileDto.FileNameWithExtencion);

                        content.Add(new StringContent(fileDto.FileNameWithExtencion), "FileDto.FileNameWithExtencion");
                        content.Add(new StringContent(fileDto.Catalog), "FileDto.Catalog");
                        content.Add(new StringContent(fileDto.DtoType), "FileDto.DtoType");
                        content.Add(new StringContent(fileDto.DtoId.ToString()), "FileDto.DtoId");

                        IAccessResult<Guid> addResult = await _access.SendMultipartAsync(content);

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
