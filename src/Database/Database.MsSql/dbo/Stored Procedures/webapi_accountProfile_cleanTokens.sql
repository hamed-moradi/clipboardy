
CREATE PROCEDURE [dbo].[webapi_accountProfile_cleanTokens]
	@AccountId INT
AS
BEGIN
	IF(EXISTS(SELECT 1 FROM dbo.AccountProfile WHERE AccountId = @AccountId))
	BEGIN
		UPDATE dbo.AccountProfile
		SET [ForgotPasswordToken] = NULL
		WHERE AccountId = @AccountId;
	
		RETURN 200;
	END

	RETURN 404;
END;