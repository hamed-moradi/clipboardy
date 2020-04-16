
CREATE PROCEDURE [dbo].[webapi_accountProfile_update]
	@Id INT,
	@ForgotPasswordToken nvarchar(128) = NULL,
	@StatusId int = NULL
AS
BEGIN
	IF(EXISTS(SELECT 1 FROM dbo.AccountProfile WHERE Id = @Id))
	BEGIN
		UPDATE dbo.AccountProfile
		SET [ForgotPasswordToken] = ISNULL(@ForgotPasswordToken, [ForgotPasswordToken]), [StatusId] = ISNULL(@StatusId, [StatusId])
		WHERE Id = @Id;
	
		RETURN 200;
	END

	RETURN 404;
END;