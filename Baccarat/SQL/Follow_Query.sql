
/*[ID], UIResult,RealCard, AutoSessionID, Profit, Acc_Profit, Acc_RealProfit, ProfitMod, Acc_ProfitMod, Acc_RealProfitMod,Profit_CutLoss &*/


SELECT  AutoFollow2In.[ID], 
		UIResult
      ,CASE WHEN [Card] = 1 THEN 'Banker' ELSE 'Player' END RealCard
	  , AutoSessionID
	  
	  , Profit
	  ,SUM(Profit) OVER (ORDER BY AutoSessionID, AutoFollow2In.ID ASC) AS Acc_Profit
	  ,SUM(CASE WHEN [Card] = 1 AND Profit >0 THEN 0.95 * Profit ELSE 1.00 * Profit END) OVER (ORDER BY AutoSessionID, AutoFollow2In.ID ASC) AS Acc_RealProfit
	  
	  ,ProfitMod	  
	  ,SUM(ProfitMod) OVER (ORDER BY AutoSessionID, AutoFollow2In.ID ASC) AS Acc_ProfitMod
	  ,SUM(CASE WHEN [Card] = 1 AND ProfitMod >0 THEN 0.95 * ProfitMod ELSE 1.00 * ProfitMod END) OVER (ORDER BY AutoSessionID, AutoFollow2In.ID ASC) AS Acc_RealProfitMod

	  ,Profit_CutLoss
	  ,SUM(Profit_CutLoss) OVER (ORDER BY AutoSessionID, AutoFollow2In.ID ASC) AS Acc_Profit_CutLoss
	  ,SUM(CASE WHEN [Card] = 1 AND Profit_CutLoss >0 THEN 0.95 * Profit_CutLoss ELSE 1.00 * Profit_CutLoss END) OVER (ORDER BY AutoSessionID, AutoFollow2In.ID ASC) AS Acc_RealProfit_CutLoss

	  ,ProfitMod_CutLoss
	  ,SUM(ProfitMod_CutLoss) OVER (ORDER BY AutoSessionID, AutoFollow2In.ID ASC) AS Acc_Profit_CutLoss
	  ,SUM(CASE WHEN [Card] = 1 AND ProfitMod_CutLoss >0 THEN 0.95 * ProfitMod_CutLoss ELSE 1.00 * ProfitMod_CutLoss END) OVER (ORDER BY AutoSessionID, AutoFollow2In.ID ASC) AS Acc_RealProfitMod_CutLoss

FROM AutoFollow2In  INNER JOIN 
AutoSessions ON AutoFollow2In.AutoSessionID = AutoSessions.ID
WHERE TableNumber = 1
ORDER BY AutoSessionID, AutoFollow2In.ID


--CREATE INDEX IDX_AutoFollowF_Card
--ON AutoFollowF (Card)

--CREATE INDEX IDX_AutoFollowF_AutoSessionID
--ON AutoFollowF (AutoSessionID)

--select * from AutoSessions


