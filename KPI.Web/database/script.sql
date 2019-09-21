USE [KPI]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActionPlanDetails]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionPlanDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActionPlanID] [int] NOT NULL,
	[USerID] [int] NOT NULL,
	[Sent] [bit] NOT NULL,
	[Seen] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.ActionPlanDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActionPlans]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionPlans](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Deadline] [datetime] NOT NULL,
	[SubmitDate] [datetime] NOT NULL,
	[ApprovedBy] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[DataID] [int] NOT NULL,
	[CommentID] [int] NOT NULL,
	[KPILevelCodeAndPeriod] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[ApprovedStatus] [bit] NOT NULL,
	[Tag] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
	[TagID] [int] NOT NULL,
	[Link] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ActionPlans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[LevelID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ParentID] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CommentMsg] [nvarchar](max) NULL,
	[CommentedDate] [datetime] NOT NULL,
	[UserID] [int] NOT NULL,
	[DataID] [int] NOT NULL,
	[Link] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Comments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Data]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Data](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[Period] [nvarchar](max) NULL,
	[Year] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[DateUpload] [nvarchar](max) NULL,
	[Week] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[KPILevelCode] [nvarchar](max) NULL,
	[Remark] [nvarchar](max) NULL,
	[Target] [float] NOT NULL,
 CONSTRAINT [PK_dbo.Data] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Favourites]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favourites](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KPILevelCode] [nvarchar](max) NULL,
	[UserID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Period] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Favourites] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KPILevels]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KPILevels](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KPILevelCode] [nvarchar](max) NULL,
	[UserCheck] [nvarchar](max) NULL,
	[KPIID] [int] NOT NULL,
	[LevelID] [int] NOT NULL,
	[Period] [nvarchar](max) NULL,
	[Weekly] [int] NULL,
	[Monthly] [datetime] NULL,
	[Quarterly] [datetime] NULL,
	[Yearly] [datetime] NULL,
	[Checked] [bit] NULL,
	[WeeklyChecked] [bit] NULL,
	[MonthlyChecked] [bit] NULL,
	[QuarterlyChecked] [bit] NULL,
	[YearlyChecked] [bit] NULL,
	[CheckedPeriod] [bit] NULL,
	[TimeCheck] [datetime] NULL,
	[CreateTime] [datetime] NOT NULL,
	[LevelNumber] [int] NOT NULL,
	[TeamID] [int] NULL,
	[WeeklyPublic] [bit] NULL,
	[MonthlyPublic] [bit] NULL,
	[QuarterlyPublic] [bit] NULL,
	[YearlyPublic] [bit] NULL,
	[WeeklyStandard] [int] NOT NULL,
	[MonthlyStandard] [int] NOT NULL,
	[QuarterlyStandard] [int] NOT NULL,
	[YearlyStandard] [int] NOT NULL,
	[WeeklyTarget] [int] NOT NULL,
	[MonthlyTarget] [int] NOT NULL,
	[QuarterlyTarget] [int] NOT NULL,
	[YearlyTarget] [int] NOT NULL,
 CONSTRAINT [PK_dbo.KPILevels] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KPIs]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KPIs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [nvarchar](max) NULL,
	[CategoryID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[LevelID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ParentID] [nvarchar](max) NULL,
	[Unit] [int] NOT NULL,
 CONSTRAINT [PK_dbo.KPIs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Levels]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Levels](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[ParentID] [int] NULL,
	[ParentCode] [nvarchar](max) NULL,
	[LevelNumber] [int] NULL,
	[State] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Levels] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Link] [nvarchar](max) NULL,
	[FontAwesome] [nvarchar](max) NULL,
	[BackgroudColor] [nvarchar](max) NULL,
	[Permission] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Menus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationDetails]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[NotificationID] [int] NOT NULL,
	[Seen] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.NotificationDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[KPIName] [nvarchar](max) NULL,
	[Period] [nvarchar](max) NULL,
	[Seen] [bit] NOT NULL,
	[Link] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Tag] [nvarchar](max) NULL,
	[TagID] [int] NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Notifications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [nvarchar](max) NULL,
	[URLDefault] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Permissions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resources]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resources](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Link] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[State] [bit] NOT NULL,
	[Permission] [int] NOT NULL,
	[FontAwesome] [nvarchar](max) NULL,
	[CheckRole] [bit] NOT NULL,
	[BackgroudColor] [nvarchar](max) NULL,
	[Menu] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Resources] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Revises]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Revises](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KPILevelCodePeriod] [nvarchar](max) NULL,
	[PeriodValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Revises] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SeenComments]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SeenComments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CommentID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.SeenComments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[CommentID] [int] NOT NULL,
	[ActionPlanID] [int] NOT NULL,
	[NotificationID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Tags] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Units]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Units](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Units] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[State] [bit] NOT NULL,
	[LevelID] [int] NOT NULL,
	[Role] [int] NOT NULL,
	[TeamID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[FullName] [nvarchar](max) NULL,
	[Permission] [int] NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Skype] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActionPlans] ADD  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[ActionPlans] ADD  DEFAULT ((0)) FOR [DataID]
