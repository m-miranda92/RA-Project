using Sabio.Models.Requests.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data.Providers;
using System.Data.SqlClient;
using System.Data;
using Sabio.Services.Interfaces;
using Sabio.Data;
using Sabio.Models;
using Sabio.Models.Domain.Files;
using System.Net;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Http.HttpResults;
using Sabio.Models.Domain;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Sabio.Models.Enums;
using Amazon;
using Sabio.Services.Interfaces.Files;
using Sabio.Models.AWS;
using RA_Project.AWS;


namespace Sabio.Services
{
    public class FileService : IFileService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;
        private FileKeys _keys;
        private static IAmazonS3 s3Client;
        private readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;

        public FileService(IDataProvider data, ILookUpService lookUpService, IOptions<FileKeys> keys)
        {
            _data = data;
            _lookUpService = lookUpService;
            _keys = keys.Value;
        }

        public async Task<List<BaseFile>> AddFile(List<IFormFile> files, int userId)
        {
            string procName = "[dbo].[Files_Insert]";

            DataTable fileTable = null;

            List<BaseFile> resultList = null;

            List<FileAddRequest> models = new List<FileAddRequest>();

            FileAddRequest model = null;

            foreach (IFormFile file in files)
            {
                string fileGuid = Guid.NewGuid().ToString();

                bool uploadSuccess = await UploadFileAsync(file, fileGuid);

                if (uploadSuccess)
                {
                    model = new FileAddRequest();

                    model.Name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                    model.Url = $"{_keys.Domain}{file.FileName}{fileGuid}";
                    model.FileTypeId = (int)GetFileType(System.IO.Path.GetExtension(file.FileName).Trim('.'));
                    models.Add(model);

                }

            }

            fileTable = AddCommonParams(models);

            _data.ExecuteCmd(procName,
                     delegate (SqlParameterCollection col)
                     {
                         col.AddWithValue("@CreatedBy", userId);
                         col.AddWithValue("@BatchFiles", fileTable);

                     }, delegate (IDataReader reader, short set)
                     {
                         var result = BaseFileSingleMapper(reader);
                         if (resultList == null)
                         {
                             resultList = new List<BaseFile>();
                         }
                         resultList.Add(result);
                     });

            return resultList;
        }
        private async Task<bool> UploadFileAsync(IFormFile file, string fileGuid)
        {
            try
            {
                s3Client = new AmazonS3Client(_keys.AccessKey, _keys.Secret, bucketRegion);

                var fileTransferUtility = new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(file.OpenReadStream(), _keys.BucketName, $"{file.FileName}{fileGuid}");

            }
            catch (AmazonS3Exception e)
            {

                throw new Exception($"Error encountered on server. Message:'{0}' when writing an object: {e.Message}");

            }
            catch (Exception e)
            {

                throw new Exception($"Error encountered on server. Message:'{0}' when writing an object: {e.Message}");

            }

            return true;

        }
        public Paged<File> SelectAllPagination(int pageIndex, int pageSize)
        {
            Paged<File> pageList = null;
            List<File> list = null;
            string procName = "[dbo].[Files_SelectAllPaginated]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },
            (reader, recordSetIndex) =>
            {
                int index = 0;
                File file = MapSingleFile(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }

                if (list == null)
                {
                    list = new List<File>();
                }
                list.Add(file);
            });
            if (list != null)
            {
                pageList = new Paged<File>(list, pageIndex, pageSize, totalCount);
            }
            return pageList;
        }
        public File GetByCreatedBy(int Id)
        {

            string procName = "[dbo].[Files_Select_ByCreatedBy]";

            File fileProvider = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {


                paramCollection.AddWithValue("@Id", Id);

            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                fileProvider = MapSingleFile(reader, ref index);
            }

            );

            return fileProvider;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[Files_DeleteById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {

                    col.AddWithValue("@Id", id);

                }, returnParameters: null);
        }
        private static FileType GetFileType(string fileType)
        {
            switch (fileType.ToLower())
            {
                case "jpg":
                    return FileType.jpg;
                case "pdf":
                    return FileType.pdf;
                case "jpeg":
                    return FileType.jpeg;
                case "doc":
                    return FileType.doc;
                case "png":
                    return FileType.png;
                case "gif":
                    return FileType.gif;
                case "webp":
                    return FileType.webp;
                case "svg":
                    return FileType.svg;
                case "html":
                    return FileType.html;
                case "xhtml":
                    return FileType.xhtml;
                default:
                    return FileType.jpg;
            }
        }
        private static BaseFile BaseFileSingleMapper(IDataReader reader)
        {
            int index = 0;
            BaseFile file = new BaseFile();

            {
                file.Id = reader.GetSafeInt32(index++);
                file.Url = reader.GetString(index++);
            }

            return file;
        }
        private static DataTable AddCommonParams(List<FileAddRequest> model)
        {
            DataTable fileTable = new DataTable();

            fileTable.Columns.Add("Name", typeof(string));
            fileTable.Columns.Add("Url", typeof(string));
            fileTable.Columns.Add("FileType", typeof(int));

            foreach (var file in model)
            {
                DataRow fileRow = fileTable.NewRow();
                int startingIndex = 0;

                fileRow[startingIndex++] = file.Name;
                fileRow[startingIndex++] = file.Url;
                fileRow[startingIndex++] = file.FileTypeId;

                fileTable.Rows.Add(fileRow);
            }

            return fileTable;
        }
        private File MapSingleFile(IDataReader reader, ref int index)
        {
            File file = new File();

            file.Id = reader.GetSafeInt32(index++);
            file.Name = reader.GetSafeString(index++);
            file.Url = reader.GetSafeString(index++);
            file.FileType = _lookUpService.MapSingleLookUp(reader, ref index);
            file.IsDeleted = reader.GetSafeBool(index++);
            file.CreatedBy = reader.DeserializeObject<BaseUser>(index++);

            return file;
        }

        public Paged<File> SearchPagination(int pageIndex, int pageSize, string query)
        {
            int startingIndex = 0;
            Paged<File> pagedList = null;
            List<File> list = null;
            int totalCount = 0;


            _data.ExecuteCmd("[dbo].[Files_Select_Paginate_Search]",
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@PageIndex", pageIndex);
                    paramCol.AddWithValue("@PageSize", pageSize);
                    paramCol.AddWithValue("@Query", query);

                },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                File files = MapSingleFile(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }

                if (list == null)
                {
                    list = new List<File>();
                }
                list.Add(files);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<File>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void Recover(int id)
        {
            string procName = "[dbo].[Files_RecoverById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }

    }
}