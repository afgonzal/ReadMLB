DECLARE @f varchar(50)
DECLARE @l varchar(50)
SET @f = 'Brian'
SET @l = 'Anderson'
SELECT * FROM Players WHERE FirstName = @f AND LastName = @l

SELECT * FROM Rosters R
JOIN Teams T on R.TeamId = T.TeamId
WHERE PlayerId IN (SELECT PlayerId FROM Players WHERE FirstName = @f AND LastName = @l)

SELECT * FROM Batting WHERE PlayerId IN (SELECT PlayerId FROM Players WHERE FirstName = @f AND LastName = @l)

SELECT * FROM Pitching WHERE PlayerId IN (SELECT PlayerId FROM Players WHERE FirstName = @f AND LastName = @l)