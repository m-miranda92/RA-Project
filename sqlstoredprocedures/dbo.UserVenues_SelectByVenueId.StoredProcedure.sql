USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[UserVenues_SelectByVenueId]    Script Date: 2/26/2024 9:43:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Mike Miranda
-- Create date: 01/13/2024
-- Description: Used to Select by VenueId
-- Code Reviewer: 

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer: 
-- Note: 
-- =============================================

CREATE proc [dbo].[UserVenues_SelectByVenueId]
			@Id int

as
/* ----------- TEST CODE -------------

Declare @Id int = 6
Execute dbo.UserVenues_SelectByVenueId @Id

*/

Begin

	SELECT u.Id
			,u.FirstName
			,u.LastName
			,u.Mi
			,u.AvatarUrl
	FROM Users as u
	INNER JOIN UserVenues as uv ON u.Id = uv.UserId
	INNER JOIN Venues as v ON uv.VenueId = v.Id
	WHERE v.Id = @Id



End
GO
