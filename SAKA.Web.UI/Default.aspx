<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SAKA.Web.UI.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:GridView ID="SCORECARD" runat="server" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" GridLines="None">
            <Columns>
                <asp:BoundField HeaderText="Name" DataField="Name" ReadOnly="true" />
                 <asp:BoundField HeaderText="Date" DataField="Date" ReadOnly="true" />
                 <asp:BoundField HeaderText="Value" DataField="Value" ReadOnly="true" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Image ID="Status" runat="server" ImageUrl='<%# Eval("status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>

        <asp:PlaceHolder ID="P1" runat="server"></asp:PlaceHolder>
       
     <asp:PlaceHolder 
    </form>
</body>
</html>
