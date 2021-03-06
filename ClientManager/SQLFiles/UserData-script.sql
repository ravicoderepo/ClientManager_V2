USE [ClientManager_v2]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/25/2021 9:06:14 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'admin@123', N'Admin', N'admin@gmail.com', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-08-29T00:00:00.000' AS DateTime), 1, CAST(N'2021-09-23T22:46:33.597' AS DateTime), 1)
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'manager1@123', N'Velmurugan', N'manager1@gmail.com', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, CAST(N'2021-08-26T00:00:00.000' AS DateTime), 1, CAST(N'2021-09-22T13:06:34.470' AS DateTime), 1)
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'salesrep1@123', N'Rajaseakar', N'salesrep1@gmail.com', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, CAST(N'2021-08-26T00:00:00.000' AS DateTime), 1, CAST(N'2021-09-22T13:06:31.680' AS DateTime), 1)
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'manager2@123', N'Ravikumar', N'manager2@gmail.com', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, CAST(N'2021-08-26T18:07:22.950' AS DateTime), 1, CAST(N'2021-09-22T13:06:28.197' AS DateTime), 1)
INSERT [dbo].[Users] ([Id], [Password], [FullName], [Email], [IsActive], [DateOfBirth], [DateOfJoining], [EmployeeId], [AddressLine1], [AddressLine2], [State], [City], [Pincode], [ReportingManager], [SaleTarget], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'salesrep2@123', N'Michael', N'salesrep2@gmail.com', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, CAST(N'2021-08-26T18:13:57.833' AS DateTime), 1, CAST(N'2021-09-22T13:06:24.670' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
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
