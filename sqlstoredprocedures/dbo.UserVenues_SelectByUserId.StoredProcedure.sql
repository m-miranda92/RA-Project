USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[UserVenues_SelectByUserId]    Script Date: 2/26/2024 9:43:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Mike Miranda
-- Create date: 01/13/2024
-- Description: Used to Select by UserId
-- Code Reviewer: Roland Adams

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer: 
-- Note: 
-- =============================================

CREATE proc [dbo].[UserVenues_SelectByUserId]
			@Id int

as
/* ----------- TEST CODE -------------

Declare @Id int = 1047
Execute dbo.UserVenues_SelectByUserId @Id

*/

Begin

SELECT v.Id as VenueId
		,v.OrganizationId
		,v.Name as VenueName
		,v.Description
		,v.Url
		,vt.Id
		,vt.Name as VenueType
		,L.Id
		,L.LineOne
		,L.LineTwo
		,L.City
		,L.Zip
		,S.Id
		,S.Name as State
		,L.Latitude
		,L.Longitude
FROM Users as u
INNER JOIN UserVenues as uv ON u.Id = uv.UserId
INNER JOIN Venues as v ON uv.VenueId = v.Id
INNER JOIN Locations L ON V.LocationId = L.[Id] 
INNER JOIN States S on L.StateId = s.Id
INNER JOIN VenueTypes vt on v.VenueTypeId = vt.Id
WHERE u.Id = @Id


End
GO
