

INSERT [dbo].[aspnet_Applications] ([ApplicationName], [LoweredApplicationName], [ApplicationId], [Description]) VALUES (N'TicketDesk', N'ticketdesk', N'3f646383-8861-472e-9840-d0cdabd4b762', NULL)



INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'1885ead0-5f3e-49ae-8450-2bf066b8333c', N'Administrators', N'administrators', NULL)
INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'abeaf947-dfa2-426e-bfcb-ca4d66e176e4', N'HelpDesk', N'helpdesk', NULL)
INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'b4403567-4c5e-457e-929c-6c2b96128102', N'TicketSubmitters', N'ticketsubmitters', NULL)


INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'86928a56-1766-4cc3-b76d-347f94265408', N'admin', N'admin', NULL, 0, CAST(0x00009E4301796A13 AS DateTime))
INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'25ea6439-5480-479a-9704-6a7ca9a1f5f2', N'otherstaffer', N'otherstaffer', NULL, 0, CAST(0x00009E430179D31C AS DateTime))
INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'b2c534e4-c1e1-4896-b4ba-20046d965ba6', N'toastman', N'toastman', NULL, 0, CAST(0x00009E430179AAE2 AS DateTime))


INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'86928a56-1766-4cc3-b76d-347f94265408', N'admin', 0, N'yNCpvO5CRF5PV2ZAspHSZw==', NULL, N'admin@nowhere.com', N'admin@nowhere.com', NULL, NULL, 1, 0, CAST(0x00009E430178494C AS DateTime), CAST(0x00009E43017941D6 AS DateTime), CAST(0x00009E430178494C AS DateTime), CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), N'Admin User')
INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'25ea6439-5480-479a-9704-6a7ca9a1f5f2', N'otherstaffer', 0, N'w42VUJOAnKgcTXn6n/B+7w==', NULL, N'otherstaffer@nowhere.com', N'otherstaffer@nowhere.com', NULL, NULL, 1, 0, CAST(0x00009E4301788C18 AS DateTime), CAST(0x00009E430179B875 AS DateTime), CAST(0x00009E4301788C18 AS DateTime), CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), N'Other Staffer')
INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'3f646383-8861-472e-9840-d0cdabd4b762', N'b2c534e4-c1e1-4896-b4ba-20046d965ba6', N'toastman', 0, N'IvoJVOLICBLMzG2zybwsPQ==', NULL, N'toastman@nowhere.com', N'toastman@nowhere.com', NULL, NULL, 1, 0, CAST(0x00009E4301786ECC AS DateTime), CAST(0x00009E43017978FB AS DateTime), CAST(0x00009E4301786ECC AS DateTime), CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), N'Toastman Wazisname')

INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'86928a56-1766-4cc3-b76d-347f94265408', N'1885ead0-5f3e-49ae-8450-2bf066b8333c')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'b2c534e4-c1e1-4896-b4ba-20046d965ba6', N'b4403567-4c5e-457e-929c-6c2b96128102')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'86928a56-1766-4cc3-b76d-347f94265408', N'b4403567-4c5e-457e-929c-6c2b96128102')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'25ea6439-5480-479a-9704-6a7ca9a1f5f2', N'b4403567-4c5e-457e-929c-6c2b96128102')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'86928a56-1766-4cc3-b76d-347f94265408', N'abeaf947-dfa2-426e-bfcb-ca4d66e176e4')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'25ea6439-5480-479a-9704-6a7ca9a1f5f2', N'abeaf947-dfa2-426e-bfcb-ca4d66e176e4')


