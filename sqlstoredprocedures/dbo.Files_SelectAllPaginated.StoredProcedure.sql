USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[Files_SelectAllPaginated]    Script Date: 2/26/2024 9:43:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Mike Miranda
-- Create date: 12/15/2023
-- Description: Used to get back a paginated list of files from the database.
-- Code Reviewer:

-- MODIFIED BY: Mike Henry
-- MODIFIED DATE: 12/21/2023
-- Code Reviewer: Selva
-- Note: Changes approved and implemented by Selva
-- =============================================


CREATE proc [dbo].[Files_SelectAllPaginated]
				@PageIndex int
				,@PageSize int

as
/* ----------- TEST CODE -------------

Declare @PageIndex int = 0, @PageSize int = 10

Execute dbo.Files_SelectAllPaginated @PageIndex, @PageSize

*/

BEGIN

	Declare @offset int = @PageIndex * @PageSize

	SELECT f.[Id]
			,f.[Name]
			,f.[Url]
			,f.[FileTypeId]
			,ft.[Name] as FileTypeName
			,f.[IsDeleted]
			,CreatedBy = dbo.fn_GetUserJSON(f.CreatedBy)
			,TotalCount = COUNT(1) OVER()

        FROM    dbo.Files as f inner join dbo.FileTypes as ft
			on f.FileTypeId = ft.Id
        ORDER BY f.Id

	OFFSET @offSet Rows
	Fetch Next @PageSize Rows ONLY


END
GO
