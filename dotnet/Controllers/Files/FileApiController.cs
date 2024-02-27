using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Services;
using Sabio.Models.Domain.Files;
using Sabio.Models.Requests.Files;
using Sabio.Services.Interfaces.Files;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System.Runtime.InteropServices;

namespace Sabio.Web.Api.Controllers.Files
{
    [Route("api/files")]
    [ApiController]
    public class FileApiController : BaseApiController
    {

        IFileService _fService = null;
        private IAuthenticationService<int> _authService = null;

        public FileApiController(IFileService service, ILogger<FileApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _authService = authService;
            _fService = service;
        }

        [HttpPost("")]
        public async Task<ActionResult> FileAdd(List<IFormFile> files)
        {
            ObjectResult result = null;

            try
            {

                int userId = _authService.GetCurrentUserId();

                Task<List<BaseFile>> listTask = _fService.AddFile(files, userId);

                List<BaseFile> list = await listTask;

                ItemsResponse<BaseFile> response = new ItemsResponse<BaseFile>() { Items = list };

                result = StatusCode(201, response);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<File>>> SelectAllPagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<File> paged = _fService.SelectAllPagination(pageIndex, pageSize);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Something went wrong");
                }
                else
                {
                    response = new ItemResponse<Paged<File>>() { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                _fService.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Exception Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<File>> GetByCreatedBy(int id)
        {
            try
            {
                File file = _fService.GetByCreatedBy(id);
                ItemResponse<File> response = new ItemResponse<File>();
                response.Item = file;
                if (response.Item == null)
                {
                    return NotFound404(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                return base.StatusCode(500, new ErrorResponse($"Generic Error: {ex.Message}"));
            }

        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<File>>> SearchPagination(int pageIndex, int pageSize, string query)
        {
            ActionResult result = null;
            try
            {
                Paged<File> paged = _fService.SearchPagination(pageIndex, pageSize, query);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<File>> response = new ItemResponse<Paged<File>>();
                    response.Item = paged;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }

        [HttpDelete("recover/{id:int}")]
        public ActionResult<SuccessResponse> Recover(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _fService.Recover(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);

        }
    }

}
