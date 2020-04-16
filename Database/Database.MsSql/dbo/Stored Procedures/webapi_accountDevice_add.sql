
CREATE PROCEDURE [dbo].[webapi_accountDevice_add]
	@AccountId int,
	@Token varchar(128),
	@DeviceId nvarchar(128),
	@DeviceName nvarchar(128),
	@DeviceType nvarchar(128),
	@CreatedAt datetime,
	@StatusId INT
AS
BEGIN
	INSERT INTO dbo.AccountDevice(AccountId, Token, DeviceId, DeviceName, DeviceType, CreatedAt, StatusId)
	VALUES(@AccountId, @Token, @DeviceId, @DeviceName, @DeviceType, @CreatedAt, @StatusId)
	
	SELECT SCOPE_IDENTITY();

	RETURN 200;
END;