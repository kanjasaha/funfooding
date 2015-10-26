USE [ThreeSixtyTwo]
GO

/****** Object:  Table [dbo].[Connections]    Script Date: 10/26/2015 12:37:35 AM ******/
DROP TABLE [dbo].[Connections]
GO

/****** Object:  Table [dbo].[Connections]    Script Date: 10/26/2015 12:37:35 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Connections](
	[ConnectionID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ContactID] [int] NOT NULL,
	[ContactEmail] [varchar](100) NOT NULL,
	[ContactStrength] [int] NOT NULL,
	[DegreeOfSeparation] [int] NOT NULL,
	[SharesFood] [int] NOT NULL,
	[BoughtFoodFromUser] [int] NOT NULL,
	[SoldFoodToUser] [int] NOT NULL,
 CONSTRAINT [PK_Connections] PRIMARY KEY CLUSTERED 
(
	[ConnectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


