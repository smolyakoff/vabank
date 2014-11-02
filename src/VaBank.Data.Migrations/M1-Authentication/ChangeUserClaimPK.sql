EXECUTE sp_rename N'Membership.PK_UserClaim', N'Membership.PK_UserClaim_old', 'OBJECT'
EXECUTE sp_rename N'Membership.FK_UserClaim_To_User', N'[Membership.FK_UserClaim_To_User_old', 'OBJECT'

CREATE TABLE [Membership].[Temp_UserClaim](
[UserID] uniqueidentifier NOT NULL,
[Type] varchar(100) NOT NULL,
[Value] nvarchar(512) NOT NULL

CONSTRAINT [PK_UserClaim] PRIMARY KEY CLUSTERED ([UserID], [Type], [Value]),
CONSTRAINT [FK_UserClaim_To_User] FOREIGN KEY ([UserID]) REFERENCES [Membership].[User] ([UserID])
)

INSERT INTO [Membership].[Temp_UserClaim] ([UserId], [Type], [Value])
SELECT [UserId], [Type], [Value] FROM [Membership].[UserClaim]

DROP TABLE [Membership].[UserClaim]
EXECUTE sp_rename N'Membership.Temp_UserClaim', N'UserClaim', 'OBJECT'
