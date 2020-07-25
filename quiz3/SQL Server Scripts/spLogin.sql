CREATE PROCEDURE spLogin
(
@userid VARCHAR(6)
)
AS
BEGIN 
	SELECT userpassword
	FROM users
	WHERE userid like @userid
END
GO