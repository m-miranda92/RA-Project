USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[Files_RecoverById]    Script Date: 2/26/2024 9:43:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Mike Miranda
-- Create date: 12/15/2023
-- Description: Used to delete records out of the Files table
-- Code Reviewer:

-- MODIFIED BY: Mike Henry
-- MODIFIED DATE: 12/21/2023
-- Code Reviewer: Selva Miranda
-- Note:Selva approved changes
-- =============================================

Create proc [dbo].[Files_RecoverById]
			@Id int
			
as
/* ---------- TEST CODE ------------

	Declare @Id int =  2

		SELECT [Id]
			  ,[Name]
			  ,[Url]
			  ,[FileTypeId]
			  ,[IsDeleted]
			  ,[CreatedBy]
			  ,[DateCreated]
		From dbo.Files
		Where Id = @Id

	Execute dbo.Files_DeleteById @Id

		SELECT [Id]
			  ,[Name]
			  ,[Url]
			  ,[FileTypeId]
			  ,[IsDeleted]
			  ,[CreatedBy]
			  ,[DateCreated]
		From dbo.Files
		Where Id = @Id


*/

BEGIN

		update [dbo].[Files]

		set IsDeleted = 0

		WHERE Id = @Id
				

End
GO
