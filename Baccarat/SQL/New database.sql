/****** Object:  Table [dbo].[AutoResults]    Script Date: 3/9/2022 10:08:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoResults](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Card] [smallint] NOT NULL,
	[InputDateTime] [datetime] NOT NULL,
	[AutoSessionID] [int] NOT NULL,
	[UIResult] [nvarchar](200) NULL,
 CONSTRAINT [PK_dbo.StandardResults] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AutoRoots]    Script Date: 3/9/2022 10:08:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoRoots](
	[ID] [int] NOT NULL,
	[Card] [smallint] NOT NULL,
	[InputDateTime] [datetime] NOT NULL,
	[GlobalIndex] [int] NOT NULL,
	[AutoSessionID] [int] NOT NULL,
	[MainProfit] [int] NOT NULL,
	[Profit0] [int] NOT NULL,
	[Profit1] [int] NOT NULL,
	[Profit2] [int] NOT NULL,
	[Profit3] [int] NOT NULL,
	[AllSubProfit] [int] NOT NULL,
	[ModMainProfit] [int] NULL,
	[ModProfit0] [int] NULL,
	[ModProfit1] [int] NULL,
	[ModProfit2] [int] NULL,
	[ModProfit3] [int] NULL,
	[ModAllSubProfit] [int] NULL,
	[ListCurrentPredicts] [nvarchar](500) NOT NULL,
	[ListCurrentModCoeffs] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_dbo.StandardRoots] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AutoSessions]    Script Date: 3/9/2022 10:08:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoSessions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[NoOfSteps] [int] NOT NULL,
	[NoOfStepsRoot] [int] NOT NULL,
	[NoOfStepsQuad] [int] NOT NULL,
	[TableNumber] [int] NOT NULL,
	[IsClosed] [bit] NOT NULL,
	[MaxQuad] [int] NOT NULL,
	[MinQuad] [int] NOT NULL,
	[Profit14] [int] NOT NULL,
	[Profit25] [int] NOT NULL,
	[Profit36] [int] NOT NULL,
	[Profit47] [int] NOT NULL,
	[Profit58] [int] NOT NULL,
	[Profit61] [int] NOT NULL,
	[Profit72] [int] NOT NULL,
	[Profit83] [int] NOT NULL,
	[MaxRoot] [int] NOT NULL,
	[MinRoot] [int] NOT NULL,
	[RootMainProfit] [int] NOT NULL,
	[RootProfit0] [int] NOT NULL,
	[RootProfit1] [int] NOT NULL,
	[RootProfit2] [int] NOT NULL,
	[RootProfit3] [int] NOT NULL,
	[RootAllSub] [int] NOT NULL,
 CONSTRAINT [PK_dbo.StandardSessions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Results]    Script Date: 3/9/2022 10:08:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Results](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Card] [smallint] NOT NULL,
	[InputDateTime] [datetime] NOT NULL,
	[SessionID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Results] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roots]    Script Date: 3/9/2022 10:08:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roots](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Card] [smallint] NOT NULL,
	[InputDateTime] [datetime] NOT NULL,
	[MainCoeff] [int] NOT NULL,
	[Coeff0] [int] NOT NULL,
	[Coeff1] [int] NOT NULL,
	[Coeff2] [int] NOT NULL,
	[Coeff3] [int] NOT NULL,
	[AllSubCoeff] [int] NOT NULL,
	[MainProfit] [int] NOT NULL,
	[Profit0] [int] NOT NULL,
	[Profit1] [int] NOT NULL,
	[Profit2] [int] NOT NULL,
	[Profit3] [int] NOT NULL,
	[AllSubProfit] [int] NOT NULL,
	[ListCurrentPredicts] [nvarchar](500) NOT NULL,
	[GlobalOrder] [int] NOT NULL,
	[Flat095Main] [decimal](10, 2) NOT NULL,
	[Flat095Profit0] [decimal](10, 2) NOT NULL,
	[Flat095Profit1] [decimal](10, 2) NOT NULL,
	[Flat095Profit2] [decimal](10, 2) NOT NULL,
	[Flat095Profit3] [decimal](10, 2) NOT NULL,
	[Flat095AllSub] [decimal](10, 2) NOT NULL,
	[ModMainCoeff] [int] NOT NULL,
	[ModCoeff0] [int] NOT NULL,
	[ModCoeff1] [int] NOT NULL,
	[ModCoeff2] [int] NOT NULL,
	[ModCoeff3] [int] NOT NULL,
	[ModAllSubCoeff] [int] NOT NULL,
	[ModMainProfit] [int] NOT NULL,
	[ModProfit0] [int] NOT NULL,
	[ModProfit1] [int] NOT NULL,
	[ModProfit2] [int] NOT NULL,
	[ModProfit3] [int] NOT NULL,
	[ModAllSubProfit] [int] NOT NULL,
	[Mod095Main] [decimal](10, 2) NOT NULL,
	[Mod095Profit0] [decimal](10, 2) NOT NULL,
	[Mod095Profit1] [decimal](10, 2) NOT NULL,
	[Mod095Profit2] [decimal](10, 2) NOT NULL,
	[Mod095Profit3] [decimal](10, 2) NOT NULL,
	[Mod095AllSub] [decimal](10, 2) NOT NULL,
	[RootSessionID] [int] NULL,
 CONSTRAINT [PK_dbo.Roots] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sessions]    Script Date: 3/9/2022 10:08:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[NoOfSteps] [int] NOT NULL,
	[Profit14] [int] NOT NULL,
	[Profit25] [int] NOT NULL,
	[Profit36] [int] NOT NULL,
	[Profit47] [int] NOT NULL,
	[Profit58] [int] NOT NULL,
	[Profit61] [int] NOT NULL,
	[Profit72] [int] NOT NULL,
	[Profit83] [int] NOT NULL,
	[ImportFileName] [nvarchar](200) NULL,
 CONSTRAINT [PK_dbo.Sessions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Flat095Main]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Flat095Profit0]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Flat095Profit1]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Flat095Profit2]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Flat095Profit3]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Flat095AllSub]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModMainCoeff]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModCoeff0]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModCoeff1]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModCoeff2]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModCoeff3]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModAllSubCoeff]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModMainProfit]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModProfit0]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModProfit1]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModProfit2]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModProfit3]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [ModAllSubProfit]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Mod095Main]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Mod095Profit0]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Mod095Profit1]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Mod095Profit2]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Mod095Profit3]
GO
ALTER TABLE [dbo].[Roots] ADD  DEFAULT ((0)) FOR [Mod095AllSub]
GO
ALTER TABLE [dbo].[Results]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Results_dbo.Sessions_SessionID] FOREIGN KEY([SessionID])
REFERENCES [dbo].[Sessions] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Results] CHECK CONSTRAINT [FK_dbo.Results_dbo.Sessions_SessionID]
GO
/****** Object:  StoredProcedure [dbo].[CommonQuery]    Script Date: 3/9/2022 10:08:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CommonQuery]
	@TABLES NVARCHAR(100), 
	@BEGIN DateTime, 
	@END DateTime
AS
BEGIN

	declare @Run iNT = 0
	declare @today datetime2 =  CAST (getdate() as date)
	declare @yesterday datetime2 =  CAST (dateadd(day, -1, getdate()) as date)
	declare @rightnow datetime2 = getdate()

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

	SELECT [ID]
		  ,CASE WHEN [Card] = 1 THEN 'Banker' ELSE 'Player' END Card
		  ,[InputDateTime]      
		  ,[AutoSessionID] AS ShoeID
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
					StartDateTime between '2022-03-28' and @rightnow					
					AND TableNumber IN (select VALUE FROM string_split(@TABLES, ','))
					AND IsClosed = 1
				) 
				ORDER BY [AutoSessionID],ID ASC

END
GO

CREATE INDEX IDX_AutoResults_AutoSessionID
ON AutoResults (AutoSessionID)

CREATE INDEX IDX_AutoRoots_AutoSessionID
ON AutoRoots (AutoSessionID)

CREATE INDEX IDX_AutoSessions_TableNumber
ON AutoSessions (TableNumber)

CREATE INDEX IDX_AutoSessions_IsClosed
ON AutoSessions (IsClosed)