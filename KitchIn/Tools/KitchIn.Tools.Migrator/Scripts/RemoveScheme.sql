
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK1D67E2AC2735B34F]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleUsers]'))
ALTER TABLE [dbo].[RoleUsers] DROP CONSTRAINT [FK1D67E2AC2735B34F]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK1D67E2AC95B26C1B]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleUsers]'))
ALTER TABLE [dbo].[RoleUsers] DROP CONSTRAINT [FK1D67E2AC95B26C1B]
GO

USE [KitchIn]
GO

/****** Object:  Table [dbo].[RoleUsers]    Script Date: 09/14/2012 17:21:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleUsers]') AND type in (N'U'))
DROP TABLE [dbo].[RoleUsers]
GO


/****** Object:  Table [dbo].[Role]    Script Date: 09/14/2012 17:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND type in (N'U'))
DROP TABLE [dbo].[Role]
GO

/****** Object:  Table [dbo].[User]    Script Date: 09/14/2012 17:21:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO