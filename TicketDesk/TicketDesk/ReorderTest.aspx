<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReorderTest.aspx.cs" Inherits="TicketDesk.ReorderTest" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <div>
        <ajaxToolkit:ReorderList ID="OrderList" runat="server" AllowReorder="true"
            LayoutType="Table" OnItemReorder="OrderList_ItemReorder" PostBackOnReorder="true">
            <ItemTemplate>
                <span style="cursor: pointer;">
                    <asp:Label ID="ItemName" runat="server" Text='<%# Container.DataItem %>' />
                </span>
            </ItemTemplate>
        </ajaxToolkit:ReorderList>
        <asp:Label ID="Message" runat="server" />
        <asp:Button ID="SaveOrder" runat="server" Text="Save" OnClick="SaveOrder_Click" />
    </div>
    </form>
</body>
</html>
