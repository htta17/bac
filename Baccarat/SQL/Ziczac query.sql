
/*[ID], UIResult,RealCard, AutoSessionID, Profit, Acc_Profit, Acc_RealProfit, ProfitMod, Acc_ProfitMod, Acc_RealProfitMod,Profit_CutLoss &*/


SELECT  AutoZiczacF.[ID], 
		UIResult
      ,CASE WHEN [Card] = 1 THEN 'Banker' ELSE 'Player' END RealCard
	  , AutoSessionID
	  
	  , Profit
	  ,SUM(Profit) OVER (ORDER BY AutoSessionID, AutoZiczacF.ID ASC) AS Acc_Profit
	  ,SUM(CASE WHEN [Card] = 1 AND Profit >0 THEN 0.95 * Profit ELSE 1.00 * Profit END) OVER (ORDER BY AutoSessionID, AutoZiczacF.ID ASC) AS Acc_RealProfit
	  
	  ,ProfitMod	  
	  ,SUM(ProfitMod) OVER (ORDER BY AutoSessionID, AutoZiczacF.ID ASC) AS Acc_ProfitMod
	  ,SUM(CASE WHEN [Card] = 1 AND ProfitMod >0 THEN 0.95 * ProfitMod ELSE 1.00 * ProfitMod END) OVER (ORDER BY AutoSessionID, AutoZiczacF.ID ASC) AS Acc_RealProfitMod

	  ,Profit_CutLoss
	  ,SUM(Profit_CutLoss) OVER (ORDER BY AutoSessionID, AutoZiczacF.ID ASC) AS Acc_Profit_CutLoss
	  ,SUM(CASE WHEN [Card] = 1 AND Profit_CutLoss >0 THEN 0.95 * Profit_CutLoss ELSE 1.00 * Profit_CutLoss END) OVER (ORDER BY AutoSessionID, AutoZiczacF.ID ASC) AS Acc_RealProfit_CutLoss

	  ,ProfitMod_CutLoss
	  ,SUM(ProfitMod_CutLoss) OVER (ORDER BY AutoSessionID, AutoZiczacF.ID ASC) AS Acc_Profit_CutLoss
	  ,SUM(CASE WHEN [Card] = 1 AND ProfitMod_CutLoss >0 THEN 0.95 * ProfitMod_CutLoss ELSE 1.00 * ProfitMod_CutLoss END) OVER (ORDER BY AutoSessionID, AutoZiczacF.ID ASC) AS Acc_RealProfitMod_CutLoss

FROM AutoZiczacF  INNER JOIN 
AutoSessions ON AutoZiczacF.AutoSessionID = AutoSessions.ID
WHERE TableNumber = 1 AND AutoZiczacF.AutoSessionID BETWEEN 3000 AND 5000
ORDER BY AutoSessionID, AutoZiczacF.ID


--CREATE INDEX IDX_AutoZiczacF_Card
--ON AutoZiczacF (Card)

--CREATE INDEX IDX_AutoZiczacF_AutoSessionID
--ON AutoZiczacF (AutoSessionID)


