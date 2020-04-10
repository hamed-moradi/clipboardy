CREATE PROCEDURE dbo.webapi_account_authenticate
  @Token VARCHAR(128),
  @StatusCode INT OUT
AS
  DECLARE @accountId INT = NULL, @statusId INT = NULL;
  SELECT @accountId = ad.AccountId, @statusId = ad.StatusId FROM AccountDevice ad WHERE ad.Token = @Token;
  IF(@accountId IS NULL)
    SET @StatusCode = 400;
  IF(@statusId <> 10)
    SET @StatusCode = 404;
  SET @StatusCode = 200;
  RETURN @StatusCode;