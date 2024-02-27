using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Tables;
using Sabio.Models.Domain.Venues;
using Sabio.Models.Requests.UserVenues;
using Sabio.Models.Requests.Venues;
using Sabio.Services;
using Sabio.Services.Interfaces.Venues;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sabio.Web.Api.Controllers.Venues
{
    [Route("api/venues")]
    [ApiController]
    public class VenuesApiController : BaseApiController
    {
        private IVenueService _service = null;
        private IAuthenticationService<int> _authService = null;
        public VenuesApiController(IVenueService service, IAuthenticationService<int> authService, ILogger<VenuesApiController> logger) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet("paginate")]
        [AllowAnonymous]
        public ActionResult<ItemsResponse<Paged<Venue>>> Get(int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                Paged<Venue> paged = _service.Get(pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Venue>> response = new ItemResponse<Paged<Venue>>();
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
        [HttpGet]
        public ActionResult<ItemsResponse<Paged<Venue>>> GetByCreated(int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<Venue> paged = _service.Get(userId, pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Venue>> response = new ItemResponse<Paged<Venue>>();
                    response.Item = paged;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }


        [HttpGet("byOrganization")]
        public ActionResult<ItemsResponse<LookUp>> GetAllByOrgId()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                List<LookUp> list = _service.GetAllByOrgId(userId);
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<LookUp> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("organization")]
        public ActionResult<ItemResponse<Paged<Venue>>> GetByOrgId(int organizationId, int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Venue> paged = _service.GetByOrgId(organizationId, pageIndex, pageSize);

                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Venue not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Venue>> { Item = paged };

                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex, ex.ToString());

            }

            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Venue>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                Venue venue = _service.Get(id);
                if (venue == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Venue Not Found");
                }
                else
                {
                    response = new ItemResponse<Venue> { Item = venue };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }
        [HttpPost("")]
        public ActionResult<ItemResponse<int>> Create(VenueAddRequest request)
        {
            ObjectResult result = null;
            try
            {
                int createdBy = _authService.GetCurrentUserId();
                int id = _service.Add(request, createdBy);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(int id, VenueUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int modifiedBy = _authService.GetCurrentUserId();
                _service.Update(model, modifiedBy);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        [HttpPost("favorites")]
        public ActionResult<ItemResponse<int>> CreateFavorite(UserVenueAddRequest request)
        {
            ObjectResult result = null;
            try
            {
                int createdBy = _authService.GetCurrentUserId();
                int id = _service.AddFavorite(request, createdBy);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }
        [HttpDelete("favorites/{id:int}")]
        public ActionResult<SuccessResponse> DeleteFavoriteVenue(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.DeleteFavorite(userId, id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }
        [HttpGet("favorites")]
        public ActionResult<ItemsResponse<Venue>> GetByUserId()
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                List<Venue> userVenues = _service.GetByUserId(userId);
                if (userVenues == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Venue Not Found");
                }
                else
                {
                    response = new ItemsResponse<Venue> { Items = userVenues };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }
        [HttpGet("favorites/{id:int}")]
        public ActionResult<ItemsResponse<BaseUser>> GetByVenueId(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                List<BaseUser> baseUsers = _service.GetByVenueId(id);
                if (baseUsers == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Venue Not Found");
                }
                else
                {
                    response = new ItemsResponse<BaseUser> { Items = baseUsers };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }
    }
}