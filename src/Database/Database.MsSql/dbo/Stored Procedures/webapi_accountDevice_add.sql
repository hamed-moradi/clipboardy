
CREATE PROCEDURE [dbo].[webapi_accountDevice_add]
	@AccountId int,
	@DeviceId nvarchar(128),
	@DeviceName nvarchar(128),
	@DeviceType nvarchar(128),
	@CreatedAt datetime,
	@StatusId INT
AS
BEGIN
	INSERT INTO dbo.AccountDevice(AccountId, DeviceId, DeviceName, DeviceType, CreatedAt, StatusId)
	VALUES(@AccountId, @DeviceId, @DeviceName, @DeviceType, @CreatedAt, @StatusId)
	
	SELECT SCOPE_IDENTITY();

	RETURN 200;
END;