GO
ALTER TABLE [dbo].[ActionPlans] ADD  DEFAULT ((0)) FOR [CommentID]
GO
ALTER TABLE [dbo].[ActionPlans] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[ActionPlans] ADD  DEFAULT ((0)) FOR [ApprovedStatus]
GO
ALTER TABLE [dbo].[ActionPlans] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
ALTER TABLE [dbo].[ActionPlans] ADD  DEFAULT ((0)) FOR [TagID]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT ((0)) FOR [DataID]
GO
ALTER TABLE [dbo].[Data] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
ALTER TABLE [dbo].[Data] ADD  DEFAULT ((0)) FOR [Target]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [LevelNumber]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [TeamID]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [WeeklyStandard]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [MonthlyStandard]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [QuarterlyStandard]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [YearlyStandard]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [WeeklyTarget]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [MonthlyTarget]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [QuarterlyTarget]
GO
ALTER TABLE [dbo].[KPILevels] ADD  DEFAULT ((0)) FOR [YearlyTarget]
GO
ALTER TABLE [dbo].[Levels] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
ALTER TABLE [dbo].[Menus] ADD  DEFAULT ((0)) FOR [Permission]
GO
ALTER TABLE [dbo].[Menus] ADD  DEFAULT ((0)) FOR [ParentID]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [CreateTime]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ((0)) FOR [TagID]
GO
ALTER TABLE [dbo].[Resources] ADD  DEFAULT ((0)) FOR [CheckRole]
GO
ALTER TABLE [dbo].[Resources] ADD  DEFAULT ((0)) FOR [Menu]
GO
ALTER TABLE [dbo].[Resources] ADD  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[Tags] ADD  DEFAULT ((0)) FOR [NotificationID]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Role]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [TeamID]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Permission]
GO
/****** Object:  StoredProcedure [dbo].[SqlQueryNotificationStoredProcedure-4a464966-f556-4fc9-bedd-ace4c41ffd99]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SqlQueryNotificationStoredProcedure-4a464966-f556-4fc9-bedd-ace4c41ffd99] AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM [SqlQueryNotificationService-4a464966-f556-4fc9-bedd-ace4c41ffd99]; IF (SELECT COUNT(*) FROM [SqlQueryNotificationService-4a464966-f556-4fc9-bedd-ace4c41ffd99] WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = 'SqlQueryNotificationService-4a464966-f556-4fc9-bedd-ace4c41ffd99') > 0)   DROP SERVICE [SqlQueryNotificationService-4a464966-f556-4fc9-bedd-ace4c41ffd99]; if (OBJECT_ID('SqlQueryNotificationService-4a464966-f556-4fc9-bedd-ace4c41ffd99', 'SQ') IS NOT NULL)   DROP QUEUE [SqlQueryNotificationService-4a464966-f556-4fc9-bedd-ace4c41ffd99]; DROP PROCEDURE [SqlQueryNotificationStoredProcedure-4a464966-f556-4fc9-bedd-ace4c41ffd99]; END COMMIT TRANSACTION; END
GO
/****** Object:  StoredProcedure [dbo].[SqlQueryNotificationStoredProcedure-b1c46801-72ad-4ee9-ab12-07f5f42862bd]    Script Date: 9/21/2019 4:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SqlQueryNotificationStoredProcedure-b1c46801-72ad-4ee9-ab12-07f5f42862bd] AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM [SqlQueryNotificationService-b1c46801-72ad-4ee9-ab12-07f5f42862bd]; IF (SELECT COUNT(*) FROM [SqlQueryNotificationService-b1c46801-72ad-4ee9-ab12-07f5f42862bd] WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = 'SqlQueryNotificationService-b1c46801-72ad-4ee9-ab12-07f5f42862bd') > 0)   DROP SERVICE [SqlQueryNotificationService-b1c46801-72ad-4ee9-ab12-07f5f42862bd]; if (OBJECT_ID('SqlQueryNotificationService-b1c46801-72ad-4ee9-ab12-07f5f42862bd', 'SQ') IS NOT NULL)   DROP QUEUE [SqlQueryNotificationService-b1c46801-72ad-4ee9-ab12-07f5f42862bd]; DROP PROCEDURE [SqlQueryNotificationStoredProcedure-b1c46801-72ad-4ee9-ab12-07f5f42862bd]; END COMMIT TRANSACTION; END
GO
