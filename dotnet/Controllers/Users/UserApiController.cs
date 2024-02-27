using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Core;
using Sabio.Web.Models.Responses;
using Stripe;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UsersApiController : BaseApiController
    {
        private IUserService _userService = null;
        private IAuthenticationService<int> _authenticationService;
        IOptions<SecurityConfig> _options;

        public UsersApiController(IAuthenticationService<int> authService, IUserService userService, ILogger<UsersApiController> logger, IOptions<SecurityConfig> options) : base(logger)
        {
            _authenticationService = authService;
            _userService = userService;
            _options = options;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserAddRequest userModel)
        {
            ObjectResult result = null;
            try
            {
                int id = _userService.Create(userModel);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }
        [HttpPut("confirmUser")]
        [AllowAnonymous]
        public ActionResult ConfirmAccount(string tokenId)
        {
            int code = 0;
            BaseResponse response = null;

            try
            {
                _userService.ConfirmAccount(tokenId);
                code = 200;
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [AllowAnonymous]
        [HttpPut("passwordReset")]
        public ActionResult passwordResetRequest(string email)
        {
            int code = 0;
            BaseResponse response = null;

            try
            {
                _userService.passwordReset(email);
                code = 200;
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpPut("updateStatus")]
        public ActionResult UpdateUserStatus(int Id, int StatusId)
        {
            int code = 0;
            BaseResponse response = null;

            try
            {
                _userService.UpdateUserStatus(Id, StatusId);
                code = 200;
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }



        [HttpPut("business/confirm")]
        public ActionResult ConfirmBusiness(int userId, string firstName, string email)
        {
            int code = 0;
            BaseResponse response = null;

            try
            {
                _userService.ConfirmBusinessAsync(userId, firstName, email);
                code = 200;
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpPut("business/reject")]
        public ActionResult RejectBusiness(int userId, string firstName, string email)
        {
            int code = 0;
            BaseResponse response = null;

            try
            {
                _userService.RejectBusinessAsync(userId, firstName, email);
                code = 200;
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpPut("updatePassword")]
        [AllowAnonymous]
        public ActionResult UpdatePassword(PasswordChangeRequest req)
        {
            int code = 0;
            BaseResponse response = null;

            try
            {
                _userService.UpdatePassword(req);
                code = 200;
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpGet("getAll")]
        public ActionResult<ItemResponse<Paged<User>>> GetAll(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                Paged<User> list = _userService.GetAllPaginated(pageIndex, pageSize);

                if (list == null)
                {
                    result = NotFound404(new ErrorResponse("Its gone! Where did it go?"));
                }
                else
                {
                    ItemResponse<Paged<User>> response = new ItemResponse<Paged<User>>();
                    response.Item = list;
                    result = Ok(response);
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
                base.Logger.LogError(ex.ToString());
            }
            return result;
        }
        [HttpGet("getAllEmployeesBusinesses")]
        public ActionResult<ItemResponse<Paged<User>>> GetAllBusinessesEmployees(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                Paged<User> list = _userService.GetAllBusinessesEmployees(pageIndex, pageSize);

                if (list == null)
                {
                    result = NotFound404(new ErrorResponse("The requested data was not found"));
                }
                else
                {
                    ItemResponse<Paged<User>> response = new ItemResponse<Paged<User>>();
                    response.Item = list;
                    result = Ok(response);
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
                base.Logger.LogError(ex.ToString());
            }
            return result;
        }
        [HttpGet("getAllByRole")]
        public ActionResult<ItemResponse<Paged<User>>> GetAllByRole(int pageIndex, int pageSize, int roleId)
        {
            ActionResult result = null;

            try
            {
                Paged<User> list = _userService.GetAllByRole(pageIndex, pageSize, roleId);

                if (list == null)
                {
                    result = NotFound404(new ErrorResponse("The requested data was not found"));
                }
                else
                {
                    ItemResponse<Paged<User>> response = new ItemResponse<Paged<User>>();
                    response.Item = list;
                    result = Ok(response);
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
                base.Logger.LogError(ex.ToString());
            }
            return result;
        }
        [HttpGet("{email}")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<User>> GetByEmail(string email)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                User user = _userService.GetByEmail(email);
                if (user != null)
                {
                    response = new ItemResponse<User> { Item = user };
                }
                else
                {
                    code = 404;
                    response = new ErrorResponse("User not found");
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
        [HttpGet("role")]
        public ActionResult<ItemResponse<int>> GetCountByRole()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<int> total = _userService.GetTotalCountByRole();
                if (total.Count > 0)
                {
                    response = new ItemsResponse<int> { Items = total };
                }
                else
                {
                    code = 404;
                    response = new ErrorResponse("No users found");
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
        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult LogInAsync(LoginRequest request)
        {
            int code = 0;
            BaseResponse response = null;

            try
            {
                Task<bool> isSuccessful = _userService.LogInAsync(request);
                if (isSuccessful.Result == true)
                {
                    code = 200;
                    response = new ItemResponse<object>() { Item = _options };

                }
                else
                {
                    code = 401;
                    response = new ErrorResponse(isSuccessful.Exception.Message);
                }

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);

        }
        [HttpGet("logout")]
        public async Task<ActionResult<SuccessResponse>> LogoutAsync()
        {
            await _authenticationService.LogOutAsync();
            SuccessResponse response = new SuccessResponse();
            return Ok200(response);
        }
        [HttpGet("current")]
        public ActionResult<ItemResponse<IUserAuthData>> GetCurrrent()
        {
            IUserAuthData user = _authenticationService.GetCurrentUser();
            ItemResponse<IUserAuthData> response = new ItemResponse<IUserAuthData>();
            response.Item = user;

            return Ok200(response);
        }

        [HttpGet("business/approvals")]
        public ActionResult<ItemsResponse<UsersRoleStatus>> getByRoleAndStatusId(int StatusId, int RoleId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<UsersRoleStatus> userRole = _userService.GetByRoleAndStatusId(StatusId, RoleId);
                if (userRole != null)
                {
                    response = new ItemsResponse<UsersRoleStatus> { Items = userRole };
                }
                else
                {
                    code = 404;
                    response = new ErrorResponse("User not found");
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
        [HttpPut("demographics")]
        public ActionResult<int> UpdateDemographics(UserDemographicsUpdateRequest model)
        {

            int code = 200;
            BaseResponse response = null;

            try
            {
                _userService.UpdateDemographics(model);
                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            };


            return StatusCode(code, response);
        }
    }
}