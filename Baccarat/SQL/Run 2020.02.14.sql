ALTER TABLE Roots ADD [Flat095Main] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Flat095Profit0] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Flat095Profit1] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Flat095Profit2] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Flat095Profit3] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Flat095AllSub] DECIMAL(10,2) NOT NULL DEFAULT (0)

ALTER TABLE Roots ADD [ModMainCoeff] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModCoeff0] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModCoeff1] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModCoeff2] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModCoeff3] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModAllSubCoeff] INT NOT NULL DEFAULT (0)


ALTER TABLE Roots ADD [ModMainProfit] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModProfit0] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModProfit1] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModProfit2] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModProfit3] INT NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [ModAllSubProfit] INT NOT NULL DEFAULT (0)

ALTER TABLE Roots ADD [Mod095Main] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Mod095Profit0] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Mod095Profit1] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Mod095Profit2] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Mod095Profit3] DECIMAL(10,2) NOT NULL DEFAULT (0)
ALTER TABLE Roots ADD [Mod095AllSub] DECIMAL(10,2) NOT NULL DEFAULT (0)

GO

------------------------------------------------------------------------------------------------------------------

UPDATE Roots SET Flat095Main = MainProfit * 0.95 WHERE Card = 1 AND MainProfit > 0
UPDATE Roots SET Flat095Profit0 = Profit0 * 0.95 WHERE Card = 1 AND Profit0 > 0
UPDATE Roots SET Flat095Profit1 = Profit1 * 0.95 WHERE Card = 1 AND Profit1 > 0
UPDATE Roots SET Flat095Profit2 = Profit2 * 0.95 WHERE Card = 1 AND Profit2 > 0
UPDATE Roots SET Flat095Profit3 = Profit3 * 0.95 WHERE Card = 1 AND Profit3 > 0
UPDATE Roots SET Flat095AllSub = AllSubProfit * 0.95 WHERE Card = 1 AND AllSubProfit > 0

UPDATE Roots SET Flat095Main = MainProfit  WHERE NOT( Card = 1 AND MainProfit > 0)
UPDATE Roots SET Flat095Profit0 = Profit0  WHERE NOT(Card = 1 AND Profit0 > 0)
UPDATE Roots SET Flat095Profit1 = Profit1  WHERE NOT(Card = 1 AND Profit1 > 0)
UPDATE Roots SET Flat095Profit2 = Profit2  WHERE NOT(Card = 1 AND Profit2 > 0)
UPDATE Roots SET Flat095Profit3 = Profit3  WHERE NOT(Card = 1 AND Profit3 > 0)
UPDATE Roots SET Flat095AllSub = AllSubProfit WHERE NOT(Card = 1 AND AllSubProfit > 0)

UPDATE Roots SET 
	[ModMainCoeff] = 11,
	[ModCoeff0] = 11,
	[ModCoeff1] = 11,
	[ModCoeff2] = 11,
	[ModCoeff3] = 11,
	[ModAllSubCoeff] = 11

	GO

----------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[RootSession](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[NoOfSteps] [int] NOT NULL,
	[MaxMainProfit] [int] NOT NULL,
	[MinMainProfit] [int] NOT NULL,
	[MaxProfit0] [int] NOT NULL,
	[MinProfit0] [int] NOT NULL,
	[MaxProfit1] [int] NOT NULL,
	[MinProfit1] [int] NOT NULL,
	[MaxProfit2] [int] NOT NULL,
	[MinProfit2] [int] NOT NULL,
	[MaxProfit3] [int] NOT NULL,
	[MinProfit3] [int] NOT NULL,
	[MaxAllSub] [int] NOT NULL,
	[MinAllSub] [int] NOT NULL,
	[LastUpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_RootSession] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-------------------------------------------------------------------