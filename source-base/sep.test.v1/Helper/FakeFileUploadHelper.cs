using Microsoft.AspNetCore.Http;
using sep.backend.v1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sep.test.v1.Helper
{
    public interface IFileUploadHelper
    {
        Task<string> UploadFile(IFormFile file, string folder);
    }
    public class FakeFileUploadHelper : IFileUploadHelper
    {
        private readonly FileUploadHelper _fileUploadHelper = new();

        public Task<string> UploadFile(IFormFile file, string folder)
        {
            return _fileUploadHelper.UploadFile(file, folder);
        }
    }
}
