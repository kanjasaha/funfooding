USE [master]
GO

ALTER DATABASE [ThreeSixtyTwo] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ThreeSixtyTwo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ThreeSixtyTwo] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET ANSI_NULLS OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET ANSI_PADDING OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET ARITHABORT OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET AUTO_CLOSE ON
GO
ALTER DATABASE [ThreeSixtyTwo] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [ThreeSixtyTwo] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [ThreeSixtyTwo] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [ThreeSixtyTwo] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET  DISABLE_BROKER
GO
ALTER DATABASE [ThreeSixtyTwo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [ThreeSixtyTwo] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [ThreeSixtyTwo] SET  READ_WRITE
GO
ALTER DATABASE [ThreeSixtyTwo] SET RECOVERY FULL
GO
ALTER DATABASE [ThreeSixtyTwo] SET  MULTI_USER
GO
ALTER DATABASE [ThreeSixtyTwo] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [ThreeSixtyTwo] SET DB_CHAINING OFF
GO
USE [ThreeSixtyTwo]
GO
CREATE LOGIN [rtks71] WITH PASSWORD=N'Test1234', DEFAULT_DATABASE=[ThreeSixtyTwo], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=ON, CHECK_POLICY=ON
GO

CREATE LOGIN [solradmin] WITH PASSWORD=N'Test1234', DEFAULT_DATABASE=[ThreeSixtyTwo], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=ON, CHECK_POLICY=ON
GO




/****** Object:  Schema [search]    Script Date: 03/02/2015 07:05:52 ******/
CREATE SCHEMA [search] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [ecom]    Script Date: 03/02/2015 07:05:52 ******/
CREATE SCHEMA [ecom] AUTHORIZATION [dbo]
GO
/****** Object:  UserDefinedDataType [dbo].[description]    Script Date: 03/02/2015 07:05:52 ******/
CREATE TYPE [dbo].[description] FROM [varchar](max) NOT NULL
GO
/****** Object:  Table [ecom].[GoogleCheckOut]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ecom].[GoogleCheckOut](
	[UserID] [int] NOT NULL,
	[MerchantID] [int] NOT NULL,
 CONSTRAINT [PK_GoogleCheckOut] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ecom].[AmazonPay]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ecom].[AmazonPay](
	[UserID] [int] NOT NULL,
	[MerchantID] [int] NOT NULL,
 CONSTRAINT [PK_AmazonPay] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cart](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[CartID] [varchar](50) NOT NULL,
	[MealAdID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [ecom].[PayPal]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ecom].[PayPal](
	[UserID] [int] NOT NULL,
	[MerchantID] [int] NOT NULL,
 CONSTRAINT [PK_PayPal] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[AllEmails]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AllEmails](
	[EmailID] [int] IDENTITY(1,1) NOT NULL,
	[ToAddress] [varchar](400) NOT NULL,
	[FromAddress] [varchar](400) NOT NULL,
	[CCAddress] [varchar](400) NULL,
	[BCCAddress] [varchar](400) NULL,
	[EmailSubject] [varchar](100) NOT NULL,
	[EmailBodyText] [varchar](max) NOT NULL,
	[SendDate] [datetime] NOT NULL,
	[EmailStatus] [varchar](10) NULL,
	[Comment] [varchar](300) NULL,
 CONSTRAINT [PK_EmailInvitation] PRIMARY KEY CLUSTERED 
(
	[EmailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailSubscriptions]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailSubscriptions](
	[EmailSubscriptionID] [int] IDENTITY(1,1) NOT NULL,
	[Sender] [varchar](100) NOT NULL,
	[Recipient] [varchar](100) NOT NULL,
	[ActivityTypeID] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[country]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[country](
	[country_id] [int] IDENTITY(1,1) NOT NULL,
	[country_name] [varchar](50) NOT NULL,
	[country_abbrev] [varchar](3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[country_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContactList](
	[ContactListID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[SenderEmailAddress] [varchar](50) NOT NULL,
	[RecipientUserID] [int] NULL,
	[RecipientEmailAddress] [varchar](50) NOT NULL,
	[RequestAccepted] [int] NULL,
 CONSTRAINT [PK_ContactList] PRIMARY KEY CLUSTERED 
(
	[ContactListID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Connections]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Connections](
	[UserId] [int] NOT NULL,
	[ContactID] [int] NULL,
	[ContactEmail] [varchar](100) NOT NULL,
	[ContactStrength] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AvailabilityTypes]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AvailabilityTypes](
	[AvaiilabilityTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityType] [varchar](56) NOT NULL,
	[Descriptions] [varchar](256) NOT NULL,
 CONSTRAINT [PK_AvailabilityTypes] PRIMARY KEY CLUSTERED 
(
	[AvaiilabilityTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[FunOrders]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FunOrders](
	[UserId] [int] NOT NULL,
	[EstimatedPickupTime] [datetime] NOT NULL,
	[ActualPickUpTime] [datetime] NULL,
	[FeedbackOnTime] [int] NULL,
	[GeneralFeedback] [varchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
	[Status] [int] NOT NULL,
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[PaymentOptionID] [int] NOT NULL,
	[DeliveryMethodID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LKUPAssociationType]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LKUPAssociationType](
	[AssociationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AssociationType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AssociationTypeLKUP] PRIMARY KEY CLUSTERED 
(
	[AssociationTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LKUPAllergenicFood]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPAllergenicFood](
	[AllergenicFoodID] [int] IDENTITY(1,1) NOT NULL,
	[AllergenicFood] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LKUPAllergenicFood] PRIMARY KEY CLUSTERED 
(
	[AllergenicFoodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[LKUPActivityType]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPActivityType](
	[ActivityTypeLKUPID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LKUPActivityType] PRIMARY KEY CLUSTERED 
(
	[ActivityTypeLKUPID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ip2location_db3_ipv6]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ip2location_db3_ipv6](
	[ip_from] [char](39) NOT NULL,
	[ip_to] [char](39) NOT NULL,
	[country_code] [nvarchar](2) NOT NULL,
	[country_name] [nvarchar](64) NOT NULL,
	[region_name] [nvarchar](128) NOT NULL,
	[city_name] [nvarchar](128) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPDeliveryMethod](
	[DeliveryMethodID] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryMethod] [varchar](50) NOT NULL,
	[Description] [varchar](100) NOT NULL,
 CONSTRAINT [PK_LKUPDeliveryMethod] PRIMARY KEY CLUSTERED 
(
	[DeliveryMethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[NotificationFrequency]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NotificationFrequency](
	[NotificationFrequencyID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](10) NOT NULL,
 CONSTRAINT [PK_NotificationFrequency] PRIMARY KEY CLUSTERED 
(
	[NotificationFrequencyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  UserDefinedDataType [dbo].[name]    Script Date: 03/02/2015 07:05:53 ******/
CREATE TYPE [dbo].[name] FROM [varchar](100) NOT NULL
GO
/****** Object:  Table [dbo].[LocationsSearched]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LocationsSearched](
	[SearchID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Location] [varchar](30) NOT NULL,
	[Latitude] [decimal](9, 6) NOT NULL,
	[Longitude] [decimal](9, 6) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Keywords] [varchar](50) NULL,
	[DateRangeStart] [datetime] NULL,
	[DateRangeEnd] [datetime] NULL,
	[Distance] [int] NULL,
	[DistanceUnit] [char](2) NULL,
 CONSTRAINT [PK_LocationsSearched] PRIMARY KEY CLUSTERED 
(
	[SearchID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LKUPServingUnit]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPServingUnit](
	[ServingUnitID] [int] IDENTITY(1,1) NOT NULL,
	[ServingUnit] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LKUPServingUnit] PRIMARY KEY CLUSTERED 
(
	[ServingUnitID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LKUPPrivacySettings]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPPrivacySettings](
	[PrivacySettingsID] [int] NOT NULL,
	[PrivacySettings] [varchar](50) NOT NULL,
	[Description] [varchar](200) NULL,
 CONSTRAINT [PK_LKUPPrivacySettings] PRIMARY KEY CLUSTERED 
(
	[PrivacySettingsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[UserProfile]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
	[FirstName] [varchar](56) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[UserPaymentOptions]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserPaymentOptions](
	[PaymentOptionID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PaymentType] [varchar](50) NOT NULL,
	[PaymentID] [int] NOT NULL,
 CONSTRAINT [PK_UserPO] PRIMARY KEY CLUSTERED 
(
	[PaymentOptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 03/02/2015 07:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--uspCreatePassword 5,5,5,5


CREATE PROCEDURE [dbo].[uspCreatePassword]
(
	@UpperCaseItems SMALLINT,
	@LowerCaseItems SMALLINT,
	@NumberItems SMALLINT,
	@SpecialItems SMALLINT
)
AS

SET NOCOUNT ON

-- Initialize some variables
DECLARE	@UpperCase VARCHAR(26),
	@LowerCase VARCHAR(26),
	@Numbers VARCHAR(10),
	@Special VARCHAR(13),
	@Temp VARCHAR(8000),
	@Password VARCHAR(8000),
	@i SMALLINT,
	@c VARCHAR(1),
	@v TINYINT 

-- Set the default items in each group of characters
SELECT	@UpperCase = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ',
	@LowerCase = 'abcdefghijklmnopqrstuvwxyz',
	@Numbers = '0123456789',
	@Special = '!@#$%&*()_+-=',
	@Temp = '',
	@Password = '' 

-- Enforce some limits on the length of the password
IF @UpperCaseItems > 20
	SET @UpperCaseItems = 20

IF @UpperCaseItems < -20
	SET @UpperCaseItems = -20

IF @LowerCaseItems > 20
	SET @LowerCaseItems = 20

IF @LowerCaseItems < -20
	SET @LowerCaseItems = -20

IF @NumberItems > 20
	SET @NumberItems = 20

IF @NumberItems < -20
	SET @NumberItems = -20

IF @SpecialItems > 20
	SET @SpecialItems = 20

IF @SpecialItems < -20
	SET @SpecialItems = -20

-- Get the Upper Case Items
SET	@i = ABS(@UpperCaseItems) 

WHILE @i > 0 AND LEN(@UpperCase) > 0
	SELECT	@v = ABS(CAST(CAST(NEWID() AS BINARY(16)) AS BIGINT)) % LEN(@UpperCase) + 1,
		@c = SUBSTRING(@UpperCase, @v, 1),
		@UpperCase =	CASE
					WHEN @UpperCaseItems < 0 THEN STUFF(@UpperCase, @v, 1, '') 
					ELSE @UpperCase 
				END,
		@Temp = @Temp + @c,
		@i = @i - 1

-- Get the Lower Case Items
SET	@i = ABS(@LowerCaseItems) 

WHILE @i > 0 AND LEN(@LowerCase) > 0    
	SELECT	@v = ABS(CAST(CAST(NEWID() AS BINARY(16)) AS BIGINT)) % LEN(@LowerCase) + 1,
		@c = SUBSTRING(@LowerCase, @v, 1),
		@LowerCase =	CASE 
					WHEN @LowerCaseItems < 0 THEN STUFF(@LowerCase, @v, 1, '') 
					ELSE @LowerCase 
				END,
		@Temp = @Temp + @c,
		@i = @i - 1 

-- Get the Number Items
SET	@i = ABS(@NumberItems) 

WHILE @i > 0 AND LEN(@Numbers) > 0    
	SELECT	@v = ABS(CAST(CAST(NEWID() AS BINARY(16)) AS BIGINT)) % LEN(@Numbers) + 1,
		@c = SUBSTRING(@Numbers, @v, 1),
		@Numbers =	CASE 
					WHEN @NumberItems < 0 THEN STUFF(@Numbers, @v, 1, '') 
					ELSE @Numbers 
				END,
		@Temp = @Temp + @c,
		@i = @i - 1 

-- Get the Special Items
SET	@i = ABS(@SpecialItems) 

WHILE @i > 0 AND LEN(@Special) > 0    
	SELECT	@v = ABS(CAST(CAST(NEWID() AS BINARY(16)) AS BIGINT)) % LEN(@Special) + 1,
		@c = SUBSTRING(@Special, @v, 1),
		@Special =	CASE 
					WHEN @SpecialItems < 0 THEN STUFF(@Special, @v, 1, '')
					ELSE @Special 
				END,
		@Temp = @Temp + @c,
		@i = @i - 1 

-- Scramble the order of the selected items
WHILE LEN(@Temp) > 0    
	SELECT	@v = ABS(CAST(CAST(NEWID() AS BINARY(16)) AS BIGINT)) % LEN(@Temp) + 1,
		@Password = @Password + SUBSTRING(@Temp, @v, 1),
		@Temp = STUFF(@Temp, @v, 1, '') 

SELECT	@Password
GO
/****** Object:  Table [dbo].[UserAgreementsAcceptanceDetails]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAgreementsAcceptanceDetails](
	[UserAgreementID] [int] IDENTITY(1,1) NOT NULL,
	[AgreementID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[AgreementAccepetedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserAgreementsAcceptanceDetails] PRIMARY KEY CLUSTERED 
(
	[UserAgreementID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UserAgreements]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAgreements](
	[AgreementID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[AgreementDetails] [varchar](max) NOT NULL,
	[DateOfAgreement] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[TempUserOrder]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TempUserOrder](
	[userid] [int] IDENTITY(1,1) NOT NULL,
	[sessionId] [varchar](50) NOT NULL,
	[MealItemId] [varchar](max) NOT NULL,
	[itemName] [varchar](max) NULL,
	[lineitemcost] [money] NOT NULL,
	[qty] [int] NOT NULL,
	[TotalCost] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TempOrderList]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TempOrderList](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NOT NULL,
	[sessionId] [varchar](50) NOT NULL,
	[MealItemId] [varchar](max) NOT NULL,
	[itemName] [varchar](max) NULL,
	[lineitemcost] [money] NOT NULL,
	[qty] [int] NOT NULL,
	[TotalCost] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[state]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[state](
	[state_id] [int] IDENTITY(1,1) NOT NULL,
	[state_name] [varchar](50) NOT NULL,
	[state_abbrev] [varchar](2) NOT NULL,
	[countryid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[state_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SendText]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SendText](
	[SendTextID] [int] IDENTITY(1,1) NOT NULL,
	[SenderPhone] [varchar](100) NOT NULL,
	[RecipientPhone] [varchar](100) NOT NULL,
	[Message] [varchar](50) NOT NULL,
	[DeliveryTime] [datetime] NOT NULL,
	[DeliveryMethod] [varchar](50) NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[SendDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SendEmail]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SendEmail](
	[SendEmailID] [int] IDENTITY(1,1) NOT NULL,
	[SenderEmailAddress] [varchar](100) NOT NULL,
	[RecipientEmailAddress] [varchar](100) NOT NULL,
	[Subject] [varchar](50) NOT NULL,
	[Body] [varchar](1000) NOT NULL,
	[DeliveryTime] [datetime] NOT NULL,
	[DeliveryMethod] [varchar](50) NOT NULL,
	[Status] [varchar](50) NULL,
	[SendDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SellerDefaultSetting]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SellerDefaultSetting](
	[SellerSettingID] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NOT NULL,
	[OrderFulfillmentTime] [int] NULL,
	[UnitOfTime] [int] NULL,
	[Delivery] [int] NULL,
	[Shipping] [int] NULL,
	[PickUp] [int] NULL,
	[AcceptCard] [int] NULL,
	[AcceptCashOnDelivery] [int] NULL,
	[HasPermit] [int] NULL,
	[Permit] [int] NULL,
	[PermitImage] [varchar](50) NULL,
 CONSTRAINT [PK_SellerDefaultSetting] PRIMARY KEY CLUSTERED 
(
	[SellerSettingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PaymentOptions]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PaymentOptions](
	[PaymentOptionID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentOption] [varchar](100) NOT NULL,
 CONSTRAINT [PK_PaymentOptions] PRIMARY KEY CLUSTERED 
(
	[PaymentOptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Orders]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Orders](
	[UserId] [int] NOT NULL,
	[EstimatedPickupTime] [datetime] NOT NULL,
	[ActualPickUpTime] [datetime] NULL,
	[FeedbackOnTime] [int] NULL,
	[GeneralFeedback] [varchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
	[Status] [int] NOT NULL,
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[PaymentOptionID] [int] NOT NULL,
	[DeliveryMethodID] [int] NOT NULL,
 CONSTRAINT [PK_MealOrders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSettings]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSettings](
	[UserSettingsID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ActivityTypeID] [int] NOT NULL,
	[PrivacySettingsID] [int] NOT NULL,
	[ReceiveEmailNotification] [int] NOT NULL,
	[ReceiveMobileTextNotification] [int] NOT NULL,
	[NotificationFrequencyID] [int] NOT NULL,
	[Verified] [int] NULL,
	[VerificationDate] [datetime] NULL,
 CONSTRAINT [PK_UserSettings] PRIMARY KEY CLUSTERED 
(
	[UserSettingsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LKUPMealType]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPMealType](
	[MealTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [dbo].[name] NULL,
	[Description] [varchar](100) NULL,
	[DateCreated] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedById] [int] NULL,
	[UpdatedById] [int] NULL,
 CONSTRAINT [PK_MealTypeLKUP] PRIMARY KEY CLUSTERED 
(
	[MealTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[LKUPKitchenType]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPKitchenType](
	[KitchenTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [dbo].[name] NULL,
	[Description] [varchar](100) NULL,
	[DateCreated] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedById] [int] NULL,
	[UpdatedById] [int] NULL,
 CONSTRAINT [PK_KitchenTypeLKUP] PRIMARY KEY CLUSTERED 
(
	[KitchenTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[LKUPDietType]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPDietType](
	[DietTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [dbo].[name] NULL,
	[Description] [varchar](100) NULL,
	[DateCreated] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedById] [int] NULL,
	[UpdatedById] [int] NULL,
 CONSTRAINT [PK_DietTypeLKUP] PRIMARY KEY CLUSTERED 
(
	[DietTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LKUPCuisineType]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPCuisineType](
	[CuisineTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [dbo].[name] NULL,
	[Description] [varchar](100) NULL,
	[DateCreated] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedById] [int] NULL,
	[UpdatedById] [int] NULL,
 CONSTRAINT [PK_CuisineTypeLKUP] PRIMARY KEY CLUSTERED 
(
	[CuisineTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[LKUPCountry]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LKUPCountry](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[Country] [dbo].[name] NOT NULL,
	[Currency] [varchar](100) NOT NULL,
	[CurrencyAbbreviation] [varchar](100) NOT NULL,
	[IdentificationType] [varchar](100) NOT NULL,
	[CountryAbbr] [nvarchar](50) NULL,
 CONSTRAINT [PK_CountryLKUP] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FunOrderDetails]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FunOrderDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[FeedbackOnQuality] [int] NULL,
	[FeedbackOnPrice] [int] NULL,
	[OrderID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [search].[ActiveMealAds]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [search].[ActiveMealAds](
	[MealAdID] [int] NOT NULL,
	[MealItemName] [varchar](100) NOT NULL,
	[PickUpTime] [datetime] NOT NULL,
	[MaxOrders] [int] NOT NULL,
	[PlacedOrder] [int] NOT NULL,
	[LastOrderTime] [datetime] NOT NULL,
	[CuisineType] [dbo].[name] NOT NULL,
	[MealType] [dbo].[name] NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[Ingredients] [varchar](1000) NOT NULL,
	[Photo] [varchar](200) NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Currency] [dbo].[name] NOT NULL,
	[FirstName] [dbo].[name] NOT NULL,
	[LastName] [dbo].[name] NOT NULL,
	[Address1] [varchar](200) NOT NULL,
	[Address2] [varchar](200) NULL,
	[City] [varchar](200) NOT NULL,
	[Province] [varchar](200) NOT NULL,
	[Zip] [varchar](200) NOT NULL,
	[Country] [varchar](200) NOT NULL,
	[Telephone] [varchar](200) NOT NULL,
	[Email] [dbo].[name] NOT NULL,
	[FoodAllergy] [varchar](50) NULL,
	[FoodType] [varchar](50) NOT NULL,
	[Latitude] [decimal](9, 6) NOT NULL,
	[Longitude] [decimal](9, 6) NOT NULL,
	[latlng] [varchar](200) NOT NULL,
	[PriceRange] [varchar](200) NOT NULL,
	[AllInfo] [varchar](max) NOT NULL,
	[ProviderType] [varchar](30) NULL,
	[ProviderName] [varchar](30) NULL,
	[FullAddress] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AddressList]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AddressList](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Address1] [varchar](200) NOT NULL,
	[Address2] [varchar](200) NULL,
	[City] [varchar](200) NOT NULL,
	[Province] [varchar](200) NOT NULL,
	[Zip] [varchar](200) NOT NULL,
	[Telephone] [varchar](200) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
	[CountryID] [int] NOT NULL,
	[IsBillingAddress] [int] NOT NULL,
	[Latitude] [decimal](9, 6) NULL,
	[Longitude] [decimal](9, 6) NULL,
	[latlng] [varchar](200) NULL,
	[IsCurrent] [int] NOT NULL,
 CONSTRAINT [PK_AddressList] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MealItems]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MealItems](
	[MealItemId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[MealItemName] [varchar](100) NOT NULL,
	[Ingredients] [varchar](1000) NOT NULL,
	[ServingUnit] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[CusineTypeID] [int] NOT NULL,
	[MealTypeID] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Description] [varchar](500) NULL,
	[Quantity] [int] NOT NULL,
	[DietTypeID] [int] NULL,
 CONSTRAINT [PK_MealItems] PRIMARY KEY CLUSTERED 
(
	[MealItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [search].[UpdateAllInfo]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [search].[UpdateAllInfo]
as

begin

update [search].[ActiveMealAds]

set AllInfo=[MealItemName] + ',' +
[CuisineType] + ',' + [MealType]  + ',' +[Description]
 + ',' +[Ingredients]  + ',' +[FirstName]  + ',' +[LastName] + ',' +
[Address1]  + ',' + coalesce([Address2],'')  + ',' +[City]
 + ',' +[Province] + ',' +[Zip] + ',' +[Country] + ',' +[Telephone]  + ',' +[Email]
  + ',' +coalesce([FoodAllergy] ,'') + ',' +[FoodType]  
  
  end
GO
/****** Object:  StoredProcedure [dbo].[updateActiveMealAdPublistdates]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  procedure [dbo].[updateActiveMealAdPublistdates]
as
begin

declare @noofdays int
select @noofdays=(select top 1 datediff(D,PickUpTime,getdate()) from search.ActiveMealAds order by PickUpTime)

update search.ActiveMealAds
--set PickUpTime=DATEADD(D,@noofdays+3,PickUpTime)
set PickUpTime=GETDATE(),--DATEADD(D,-1,getdate()),
FullAddress=Coalesce(Address1,'') + ',' + Coalesce(Address2,'')+ ',' + Coalesce(City,'') + ',' + Coalesce(Province,'') + ','  + Coalesce(Zip,'') + ',' + Coalesce(Country,'')

select * from search.ActiveMealAds




end
GO
/****** Object:  Table [dbo].[UserDetails]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserDetails](
	[UserId] [int] NOT NULL,
	[FirstName] [dbo].[name] NOT NULL,
	[LastName] [dbo].[name] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
	[UserStatus] [int] NOT NULL,
	[Notes] [varchar](max) NULL,
	[Photo] [varchar](500) NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[AllInfo] [varchar](max) NULL,
	[AddressID] [int] NOT NULL,
	[GoogleCheckoutID] [int] NULL,
	[PayPalID] [int] NULL,
	[AmazonPayID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[UserDetails] ADD [SkypeID] [varchar](20) NULL
ALTER TABLE [dbo].[UserDetails] ADD [Mobile] [varchar](20) NULL
ALTER TABLE [dbo].[UserDetails] ADD [IdentificationNumber] [nvarchar](100) NULL
ALTER TABLE [dbo].[UserDetails] ADD [PassportNumber] [nvarchar](100) NULL
ALTER TABLE [dbo].[UserDetails] ADD [CountryOfIssuance] [nvarchar](100) NULL
ALTER TABLE [dbo].[UserDetails] ADD [IsSeller] [int] NULL
SET ANSI_PADDING ON
ALTER TABLE [dbo].[UserDetails] ADD [KitchenName] [varchar](56) NULL
ALTER TABLE [dbo].[UserDetails] ADD [KitchenTypeID] [int] NULL
ALTER TABLE [dbo].[UserDetails] ADD [SellersTermsAndConditionAccepted] [int] NOT NULL
ALTER TABLE [dbo].[UserDetails] ADD  CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MealItems_Photos]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MealItems_Photos](
	[MealItemPhotoID] [int] IDENTITY(1,1) NOT NULL,
	[MealItemID] [int] NOT NULL,
	[Photo] [varchar](300) NOT NULL,
	[IsCover] [int] NOT NULL,
 CONSTRAINT [PK_MealItems_Photos] PRIMARY KEY CLUSTERED 
(
	[MealItemPhotoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MealItems_AllergenicFoods]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealItems_AllergenicFoods](
	[AllergenicFoodID] [int] NOT NULL,
	[MealItemID] [int] NOT NULL,
	[MealItem_AllergenID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_MealItems_AllergenicFoods] PRIMARY KEY CLUSTERED 
(
	[MealItem_AllergenID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MealAds]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealAds](
	[MealAdID] [int] IDENTITY(1,1) NOT NULL,
	[MealItemID] [int] NOT NULL,
	[MaxOrders] [int] NOT NULL,
	[PlacedOrder] [int] NOT NULL,
	[AvailabilityTypeID] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_MealAds] PRIMARY KEY CLUSTERED 
(
	[MealAdID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MealAd_Schedules]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealAd_Schedules](
	[MealAdScheduleID] [int] IDENTITY(1,1) NOT NULL,
	[MealAdID] [int] NOT NULL,
	[PickUpStartDateTime] [datetime] NOT NULL,
	[PickUpEndDateTime] [datetime] NOT NULL,
	[LastOrderDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MealAd_Schedules] PRIMARY KEY CLUSTERED 
(
	[MealAdScheduleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[MealAds_PaymentOptions]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealAds_PaymentOptions](
	[PaymentOptionID] [int] NOT NULL,
	[MealAdID] [int] NOT NULL,
	[MealAdPaymentOptionID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_MealAds_PaymentOptions] PRIMARY KEY CLUSTERED 
(
	[MealAdPaymentOptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MealAds_DeliveryMethods]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealAds_DeliveryMethods](
	[MealAd_DeliveryMethodID] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryMethodID] [int] NOT NULL,
	[MealAdID] [int] NOT NULL,
 CONSTRAINT [PK_MealAds_DeliveryMethods] PRIMARY KEY CLUSTERED 
(
	[MealAd_DeliveryMethodID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 03/02/2015 07:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[MealAdID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[FeedbackOnQuality] [int] NULL,
	[FeedbackOnPrice] [int] NULL,
	[OrderID] [int] NOT NULL,
 CONSTRAINT [PK_MealOrderDetails] PRIMARY KEY CLUSTERED 
(
	[MealAdID] ASC,
	[OrderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_GetFoodAllergenic]    Script Date: 03/02/2015 07:05:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ufn_GetFoodAllergenic](  @mealItemId int)

RETURNS varchar(200)
AS
BEGIN
	return (SELECT  ( SELECT f.AllergenicFood+ ','

           FROM dbo.MealItems_AllergenicFoods mf
           left join dbo.LKUPAllergenicFood f
            on f.AllergenicFoodID =mf.AllergenicFoodID
            

          WHERE mf.MealItemID= p1.MealItemID and mf.MealItemID=1

          ORDER BY AllergenicFood

            FOR XML PATH('') ) AS Products

      FROM MealItems_AllergenicFoods p1 where p1.MealItemID=1

      GROUP BY MealItemID)

END
GO
/****** Object:  StoredProcedure [dbo].[test]    Script Date: 03/02/2015 07:05:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[test]

as

begin

select * from UserDetails

end
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteUser]    Script Date: 03/02/2015 07:05:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec  [dbo].[usp_DeleteUser] 'kanjasaha@gmail.com'
CREATE procedure [dbo].[usp_DeleteUser]
 @email varchar(250)
 as
begin
declare @userid int
Select @userid=userid
from dbo.UserProfile
Where UserName=@email

delete from [ContactList]
where UserID=@userid


delete dbo.MealAds_DeliveryMethods
where MealAdID in (select MealAdID from MealAds
where mealitemid in (select MealItemID from MealItems
where UserId = @userid))


delete dbo.MealAds
where mealitemid in (select MealItemID from MealItems
where UserId = @userid)


delete dbo.MealItems_Photos
where  mealitemid in (select MealItemID from MealItems
where UserId = @userid)


delete dbo.MealItems_AllergenicFoods
where  mealitemid in (select MealItemID from MealItems
where UserId = @userid)

delete MealItems 
where UserId = @userid

delete [UserDetails]
where USERID = @userid

delete dbo.UserProfile
Where UserName=@email

end
GO
/****** Object:  StoredProcedure [dbo].[PopulateActiveMealAd]    Script Date: 03/02/2015 07:05:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[PopulateActiveMealAd] 124,'C'

CREATE procedure [dbo].[PopulateActiveMealAd]
@MealAdID Int,
@CRUD char(1)
as
begin


--if @CRUD ='D' 
--delete from [search].[ActiveMealAds] where MealAdID=@MealAdID

--if @CRUD ='U' 
--delete from [search].[ActiveMealAds] where MealAdID=@MealAdID


if @CRUD ='C' 
begin tran
INSERT INTO [search].[ActiveMealAds]
           ([MealAdID],[MealItemName],[PickUpTime],[MaxOrders],[PlacedOrder],[LastOrderTime],[CuisineType],[MealType]
           ,[Description],[Ingredients],Photo,[Quantity],[Price],[Currency],[FirstName],[LastName]
		   ,[Address1],[Address2],[City],[Province],[Zip],[Country],[Telephone],[Email],[FoodAllergy],[FoodType]
           ,[Latitude],[Longitude],[latlng],[PriceRange],[AllInfo])

SELECT 
i.[MealAdID],[MealItemName],PickUpStartDateTime,[MaxOrders],[PlacedOrder],LastOrderDateTime,cui.Name as CuisineType, mt.Name as [MealType]
           ,coalesce(m.[Description],''),[Ingredients],mp.[Photo] ,[Quantity],[Price],coun.[Currency],u.FirstName,[LastName]
		   ,[Address1],[Address2],[City],[Province],[Zip],[Country],[Telephone],up.UserName as [Email],dbo.ufn_GetFoodAllergenic(i.mealItemId),diet.Name
           ,[Latitude],[Longitude],cast([Latitude] as varchar)+','+cast([Longitude] as varchar) as [latlng],[Price] as [PriceRange], 
		   [MealItemName] + ',' +cui.Name+',' +[Ingredients]  as [AllInfo]
		 
  FROM MealAds i
  join MealItems m
  on i.MealItemID=m.mealItemId and i.MealAdID=@MealAdID
  join UserDetails u
  on m.userid=u.userid
  join LKUPCuisineType cui
  on cui.CuisineTypeID =m.CusineTypeID
  join LKUPDietType diet
  on diet.DietTypeID =m.DietTypeID
  join LKUPMealType mt
  on m.MealTypeID=mt.MealTypeID
  join AddressList al
  on u.AddressID = al.AddressID
  join LKUPCountry coun
  on al.CountryID=coun.CountryID
  join UserProfile up
  on up.UserId=m.UserId
  join MealAd_Schedules mealad
  on i.MealAdID = mealad.MealAdID
  left join [dbo].[MealItems_Photos] mp
  on m.MealItemId=mp.MealItemID and IsCover=1
 
 /*
  left join [dbo].[MealItems_AllergenicFoods] maf
  on i.MealItemID = maf.MealItemID
  left join [dbo].[LKUPAllergenicFood] allergen
  on maf.AllergenicFoodID = allergen.AllergenicFoodID
 
 
 */
  
  commit tran

  


END
GO
/****** Object:  Default [DF_ContactList_RequestAccepted]    Script Date: 03/02/2015 07:05:53 ******/
ALTER TABLE [dbo].[ContactList] ADD  CONSTRAINT [DF_ContactList_RequestAccepted]  DEFAULT ((0)) FOR [RequestAccepted]
GO
/****** Object:  Default [DF__webpages___IsCon__656C112C]    Script Date: 03/02/2015 07:05:53 ******/
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [IsConfirmed]
GO
/****** Object:  Default [DF__webpages___Passw__66603565]    Script Date: 03/02/2015 07:05:53 ******/
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [PasswordFailuresSinceLastSuccess]
GO
/****** Object:  Default [DF_UserAgreementsAcceptanceDetails_AgreementAccepetedOn]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserAgreementsAcceptanceDetails] ADD  CONSTRAINT [DF_UserAgreementsAcceptanceDetails_AgreementAccepetedOn]  DEFAULT (getdate()) FOR [AgreementAccepetedOn]
GO
/****** Object:  Default [DF_UserAgreements_DateOfAgreement]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserAgreements] ADD  CONSTRAINT [DF_UserAgreements_DateOfAgreement]  DEFAULT (getdate()) FOR [DateOfAgreement]
GO
/****** Object:  Default [DF_UserSettings_VerificationDate]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserSettings] ADD  CONSTRAINT [DF_UserSettings_VerificationDate]  DEFAULT (getdate()) FOR [VerificationDate]
GO
/****** Object:  Default [DF_LKUPMealType_DateCreated]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[LKUPMealType] ADD  CONSTRAINT [DF_LKUPMealType_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_LKUPKitchenType_DateCreated]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[LKUPKitchenType] ADD  CONSTRAINT [DF_LKUPKitchenType_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_LKUPDietType_DateCreated]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[LKUPDietType] ADD  CONSTRAINT [DF_LKUPDietType_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_UserAddressList_DateCreated]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[AddressList] ADD  CONSTRAINT [DF_UserAddressList_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_UserInfo_DateCreated]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserDetails] ADD  CONSTRAINT [DF_UserInfo_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_UserInfo_UserStatus]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserDetails] ADD  CONSTRAINT [DF_UserInfo_UserStatus]  DEFAULT ((1)) FOR [UserStatus]
GO
/****** Object:  Default [DF__UserDetai__Selle__32AB8735]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserDetails] ADD  DEFAULT ((0)) FOR [SellersTermsAndConditionAccepted]
GO
/****** Object:  Default [DF_MealAds_PlacedOrder]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAds] ADD  CONSTRAINT [DF_MealAds_PlacedOrder]  DEFAULT ((0)) FOR [PlacedOrder]
GO
/****** Object:  ForeignKey [fk_RoleId]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
/****** Object:  ForeignKey [fk_UserId]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
/****** Object:  ForeignKey [FK_UserSettings_LKUPActivityType]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserSettings]  WITH CHECK ADD  CONSTRAINT [FK_UserSettings_LKUPActivityType] FOREIGN KEY([ActivityTypeID])
REFERENCES [dbo].[LKUPActivityType] ([ActivityTypeLKUPID])
GO
ALTER TABLE [dbo].[UserSettings] CHECK CONSTRAINT [FK_UserSettings_LKUPActivityType]
GO
/****** Object:  ForeignKey [FK_UserSettings_LKUPPrivacySettings]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserSettings]  WITH CHECK ADD  CONSTRAINT [FK_UserSettings_LKUPPrivacySettings] FOREIGN KEY([PrivacySettingsID])
REFERENCES [dbo].[LKUPPrivacySettings] ([PrivacySettingsID])
GO
ALTER TABLE [dbo].[UserSettings] CHECK CONSTRAINT [FK_UserSettings_LKUPPrivacySettings]
GO
/****** Object:  ForeignKey [FK_UserSettings_NotificationFrequency]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserSettings]  WITH CHECK ADD  CONSTRAINT [FK_UserSettings_NotificationFrequency] FOREIGN KEY([NotificationFrequencyID])
REFERENCES [dbo].[NotificationFrequency] ([NotificationFrequencyID])
GO
ALTER TABLE [dbo].[UserSettings] CHECK CONSTRAINT [FK_UserSettings_NotificationFrequency]
GO
/****** Object:  ForeignKey [FK_FunOrderDetails_FunOrders]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[FunOrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_FunOrderDetails_FunOrders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[FunOrders] ([OrderID])
GO
ALTER TABLE [dbo].[FunOrderDetails] CHECK CONSTRAINT [FK_FunOrderDetails_FunOrders]
GO
/****** Object:  ForeignKey [FK_UserAddressList_CountryLKUP]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[AddressList]  WITH CHECK ADD  CONSTRAINT [FK_UserAddressList_CountryLKUP] FOREIGN KEY([CountryID])
REFERENCES [dbo].[LKUPCountry] ([CountryID])
GO
ALTER TABLE [dbo].[AddressList] CHECK CONSTRAINT [FK_UserAddressList_CountryLKUP]
GO
/****** Object:  ForeignKey [FK_MealItems_CuisineTypeLKUP]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealItems]  WITH CHECK ADD  CONSTRAINT [FK_MealItems_CuisineTypeLKUP] FOREIGN KEY([CusineTypeID])
REFERENCES [dbo].[LKUPCuisineType] ([CuisineTypeID])
GO
ALTER TABLE [dbo].[MealItems] CHECK CONSTRAINT [FK_MealItems_CuisineTypeLKUP]
GO
/****** Object:  ForeignKey [FK_MealItems_LKUPDietType]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealItems]  WITH CHECK ADD  CONSTRAINT [FK_MealItems_LKUPDietType] FOREIGN KEY([DietTypeID])
REFERENCES [dbo].[LKUPDietType] ([DietTypeID])
GO
ALTER TABLE [dbo].[MealItems] CHECK CONSTRAINT [FK_MealItems_LKUPDietType]
GO
/****** Object:  ForeignKey [FK_MealItems_LKUPServingUnit]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealItems]  WITH CHECK ADD  CONSTRAINT [FK_MealItems_LKUPServingUnit] FOREIGN KEY([ServingUnit])
REFERENCES [dbo].[LKUPServingUnit] ([ServingUnitID])
GO
ALTER TABLE [dbo].[MealItems] CHECK CONSTRAINT [FK_MealItems_LKUPServingUnit]
GO
/****** Object:  ForeignKey [FK_MealItems_MealTypeLKUP]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealItems]  WITH CHECK ADD  CONSTRAINT [FK_MealItems_MealTypeLKUP] FOREIGN KEY([MealTypeID])
REFERENCES [dbo].[LKUPMealType] ([MealTypeID])
GO
ALTER TABLE [dbo].[MealItems] CHECK CONSTRAINT [FK_MealItems_MealTypeLKUP]
GO
/****** Object:  ForeignKey [FK_UserDetails_AddressList]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserDetails]  WITH CHECK ADD  CONSTRAINT [FK_UserDetails_AddressList] FOREIGN KEY([AddressID])
REFERENCES [dbo].[AddressList] ([AddressID])
GO
ALTER TABLE [dbo].[UserDetails] CHECK CONSTRAINT [FK_UserDetails_AddressList]
GO
/****** Object:  ForeignKey [FK_UserDetails_AddressList1]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserDetails]  WITH CHECK ADD  CONSTRAINT [FK_UserDetails_AddressList1] FOREIGN KEY([AddressID])
REFERENCES [dbo].[AddressList] ([AddressID])
GO
ALTER TABLE [dbo].[UserDetails] CHECK CONSTRAINT [FK_UserDetails_AddressList1]
GO
/****** Object:  ForeignKey [FK_UserDetails_KitchenType]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[UserDetails]  WITH CHECK ADD  CONSTRAINT [FK_UserDetails_KitchenType] FOREIGN KEY([KitchenTypeID])
REFERENCES [dbo].[LKUPKitchenType] ([KitchenTypeID])
GO
ALTER TABLE [dbo].[UserDetails] CHECK CONSTRAINT [FK_UserDetails_KitchenType]
GO
/****** Object:  ForeignKey [FK_MealItems_Photos_MealItems]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealItems_Photos]  WITH CHECK ADD  CONSTRAINT [FK_MealItems_Photos_MealItems] FOREIGN KEY([MealItemID])
REFERENCES [dbo].[MealItems] ([MealItemId])
GO
ALTER TABLE [dbo].[MealItems_Photos] CHECK CONSTRAINT [FK_MealItems_Photos_MealItems]
GO
/****** Object:  ForeignKey [FK_MealItems_AllergenicFoods_LKUPAllergenicFood]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealItems_AllergenicFoods]  WITH CHECK ADD  CONSTRAINT [FK_MealItems_AllergenicFoods_LKUPAllergenicFood] FOREIGN KEY([AllergenicFoodID])
REFERENCES [dbo].[LKUPAllergenicFood] ([AllergenicFoodID])
GO
ALTER TABLE [dbo].[MealItems_AllergenicFoods] CHECK CONSTRAINT [FK_MealItems_AllergenicFoods_LKUPAllergenicFood]
GO
/****** Object:  ForeignKey [FK_MealItems_AllergenicFoods_MealItems]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealItems_AllergenicFoods]  WITH CHECK ADD  CONSTRAINT [FK_MealItems_AllergenicFoods_MealItems] FOREIGN KEY([MealItemID])
REFERENCES [dbo].[MealItems] ([MealItemId])
GO
ALTER TABLE [dbo].[MealItems_AllergenicFoods] CHECK CONSTRAINT [FK_MealItems_AllergenicFoods_MealItems]
GO
/****** Object:  ForeignKey [FK_MealAds_AvailabilityTypes]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAds]  WITH CHECK ADD  CONSTRAINT [FK_MealAds_AvailabilityTypes] FOREIGN KEY([AvailabilityTypeID])
REFERENCES [dbo].[AvailabilityTypes] ([AvaiilabilityTypeID])
GO
ALTER TABLE [dbo].[MealAds] CHECK CONSTRAINT [FK_MealAds_AvailabilityTypes]
GO
/****** Object:  ForeignKey [FK_MealAds_MealItems]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAds]  WITH CHECK ADD  CONSTRAINT [FK_MealAds_MealItems] FOREIGN KEY([MealItemID])
REFERENCES [dbo].[MealItems] ([MealItemId])
GO
ALTER TABLE [dbo].[MealAds] CHECK CONSTRAINT [FK_MealAds_MealItems]
GO
/****** Object:  ForeignKey [FK_MealAds_MealItems1]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAds]  WITH CHECK ADD  CONSTRAINT [FK_MealAds_MealItems1] FOREIGN KEY([MealItemID])
REFERENCES [dbo].[MealItems] ([MealItemId])
GO
ALTER TABLE [dbo].[MealAds] CHECK CONSTRAINT [FK_MealAds_MealItems1]
GO
/****** Object:  ForeignKey [FK_MealAd_Schedules_MealAds]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAd_Schedules]  WITH CHECK ADD  CONSTRAINT [FK_MealAd_Schedules_MealAds] FOREIGN KEY([MealAdID])
REFERENCES [dbo].[MealAds] ([MealAdID])
GO
ALTER TABLE [dbo].[MealAd_Schedules] CHECK CONSTRAINT [FK_MealAd_Schedules_MealAds]
GO
/****** Object:  ForeignKey [FK_GoogleCheckOut_UserDetails]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [ecom].[GoogleCheckOut]  WITH CHECK ADD  CONSTRAINT [FK_GoogleCheckOut_UserDetails] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserDetails] ([UserId])
GO
ALTER TABLE [ecom].[GoogleCheckOut] CHECK CONSTRAINT [FK_GoogleCheckOut_UserDetails]
GO
/****** Object:  ForeignKey [FK_AmazonPay_UserDetails]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [ecom].[AmazonPay]  WITH CHECK ADD  CONSTRAINT [FK_AmazonPay_UserDetails] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserDetails] ([UserId])
GO
ALTER TABLE [ecom].[AmazonPay] CHECK CONSTRAINT [FK_AmazonPay_UserDetails]
GO
/****** Object:  ForeignKey [FK_Cart_MealAds]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_MealAds] FOREIGN KEY([MealAdID])
REFERENCES [dbo].[MealAds] ([MealAdID])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_MealAds]
GO
/****** Object:  ForeignKey [FK_PayPal_UserDetails]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [ecom].[PayPal]  WITH CHECK ADD  CONSTRAINT [FK_PayPal_UserDetails] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserDetails] ([UserId])
GO
ALTER TABLE [ecom].[PayPal] CHECK CONSTRAINT [FK_PayPal_UserDetails]
GO
/****** Object:  ForeignKey [FK_MealAds_PaymentOptions_MealAds]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAds_PaymentOptions]  WITH CHECK ADD  CONSTRAINT [FK_MealAds_PaymentOptions_MealAds] FOREIGN KEY([MealAdID])
REFERENCES [dbo].[MealAds] ([MealAdID])
GO
ALTER TABLE [dbo].[MealAds_PaymentOptions] CHECK CONSTRAINT [FK_MealAds_PaymentOptions_MealAds]
GO
/****** Object:  ForeignKey [FK_MealAds_PaymentOptions_PaymentOptions]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAds_PaymentOptions]  WITH CHECK ADD  CONSTRAINT [FK_MealAds_PaymentOptions_PaymentOptions] FOREIGN KEY([PaymentOptionID])
REFERENCES [dbo].[PaymentOptions] ([PaymentOptionID])
GO
ALTER TABLE [dbo].[MealAds_PaymentOptions] CHECK CONSTRAINT [FK_MealAds_PaymentOptions_PaymentOptions]
GO
/****** Object:  ForeignKey [FK_MealAds_DeliveryMethods_LKUPDeliveryMethod]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAds_DeliveryMethods]  WITH CHECK ADD  CONSTRAINT [FK_MealAds_DeliveryMethods_LKUPDeliveryMethod] FOREIGN KEY([DeliveryMethodID])
REFERENCES [dbo].[LKUPDeliveryMethod] ([DeliveryMethodID])
GO
ALTER TABLE [dbo].[MealAds_DeliveryMethods] CHECK CONSTRAINT [FK_MealAds_DeliveryMethods_LKUPDeliveryMethod]
GO
/****** Object:  ForeignKey [FK_MealAds_DeliveryMethods_MealAds]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[MealAds_DeliveryMethods]  WITH CHECK ADD  CONSTRAINT [FK_MealAds_DeliveryMethods_MealAds] FOREIGN KEY([MealAdID])
REFERENCES [dbo].[MealAds] ([MealAdID])
GO
ALTER TABLE [dbo].[MealAds_DeliveryMethods] CHECK CONSTRAINT [FK_MealAds_DeliveryMethods_MealAds]
GO
/****** Object:  ForeignKey [FK_MealOrderDetails_MealOrders]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_MealOrderDetails_MealOrders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_MealOrderDetails_MealOrders]
GO
/****** Object:  ForeignKey [FK_OrderDetails_MealAds]    Script Date: 03/02/2015 07:05:55 ******/
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_MealAds] FOREIGN KEY([MealAdID])
REFERENCES [dbo].[MealAds] ([MealAdID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_MealAds]
GO
ALter TABLE search.ActiveMealAds 	add [ActiveMealAdID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ActiveMealAds] PRIMARY KEY CLUSTERED 
(
	[ActiveMealAdID] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


GO

USE [ThreeSixtyTwo]
GO

/****** Object:  StoredProcedure [dbo].[PopulateActiveMealAd]    Script Date: 7/24/2015 6:56:44 AM ******/
DROP PROCEDURE [dbo].[PopulateActiveMealAd]
GO

/****** Object:  StoredProcedure [dbo].[PopulateActiveMealAd]    Script Date: 7/24/2015 6:56:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--exec [dbo].[PopulateActiveMealAd] 124,'C'

CREATE procedure [dbo].[PopulateActiveMealAd]
@MealAdID Int,
@CRUD char(1)
as
begin


--if @CRUD ='D' 
--delete from [search].[ActiveMealAds] where MealAdID=@MealAdID

--if @CRUD ='U' 
--delete from [search].[ActiveMealAds] where MealAdID=@MealAdID


if @CRUD ='C' 
begin tran
INSERT INTO [search].[ActiveMealAds]
           ([MealAdID],[MealItemName],[PickUpTime],[MaxOrders],[PlacedOrder],[LastOrderTime],[CuisineType],[MealType]
           ,[Description],[Ingredients],Photo,[Quantity],[Price],[Currency],[FirstName],[LastName]
		   ,[Address1],[Address2],[City],[Province],[Zip],[Country],[Telephone],[Email],[FoodAllergy],[FoodType]
           ,[Latitude],[Longitude],[latlng],[PriceRange],[AllInfo])

SELECT 
i.[MealAdID],[MealItemName],PickUpStartDateTime,[MaxOrders],[PlacedOrder],LastOrderDateTime,cui.Name as CuisineType, mt.Name as [MealType]
           ,coalesce(m.[Description],''),[Ingredients],mp.[Photo] ,[Quantity],[Price],coun.[Currency],u.FirstName,[LastName]
		   ,[Address1],[Address2],[City],[Province],[Zip],[Country],[Telephone],up.UserName as [Email],dbo.ufn_GetFoodAllergenic(i.mealItemId),diet.Name
           ,[Latitude],[Longitude],cast([Latitude] as varchar)+','+cast([Longitude] as varchar) as [latlng],[Price] as [PriceRange], 
		   [MealItemName] + ',' +cui.Name+',' +[Ingredients]  as [AllInfo]
		 
  FROM MealAds i
  join MealItems m
  on i.MealItemID=m.mealItemId and i.MealAdID=@MealAdID
  join UserDetails u
  on m.userid=u.userid
  join LKUPCuisineType cui
  on cui.CuisineTypeID =m.CusineTypeID
  join LKUPDietType diet
  on diet.DietTypeID =m.DietTypeID
  join LKUPMealType mt
  on m.MealTypeID=mt.MealTypeID
  join AddressList al
  on u.AddressID = al.AddressID
  join LKUPCountry coun
  on al.CountryID=coun.CountryID
  join UserProfile up
  on up.UserId=m.UserId
  join MealAd_Schedules mealad
  on i.MealAdID = mealad.MealAdID
  left join [dbo].[MealItems_Photos] mp
  on m.MealItemId=mp.MealItemID and IsCover=1
 
 /*
  left join [dbo].[MealItems_AllergenicFoods] maf
  on i.MealItemID = maf.MealItemID
  left join [dbo].[LKUPAllergenicFood] allergen
  on maf.AllergenicFoodID = allergen.AllergenicFoodID
 
 
 */
  
  commit tran

  


END



GO

