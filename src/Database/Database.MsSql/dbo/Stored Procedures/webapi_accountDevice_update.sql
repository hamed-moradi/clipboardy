
CREATE PROCEDURE [dbo].[webapi_accountDevice_update]
	@Id INT,
	@StatusId int = NULL
AS
BEGIN
	IF(EXISTS(SELECT 1 FROM dbo.AccountDevice WHERE Id = @Id))
	BEGIN
		UPDATE dbo.AccountDevice
		SET [StatusId] = ISNULL(@StatusId, [StatusId])
		WHERE Id = @Id;
	
		RETURN 200;
	END

	RETURN 404;
END;