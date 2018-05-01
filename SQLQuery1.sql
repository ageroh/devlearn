
CREATE ASSEMBLY [SqlClr]
FROM 'C:\Code\DevLearn\SqlClr\bin\Debug\SqlClr.dll' 
WITH PERMISSION_SET = UNSAFE
GO
CREATE SCHEMA [rmq]
GO
CREATE PROCEDURE [dbo].[pr_clr_PostRabbitMsg]
	@msg [nvarchar](max)
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [SqlClr].[SqlClr.RabbitMqSqlServer].[Pr_clr_PostRabbitMsg]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Scores_On_After_Insert]
   ON  [dbo].[Score]
   AFTER INSERT
AS 
BEGIN
	DECLARE @RetVal NVARCHAR(MAX)
    SELECT @RetVal = (
		SELECT inserted.*, HomeTeam as 'HomeTeam', AwayTeam as 'AwayTeam'
		FROM inserted 
		INNER JOIN dbo.[Event]
		ON [Event].EventId = inserted.EventId
		FOR JSON PATH)

	EXEC dbo.pr_clr_PostRabbitMsg @RetVal
END
GO
ALTER TABLE [dbo].[Score] ENABLE TRIGGER [Scores_On_After_Insert]
GO

USE [master]
GO
ALTER DATABASE [DevLearn] SET  READ_WRITE 
GO


-- ENABLE CLR
sp_configure @configname=clr_enabled, @configvalue=1
GO

RECONFIGURE
Go
