select count(*) as Players from players

SELECT InPO,count(*) as Batting from batting GROUP BY InPO

SELECT InPO,count(*) as Rosters from rosters GROUP BY InPO

SELECT InPO, count(*) as Pitching from pitching GROUP BY InPO

SELECT InPO, count(*) as Running from Running GROUP BY InPO

SELECT InPO, count(*) as Defense from Defense GROUP BY InPO

