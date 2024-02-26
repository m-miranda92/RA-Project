using Sabio.Data.Providers;
using Sabio.Models.Requests.Venues;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sabio.Models.Domain.Venues;
using Sabio.Data;
using Sabio.Models;
using Sabio.Models.Domain;
using Microsoft.AspNetCore.Components.Routing;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Sabio.Services.Interfaces.Locations;
using Sabio.Services.Interfaces;
using System.Diagnostics;
using Sabio.Services.Interfaces.Venues;
using Sabio.Models.Requests.UserVenues;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.Blogs;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace Sabio.Services.Venues
{

    public class VenueService : IVenueService
    {
        private IDataProvider _data = null;
        private ILocationService _location = null;
        ILookUpService _lookUp = null;
        public VenueService(IDataProvider data, ILocationService location, ILookUpService lookup)
        {
            _data = data;
            _location = location;
            _lookUp = lookup;
        }
        public int Add(VenueAddRequest request, int userId)
        {
            int Id = 0;

            string procName = "dbo.Venues_Insert";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {

                    commonParams(request, col);
                    col.AddWithValue("@CreatedBy", userId);


                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    col.Add(idOut);
                }, returnParameters: delegate (SqlParameterCollection returnCol)
                {
                    object oId = returnCol["@Id"].Value;
                    int.TryParse(oId.ToString(), out Id);
                });

            return Id;
        }
        public void Update(VenueUpdateRequest request, int userId)
        {
            string procName = "[dbo].[Venues_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", request.Id);

                    commonParams(request, col);


                    col.AddWithValue("@ModifiedBy", userId);
                }, returnParameters: null);
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[Venues_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                }, returnParameters: null);
        }
        public Venue Get(int id)
        {
            string procName = "[dbo].[Venues_Select_ById]";
            Venue venue = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {

                int startingIndex = 0;
                venue = MapVenue(reader, ref startingIndex);
            });

            return venue;
        }
        public int AddFavorite(UserVenueAddRequest request, int userId)
        {
            int Id = 0;

            string procName = "[dbo].[UserVenues_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {

                    addCommonParams(request, col);

                });
            return Id;
        }
        public void DeleteFavorite(int Id, int venueId)
        {
            string procName = "[dbo].[UserVenues_Delete]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", Id);
                    col.AddWithValue("@VenueId", venueId);
                }, returnParameters: null);
        }
        public List<BaseUser> GetByVenueId(int id)
        {
            string procName = "[dbo].[UserVenues_SelectByVenueId]";
            List<BaseUser> baseUsers = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {

                int startingIndex = 0;
                BaseUser baseUser = MapVenueBaseUser(reader, ref startingIndex);
                if (baseUsers == null)
                {
                    baseUsers = new List<BaseUser>();
                }
                baseUsers.Add(baseUser);

            });
            return baseUsers;
        }
        public List<Venue> GetByUserId(int id)
        {
            string procName = "[dbo].[UserVenues_SelectByUserId]";
            List<Venue> userVenues = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Venue userVenue = MapVenueBaseVenue(reader, ref startingIndex);
                if (userVenues == null)
                {
                    userVenues = new List<Venue>();
                }
                userVenues.Add(userVenue);
            });
            return userVenues;
        }
        public Paged<Venue> Get(int pageIndex, int pageSize)
        {
            Paged<Venue> pagedList = null;
            List<Venue> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Venues_SelectAll]",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    if (list == null)
                    {
                        list = new List<Venue>();
                    }


                    int startingIndex = 0;
                    Venue venue = MapVenue(reader, ref startingIndex);
                    list.Add(venue);

                    if (recordSetIndex == 0)
                    {

                        totalCount = reader.GetSafeInt32(reader.FieldCount - 1);
                    }
                }
            );

            if (list != null)
            {

                pagedList = new Paged<Venue>(
                    data: list,
                    page: pageIndex,
                    pagesize: pageSize,
                    totalCount: totalCount
                );
            }
            return pagedList;
        }
        public Paged<Venue> Get(int createdBy, int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Venues_Select_ByCreatedBy]";
            Paged<Venue> pagedList = null;
            List<Venue> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                (param) =>
                {
                    param.AddWithValue("@CreatedBy", createdBy);
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    if (list == null)
                    {
                        list = new List<Venue>();
                    }

                    int startingIndex = 0;
                    Venue venue = MapVenue(reader, ref startingIndex);
                    list.Add(venue);

                    if (recordSetIndex == 0)
                    {

                        totalCount = reader.GetSafeInt32(reader.FieldCount - 1);
                    }
                }
            );

            if (list != null)
            {

                pagedList = new Paged<Venue>(
                    data: list,
                    page: pageIndex,
                    pagesize: pageSize,
                    totalCount: totalCount
                );
            }

            return pagedList;
        }
        public Paged<Venue> GetByOrgId(int OrganizationId, int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Venues_Select_ByCreatedBy]";
            Paged<Venue> pagedList = null;
            List<Venue> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                (param) =>
                {
                    param.AddWithValue("@OrganizationId", OrganizationId);
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    if (list == null)
                    {
                        list = new List<Venue>();
                    }

                    int startingIndex = 0;
                    Venue venue = MapVenue(reader, ref startingIndex);
                    list.Add(venue);
                    totalCount = reader.GetSafeInt32(reader.FieldCount - 1);
                }
            );

            if (list != null)
            {
                pagedList = new Paged<Venue>(
                data: list,
                page: pageIndex,
                pagesize: pageSize,
                totalCount: totalCount
            );
            }

            return pagedList;
        }

        private static void commonParams(VenueAddRequest request, SqlParameterCollection col)
        {
            col.AddWithValue("@OrganizationId", request.OrganizationId);
            col.AddWithValue("@Name", request.Name);
            col.AddWithValue("@Description", request.Description);
            col.AddWithValue("@LocationId", request.LocationId);
            col.AddWithValue("@Url", request.Url);
            col.AddWithValue("@VenueTypeId", request.VenueTypeId);
        }
        private static void addCommonParams(UserVenueAddRequest request, SqlParameterCollection col)
        {
            col.AddWithValue("@UserId", request.UserId);
            col.AddWithValue("@VenueId", request.VenueId);

        }
        private BaseUser MapVenueBaseUser(IDataReader reader, ref int startingIndex)
        {
            BaseUser baseUser = new BaseUser();

            baseUser.Id = reader.GetSafeInt32(startingIndex++);
            baseUser.FirstName = reader.GetSafeString(startingIndex++);
            baseUser.LastName = reader.GetSafeString(startingIndex++);
            baseUser.Mi = reader.GetSafeString(startingIndex++);
            baseUser.AvatarUrl = reader.GetSafeString(startingIndex++);

            return baseUser;
        }
        private Venue MapVenueBaseVenue(IDataReader reader, ref int startingIndex)
        {
            Venue userVenue = new Venue();
            userVenue.Id = reader.GetSafeInt32(startingIndex++);
            userVenue.OrganizationId = reader.GetSafeInt32(startingIndex++);
            userVenue.Name = reader.GetSafeString(startingIndex++);
            userVenue.Description = reader.GetSafeString(startingIndex++);
            userVenue.Url = reader.GetSafeString(startingIndex++);
            userVenue.VenueType = MapSingleLookUp(reader, ref startingIndex);
            userVenue.Location = new Location
            {
                Id = reader.GetSafeInt32(startingIndex++),
                LineOne = reader.GetSafeString(startingIndex++),
                LineTwo = reader.GetSafeString(startingIndex++),
                City = reader.GetSafeString(startingIndex++),
                Zip = reader.GetSafeString(startingIndex++),
                State = MapSingleLookUp(reader, ref startingIndex)
            };
            userVenue.Location.Latitude = reader.GetSafeDouble(startingIndex++);
            userVenue.Location.Longitude = reader.GetSafeDouble(startingIndex++);
            return userVenue;
        }
        public LookUp MapSingleLookUp(IDataReader reader, ref int startingIndex)
        {
            LookUp lookUp = new LookUp();
            lookUp.Id = reader.GetSafeInt32(startingIndex++);
            lookUp.Name = reader.GetSafeString(startingIndex++);
            return lookUp;
        }
        private Venue MapVenue(IDataReader reader, ref int startingIndex)
        {
            Venue venue = new Venue();

            venue.Id = reader.GetSafeInt32(startingIndex++);
            venue.OrganizationId = reader.GetSafeInt32(startingIndex++);
            venue.Name = reader.GetSafeString(startingIndex++);
            venue.Description = reader.GetSafeString(startingIndex++);
            venue.Url = reader.GetSafeString(startingIndex++);
            venue.CreatedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            venue.ModifiedBy = reader.DeserializeObject<BaseUser>(startingIndex++);
            venue.DateCreated = reader.GetSafeDateTime(startingIndex++);
            venue.DateModified = reader.GetSafeDateTime(startingIndex++);
            venue.VenueType = _lookUp.MapSingleLookUp(reader, ref startingIndex);
            venue.Location = _location.MapLocation(reader, ref startingIndex);
            return venue;
        }

        public List<LookUp> GetAllByOrgId(int userId)
        {
            string procName = "[dbo].[Venues_SelectAll_ByOrgId]";
            List<LookUp> list = new List<LookUp>();

            _data.ExecuteCmd(procName,
                (param) =>
                {
                    param.AddWithValue("@UserId", userId);
                },
                (reader, recordSetIndex) =>
                {
                    if (list == null)
                    {
                        list = new List<LookUp>();
                    }
                    int startingIndex = 0;
                    LookUp venue = _lookUp.MapSingleLookUp(reader, ref startingIndex);
                    list.Add(venue);
                }
            );
            return list;
        }
    }
}

