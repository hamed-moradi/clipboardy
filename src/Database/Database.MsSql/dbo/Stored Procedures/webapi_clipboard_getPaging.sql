CREATE PROCEDURE [dbo].[webapi_clipboard_getPaging]
	@AccountId INT = NULL,
	@DeviceId INT = NULL,
	@TypeId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@CreatedAt datetime = NULL,
	@StatusId int = NULL,
	@Skip INT = 0,
	@Take INT = 10,
	@Order VARCHAR(4) = NULL,
	@OrderBy VARCHAR(32) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *, COUNT(1) OVER() AS TotalCount FROM dbo.Clipboard AS clb WITH(NOLOCK)
	WHERE (@AccountId IS NULL OR [AccountId] = @AccountId)
	    AND (@DeviceId IS NULL OR [DeviceId] = @DeviceId)
	    AND (@TypeId IS NULL OR [TypeId] = @TypeId)
		AND (@Content IS NULL OR Content LIKE '%'+@Content+'%')
	    AND (@CreatedAt IS NULL OR CONVERT(date, [CreatedAt]) = CONVERT(date, @CreatedAt))
	    AND (@StatusId IS NULL OR [StatusId] = @StatusId)
	ORDER BY
		CASE WHEN @OrderBy = 'Id' AND @Order = 'ASC' THEN Id END ASC,
		CASE WHEN @OrderBy = 'Id' AND (@Order <> 'ASC' OR @Order IS NULL) THEN Id END DESC,
		CASE WHEN @OrderBy = 'AccountId' AND @Order = 'ASC' THEN AccountId END ASC,
		CASE WHEN @OrderBy = 'AccountId' AND (@Order <> 'ASC' OR @Order IS NULL) THEN AccountId END DESC,
		CASE WHEN @OrderBy = 'DeviceId' AND @Order = 'ASC' THEN DeviceId END ASC,
		CASE WHEN @OrderBy = 'DeviceId' AND (@Order <> 'ASC' OR @Order IS NULL) THEN DeviceId END DESC,
	    CASE WHEN @OrderBy = 'TypeId' AND @Order = 'ASC' THEN TypeId END ASC,
		CASE WHEN @OrderBy = 'TypeId' AND (@Order <> 'ASC' OR @Order IS NULL) THEN TypeId END DESC,
		CASE WHEN @OrderBy = 'CreatedAt' AND @Order = 'ASC' THEN CreatedAt END ASC,
		CASE WHEN @OrderBy = 'CreatedAt' AND (@Order <> 'ASC' OR @Order IS NULL) THEN CreatedAt END DESC,
		CASE WHEN @OrderBy = 'StatusId' AND @Order = 'ASC' THEN StatusId END ASC,
		CASE WHEN @OrderBy = 'StatusId' AND (@Order <> 'ASC' OR @Order IS NULL) THEN StatusId END DESC,
		CASE WHEN @OrderBy IS NULL THEN CURRENT_TIMESTAMP END
	OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

	RETURN 200;
END;