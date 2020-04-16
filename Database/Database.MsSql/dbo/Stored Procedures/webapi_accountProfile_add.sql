
CREATE PROCEDURE [dbo].[webapi_accountProfile_add]
	@AccountId int,
	@TypeId int,
	@LinkedId nvarchar(128),
	@CreatedAt datetime,
	@StatusId INT
AS
BEGIN
	INSERT INTO dbo.AccountProfile(AccountId, TypeId, LinkedId, CreatedAt, StatusId)
	VALUES(@AccountId, @TypeId, @LinkedId, @CreatedAt, @StatusId)
	
	SELECT SCOPE_IDENTITY();

	RETURN 200;
END;