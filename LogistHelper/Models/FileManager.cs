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

            IAccessResult<bool> result = await _access.UploadFileAsync<FileDto>(entityID, fileDto, fileData.LocalFullFilePath);

            return result.Result;
        }
    }
}
