declare @Run iNT = 0


IF (@Run = 1)
BEGIN 
	SELECT [ID]
		  ,[StartDateTime]
		  ,[NoOfSteps]
		  ,[NoOfStepsRoot]      
		  ,[TableNumber]      
		  ,[MaxRoot]
		  ,[MinRoot]
		  ,[RootMainProfit]
		  ,[RootProfit0]
		  ,[RootProfit1]
		  ,[RootProfit2]
		  ,[RootProfit3]
		  ,[RootAllSub]
	  FROM [dbo].[AutoSessions]
	WHERE IsClosed = 1
END

/*
Shoe ID,Start At, No of Steps, No of Step without TIE, Table No, Max (6 threads), Min (6 threads), MainProfit, Profit0, Profit1, Profit2, Profit3, AllSubProfit
*/
--DECLARE @TableNumber INT = 6
DECLARE @StopLoss INT = -5
DECLARE @From DateTime = '2022-03-31'
DECLARE @To DateTime = '2022-04-01'

SELECT [ID]
      ,CASE WHEN [Card] = 1 THEN 'Banker' ELSE 'Player' END Card
      ,[InputDateTime]      
      ,[AutoSessionID] AS ShoeID
	  , ROW_NUMBER() OVER (ORDER BY AutoSessionID,ID) AS Step
      ,[MainProfit]	       
      ,[AllSubProfit]
	  , [MainProfit] + [AllSubProfit] * 2 AS CurrentTradeProfit
	  , CASE WHEN [Card] = 1 AND [MainProfit] + [AllSubProfit] * 2 > 0 THEN ([MainProfit] + [AllSubProfit] * 2) * 0.95 ELSE ([MainProfit] + [AllSubProfit] * 2) END RealProfit
	  ,SUM([MainProfit]) OVER (ORDER BY ID ASC) AS Accu_Main
	  ,SUM([AllSubProfit]) OVER (ORDER BY ID ASC) AS Accu_AllSub
	  ,SUM([AllSubProfit] * 2 + [MainProfit]) OVER (ORDER BY ID ASC) AS Accu_All
	  ,SUM(CASE WHEN [Card] = 1 AND [MainProfit] + [AllSubProfit] * 2 > 0 THEN ([MainProfit] + [AllSubProfit] * 2) * 0.95 ELSE ([MainProfit] + [AllSubProfit] * 2) END ) OVER (ORDER BY ID ASC) Acc_RealProfit
  FROM [dbo].[AutoRoots]
WHERE AutoSessionID IN 
	(	SELECT ID FROM AutoSessions 
		WHERE 
				StartDateTime between @From and @To
				--AND 
				--AutoSessionID = 715
				 --AND TableNumber IN (@TableNumber)
				AND IsClosed = 1
			) 
			ORDER BY AutoSessionID, ID
/*
PositionID, Card, Time, ShoeID, Main Profit, All Sub Profit, Current TradeProfit,RealProfit, Accu Main, Accu All Sub, Accu All, Acc_RealProfit
*/

			
DECLARE @Summary FLOAT = 0
DECLARE @LocalSum FLOAT = 0


DECLARE @ID INT

DECLARE cursor_product CURSOR
FOR SELECT ID FROM AutoSessions WHERE StartDateTime between @From and @To --AND TableNumber IN (@TableNumber)

OPEN cursor_product;

FETCH NEXT FROM cursor_product INTO @ID

WHILE @@FETCH_STATUS = 0
    BEGIN
		SET @LocalSum = (
				SELECT TOP 1 Acc_RealProfit 				
				FROM 
				(
				SELECT [ID]
					  ,CASE WHEN [Card] = 1 THEN 'Banker' ELSE 'Player' END Card
					  ,[InputDateTime]      
					  ,[AutoSessionID] AS ShoeID
					  , ROW_NUMBER() OVER (ORDER BY AutoSessionID,ID) AS Step
					  , COUNT(Card) OVER (PARTITION BY AutoSessionID) AS CountStep
					  
					  ,[MainProfit]	       
					  ,[AllSubProfit]
					  , [MainProfit] + [AllSubProfit] * 2 AS CurrentTradeProfit
					  , CASE WHEN [Card] = 1 AND [MainProfit] + [AllSubProfit] * 2 > 0 THEN ([MainProfit] + [AllSubProfit] * 2) * 0.95 ELSE ([MainProfit] + [AllSubProfit] * 2) END RealProfit
					  ,SUM([MainProfit]) OVER (ORDER BY ID ASC) AS Accu_Main
					  ,SUM([AllSubProfit]) OVER (ORDER BY ID ASC) AS Accu_AllSub
					  ,SUM([AllSubProfit] * 2 + [MainProfit]) OVER (ORDER BY ID ASC) AS Accu_All
					  ,SUM(   
					            CASE WHEN [Card] = 1 AND [MainProfit] + [AllSubProfit] * 2 > 0 THEN ([MainProfit] + [AllSubProfit] * 2) * 0.95 ELSE ([MainProfit] + [AllSubProfit] * 2) * 1.00 END  
							) OVER (ORDER BY ID ASC) Acc_RealProfit
				  FROM [dbo].[AutoRoots]
				WHERE AutoSessionID IN 
					(	SELECT ID FROM AutoSessions 
						WHERE 
								StartDateTime between @From and @To
								AND 
								AutoSessionID = @ID								 
							) 	
				) t 
				WHERE (Step < 40 AND Acc_RealProfit <= @StopLoss) 
				OR 
				(Step = 40  AND (Acc_RealProfit > 10 OR Acc_RealProfit < 0))
				OR (Step = CountStep)	)
				
				--PRINT '----------------------------------------'
				--PRINT 'Shoe ' + CAST(@ID AS NVARCHAR(10))
				
				
				IF (@LocalSum IS NULL)
					SET @LocalSum = 0.0
				SET @Summary = @Summary + @LocalSum


				--PRINT 'Profit ' + CAST(@LocalSum AS NVARCHAR(10))
				PRINT @Summary

        FETCH NEXT FROM cursor_product INTO @ID
    END;

CLOSE cursor_product;

DEALLOCATE cursor_product;


SELECT @Summary



--SELECT * FROM AutoSessions WHERE StartDateTime between '2022-03-30' and '2022-04-01'

