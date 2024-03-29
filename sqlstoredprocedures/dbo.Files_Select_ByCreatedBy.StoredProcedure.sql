USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[Files_Select_ByCreatedBy]    Script Date: 2/26/2024 9:43:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Mike Miranda
-- Create date: 12/15/2023
-- Description: Used to see all the files created by a user; CreatedBy is joined with the Users table under FK constraints
-- Code Reviewer:

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE proc [dbo].[Files_Select_ByCreatedBy]
			@Id int

as
/* -------- TEST CODE -----------
Declare @Id int = 4

Execute dbo.Files_Select_ByCreatedBy @Id




*/

BEGIN

	Select f.Id
			,f.Name
			,f.Url
			,f.FileTypeId
			,ft.Name as FileName
			,f.IsDeleted
			, dbo.fn_GetUserJSON(@Id) as CreatedBy
	from dbo.Files as f
		inner join dbo.FileTypes as ft
		on f.FileTypeId = ft.Id
	Where f.CreatedBy = @Id

END
GO
