USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[UserVenues_Insert]    Script Date: 2/26/2024 9:43:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Mike Miranda
-- Create date: 01/16/2024
-- Description: Used to insert a favorited venue
-- Code Reviewer: 

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer: 
-- Note: 
-- =============================================

CREATE proc [dbo].[UserVenues_Insert]
			@UserId int
			,@VenueId int

as
/* ----------- TEST CODE -----------

Declare @UserId int = 1047
Declare @VenueId int = 46

Execute dbo.UserVenues_Insert @UserId
							,@VenueId

SELECT [UserId]
      ,[VenueId]
  FROM [dbo].[UserVenues]




*/

BEGIN

INSERT INTO [dbo].[UserVenues]
           ([UserId]
           ,[VenueId])
     VALUES
           (@UserId
           ,@VenueId)

END
GO
