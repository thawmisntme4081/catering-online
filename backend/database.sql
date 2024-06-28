DROP DATABASE OnlineCateringDB;
CREATE DATABASE OnlineCateringDB;
USE OnlineCateringDB;
---------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Users]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[Type] int NOT NULL,
	[Email] nvarchar(255) UNIQUE NOT NULL,
	[Password] nvarchar(255) NOT NULL,
	[CreatedAt] datetime2 NOT NULL,
	[UpdatedAt] datetime2 NOT NULL,
	CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
---------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Profiles]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[UserId] int NOT NULL,
	[FirstName] nvarchar(255) NOT NULL,
	[LastName] nvarchar(255) NOT NULL,
	[PhoneNumber] nvarchar(16) NOT NULL,
	[Address] nvarchar(255) NOT NULL,
	[Image] nvarchar(1000) NOT NULL,
	CONSTRAINT [PK_Profiles] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Profiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);
---------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Caterers]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[ProfileId] int NOT NULL,
	CONSTRAINT [PK_Caterers] PRIMARY KEY ([Id]), 
	CONSTRAINT [FK_Caterers_Profiles_ProfileId] FOREIGN KEY ([ProfileId]) REFERENCES [dbo].[Profiles] ([Id]) ON DELETE CASCADE
);
---------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Messages]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[SenderId] int NOT NULL,
	[ReceiverId] int NOT NULL,
	[Content] text NOT NULL,
	[CreatedAt] datetime2 NOT NULL,
	[UpdatedAt] datetime2 NOT NULL,
	CONSTRAINT [PK_Messages] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Messages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_Messages_Users_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [dbo].[Users] ([Id]),
);
---------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[FavoriteList]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[UserId] int NOT NULL,
	[CatererId] int NOT NULL,
	[CreatedAt] datetime2 NOT NULL,
	[UpdatedAt] datetime2 NOT NULL,
	CONSTRAINT [PK_FavoriteList] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_FavoriteList_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_FavoriteList_Caterers_CatererId] FOREIGN KEY ([CatererId]) REFERENCES [dbo].[Caterers] ([Id]),
);
---------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[CuisineTypes]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[CuisineName] nvarchar(255) UNIQUE NOT NULL,
	[CreatedAt] datetime2 NOT NULL,
	[UpdatedAt] datetime2 NOT NULL,
	CONSTRAINT [PK_CuisineTypes] PRIMARY KEY ([Id])
);
---------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Items]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(255) UNIQUE NOT NULL,
	[Image] nvarchar(1000) NOT NULL,
	[ServesCount] int NOT NULL,
	[Price] decimal(18, 2) NOT NULL,
	[CatererId] int NOT NULL,
	[CuisineId] int NOT NULL,
	[CreatedAt] datetime2 NOT NULL,
	[UpdatedAt] datetime2 NOT NULL,
	CONSTRAINT [PK_Items] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Items_Caterers_CatererId] FOREIGN KEY ([CatererId]) REFERENCES [dbo].[Caterers] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_Items_CuisineTypes_CuisineId] FOREIGN KEY ([CuisineId]) REFERENCES [dbo].[CuisineTypes] ([Id]) ON DELETE CASCADE
);
---------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Bookings]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[CustomerId] int NOT NULL,
	[CatererId] int NOT NULL,
	[BookingDate] date NOT NULL,
	[EventDate] date NOT NULL,
	[Venue] nvarchar(255) NOT NULL,
	[TotalAmount] decimal(18, 2) NOT NULL,
	[Note] nvarchar(255) NOT NULL,
	[Status] nvarchar(20) DEFAULT 'Pending' NOT NULL,
	[PaymentMethod] int NOT NULL,
	[CreatedAt] datetime2 NOT NULL,
	[UpdatedAt] datetime2 NOT NULL
	CONSTRAINT [PK_Bookings] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Bookings_Users_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_Bookings_Caterers_CatererId] FOREIGN KEY ([CatererId]) REFERENCES [dbo].[Caterers] ([Id])
);
