CREATE PROCEDURE [dbo].[webapi_account_authenticate]
  @Token VARCHAR(128)
AS
  DECLARE @devieId INT = NULL, @statusId INT = NULL;
  SELECT @devieId = ad.Id, @statusId = ad.StatusId FROM AccountDevice ad WITH(NOLOCK) WHERE ad.Token = @Token;
  IF(@devieId IS NULL)
    RETURN 404;
  IF(@statusId <> 10)
    RETURN 401;

  SELECT
    a.Id, a.Username, a.LastSignedinAt,
    ad.DeviceId
  FROM Account a WITH(NOLOCK)
  INNER JOIN AccountDevice ad WITH(NOLOCK) ON a.Id = ad.AccountId
  WHERE ad.Id = @devieId

  RETURN 200;