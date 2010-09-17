alter table TicketAttachments ADD IsPending bit not null CONSTRAINT TicketAttachments_IsPending DEFAULT 0 WITH VALUES 
go
alter table TicketAttachments  DROP CONSTRAINT PK_TicketAttachments 
go
alter table TicketAttachments ALTER COLUMN TicketId int null
go
alter table TicketAttachments ADD CONSTRAINT PK_TicketAttachments PRIMARY KEY CLUSTERED (FileID)
go
alter table TicketAttachments DROP CONSTRAINT [FK_TicketAttachments_Tickets]
go
ALTER TABLE [TicketAttachments]  WITH NOCHECK ADD CONSTRAINT [FK_TicketAttachments_Tickets] FOREIGN KEY([TicketId])
REFERENCES [Tickets] ([TicketId])
go