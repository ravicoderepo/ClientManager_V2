
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/29/2022 08:24:46
-- Generated from EDMX file: C:\Users\kanim\source\repos\ClientManager_V2\DBOperation\DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ph17179099821_ClientManager];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ClientContacts_Clients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientContacts] DROP CONSTRAINT [FK_ClientContacts_Clients];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientContacts_Contact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientContacts] DROP CONSTRAINT [FK_ClientContacts_Contact];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientContacts_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientContacts] DROP CONSTRAINT [FK_ClientContacts_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientContacts_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientContacts] DROP CONSTRAINT [FK_ClientContacts_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_Clients_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Clients] DROP CONSTRAINT [FK_Clients_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Clients_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Clients] DROP CONSTRAINT [FK_Clients_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_Contact_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contact] DROP CONSTRAINT [FK_Contact_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Contact_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contact] DROP CONSTRAINT [FK_Contact_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentManagement_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentManagement] DROP CONSTRAINT [FK_DocumentManagement_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentManagement_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentManagement] DROP CONSTRAINT [FK_DocumentManagement_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpenceCategory_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExpenceCategory] DROP CONSTRAINT [FK_ExpenceCategory_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpenceCategory_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExpenceCategory] DROP CONSTRAINT [FK_ExpenceCategory_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpenceTracker_ExpenceCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExpenseTracker] DROP CONSTRAINT [FK_ExpenceTracker_ExpenceCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpenceTracker_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExpenseTracker] DROP CONSTRAINT [FK_ExpenceTracker_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpenseTracker_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExpenseTracker] DROP CONSTRAINT [FK_ExpenseTracker_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_PettyCash_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PettyCash] DROP CONSTRAINT [FK_PettyCash_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_PettyCash_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PettyCash] DROP CONSTRAINT [FK_PettyCash_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectClient_ClientDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectClient] DROP CONSTRAINT [FK_ProjectClient_ClientDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectClient_ProjectDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectClient] DROP CONSTRAINT [FK_ProjectClient_ProjectDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectDetails_ProjectStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_ProjectDetails_ProjectStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectDetails_UserDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_ProjectDetails_UserDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_RepresentativeSaleTarget_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RepresentativeSaleTarget] DROP CONSTRAINT [FK_RepresentativeSaleTarget_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_RepresentativeSaleTarget_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RepresentativeSaleTarget] DROP CONSTRAINT [FK_RepresentativeSaleTarget_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_Roles_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [FK_Roles_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Roles_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [FK_Roles_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleActivity_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleActivity] DROP CONSTRAINT [FK_SaleActivity_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleActivity_SalesStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleActivity] DROP CONSTRAINT [FK_SaleActivity_SalesStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleActivity_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleActivity] DROP CONSTRAINT [FK_SaleActivity_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleActivity_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleActivity] DROP CONSTRAINT [FK_SaleActivity_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleActivity_Users2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleActivity] DROP CONSTRAINT [FK_SaleActivity_Users2];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleDetails_SalesStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sales] DROP CONSTRAINT [FK_SaleDetails_SalesStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleDetails_UserDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sales] DROP CONSTRAINT [FK_SaleDetails_UserDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleProducts_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleProducts] DROP CONSTRAINT [FK_SaleProducts_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleProducts_Sales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleProducts] DROP CONSTRAINT [FK_SaleProducts_Sales];
GO
IF OBJECT_ID(N'[dbo].[FK_Sales_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sales] DROP CONSTRAINT [FK_Sales_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_UserContacts_ContactDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserContacts] DROP CONSTRAINT [FK_UserContacts_ContactDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_UserContacts_UserDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserContacts] DROP CONSTRAINT [FK_UserContacts_UserDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_UserContacts_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserContacts] DROP CONSTRAINT [FK_UserContacts_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoles_Roles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Roles];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoles_Roles1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Roles1];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoles_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoles_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoles_Users2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Users2];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Users2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Users2];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ClientContacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientContacts];
GO
IF OBJECT_ID(N'[dbo].[Clients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Clients];
GO
IF OBJECT_ID(N'[dbo].[Contact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contact];
GO
IF OBJECT_ID(N'[dbo].[DocumentManagement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentManagement];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[ExpenceCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpenceCategory];
GO
IF OBJECT_ID(N'[dbo].[ExpenseTracker]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpenseTracker];
GO
IF OBJECT_ID(N'[dbo].[PettyCash]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PettyCash];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[ProjectClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectClient];
GO
IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[ProjectStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectStatus];
GO
IF OBJECT_ID(N'[dbo].[RepresentativeSaleTarget]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RepresentativeSaleTarget];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[SaleActivity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SaleActivity];
GO
IF OBJECT_ID(N'[dbo].[SaleProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SaleProducts];
GO
IF OBJECT_ID(N'[dbo].[Sales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sales];
GO
IF OBJECT_ID(N'[dbo].[SalesStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesStatus];
GO
IF OBJECT_ID(N'[dbo].[UserContacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserContacts];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ClientContacts'
CREATE TABLE [dbo].[ClientContacts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] int  NOT NULL,
    [ContactId] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'Clients'
CREATE TABLE [dbo].[Clients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] nvarchar(50)  NOT NULL,
    [ClientName] nvarchar(50)  NOT NULL,
    [ContactPersonName] nvarchar(50)  NOT NULL,
    [IsActive] bit  NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'Contacts'
CREATE TABLE [dbo].[Contacts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContactType] int  NOT NULL,
    [ContactPersonName] nvarchar(50)  NOT NULL,
    [Designation] nvarchar(50)  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [AddressLine1] nvarchar(100)  NOT NULL,
    [AddressLine2] nvarchar(100)  NULL,
    [State] nvarchar(50)  NOT NULL,
    [City] nvarchar(50)  NOT NULL,
    [Pincode] nvarchar(10)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Website] nvarchar(500)  NULL,
    [WorkPhoneNo] nvarchar(50)  NULL,
    [PersonalPhoneNo] nvarchar(50)  NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'DocumentManagements'
CREATE TABLE [dbo].[DocumentManagements] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DocumentSource] nvarchar(50)  NOT NULL,
    [ReferenceRecId] int  NULL,
    [DocumentName] nvarchar(100)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [FileName] nvarchar(100)  NOT NULL,
    [FileExtension] nvarchar(10)  NOT NULL,
    [FileData] nvarchar(max)  NOT NULL,
    [URL] nvarchar(500)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedBy] int  NULL,
    [ModifiedOn] datetime  NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int  NOT NULL,
    [FirstName] nvarchar(100)  NULL,
    [LastName] nvarchar(100)  NULL,
    [Email] nvarchar(100)  NULL,
    [IsActive] bit  NULL,
    [DateOfBirth] datetime  NULL,
    [DateOfJoining] datetime  NULL,
    [EmployeeId] nvarchar(50)  NULL,
    [AddressLine1] nvarchar(100)  NULL,
    [AddressLine2] nvarchar(100)  NULL,
    [State] nvarchar(50)  NULL,
    [City] nvarchar(50)  NULL,
    [Pincode] nvarchar(50)  NULL,
    [ReportingManager] int  NULL,
    [CreatedOn] datetime  NULL,
    [CreatedBy] int  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'ExpenceCategories'
CREATE TABLE [dbo].[ExpenceCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(100)  NOT NULL,
    [Description] nvarchar(500)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'PettyCashes'
CREATE TABLE [dbo].[PettyCashes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AmountReceived] decimal(19,4)  NOT NULL,
    [AmountRecivedDate] datetime  NOT NULL,
    [ModeOfPayment] nvarchar(200)  NOT NULL,
    [Status] nvarchar(50)  NULL,
    [Description] nvarchar(500)  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedBy] int  NULL,
    [ModifiedOn] datetime  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductName] nvarchar(50)  NOT NULL,
    [ProductCode] nvarchar(50)  NOT NULL,
    [ProductImage] nvarchar(max)  NULL,
    [Description] nvarchar(100)  NULL,
    [UnitPrice] decimal(18,0)  NOT NULL,
    [PurchasedDate] datetime  NOT NULL,
    [ExpiredOn] datetime  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'ProjectClients'
CREATE TABLE [dbo].[ProjectClients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectId] int  NULL,
    [ClientId] int  NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectId] nvarchar(50)  NOT NULL,
    [ProjectName] nvarchar(100)  NOT NULL,
    [Description] nvarchar(1000)  NOT NULL,
    [Status] int  NULL,
    [ClientCompany] int  NOT NULL,
    [ProjectLead] int  NOT NULL,
    [EstimatedBudget] decimal(18,0)  NOT NULL,
    [TotalAmountSpent] decimal(18,0)  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EstimatedEndDate] datetime  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'ProjectStatus'
CREATE TABLE [dbo].[ProjectStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StatusName] nvarchar(50)  NOT NULL,
    [Description] nvarchar(500)  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'RepresentativeSaleTargets'
CREATE TABLE [dbo].[RepresentativeSaleTargets] (
    [Id] int  NOT NULL,
    [UserId] int  NOT NULL,
    [SalesTarget] int  NOT NULL,
    [CallTarget] int  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [CompletedCount] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(50)  NOT NULL,
    [IsActive] bit  NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'SaleActivities'
CREATE TABLE [dbo].[SaleActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SaleDate] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [ClientName] nvarchar(100)  NULL,
    [ClientEmail] nvarchar(100)  NULL,
    [ClientPhoneNo] nvarchar(50)  NOT NULL,
    [ProductId] int  NULL,
    [ProductName] nvarchar(100)  NULL,
    [Capacity] nvarchar(500)  NULL,
    [Unit] nvarchar(50)  NULL,
    [RecentCallDate] datetime  NOT NULL,
    [AnticipatedClosingDate] datetime  NULL,
    [NoOfFollowUps] int  NULL,
    [Remarks] nvarchar(max)  NOT NULL,
    [SalesRepresentativeId] int  NOT NULL,
    [InvoiceNo] nvarchar(50)  NULL,
    [InvoiceAmount] decimal(18,0)  NULL,
    [DateOfClosing] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'SaleProducts'
CREATE TABLE [dbo].[SaleProducts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SaleId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [Unit] nvarchar(50)  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'Sales'
CREATE TABLE [dbo].[Sales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SaleDate] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [AnticipatedClosing] nvarchar(100)  NOT NULL,
    [NoOfFollowUps] int  NOT NULL,
    [NextFollowUpDate] datetime  NULL,
    [RepresentativeId] int  NOT NULL,
    [Remarks] nvarchar(1000)  NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'SalesStatus'
CREATE TABLE [dbo].[SalesStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(50)  NULL,
    [Description] nvarchar(100)  NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] nvarchar(50)  NULL
);
GO

-- Creating table 'UserContacts'
CREATE TABLE [dbo].[UserContacts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [ContactId] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [FullName] nvarchar(100)  NOT NULL,
    [Email] nvarchar(100)  NOT NULL,
    [IsActive] bit  NULL,
    [DateOfBirth] datetime  NULL,
    [DateOfJoining] datetime  NULL,
    [EmployeeId] nvarchar(50)  NULL,
    [AddressLine1] nvarchar(100)  NULL,
    [AddressLine2] nvarchar(100)  NULL,
    [State] nvarchar(50)  NULL,
    [City] nvarchar(50)  NULL,
    [Pincode] nvarchar(50)  NULL,
    [ReportingManager] int  NULL,
    [SaleTarget] int  NULL,
    [CreatedOn] datetime  NULL,
    [CreatedBy] int  NULL,
    [ModifiedOn] datetime  NULL,
    [ModifiedBy] int  NULL
);
GO

-- Creating table 'ExpenseTrackers'
CREATE TABLE [dbo].[ExpenseTrackers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ExpenseAmount] decimal(19,4)  NOT NULL,
    [ExpenseDate] datetime  NOT NULL,
    [ExpenseCategoryId] int  NOT NULL,
    [Description] nvarchar(200)  NOT NULL,
    [Status] nvarchar(50)  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedBy] int  NULL,
    [ModifiedOn] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ClientContacts'
ALTER TABLE [dbo].[ClientContacts]
ADD CONSTRAINT [PK_ClientContacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Clients'
ALTER TABLE [dbo].[Clients]
ADD CONSTRAINT [PK_Clients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [PK_Contacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DocumentManagements'
ALTER TABLE [dbo].[DocumentManagements]
ADD CONSTRAINT [PK_DocumentManagements]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpenceCategories'
ALTER TABLE [dbo].[ExpenceCategories]
ADD CONSTRAINT [PK_ExpenceCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PettyCashes'
ALTER TABLE [dbo].[PettyCashes]
ADD CONSTRAINT [PK_PettyCashes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectClients'
ALTER TABLE [dbo].[ProjectClients]
ADD CONSTRAINT [PK_ProjectClients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectStatus'
ALTER TABLE [dbo].[ProjectStatus]
ADD CONSTRAINT [PK_ProjectStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RepresentativeSaleTargets'
ALTER TABLE [dbo].[RepresentativeSaleTargets]
ADD CONSTRAINT [PK_RepresentativeSaleTargets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SaleActivities'
ALTER TABLE [dbo].[SaleActivities]
ADD CONSTRAINT [PK_SaleActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SaleProducts'
ALTER TABLE [dbo].[SaleProducts]
ADD CONSTRAINT [PK_SaleProducts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sales'
ALTER TABLE [dbo].[Sales]
ADD CONSTRAINT [PK_Sales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesStatus'
ALTER TABLE [dbo].[SalesStatus]
ADD CONSTRAINT [PK_SalesStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserContacts'
ALTER TABLE [dbo].[UserContacts]
ADD CONSTRAINT [PK_UserContacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpenseTrackers'
ALTER TABLE [dbo].[ExpenseTrackers]
ADD CONSTRAINT [PK_ExpenseTrackers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClientId] in table 'ClientContacts'
ALTER TABLE [dbo].[ClientContacts]
ADD CONSTRAINT [FK_ClientContacts_Clients]
    FOREIGN KEY ([ClientId])
    REFERENCES [dbo].[Clients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientContacts_Clients'
CREATE INDEX [IX_FK_ClientContacts_Clients]
ON [dbo].[ClientContacts]
    ([ClientId]);
GO

-- Creating foreign key on [ContactId] in table 'ClientContacts'
ALTER TABLE [dbo].[ClientContacts]
ADD CONSTRAINT [FK_ClientContacts_Contact]
    FOREIGN KEY ([ContactId])
    REFERENCES [dbo].[Contacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientContacts_Contact'
CREATE INDEX [IX_FK_ClientContacts_Contact]
ON [dbo].[ClientContacts]
    ([ContactId]);
GO

-- Creating foreign key on [CreatedBy] in table 'ClientContacts'
ALTER TABLE [dbo].[ClientContacts]
ADD CONSTRAINT [FK_ClientContacts_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientContacts_Users'
CREATE INDEX [IX_FK_ClientContacts_Users]
ON [dbo].[ClientContacts]
    ([CreatedBy]);
GO

-- Creating foreign key on [ModifiedBy] in table 'ClientContacts'
ALTER TABLE [dbo].[ClientContacts]
ADD CONSTRAINT [FK_ClientContacts_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientContacts_Users1'
CREATE INDEX [IX_FK_ClientContacts_Users1]
ON [dbo].[ClientContacts]
    ([ModifiedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'Clients'
ALTER TABLE [dbo].[Clients]
ADD CONSTRAINT [FK_Clients_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Clients_Users'
CREATE INDEX [IX_FK_Clients_Users]
ON [dbo].[Clients]
    ([CreatedBy]);
GO

-- Creating foreign key on [ModifiedBy] in table 'Clients'
ALTER TABLE [dbo].[Clients]
ADD CONSTRAINT [FK_Clients_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Clients_Users1'
CREATE INDEX [IX_FK_Clients_Users1]
ON [dbo].[Clients]
    ([ModifiedBy]);
GO

-- Creating foreign key on [ClientId] in table 'ProjectClients'
ALTER TABLE [dbo].[ProjectClients]
ADD CONSTRAINT [FK_ProjectClient_ClientDetails]
    FOREIGN KEY ([ClientId])
    REFERENCES [dbo].[Clients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectClient_ClientDetails'
CREATE INDEX [IX_FK_ProjectClient_ClientDetails]
ON [dbo].[ProjectClients]
    ([ClientId]);
GO

-- Creating foreign key on [CreatedBy] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [FK_Contact_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Contact_Users'
CREATE INDEX [IX_FK_Contact_Users]
ON [dbo].[Contacts]
    ([CreatedBy]);
GO

-- Creating foreign key on [ModifiedBy] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [FK_Contact_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Contact_Users1'
CREATE INDEX [IX_FK_Contact_Users1]
ON [dbo].[Contacts]
    ([ModifiedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'DocumentManagements'
ALTER TABLE [dbo].[DocumentManagements]
ADD CONSTRAINT [FK_DocumentManagement_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentManagement_Users'
CREATE INDEX [IX_FK_DocumentManagement_Users]
ON [dbo].[DocumentManagements]
    ([CreatedBy]);
GO

-- Creating foreign key on [ModifiedBy] in table 'DocumentManagements'
ALTER TABLE [dbo].[DocumentManagements]
ADD CONSTRAINT [FK_DocumentManagement_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentManagement_Users1'
CREATE INDEX [IX_FK_DocumentManagement_Users1]
ON [dbo].[DocumentManagements]
    ([ModifiedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'ExpenceCategories'
ALTER TABLE [dbo].[ExpenceCategories]
ADD CONSTRAINT [FK_ExpenceCategory_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpenceCategory_Users'
CREATE INDEX [IX_FK_ExpenceCategory_Users]
ON [dbo].[ExpenceCategories]
    ([CreatedBy]);
GO

-- Creating foreign key on [ModifiedBy] in table 'ExpenceCategories'
ALTER TABLE [dbo].[ExpenceCategories]
ADD CONSTRAINT [FK_ExpenceCategory_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpenceCategory_Users1'
CREATE INDEX [IX_FK_ExpenceCategory_Users1]
ON [dbo].[ExpenceCategories]
    ([ModifiedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'PettyCashes'
ALTER TABLE [dbo].[PettyCashes]
ADD CONSTRAINT [FK_PettyCash_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PettyCash_Users'
CREATE INDEX [IX_FK_PettyCash_Users]
ON [dbo].[PettyCashes]
    ([CreatedBy]);
GO

-- Creating foreign key on [ModifiedBy] in table 'PettyCashes'
ALTER TABLE [dbo].[PettyCashes]
ADD CONSTRAINT [FK_PettyCash_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PettyCash_Users1'
CREATE INDEX [IX_FK_PettyCash_Users1]
ON [dbo].[PettyCashes]
    ([ModifiedBy]);
GO

-- Creating foreign key on [ProductId] in table 'SaleActivities'
ALTER TABLE [dbo].[SaleActivities]
ADD CONSTRAINT [FK_SaleActivity_Products]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleActivity_Products'
CREATE INDEX [IX_FK_SaleActivity_Products]
ON [dbo].[SaleActivities]
    ([ProductId]);
GO

-- Creating foreign key on [ProductId] in table 'SaleProducts'
ALTER TABLE [dbo].[SaleProducts]
ADD CONSTRAINT [FK_SaleProducts_Products]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleProducts_Products'
CREATE INDEX [IX_FK_SaleProducts_Products]
ON [dbo].[SaleProducts]
    ([ProductId]);
GO

-- Creating foreign key on [ProjectId] in table 'ProjectClients'
ALTER TABLE [dbo].[ProjectClients]
ADD CONSTRAINT [FK_ProjectClient_ProjectDetails]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectClient_ProjectDetails'
CREATE INDEX [IX_FK_ProjectClient_ProjectDetails]
ON [dbo].[ProjectClients]
    ([ProjectId]);
GO

-- Creating foreign key on [Status] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_ProjectDetails_ProjectStatus]
    FOREIGN KEY ([Status])
    REFERENCES [dbo].[ProjectStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectDetails_ProjectStatus'
CREATE INDEX [IX_FK_ProjectDetails_ProjectStatus]
ON [dbo].[Projects]
    ([Status]);
GO

-- Creating foreign key on [ProjectLead] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_ProjectDetails_UserDetails]
    FOREIGN KEY ([ProjectLead])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectDetails_UserDetails'
CREATE INDEX [IX_FK_ProjectDetails_UserDetails]
ON [dbo].[Projects]
    ([ProjectLead]);
GO

-- Creating foreign key on [UserId] in table 'RepresentativeSaleTargets'
ALTER TABLE [dbo].[RepresentativeSaleTargets]
ADD CONSTRAINT [FK_RepresentativeSaleTarget_Users]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RepresentativeSaleTarget_Users'
CREATE INDEX [IX_FK_RepresentativeSaleTarget_Users]
ON [dbo].[RepresentativeSaleTargets]
    ([UserId]);
GO

-- Creating foreign key on [CreatedBy] in table 'RepresentativeSaleTargets'
ALTER TABLE [dbo].[RepresentativeSaleTargets]
ADD CONSTRAINT [FK_RepresentativeSaleTarget_Users1]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RepresentativeSaleTarget_Users1'
CREATE INDEX [IX_FK_RepresentativeSaleTarget_Users1]
ON [dbo].[RepresentativeSaleTargets]
    ([CreatedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [FK_Roles_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Roles_Users'
CREATE INDEX [IX_FK_Roles_Users]
ON [dbo].[Roles]
    ([CreatedBy]);
GO

-- Creating foreign key on [ModifiedBy] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [FK_Roles_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Roles_Users1'
CREATE INDEX [IX_FK_Roles_Users1]
ON [dbo].[Roles]
    ([ModifiedBy]);
GO

-- Creating foreign key on [RoleId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRoles_Roles1]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoles_Roles1'
CREATE INDEX [IX_FK_UserRoles_Roles1]
ON [dbo].[UserRoles]
    ([RoleId]);
GO

-- Creating foreign key on [Status] in table 'SaleActivities'
ALTER TABLE [dbo].[SaleActivities]
ADD CONSTRAINT [FK_SaleActivity_SalesStatus]
    FOREIGN KEY ([Status])
    REFERENCES [dbo].[SalesStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleActivity_SalesStatus'
CREATE INDEX [IX_FK_SaleActivity_SalesStatus]
ON [dbo].[SaleActivities]
    ([Status]);
GO

-- Creating foreign key on [SalesRepresentativeId] in table 'SaleActivities'
ALTER TABLE [dbo].[SaleActivities]
ADD CONSTRAINT [FK_SaleActivity_Users]
    FOREIGN KEY ([SalesRepresentativeId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleActivity_Users'
CREATE INDEX [IX_FK_SaleActivity_Users]
ON [dbo].[SaleActivities]
    ([SalesRepresentativeId]);
GO

-- Creating foreign key on [ModifiedBy] in table 'SaleActivities'
ALTER TABLE [dbo].[SaleActivities]
ADD CONSTRAINT [FK_SaleActivity_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleActivity_Users1'
CREATE INDEX [IX_FK_SaleActivity_Users1]
ON [dbo].[SaleActivities]
    ([ModifiedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'SaleActivities'
ALTER TABLE [dbo].[SaleActivities]
ADD CONSTRAINT [FK_SaleActivity_Users2]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleActivity_Users2'
CREATE INDEX [IX_FK_SaleActivity_Users2]
ON [dbo].[SaleActivities]
    ([CreatedBy]);
GO

-- Creating foreign key on [SaleId] in table 'SaleProducts'
ALTER TABLE [dbo].[SaleProducts]
ADD CONSTRAINT [FK_SaleProducts_Sales]
    FOREIGN KEY ([SaleId])
    REFERENCES [dbo].[Sales]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleProducts_Sales'
CREATE INDEX [IX_FK_SaleProducts_Sales]
ON [dbo].[SaleProducts]
    ([SaleId]);
GO

-- Creating foreign key on [Status] in table 'Sales'
ALTER TABLE [dbo].[Sales]
ADD CONSTRAINT [FK_SaleDetails_SalesStatus]
    FOREIGN KEY ([Status])
    REFERENCES [dbo].[SalesStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleDetails_SalesStatus'
CREATE INDEX [IX_FK_SaleDetails_SalesStatus]
ON [dbo].[Sales]
    ([Status]);
GO

-- Creating foreign key on [RepresentativeId] in table 'Sales'
ALTER TABLE [dbo].[Sales]
ADD CONSTRAINT [FK_SaleDetails_UserDetails]
    FOREIGN KEY ([RepresentativeId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleDetails_UserDetails'
CREATE INDEX [IX_FK_SaleDetails_UserDetails]
ON [dbo].[Sales]
    ([RepresentativeId]);
GO

-- Creating foreign key on [CreatedBy] in table 'Sales'
ALTER TABLE [dbo].[Sales]
ADD CONSTRAINT [FK_Sales_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Sales_Users'
CREATE INDEX [IX_FK_Sales_Users]
ON [dbo].[Sales]
    ([CreatedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'UserContacts'
ALTER TABLE [dbo].[UserContacts]
ADD CONSTRAINT [FK_UserContacts_ContactDetails]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserContacts_ContactDetails'
CREATE INDEX [IX_FK_UserContacts_ContactDetails]
ON [dbo].[UserContacts]
    ([CreatedBy]);
GO

-- Creating foreign key on [UserId] in table 'UserContacts'
ALTER TABLE [dbo].[UserContacts]
ADD CONSTRAINT [FK_UserContacts_UserDetails]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserContacts_UserDetails'
CREATE INDEX [IX_FK_UserContacts_UserDetails]
ON [dbo].[UserContacts]
    ([UserId]);
GO

-- Creating foreign key on [ModifiedBy] in table 'UserContacts'
ALTER TABLE [dbo].[UserContacts]
ADD CONSTRAINT [FK_UserContacts_Users]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserContacts_Users'
CREATE INDEX [IX_FK_UserContacts_Users]
ON [dbo].[UserContacts]
    ([ModifiedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRoles_Roles]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoles_Roles'
CREATE INDEX [IX_FK_UserRoles_Roles]
ON [dbo].[UserRoles]
    ([CreatedBy]);
GO

-- Creating foreign key on [UserId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRoles_Users]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoles_Users'
CREATE INDEX [IX_FK_UserRoles_Users]
ON [dbo].[UserRoles]
    ([UserId]);
GO

-- Creating foreign key on [ModifiedBy] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRoles_Users1]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoles_Users1'
CREATE INDEX [IX_FK_UserRoles_Users1]
ON [dbo].[UserRoles]
    ([ModifiedBy]);
GO

-- Creating foreign key on [UserId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRoles_Users2]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoles_Users2'
CREATE INDEX [IX_FK_UserRoles_Users2]
ON [dbo].[UserRoles]
    ([UserId]);
GO

-- Creating foreign key on [ModifiedBy] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_Users_Users]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Users_Users'
CREATE INDEX [IX_FK_Users_Users]
ON [dbo].[Users]
    ([ModifiedBy]);
GO

-- Creating foreign key on [ReportingManager] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_Users_Users1]
    FOREIGN KEY ([ReportingManager])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Users_Users1'
CREATE INDEX [IX_FK_Users_Users1]
ON [dbo].[Users]
    ([ReportingManager]);
GO

-- Creating foreign key on [CreatedBy] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_Users_Users2]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Users_Users2'
CREATE INDEX [IX_FK_Users_Users2]
ON [dbo].[Users]
    ([CreatedBy]);
GO

-- Creating foreign key on [ExpenseCategoryId] in table 'ExpenseTrackers'
ALTER TABLE [dbo].[ExpenseTrackers]
ADD CONSTRAINT [FK_ExpenceTracker_ExpenceCategory]
    FOREIGN KEY ([ExpenseCategoryId])
    REFERENCES [dbo].[ExpenceCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpenceTracker_ExpenceCategory'
CREATE INDEX [IX_FK_ExpenceTracker_ExpenceCategory]
ON [dbo].[ExpenseTrackers]
    ([ExpenseCategoryId]);
GO

-- Creating foreign key on [ModifiedBy] in table 'ExpenseTrackers'
ALTER TABLE [dbo].[ExpenseTrackers]
ADD CONSTRAINT [FK_ExpenceTracker_Users]
    FOREIGN KEY ([ModifiedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpenceTracker_Users'
CREATE INDEX [IX_FK_ExpenceTracker_Users]
ON [dbo].[ExpenseTrackers]
    ([ModifiedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'ExpenseTrackers'
ALTER TABLE [dbo].[ExpenseTrackers]
ADD CONSTRAINT [FK_ExpenseTracker_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpenseTracker_Users'
CREATE INDEX [IX_FK_ExpenseTracker_Users]
ON [dbo].[ExpenseTrackers]
    ([CreatedBy]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------