select [Year],count(*) as Players from players GROUP BY [Year]

SELECT [Year],InPO,count(*) as Batting from batting GROUP BY [Year],InPO

SELECT [Year],InPO,count(*) as Rosters from Rosters GROUP BY [Year],InPO

SELECT [Year],InPO, count(*) as Pitching from pitching GROUP BY [Year],InPO

SELECT [Year],InPO, count(*) as Running from Running GROUP BY [Year],InPO

SELECT [Year],InPO, count(*) as Defense from Defense GROUP BY [Year],InPO

SELECT [Year],InPO, count(*) as Rotations from Rotations GROUP BY [Year],InPO

