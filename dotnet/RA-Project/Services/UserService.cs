using Microsoft.AspNetCore.Mvc.Razor;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Tokens;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.EmailRequests;
using Sabio.Models.Requests.Users;
using Sabio.Services.Email;
using Sabio.Services.Interfaces;
using Stripe.Terminal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class UserService : IUserService
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;
        private IEmailService _emailService;
        private ILookUpService _lookUpService;


        public UserService(IAuthenticationService<int> authService, IDataProvider dataProvider, IEmailService emailService, ILookUpService lookUpService)
        {
            _authenticationService = authService;
            _dataProvider = dataProvider;
            _emailService = emailService;
            _lookUpService = lookUpService;
        }
        public void passwordReset(string email)
        {
            int tokenType = 4;
            User user = GetByEmail(email);
            if (user != null)
            {
                string newToken = CreateUserToken(user.Id, tokenType);
                _emailService.UserPasswordReset(user, newToken);
            }
            else
            {
                throw new Exception("no user found with this email please try again");
            }
        }
        public async Task<bool> LogInAsync(LoginRequest model)
        {
            bool isSuccessful = false;
            IUserAuthData response = Get(model.Email, model.Password);

            if (response != null)
            {
                isSuccessful = true;
            }
            Claim fullName = new Claim("Authentication", response.Email);
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });
            return isSuccessful;
        }
        public void UpdatePassword(PasswordChangeRequest req)
        {
            User user = GetByEmail(req.Email);
            UserToken tokenFromDB = getByUserId(user.Id);


            if (tokenFromDB != null && tokenFromDB.TokenType == "PasswordRecovery")
            {
                string salt = BCrypt.BCryptHelper.GenerateSalt();
                string hashedPassword = BCrypt.BCryptHelper.HashPassword(req.Password.ToString(), salt);
                _dataProvider.ExecuteNonQuery("[dbo].[Users_UpdatePassword]", inputParamMapper: paramCollection =>
                {
                    paramCollection.AddWithValue("@Id", user.Id);
                    paramCollection.AddWithValue("@Password", hashedPassword);
                }, returnParameters: null);
            }
            else
            {
                throw new Exception("We could not find your reset request, please request another password reset");
            }
        }
        public async Task<bool> LogInTest(string email, string password, int id, string role)
        {
            bool isSuccessful = false;
            var testRoles = new[] { "User", "Super", "Content Manager" };
            IUserAuthData response = new UserBase
            {
                Id = id
                ,
                Email = email
                ,
                Role = role
                ,
                TenantId = "Acme Corp UId"
            };
            Claim fullName = new Claim("CustomClaim", "Sabio Bootcamp");
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });
            return isSuccessful;
        }
        public void ConfirmAccount(string tokenId)
        {
            int validUserId = ConfirmByToken(tokenId);

            if (validUserId != 0)
            {
                int statusId = 1;

                UpdateIsConfirmed(validUserId);
                UpdateUserStatus(validUserId, statusId);
                DeleteUserToken(tokenId);

                return;
            }
            else
            {
                throw new Exception("Please try again, or request a new link.");
            }
        }
        private int ConfirmByToken(string tokenId)
        {
            int userId = 0;
            int tokenType = 0;

            _dataProvider.ExecuteCmd(
                 storedProc: "[dbo].[UserTokens_Select_ByToken]"
                 , inputParamMapper: paramCollection =>
                 {
                     paramCollection.AddWithValue("@Token", tokenId);
                 }
                 , singleRecordMapper: (IDataReader reader, short set) =>
                 {
                     int index = 0;

                     userId = reader.GetSafeInt32(index++);
                     tokenType = reader.GetSafeInt32(index++);
                 }
                 , returnParameters: null
             );

            if (tokenType == 2)
            {
                return userId;
            }
            else
            {
                throw new Exception("Please try again, or request a new link.");
            }
        }

        public async Task ConfirmBusinessAsync(int userId, string firstName, string email)
        {
            if (userId != 0)
            {
                int statusId = 1;

                await _emailService.ApproveBusinessEmail(firstName, email);
                UpdateIsConfirmed(userId);
                UpdateUserStatus(userId, statusId);
            }
            else
            {
                throw new Exception("Business approval fialed, please try again or Refresh page.");
            }
        }
        public async Task RejectBusinessAsync(int userId, string firstName, string email)
        {
            if (userId != 0)
            {
                int statusId = 5;

                await _emailService.RejectBusinessEmail(firstName, email);

                UpdateUserStatus(userId, statusId);
            }
            else
            {
                throw new Exception("Rejected business has failed please try again or request a new link.");
            }
        }
        public void UpdateIsConfirmed(int userId)
        {
            _dataProvider.ExecuteNonQuery(
            storedProc: "[dbo].[Users_Confirm]"
            , inputParamMapper: paramCollection =>
            {
                paramCollection.AddWithValue("@Id", userId);
            }
            , returnParameters: null);
        }
        public void UpdateUserStatus(int id, int statusId)
        {
            _dataProvider.ExecuteNonQuery(
                storedProc: "[dbo].[Users_UpdateStatus]"
                , inputParamMapper: paramCollection =>
                {
                    paramCollection.AddWithValue("@StatusId", statusId);
                    paramCollection.AddWithValue("@Id", id);
                }
                , returnParameters: null);
        }
        private void DeleteUserToken(string tokenId)
        {
            _dataProvider.ExecuteNonQuery(
                storedProc: "[dbo].[UserTokens_Delete_ByToken]"
                , inputParamMapper: paramCollection =>
                {
                    paramCollection.AddWithValue("@Token", tokenId);
                }
                , returnParameters: null
                );
        }
        private string CreateUserToken(int userId, int tokenType)
        {
            string tokenId = Guid.NewGuid().ToString();
            _dataProvider.ExecuteNonQuery("[dbo].[UserTokens_Insert]", inputParamMapper: paramCollection =>
            {
                SqlParameter tokenOut = new SqlParameter("@Token", SqlDbType.VarChar);
                tokenOut.Direction = ParameterDirection.Output;
                paramCollection.AddWithValue("@UserId", userId);
                paramCollection.AddWithValue("@TokenType", tokenType);
                paramCollection.AddWithValue("@Token", tokenId);
            }, delegate (SqlParameterCollection returnCol)
            {
                tokenId = returnCol["@Token"].Value.ToString();
            });
            return tokenId;
        }
        public int Create(UserAddRequest userModel)
        {
            int tokenType = 2;
            int userId = 0;
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(userModel.Password.ToString(), salt);

            _dataProvider.ExecuteNonQuery("[dbo].[Users_Insert]", delegate (SqlParameterCollection col)
            {

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.AddWithValue("@Email", userModel.Email);
                col.AddWithValue("@FirstName", userModel.FirstName);
                col.AddWithValue("@LastName", userModel.LastName);
                if (userModel.Mi != null)
                {
                    col.AddWithValue("@Mi", userModel.Mi);
                }

                if (userModel.AvatarUrl != null)
                {
                    col.AddWithValue("@AvatarUrl", userModel.AvatarUrl);
                }

                col.AddWithValue("@Password", hashedPassword);
                var isConfirmed = false;
                if (userModel.RoleId == 3)
                {
                    col.AddWithValue("@StatusId", 6);
                }
                else if (userModel.RoleId == 1)
                {
                    col.AddWithValue("@StatusId", 3);
                }
                else if (userModel.RoleId == 4)
                {
                    col.AddWithValue("@StatusId", 1);
                    isConfirmed = true;

                }
                col.AddWithValue("@RoleId", userModel.RoleId);
                col.AddWithValue("@IsConfirmed", isConfirmed);

                col.Add(idOut);

            }, delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;

                int.TryParse(oId.ToString(), out userId);
            });
            string createdToken = CreateUserToken(userId, tokenType);

            if (userModel.RoleId == 1)
            {
                _emailService.UserEmailConfirm(userModel, createdToken);

            }
            else if (userModel.RoleId == 3)
            {
                _emailService.BusinessAccountRequest(userModel, createdToken);
            }
            else if (userModel.RoleId != 4)
            {
                throw new Exception(" email service has failed please try again or request a new link.");
            }

            return userId;
        }
        private UsersRoleStatus MapSingleRoleStatusId(IDataReader reader, ref int startingIndex)
        {
            UsersRoleStatus userRole = new UsersRoleStatus();
            userRole.Id = reader.GetSafeInt32(startingIndex++);
            userRole.FirstName = reader.GetSafeString(startingIndex++);
            userRole.Email = reader.GetSafeString(startingIndex++);
            userRole.Status = new LookUp();
            userRole.Status = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            userRole.Role = new LookUp();
            userRole.Role = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            userRole.DateCreated = reader.GetDateTime(startingIndex++);
            userRole.DateModified = reader.GetDateTime(startingIndex++);
            return userRole;
        }
        public UserToken getByUserId(int userId)
        {
            UserToken userToken = null;
            _dataProvider.ExecuteCmd("dbo.UserTokens_Select_ByUserId", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", userId);
            }, delegate (IDataReader reader, short set)
            {
                userToken = new UserToken();
                int startingIndex = 0;
                userToken.Token = reader.GetSafeString(startingIndex++);
                userToken.UserId = reader.GetSafeInt32(startingIndex++);
                userToken.TokenType = reader.GetSafeString(startingIndex++);
            });
            return userToken;
        }
        public User GetByEmail(string email)
        {
            User user = null;

            _dataProvider.ExecuteCmd("dbo.Users_SelectByEmail", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Email", email);
            }, delegate (IDataReader reader, short set)
            {

                user = MapSingleUser(reader);

            });
            return user;
        }

        private static User MapSingleUser(IDataReader reader)
        {
            User user = new User();
            int startingIndex = 0;

            user.Id = reader.GetSafeInt32(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Mi = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.isConfirmed = reader.GetBoolean(startingIndex++);
            user.Status = reader.GetSafeString(startingIndex++);
            user.Role = reader.GetSafeString(startingIndex++);
            user.DateCreated = reader.GetDateTime(startingIndex++);
            user.DateModified = reader.GetDateTime(startingIndex++);
            return user;
        }
        public List<int> GetTotalCountByRole()
        {
            List<int> list = null;

            _dataProvider.ExecuteCmd("dbo.Select_User_Total_By_Role", null, delegate (IDataReader reader, short set)
            {
                int total = reader.GetSafeInt32(0);
                if (list == null)
                {
                    list = new List<int>();
                }
                list.Add(total);
            });
            return list;
        }
        public Paged<User> GetAllPaginated(int pageIndex, int pageSize)
        {
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd("[dbo].[Users_SelectAll]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            }, delegate (IDataReader reader, short set)
            {
                User user = MapSingleUser(reader);


                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(user);
                if (set == 0)
                {

                    totalCount = reader.GetSafeInt32(reader.FieldCount - 1);
                }
            });
            if (list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<User> GetAllBusinessesEmployees(int pageIndex, int pageSize)
        {
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd("dbo.Users_Select_Businesses_Employees", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            }, delegate (IDataReader reader, short set)
            {
                User user = MapSingleUser(reader);


                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(user);
                if (set == 0)
                {

                    totalCount = reader.GetSafeInt32(reader.FieldCount - 1);
                }
            });
            if (list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<User> GetAllByRole(int pageIndex, int pageSize, int roleId)
        {
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd("[dbo].[Users_SelectAll_ByRole]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@RoleId", roleId);
            }, delegate (IDataReader reader, short set)
            {
                User user = MapSingleUser(reader);


                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(user);
                if (set == 0)
                {

                    totalCount = reader.GetSafeInt32(reader.FieldCount - 1);
                }
            });
            if (list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        private UserBase GetUserAuthData(string email)
        {
            UserBase userAuthData = null;
            _dataProvider.ExecuteCmd("[dbo].[Users_Select_AuthData]", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Email", email);
            }, delegate (IDataReader reader, short set)
            {
                userAuthData = new UserBase();
                int startingIndex = 0;
                userAuthData.Id = reader.GetSafeInt32(startingIndex++);
                userAuthData.Email = reader.GetSafeString(startingIndex++);
                userAuthData.Role = reader.GetSafeString(startingIndex++);
            });
            if (userAuthData != null)
            {
                userAuthData.TenantId = "hard coded for now";
                return userAuthData;
            }
            else
            {
                throw new Exception("Invalid userName or password");
            }
        }
        private IUserAuthData Get(string email, string password)
        {
            UserBase user = null;
            string passwordFromDb = "";
            bool isConfirmed = false;
            _dataProvider.ExecuteCmd("dbo.Users_SelectPass_ByEmail", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Email", email);
            }, delegate (IDataReader reader, short set)
            {
                passwordFromDb = reader.GetSafeString(0);
                isConfirmed = reader.GetBoolean(1);
            });
            bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordFromDb);
            if (isValidCredentials)
            {
                if (isConfirmed)
                {
                    user = GetUserAuthData(email);
                }
                else
                {
                    throw new Exception("Email address not confirmed! Please check your email and try again.");
                }
            }
            else
            {
                throw new Exception("Password is incorrect");
            }
            return user;
        }
        public List<UsersRoleStatus> GetByRoleAndStatusId(int Status, int Role)
        {
            List<UsersRoleStatus> usersRole = new();
            _dataProvider.ExecuteCmd("dbo.Users_SelectByRoleId_StatusId", delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@RoleId", Role);
                col.AddWithValue("@StatusId", Status);
            }, delegate (IDataReader reader, short set)
            {

                int startingIndex = 0;
                usersRole.Add(MapSingleRoleStatusId(reader, ref startingIndex));

            });
            return usersRole;
        }
        public void UpdateDemographics(UserDemographicsUpdateRequest model)
        {
            string procName = "[dbo].[Users_UpdateDemographics]";
            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
            }, returnParameters: null);
        }
        private static void AddCommonParams(UserDemographicsUpdateRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Id", model.Id);
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Mi", model.Mi);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);
        }
    }
}