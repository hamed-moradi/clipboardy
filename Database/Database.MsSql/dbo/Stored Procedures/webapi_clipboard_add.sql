
CREATE PROCEDURE [dbo].[webapi_clipboard_add]
	@AccountId int,
	@DeviceId int,
	@TypeId int,
	@Content nvarchar(MAX),
	@CreatedAt datetime,
	@StatusId int
AS
BEGIN
	INSERT INTO dbo.Clipboard(AccountId, DeviceId, TypeId, Content, CreatedAt, StatusId)
	VALUES(@AccountId, @DeviceId, @TypeId, @Content, @CreatedAt, @StatusId)
	
	SELECT SCOPE_IDENTITY();

	RETURN 200;
END;