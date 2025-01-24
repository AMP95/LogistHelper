using DTOs.Dtos;
using LogistHelper.ViewModels.DataViewModels;
using Shared;
using System.IO;
using System.Net.Http;
using Utilities;

namespace LogistHelper.Models
{
    public class FileManager : IFileLoader<FileViewModel>
    {
        private IDataAccess _access;
        private ILogger _logger;
        public FileManager(IDataAccess dataAccess, ILogger logger)
        {
            _access = dataAccess;
            _logger = logger;
        }

        public async Task<bool> DownloadFiles(string downloadPath, IEnumerable<FileViewModel> filesData)
        {
            bool result = false;

            foreach (var file in filesData)
            {
                result |= await DownloadFile(downloadPath, file);
            }
            return result;
        }

        public async Task<bool> DownloadFile(string downloadFullPath, FileViewModel fileData)
        {
            string fileFullSavePath = Path.Combine(downloadFullPath, fileData.LocalNameWithExtension);

            IAccessResult<bool> loadResult = await _access.DownloadFileAsync(fileData.Id, fileFullSavePath);
            if (loadResult.IsSuccess)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UploadFiles(Guid entityID, IEnumerable<FileViewModel> viewModels)
        {
            bool result = false;

            foreach (var fileViewModel in viewModels) 
            {
                result |= await UploadFile(entityID, fileViewModel);
            }

            return result;
        }

        public async Task<bool> UploadFile(Guid entityID, FileViewModel fileData)
        {
            FileDto fileDto = fileData.GetDto();

            try
            {
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(File.OpenRead(fileData.LocalFullFilePath)), "File", fileDto.FileNameWithExtencion);

                    content.Add(new StringContent(fileDto.FileNameWithExtencion), "FileDto.FileNameWithExtencion");
                    content.Add(new StringContent(fileDto.Catalog), "FileDto.Catalog");
                    content.Add(new StringContent(fileDto.DtoType), "FileDto.DtoType");
                    content.Add(new StringContent(fileDto.DtoId.ToString()), "FileDto.DtoId");

                    IAccessResult<Guid> addResult = await _access.SendMultipartAsync(content);

                    if (addResult.IsSuccess)
                    {
                        fileData.Id = addResult.Result;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
               
            }

            return false;
        }
    }
}
