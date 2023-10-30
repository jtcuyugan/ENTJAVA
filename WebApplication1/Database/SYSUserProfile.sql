CREATE TABLE [dbo].[SYSUserProfile](
[SYSUserProfileID] [int] IDENTITY(1,1) NOT NULL,
[SYSUserID] [int] NOT NULL,
[AccountImage] [varchar](MAX),
[FirstName] [varchar](50) NOT NULL,
[LastName] [varchar](50) NOT NULL,
[Email] [varchar](150) NOT NULL,
[Address] [varchar](255) NOT NULL,
[PhoneNumber] [varchar](12) NOT NULL,
[Gender] [char](1) NOT NULL,
[RowCreatedSYSUserID] [int] NOT NULL,
[RowCreatedDateTime] [datetime] DEFAULT GETDATE(),
[RowModifiedSYSUserID] [int] NOT NULL,
[RowModifiedDateTime] [datetime] DEFAULT GETDATE(),
PRIMARY KEY (SYSUserProfileID)
)
GO
ALTER TABLE [dbo].[SYSUserProfile] WITH CHECK ADD FOREIGN KEY([SYSUserID])
REFERENCES [dbo].[SYSUser] ([SYSUserID])
GO

