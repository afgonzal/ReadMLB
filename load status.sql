use mlb
select [Year],count(*) as Players from players WITH (NOLOCK) GROUP BY [Year]

SELECT [Year],InPO,count(*) as Batting  from batting WITH (NOLOCK) GROUP BY [Year],InPO

SELECT [Year],InPO,count(*) as Rosters from Rosters WITH (NOLOCK) GROUP BY [Year],InPO ORDER BY [Year],InPO

SELECT [Year],InPO, count(*) as Pitching from pitching WITH (NOLOCK) GROUP BY [Year],InPO ORDER BY [Year],InPO

SELECT [Year],InPO, count(*) as Running from Running WITH (NOLOCK) GROUP BY [Year],InPO ORDER BY [Year],InPO

SELECT [Year],InPO, count(*) as Defense from Defense WITH (NOLOCK) GROUP BY [Year],InPO ORDER BY [Year],InPO

SELECT [Year],InPO, count(*) as Rotations from Rotations  WITH (NOLOCK) GROUP BY [Year],InPO ORDER BY [Year],InPO

SELECT [Year],InPO,League, count(*) as Matchs from Matches WITH (NOLOCK) GROUP BY [Year],InPO, League 
order by [Year], InPO, League

