CREATE PROCEDURE [dbo].[webapi_accountProfile_getFirst]
	@Id int = NULL,
	@AccountId int = NULL,
	@TypeId int = NULL,
	@LinkedId nvarchar(128) = NULL,
	@StatusId int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(1) Id, AccountId, TypeId, LinkedId, ForgotPasswordToken, StatusId
	FROM dbo.AccountProfile WITH(NOLOCK)
	WHERE (@Id IS NULL OR [Id] = @Id)
	    AND (@AccountId IS NULL OR [AccountId] = @AccountId)
		AND (@TypeId IS NULL OR TypeId = @TypeId)
		AND (@LinkedId IS NULL OR LinkedId LIKE '%'+@LinkedId+'%')
	    AND (@StatusId IS NULL OR [StatusId] = @StatusId)
	ORDER BY Id desc;

	RETURN 200;
END;