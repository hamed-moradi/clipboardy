
CREATE PROCEDURE [dbo].[webapi_account_add]
	@Username varchar(128),
	@Password NVARCHAR(128) = NULL,
	@ProviderId INT,
	@CreatedAt datetime,
	@StatusId INT
AS
BEGIN
	INSERT INTO dbo.[Account](Username, Password, ProviderId, CreatedAt, StatusId)
	VALUES(@Username, @Password, @ProviderId, @CreatedAt, @StatusId)
	
	SELECT SCOPE_IDENTITY();

	RETURN 200;
END;