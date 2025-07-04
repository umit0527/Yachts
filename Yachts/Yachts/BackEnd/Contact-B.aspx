<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="Contact-B.aspx.cs" Inherits="Yachts.BackEnd.Contact_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="姓名" SortExpression="Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="Phone" HeaderText="電話" SortExpression="Phone" />
            <asp:BoundField DataField="CountryName" HeaderText="國家" SortExpression="CountryName" />
            <asp:BoundField DataField="DisplayName" HeaderText="船型" SortExpression="DisplayName" />
            <asp:BoundField DataField="Comments" HeaderText="內容" SortExpression="Comments" />
            <asp:BoundField DataField="SendedAt" HeaderText="寄出時間" SortExpression="SendedAt" />
        </Columns>
    </asp:GridView>
</asp:Content>
