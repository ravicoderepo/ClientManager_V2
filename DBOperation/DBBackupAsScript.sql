USE [ph17179099821_ClientManager]
GO
/****** Object:  User [ravidev]    Script Date: 21-04-2022 00:05:03 ******/
CREATE USER [ravidev] FOR LOGIN [ravidev] WITH DEFAULT_SCHEMA=[ravidev]
GO
/****** Object:  DatabaseRole [gd_execprocs]    Script Date: 21-04-2022 00:05:04 ******/
CREATE ROLE [gd_execprocs]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [ravidev]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [ravidev]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [ravidev]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ravidev]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ravidev]
GO
/****** Object:  Schema [ravidev]    Script Date: 21-04-2022 00:05:04 ******/
CREATE SCHEMA [ravidev]
GO
/****** Object:  Table [dbo].[ClientContacts]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientContacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_ClientContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [nvarchar](50) NOT NULL,
	[ClientName] [nvarchar](50) NOT NULL,
	[ContactPersonName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_ClientDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactType] [int] NOT NULL,
	[ContactPersonName] [nvarchar](50) NOT NULL,
	[Designation] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[AddressLine1] [nvarchar](100) NOT NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[State] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[Pincode] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Website] [nvarchar](500) NULL,
	[WorkPhoneNo] [nvarchar](50) NULL,
	[PersonalPhoneNo] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_ContactDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[DateOfBirth] [datetime] NULL,
	[DateOfJoining] [datetime] NULL,
	[EmployeeId] [nvarchar](50) NULL,
	[AddressLine1] [nvarchar](100) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[State] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Pincode] [nvarchar](50) NULL,
	[ReportingManager] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[ProductCode] [nvarchar](50) NOT NULL,
	[ProductImage] [nvarchar](max) NULL,
	[Description] [nvarchar](100) NULL,
	[UnitPrice] [decimal](18, 0) NOT NULL,
	[PurchasedDate] [datetime] NOT NULL,
	[ExpiredOn] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectClient]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectClient](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[ClientId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProjectClient] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [nvarchar](50) NOT NULL,
	[ProjectName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[Status] [int] NULL,
	[ClientCompany] [int] NOT NULL,
	[ProjectLead] [int] NOT NULL,
	[EstimatedBudget] [decimal](18, 0) NOT NULL,
	[TotalAmountSpent] [decimal](18, 0) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EstimatedEndDate] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProjectDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectStatus]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProjectStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RepresentativeSaleTarget]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RepresentativeSaleTarget](
	[Id] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[SalesTarget] [int] NOT NULL,
	[CallTarget] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CompletedCount] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_RepresentativeSaleTarget] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SaleActivity]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleActivity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SaleDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[ClientName] [nvarchar](100) NULL,
	[ClientEmail] [nvarchar](100) NULL,
	[ClientPhoneNo] [nvarchar](50) NOT NULL,
	[ProductId] [int] NULL,
	[ProductName] [nvarchar](100) NULL,
	[Capacity] [nvarchar](500) NULL,
	[Unit] [nvarchar](50) NULL,
	[RecentCallDate] [datetime] NOT NULL,
	[AnticipatedClosingDate] [datetime] NULL,
	[NoOfFollowUps] [int] NULL,
	[Remarks] [nvarchar](max) NOT NULL,
	[SalesRepresentativeId] [int] NOT NULL,
	[InvoiceNo] [nvarchar](50) NULL,
	[InvoiceAmount] [decimal](18, 0) NULL,
	[DateOfClosing] [datetime] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_SaleActivity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SaleProducts]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SaleId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Unit] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SaleProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SaleDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[AnticipatedClosing] [nvarchar](100) NOT NULL,
	[NoOfFollowUps] [int] NOT NULL,
	[NextFollowUpDate] [datetime] NULL,
	[RepresentativeId] [int] NOT NULL,
	[Remarks] [nvarchar](1000) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SaleDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesStatus]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](50) NULL,
	[Description] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SalesStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserContacts]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserContacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_UserContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_UserRoles_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 21-04-2022 00:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NULL,
	[DateOfBirth] [datetime] NULL,
	[DateOfJoining] [datetime] NULL,
	[EmployeeId] [nvarchar](50) NULL,
	[AddressLine1] [nvarchar](100) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[State] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Pincode] [nvarchar](50) NULL,
	[ReportingManager] [int] NULL,
	[SaleTarget] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_UserDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (20, N'40mm Mini Rail', N'PROD0001', N'', N'40mm Mini Rail', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (21, N'60mm Mini Rail', N'PROD0002', N'', N'60mm Mini Rail', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (22, N'Mid Clamp', N'PROD0003', N'', N'Mid Clamp', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (23, N'End Clamp', N'PROD0004', N'', N'End Clamp', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (24, N'Spring Nut', N'PROD0005', N'', N'Spring Nut', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (25, N'T Nut', N'PROD0006', N'', N'T Nut', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (26, N'Universal Mid Clamp', N'PROD0007', N'', N'Universal Mid Clamp', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (27, N'Triangle - 2 Meter', N'PROD0008', N'', N'Triangle - 2 Meter', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (28, N'Triangle - 1 Meter', N'PROD0009', N'', N'Triangle - 1 Meter', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (29, N'Reverse Tilt Structure', N'PROD0010', N'', N'Reverse Tilt Structure', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (30, N'Strut Rail', N'PROD0011', N'', N'Strut Rail', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (31, N'Long Rail with L Bracket', N'PROD0012', N'', N'Long Rail with L Bracket', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (32, N'Adhesive Rail', N'PROD0013', N'', N'Adhesive Rail', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (33, N'Hot Dip structure', N'PROD0014', N'', N'Hot Dip structure', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (34, N'Fasteners', N'PROD0015', N'', N'Fasteners', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (35, N'No Requirement', N'PROD0016', N'', N'No Requirement', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (36, N'All products price', N'PROD0017', N'', N'All products price', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (37, N'Inverter', N'PROD0018', N'', N'Inverter', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductCode], [ProductImage], [Description], [UnitPrice], [PurchasedDate], [ExpiredOn], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (38, N'DC Cable', N'PROD0019', N'', N'DC Cable', CAST(0 AS Decimal(18, 0)), CAST(N'2021-08-28T00:00:00.000' AS DateTime), CAST(N'2025-08-29T00:00:00.000' AS DateTime), CAST(N'2021-08-28T00:00:00.000' AS DateTime), N'1', NULL, N'')
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([Id], [RoleName], [IsActive], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'Admin', 1, CAST(N'2021-08-26T00:00:00.000' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[Roles] ([Id], [RoleName], [IsActive], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'Manager', 1, CAST(N'2021-08-26T00:00:00.000' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[Roles] ([Id], [RoleName], [IsActive], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'SalesRep', 1, CAST(N'2021-08-26T00:00:00.000' AS DateTime), 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[SalesStatus] ON 
GO
INSERT [dbo].[SalesStatus] ([Id], [Status], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'Initial Call', N'Initial Call', CAST(N'2021-08-26T00:00:00.000' AS DateTime), N'1', NULL, NULL)
GO
INSERT [dbo].[SalesStatus] ([Id], [Status], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'In Discussion', N'In Discussion', CAST(N'2021-08-26T00:00:00.000' AS DateTime), N'1', NULL, NULL)
GO
INSERT [dbo].[SalesStatus] ([Id], [Status], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'Pending from Customer', N'Pending from Customer', CAST(N'2021-08-26T00:00:00.000' AS DateTime), N'1', NULL, NULL)
GO
INSERT [dbo].[SalesStatus] ([Id], [Status], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'Cancelled', N'Cancelled', CAST(N'2021-08-26T00:00:00.000' AS DateTime), N'1', NULL, NULL)
GO
INSERT [dbo].[SalesStatus] ([Id], [Status], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'PO Received – WIP', N'PO Received – WIP', CAST(N'2021-08-26T00:00:00.000' AS DateTime), N'1', NULL, NULL)
GO
INSERT [dbo].[SalesStatus] ([Id], [Status], [Description], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (6, N'Closed', N'Closed', CAST(N'2021-08-26T00:00:00.000' AS DateTime), N'1', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[SalesStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (6, 3, 3, CAST(N'2021-08-26T00:00:00.000' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (12, 2, 2, CAST(N'2021-09-27T10:23:05.187' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (13, 1, 1, CAST(N'2021-09-27T10:23:56.283' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (14, 5, 3, CAST(N'2021-09-27T10:24:34.833' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (18, 8, 3, CAST(N'2021-11-06T21:36:06.307' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (19, 4, 2, CAST(N'2021-11-06T23:33:28.063' AS DateTime), 4, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'admin123#', N'Admin', N'admin@gmail.com', 1, CAST(N'1999-09-08T00:00:00.000' AS DateTime), CAST(N'1999-09-08T00:00:00.000' AS DateTime), N'EMP001', N'Address Line 1', N'Address Line 2', N'Tamilnadu', N'Chennai', N'600001', 1, 55, CAST(N'2021-08-29T00:00:00.000' AS DateTime), 1, CAST(N'2021-11-06T22:12:45.650' AS DateTime), 1)
GO
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'manager1123#', N'Velmurugan', N'manager1@gmail.com', 0, CAST(N'1999-09-08T00:00:00.000' AS DateTime), CAST(N'1999-09-08T00:00:00.000' AS DateTime), N'EMP002', N'Address Line 1', N'Address Line 2', N'Tamilnadu', N'Chennai', N'600001', 1, 60, CAST(N'2021-08-26T00:00:00.000' AS DateTime), 1, CAST(N'2022-04-18T05:11:49.047' AS DateTime), 1)
GO
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'salesrep1123#', N'Rajaseakar', N'salesrep1@gmail.com', 0, CAST(N'1999-09-08T00:00:00.000' AS DateTime), CAST(N'1999-09-08T00:00:00.000' AS DateTime), N'EMP003', N'Address Line 1', N'Address Line 2', N'Tamilnadu', N'Chennai', N'600001', 1, 555, CAST(N'2021-08-26T00:00:00.000' AS DateTime), 1, CAST(N'2022-04-18T05:09:52.633' AS DateTime), 1)
GO
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'manager2123#', N'Ravikumar', N'manager2@gmail.com', 0, CAST(N'1999-09-08T00:00:00.000' AS DateTime), CAST(N'1999-09-08T00:00:00.000' AS DateTime), N'EMP004', N'Address Line 1', N'Address Line 2', N'Tamilnadu', N'Chennai', N'600001', 1, 50, CAST(N'2021-08-26T18:07:22.950' AS DateTime), 1, CAST(N'2022-04-18T05:09:44.803' AS DateTime), 1)
GO
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'salesrep2123#', N'Michael', N'salesrep2@gmail.com', 0, CAST(N'1999-09-08T00:00:00.000' AS DateTime), CAST(N'1999-09-08T00:00:00.000' AS DateTime), N'EMP005', N'Address Line 1', N'Address Line 2', N'Tamilnadu', N'Chennai', N'600001', 1, 50, CAST(N'2021-08-26T18:13:57.833' AS DateTime), 1, CAST(N'2022-04-18T05:09:40.867' AS DateTime), 1)
GO
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (8, N'salesrep2123#', N'Sriganesh', N'salesrep3@gmail.com', 0, CAST(N'1999-09-08T00:00:00.000' AS DateTime), CAST(N'1999-09-08T00:00:00.000' AS DateTime), N'EMP006', N'Address Line 1', N'Address Line 2', N'Tamilnadu', N'Chennai', N'600001', 1, 50, CAST(N'2021-09-26T09:12:33.493' AS DateTime), 1, CAST(N'2022-04-18T05:09:36.017' AS DateTime), 1)
GO
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (9, N'manager@123', N'Sandhya Rani', N'sales.s1@vrmgroup.in', 1, CAST(N'2022-04-01T00:00:00.000' AS DateTime), CAST(N'2022-04-18T00:00:00.000' AS DateTime), N'VRMS12', N'No 44, Secretariat Officers Colony, Madhavaram', N'Bharat Petrol Bunk', N'Tamil Nadu', N'Chennai', N'600060', 1, 4000000, CAST(N'2022-04-18T06:52:05.853' AS DateTime), 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Clients] ADD  CONSTRAINT [DF_ClientDetails_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ClientContacts]  WITH CHECK ADD  CONSTRAINT [FK_ClientContacts_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[ClientContacts] CHECK CONSTRAINT [FK_ClientContacts_Clients]
GO
ALTER TABLE [dbo].[ClientContacts]  WITH CHECK ADD  CONSTRAINT [FK_ClientContacts_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([Id])
GO
ALTER TABLE [dbo].[ClientContacts] CHECK CONSTRAINT [FK_ClientContacts_Contact]
GO
ALTER TABLE [dbo].[ClientContacts]  WITH CHECK ADD  CONSTRAINT [FK_ClientContacts_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ClientContacts] CHECK CONSTRAINT [FK_ClientContacts_Users]
GO
ALTER TABLE [dbo].[ClientContacts]  WITH CHECK ADD  CONSTRAINT [FK_ClientContacts_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ClientContacts] CHECK CONSTRAINT [FK_ClientContacts_Users1]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_Users]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_Users1]
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_Users]
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_Users1]
GO
ALTER TABLE [dbo].[ProjectClient]  WITH CHECK ADD  CONSTRAINT [FK_ProjectClient_ClientDetails] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[ProjectClient] CHECK CONSTRAINT [FK_ProjectClient_ClientDetails]
GO
ALTER TABLE [dbo].[ProjectClient]  WITH CHECK ADD  CONSTRAINT [FK_ProjectClient_ProjectDetails] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[ProjectClient] CHECK CONSTRAINT [FK_ProjectClient_ProjectDetails]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_ProjectDetails_ProjectStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[ProjectStatus] ([Id])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_ProjectDetails_ProjectStatus]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_ProjectDetails_UserDetails] FOREIGN KEY([ProjectLead])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_ProjectDetails_UserDetails]
GO
ALTER TABLE [dbo].[RepresentativeSaleTarget]  WITH CHECK ADD  CONSTRAINT [FK_RepresentativeSaleTarget_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[RepresentativeSaleTarget] CHECK CONSTRAINT [FK_RepresentativeSaleTarget_Users]
GO
ALTER TABLE [dbo].[RepresentativeSaleTarget]  WITH CHECK ADD  CONSTRAINT [FK_RepresentativeSaleTarget_Users1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[RepresentativeSaleTarget] CHECK CONSTRAINT [FK_RepresentativeSaleTarget_Users1]
GO
ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_Roles_Users]
GO
ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_Roles_Users1]
GO
ALTER TABLE [dbo].[SaleActivity]  WITH CHECK ADD  CONSTRAINT [FK_SaleActivity_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[SaleActivity] CHECK CONSTRAINT [FK_SaleActivity_Products]
GO
ALTER TABLE [dbo].[SaleActivity]  WITH CHECK ADD  CONSTRAINT [FK_SaleActivity_SalesStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[SalesStatus] ([Id])
GO
ALTER TABLE [dbo].[SaleActivity] CHECK CONSTRAINT [FK_SaleActivity_SalesStatus]
GO
ALTER TABLE [dbo].[SaleActivity]  WITH CHECK ADD  CONSTRAINT [FK_SaleActivity_Users] FOREIGN KEY([SalesRepresentativeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SaleActivity] CHECK CONSTRAINT [FK_SaleActivity_Users]
GO
ALTER TABLE [dbo].[SaleActivity]  WITH CHECK ADD  CONSTRAINT [FK_SaleActivity_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SaleActivity] CHECK CONSTRAINT [FK_SaleActivity_Users1]
GO
ALTER TABLE [dbo].[SaleActivity]  WITH CHECK ADD  CONSTRAINT [FK_SaleActivity_Users2] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SaleActivity] CHECK CONSTRAINT [FK_SaleActivity_Users2]
GO
ALTER TABLE [dbo].[SaleProducts]  WITH CHECK ADD  CONSTRAINT [FK_SaleProducts_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[SaleProducts] CHECK CONSTRAINT [FK_SaleProducts_Products]
GO
ALTER TABLE [dbo].[SaleProducts]  WITH CHECK ADD  CONSTRAINT [FK_SaleProducts_Sales] FOREIGN KEY([SaleId])
REFERENCES [dbo].[Sales] ([Id])
GO
ALTER TABLE [dbo].[SaleProducts] CHECK CONSTRAINT [FK_SaleProducts_Sales]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_SaleDetails_SalesStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[SalesStatus] ([Id])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_SaleDetails_SalesStatus]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_SaleDetails_UserDetails] FOREIGN KEY([RepresentativeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_SaleDetails_UserDetails]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Users]
GO
ALTER TABLE [dbo].[UserContacts]  WITH CHECK ADD  CONSTRAINT [FK_UserContacts_ContactDetails] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserContacts] CHECK CONSTRAINT [FK_UserContacts_ContactDetails]
GO
ALTER TABLE [dbo].[UserContacts]  WITH CHECK ADD  CONSTRAINT [FK_UserContacts_UserDetails] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserContacts] CHECK CONSTRAINT [FK_UserContacts_UserDetails]
GO
ALTER TABLE [dbo].[UserContacts]  WITH CHECK ADD  CONSTRAINT [FK_UserContacts_Users] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserContacts] CHECK CONSTRAINT [FK_UserContacts_Users]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles1] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles1]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users1]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users2] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users2]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users1] FOREIGN KEY([ReportingManager])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users1]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users2] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users2]
GO
/****** Object:  StoredProcedure [dbo].[GetMonthlySalesReport]    Script Date: 21-04-2022 00:05:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [dbo].[GetMonthlySalesReport] 'admin', 1,2

CREATE PROCEDURE [dbo].[GetMonthlySalesReport]
(
	@UserType nvarchar(50) = 'Admin',
	@CurrentUserId int = 1,
	@UserManagerId int = 1
)
AS
Begin
  
 -- declare @createdBy  nvarchar(max)

 -- if(@UserType = 'Admin')
	--Set @createdBy = ('select createdby from [dbo].[SaleActivity]')
	--					--  STUFF((SELECT distinct ', ' + convert(nvarchar,t1.CreatedBy)
	--					--		 from [dbo].[SaleActivity] t1
	--					--		 where t.CreatedBy = t1.CreatedBy
	--					--			FOR XML PATH(''), TYPE
	--					--			).value('.', 'NVARCHAR(MAX)'),1,2,'') CreatedBy
	--					--from  [dbo].[SaleActivity] t)

 -- else if (@UserType = 'Manager')
	-- Set @createdBy =  @CurrentUserId + ',' + @UserManagerId 
 -- else if (@UserType = 'SalesRep')
	--  Set @createdBy = ''+ @CurrentUserId + ''

 --print @createdBy
	  
  SELECT month([SaleDate]) as mname, sum(NoOfFollowUps) as calls
  Into #temp1
  FROM [dbo].[SaleActivity] 
  group by month([SaleDate]) 
  
  SELECT month([SaleDate]) as mname, Count([status]) Orders
  Into #temp2
  FROM [dbo].[SaleActivity] where [status] = 6 
  group by month([SaleDate]) 

  SELECT month([SaleDate]) as mname, Count([status]) Cancels
  Into #temp3
  FROM [dbo].[SaleActivity] where [status] = 4 
  group by month([SaleDate])  

  select t1.mname, calls, isnull(t2.Orders,0) as Orders, isnull(t3.Cancels,0) as Cancels  from #temp1 t1 left outer join #temp2 t2 on t1.mname = t2.mname
  left outer join #temp3 t3 on t1.mname = t3.mname

  IF OBJECT_ID('tempdb..#temp1') IS NOT NULL 
  DROP TABLE #temp1; 

  IF OBJECT_ID('tempdb..#temp2') IS NOT NULL 
  DROP TABLE #temp2; 

  IF OBJECT_ID('tempdb..#temp3') IS NOT NULL 
  DROP TABLE #temp3; 

  End


  
GO
