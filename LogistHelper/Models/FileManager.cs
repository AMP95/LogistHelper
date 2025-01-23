using DTOs.Dtos;
using LogistHelper.ViewModels.DataViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Xaml.Behaviors.Layout;
using Shared;
using System;
using System.IO;
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

                IFormFile file = null;
                try
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(fileViewModel.LocalFullFilePath).ToArray()))
                    {
                        file = new FormFile(stream, 0, stream.Length, null, fileViewModel.LocalNameWithExtension);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(ex, ex.Message, LogLevel.Error);
                    continue;
                }

                fileDto.File = file;

                IAccessResult<Guid> addResult = await _access.AddAsync<FileDto>(fileDto);

                if (addResult.IsSuccess) 
                {
                    fileViewModel.Id = addResult.Result;
                    result |= true;
                }
            }

            return result;
        }
    }
}
