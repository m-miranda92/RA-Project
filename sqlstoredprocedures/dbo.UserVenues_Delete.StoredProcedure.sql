USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[UserVenues_Delete]    Script Date: 2/26/2024 9:43:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Mike Miranda
-- Create date: 01/13/2024
-- Description: Used to delete a favorited venue
-- Code Reviewer: Mike Henry

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer: 
-- Note: 
-- =============================================

CREATE proc [dbo].[UserVenues_Delete]
				@Id int
				,@VenueId int

as
/* --------------TEST CODE ----------------
Declare @Id int = 32;
Declare @VenueId int = 1050;

Execute dbo.UserVenues_Delete @Id, @VenueId

*/

BEGIN

Delete dbo.UserVenues
	WHERE @Id = UserId and @VenueID = VenueId

END
GO
