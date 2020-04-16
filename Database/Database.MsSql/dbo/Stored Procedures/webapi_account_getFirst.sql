CREATE PROCEDURE [dbo].[webapi_account_getFirst]
	@Id INT = NULL,
	@Username NVARCHAR(128) = NULL,
	@Password NVARCHAR(128) = NULL,
	@ProviderId INT = NULL,
	@LastSignedinAt datetime = NULL,
	@StatusId int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(1) Id, Username, Password, ProviderId, LastSignedinAt, StatusId
	FROM dbo.Account AS clb WITH(NOLOCK)
	WHERE (@Id IS NULL OR (@Id IS NOT NULL AND [Id] = @Id))
		AND (@Username IS NULL OR Username LIKE '%'+@Username+'%')
		AND (@Password IS NULL OR Password LIKE '%'+@Password+'%')
	    AND (@ProviderId IS NULL OR (@ProviderId IS NOT NULL AND [ProviderId] = @ProviderId))
	    AND (@LastSignedinAt IS NULL OR (@LastSignedinAt IS NOT NULL AND convert(date, [LastSignedinAt]) = convert(date, @LastSignedinAt)))
	    AND (@StatusId IS NULL OR (@StatusId IS NOT NULL AND [StatusId] = @StatusId))
	ORDER BY Id desc;

	RETURN 200;
END;