INSERT [dbo].[aspnet_Profile] ([UserId], [PropertyNames], [PropertyValuesString], [PropertyValuesBinary], [LastUpdatedDate]) VALUES (N'b2c534e4-c1e1-4896-b4ba-20046d965ba6', N'UserDisplayPreferences:S:0:3414:OpenEditorWithPreview:S:3414:4:', N'<?xml version="1.0" encoding="utf-16"?>
<UserDisplayPreferences xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <TicketCenterListPreferences>
    <TicketCenterListSettings>
      <ListName>mytickets</ListName>
      <ListDisplayName>All My Tickets</ListDisplayName>
      <ListMenuDisplayOrder>0</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>Owner</ColumnName>
          <ColumnValue>toastman</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>Owner</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>opentickets</ListName>
      <ListDisplayName>All Open Tickets</ListDisplayName>
      <ListMenuDisplayOrder>1</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Ascending</SortDirection>
          <ColumnName>CurrentStatus</ColumnName>
        </TicketListSortColumn>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>CurrentStatus</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>historytickets</ListName>
      <ListDisplayName>Ticket History</ListDisplayName>
      <ListMenuDisplayOrder>2</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>Owner</ColumnName>
          <ColumnValue>toastman</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>CurrentStatus</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
  </TicketCenterListPreferences>
</UserDisplayPreferences>True', 0x, CAST(0x00009E430179AAE2 AS DateTime))
INSERT [dbo].[aspnet_Profile] ([UserId], [PropertyNames], [PropertyValuesString], [PropertyValuesBinary], [LastUpdatedDate]) VALUES (N'86928a56-1766-4cc3-b76d-347f94265408', N'UserDisplayPreferences:S:0:5679:OpenEditorWithPreview:S:5679:4:', N'<?xml version="1.0" encoding="utf-16"?>
<UserDisplayPreferences xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <TicketCenterListPreferences>
    <TicketCenterListSettings>
      <ListName>unassigned</ListName>
      <ListDisplayName>Unassigned Tickets</ListDisplayName>
      <ListMenuDisplayOrder>0</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison xsi:nil="true" />
          <ColumnName>AssignedTo</ColumnName>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>AssignedTo</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>assignedtome</ListName>
      <ListDisplayName>Tickets Assigned To Me</ListDisplayName>
      <ListMenuDisplayOrder>1</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Ascending</SortDirection>
          <ColumnName>CurrentStatus</ColumnName>
        </TicketListSortColumn>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>AssignedTo</ColumnName>
          <ColumnValue>admin</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>AssignedTo</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>mytickets</ListName>
      <ListDisplayName>All My Tickets</ListDisplayName>
      <ListMenuDisplayOrder>2</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>Owner</ColumnName>
          <ColumnValue>admin</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>Owner</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>opentickets</ListName>
      <ListDisplayName>All Open Tickets</ListDisplayName>
      <ListMenuDisplayOrder>3</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Ascending</SortDirection>
          <ColumnName>CurrentStatus</ColumnName>
        </TicketListSortColumn>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>CurrentStatus</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>historytickets</ListName>
      <ListDisplayName>Ticket History</ListDisplayName>
      <ListMenuDisplayOrder>4</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>AssignedTo</ColumnName>
          <ColumnValue>admin</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>CurrentStatus</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
  </TicketCenterListPreferences>
</UserDisplayPreferences>True', 0x, CAST(0x00009E4301796A13 AS DateTime))
INSERT [dbo].[aspnet_Profile] ([UserId], [PropertyNames], [PropertyValuesString], [PropertyValuesBinary], [LastUpdatedDate]) VALUES (N'25ea6439-5480-479a-9704-6a7ca9a1f5f2', N'UserDisplayPreferences:S:0:5700:OpenEditorWithPreview:S:5700:4:', N'<?xml version="1.0" encoding="utf-16"?>
<UserDisplayPreferences xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <TicketCenterListPreferences>
    <TicketCenterListSettings>
      <ListName>unassigned</ListName>
      <ListDisplayName>Unassigned Tickets</ListDisplayName>
      <ListMenuDisplayOrder>0</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison xsi:nil="true" />
          <ColumnName>AssignedTo</ColumnName>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>AssignedTo</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>assignedtome</ListName>
      <ListDisplayName>Tickets Assigned To Me</ListDisplayName>
      <ListMenuDisplayOrder>1</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Ascending</SortDirection>
          <ColumnName>CurrentStatus</ColumnName>
        </TicketListSortColumn>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>AssignedTo</ColumnName>
          <ColumnValue>otherstaffer</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>AssignedTo</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>mytickets</ListName>
      <ListDisplayName>All My Tickets</ListDisplayName>
      <ListMenuDisplayOrder>2</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>Owner</ColumnName>
          <ColumnValue>otherstaffer</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>Owner</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>opentickets</ListName>
      <ListDisplayName>All Open Tickets</ListDisplayName>
      <ListMenuDisplayOrder>3</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Ascending</SortDirection>
          <ColumnName>CurrentStatus</ColumnName>
        </TicketListSortColumn>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>false</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>CurrentStatus</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
    <TicketCenterListSettings>
      <ListName>historytickets</ListName>
      <ListDisplayName>Ticket History</ListDisplayName>
      <ListMenuDisplayOrder>4</ListMenuDisplayOrder>
      <ItemsPerPage>20</ItemsPerPage>
      <SortColumns>
        <TicketListSortColumn>
          <SortDirection>Descending</SortDirection>
          <ColumnName>LastUpdateDate</ColumnName>
        </TicketListSortColumn>
      </SortColumns>
      <FilterColumns>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>CurrentStatus</ColumnName>
          <ColumnValue>closed</ColumnValue>
        </TicketListFilterColumn>
        <TicketListFilterColumn>
          <UseEqualityComparison>true</UseEqualityComparison>
          <ColumnName>AssignedTo</ColumnName>
          <ColumnValue>otherstaffer</ColumnValue>
        </TicketListFilterColumn>
      </FilterColumns>
      <DisabledFilterColumNames>
        <string>CurrentStatus</string>
      </DisabledFilterColumNames>
    </TicketCenterListSettings>
  </TicketCenterListPreferences>
</UserDisplayPreferences>True', 0x, CAST(0x00009E430179D31C AS DateTime))
