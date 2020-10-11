CREATE PROCEDURE [dbo].[webapi_accountDevice_getFirst]
	@Id int = NULL,
	@AccountId int = NULL,
	@DeviceId nvarchar(128) = NULL,
	@DeviceName nvarchar(128) = NULL,
	@DeviceType nvarchar(128) = NULL,
	@StatusId int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(1) Id, AccountId, DeviceId, DeviceName, DeviceType, StatusId
	FROM dbo.AccountDevice WITH(NOLOCK)
	WHERE (@Id IS NULL OR [Id] = @Id)
	    AND (@AccountId IS NULL OR [AccountId] = @AccountId)
		AND (@DeviceId IS NULL OR DeviceId LIKE '%'+@DeviceId+'%')
		AND (@DeviceName IS NULL OR DeviceName LIKE '%'+@DeviceName+'%')
		AND (@DeviceType IS NULL OR @DeviceType LIKE '%'+@DeviceType+'%')
	    AND (@StatusId IS NULL OR [StatusId] = @StatusId)
	ORDER BY Id desc;

	RETURN 200;
END;