CREATE PROCEDURE [dbo].[webapi_clipboard_getFirst]
	@Id int = NULL,
	@AccountId int = NULL,
	@DeviceId int = NULL,
	@TypeId int = NULL,
	@Content nvarchar(128) = NULL,
	@CreatedAt datetime = NULL,
	@StatusId int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(1) Content, TypeId, CreatedAt
	FROM dbo.Clipboard WITH(NOLOCK)
	WHERE (@Id IS NULL OR [Id] = @Id)
	    AND (@AccountId IS NULL OR [AccountId] = @AccountId)
		AND (@DeviceId IS NULL OR DeviceId = @DeviceId)
		AND (@TypeId IS NULL OR TypeId = @TypeId)
		AND (@Content IS NULL OR Content LIKE '%'+@Content+'%')
		AND (@CreatedAt IS NULL OR convert(date, CreatedAt) = convert(date, @CreatedAt))
	    AND (@StatusId IS NULL OR [StatusId] = @StatusId)
	ORDER BY Id desc;

	RETURN 200;
END;