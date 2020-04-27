DECLARE @f varchar(50)
DECLARE @l varchar(50)
SET @f = 'Carlos'
SET @l = 'Martinez'
SELECT * FROM Players WHERE FirstName = @f AND LastName = @l

SELECT * FROM Rosters R
JOIN Teams T on R.TeamId = T.TeamId
WHERE PlayerId IN (SELECT PlayerId FROM Players WHERE FirstName = @f AND LastName = @l)

SELECT * FROM Batting  WHERE PlayerId IN (SELECT PlayerId FROM Players WHERE FirstName = @f AND LastName = @l) ORDER BY PlayerId

SELECT * FROM Pitching WHERE PlayerId IN (SELECT PlayerId FROM Players WHERE FirstName = @f AND LastName = @l)

/*
UPDATE Players SET IsInvalid = 1
Where PlayerId IN (138,451,713,870, 971, 1333)
*/