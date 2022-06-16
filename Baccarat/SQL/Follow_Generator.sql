DECLARE @ShoeID INT 

DECLARE shoe_cursor CURSOR FOR SELECT ID FROM AutoSessions 
WHERE IsClosed = 1 AND 
ID BETWEEN 0 AND 5000
ORDER BY ID

OPEN shoe_cursor;

FETCH NEXT FROM shoe_cursor INTO @ShoeID

WHILE @@FETCH_STATUS = 0
    BEGIN

			TRUNCATE TABLE tmp

			INSERT INTO tmp
			SELECT ID, ROW_NUMBER() OVER(ORDER BY ID ASC) AS Step, [Card], UIResult, AutoSessionID
			FROM AutoResults WHERE AutoSessionID = @ShoeID AND [Card] IN (-1,1)

			DECLARE @ID INT, @Step INT, @Card INT, @UIResult NVARCHAR(200), @AutoSessionID INT
			--DECLARE @Card3 INT = NULL
			DECLARE @Card2 INT = NULL
			DECLARE @Card1 INT = NULL

			DECLARE @Predict INT = 0
			DECLARE @Profit INT = 0
			DECLARE @ProfitMod INT = 0
			DECLARE @Coeff INT = 1
			DECLARE @Profit_CutLoss INT = 0
			DECLARE @ProfitMod_CutLoss INT = 0

			DECLARE @Acc_Profit INT = 0
			DECLARE @Acc_ProfitMod INT = 0
			DECLARE @CusLoss INT = -4

			DECLARE cursor_product CURSOR FOR SELECT * FROM tmp

			OPEN cursor_product;

			FETCH NEXT FROM cursor_product INTO @ID, @Step, @Card, @UIResult, @AutoSessionID

			WHILE @@FETCH_STATUS = 0
				BEGIN	
        
					SET @Predict= 0
					SET @Profit = 0
					SET @ProfitMod = 0
					SET @Profit_CutLoss = 0
					SET @ProfitMod_CutLoss = 0

					IF --(@Card3 IS NOT NULL) AND 
						(@Card2 IS NOT NULL) AND (@Card1 IS NOT NULL)					 			
						--AND (@Card3 != @Card2) 
						AND (@Card2 = @Card1) 
						BEGIN

							SET @Predict = @Card1
							IF (@Predict = @Card)
							BEGIN 
								SET @Profit = 1
								SET @ProfitMod = @Coeff 

								IF (@Acc_Profit > @CusLoss)
								BEGIN 
									SET @Acc_Profit += 1
									SET @Acc_ProfitMod += @Coeff
									SET @Profit_CutLoss = 1
									SET @ProfitMod_CutLoss = @Coeff
								END
								
								IF (@Coeff > 1)
									SET @Coeff = @Coeff - 1
							END
							ELSE 
							BEGIN
								SET @Profit = -1
								SET @ProfitMod = -@Coeff 

								IF (@Acc_Profit > @CusLoss)
								BEGIN 
									SET @Acc_Profit -= 1
									SET @Acc_ProfitMod -= @Coeff

									SET @Profit_CutLoss = -1
									SET @ProfitMod_CutLoss = -@Coeff
								END

								SET @Coeff = @Coeff + 1
							END							 
						END
					

					IF NOT EXISTS (SELECT * FROM AutoFollow2In WHERE ID = @ID)		 
						INSERT INTO AutoFollow2In(ID, Card, Predict, Profit, ProfitMod, Coeff, UIResult, AutoSessionID, Profit_CutLoss, ProfitMod_CutLoss) 
						VALUES
						(@ID, @Card, @Predict, @Profit, @ProfitMod, @Coeff, @UIResult, @AutoSessionID, @Profit_CutLoss, @ProfitMod_CutLoss )

					--SET @Card3  = @Card2
					SET @Card2  = @Card1
					SET @Card1  = @Card		

					FETCH NEXT FROM cursor_product INTO @ID, @Step, @Card, @UIResult, @AutoSessionID
				END;

			CLOSE cursor_product;
			DEALLOCATE cursor_product;

			FETCH NEXT FROM shoe_cursor INTO @ShoeID
	END
	CLOSE shoe_cursor;
	DEALLOCATE shoe_cursor;
