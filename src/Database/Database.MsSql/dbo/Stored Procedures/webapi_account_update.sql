
CREATE PROCEDURE [dbo].[webapi_account_update]
	@Id INT,
	@Password nvarchar(128) = NULL,
	@LastSignedinAt datetime = NULL,
	@StatusId int = NULL
AS
BEGIN
	IF(EXISTS(SELECT 1 FROM dbo.Account WHERE Id = @Id))
	BEGIN
		UPDATE dbo.Account
		SET [Password] = ISNULL(@Password, [Password]), [LastSignedinAt] = ISNULL(@LastSignedinAt, [LastSignedinAt]), [StatusId] = ISNULL(@StatusId, [StatusId])
		WHERE Id = @Id;
	
		RETURN 200;
	END

	RETURN 404;
END;