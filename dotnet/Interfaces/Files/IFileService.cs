using Sabio.Models;
using Sabio.Models.Requests.Files;
using Sabio.Models.Domain.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sabio.Services.Interfaces.Files
{
    public interface IFileService
    {
        Task<List<BaseFile>> AddFile(List<IFormFile> files, int userId);
        Paged<File> SelectAllPagination(int pageIndex, int pageSize);
        void Delete(int id);
        File GetByCreatedBy(int Id);
        Paged<File> SearchPagination(int pageIndex, int pageSize, string query);
        void Recover(int id);
    }
}
