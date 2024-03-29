USE [RumbApp]
GO
/****** Object:  StoredProcedure [dbo].[Files_Insert]    Script Date: 2/26/2024 9:43:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Mike Miranda
-- Create date: 12/15/2023
-- Description: Used to insert(Add) data into the Files table.
-- Code Reviewer: Roland Adams

-- MODIFIED BY: Mike Miranda
-- MODIFIED DATE: 01/03/2024
-- Code Reviewer: Roland Adams
-- Note: Modified to allow batch inserting for multiple file uploads.
-- =============================================

CREATE proc [dbo].[Files_Insert]
					 @CreatedBy int
					,@BatchFiles dbo.BatchFiles READONLY

AS 

/*
==========TEST CODE===========

DECLARE @BatchFiles AS dbo.BatchFiles;
INSERT INTO @BatchFiles (Name, URL, FileTypeId)
VALUES
    ('Test File 4', 'Test Url 4', 1),
    ('Test File 5', 'Test Url 5', 4)
   
EXEC [dbo].[Files_Insert]  8, @BatchFiles;

=============================
*/

BEGIN

CREATE TABLE #TempTable (Id INT, Url NVARCHAR(255))

	INSERT INTO [dbo].[Files]
				(Name
				,Url
				,FileTypeId
				,CreatedBy
				,IsDeleted)
    OUTPUT Inserted.Id, 
		   Inserted.Url 
    INTO #TempTable (Id
				   ,Url)
	SELECT Name
		  ,Url
		  ,FileTypeId
		  ,@CreatedBy
		  ,1
	FROM @BatchFiles


	SELECT Id
		  ,Url 
	FROM #TempTable

DROP TABLE #TempTable

END
GO
