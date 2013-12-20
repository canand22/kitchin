--/****** Object:  Table [dbo].[Role]    Script Date: 09/14/2012 17:19:21 ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--CREATE TABLE [dbo].[Role](
--	[Id] [bigint] IDENTITY(1,1) NOT NULL,
--	[Name] [nvarchar](255) NULL,
--PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
--) ON [PRIMARY]

--GO

--CREATE TABLE [dbo].[User](
--	[Id] [bigint] IDENTITY(1,1) NOT NULL,
--	[Login] [nvarchar](255) NULL,
--	[Password] [uniqueidentifier] NULL,
--	[Email] [nvarchar](255) NULL,
--PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
--) ON [PRIMARY]

--GO

--CREATE TABLE [dbo].[RoleUser](
--	[Role_id] [bigint] NOT NULL,
--	[User_id] [bigint] NOT NULL,
--PRIMARY KEY CLUSTERED 
--(
--	[User_id] ASC,
--	[Role_id] ASC
--)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
--) ON [PRIMARY]

--GO

--ALTER TABLE [dbo].[RoleUser]  WITH CHECK ADD  CONSTRAINT [FK1D67E2AC2735B34F] FOREIGN KEY([User_id])
--REFERENCES [dbo].[User] ([Id])
--GO

--ALTER TABLE [dbo].[RoleUser] CHECK CONSTRAINT [FK1D67E2AC2735B34F]
--GO

--ALTER TABLE [dbo].[RoleUser]  WITH CHECK ADD  CONSTRAINT [FK1D67E2AC95B26C1B] FOREIGN KEY([Role_id])
--REFERENCES [dbo].[Role] ([Id])
--GO

--ALTER TABLE [dbo].[RoleUser] CHECK CONSTRAINT [FK1D67E2AC95B26C1B]
--GO