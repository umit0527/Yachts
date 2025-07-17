<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="NewsCategory-B.aspx.cs" Inherits="Yachts.BackEnd.NewsCategory_B" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
            <asp:Button ID="btnAddNewsCategory" runat="server" Text="新增種類" 
            OnClick="btnAddNewsCategory_Click" CssClass="btn btn-warning"/>
    <div class="card">
<asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
    <ItemTemplate>
        <div style="border: 1px solid #ccc; margin: 5px; padding: 10px;">    
            <p>種類名稱：<%# Eval("Name") %></p>
            <p>建立時間：<%# Eval("CreatedAt") %></p>
            <p>修改時間：<%# Eval("UpdatedAt") %></p>
            <a href='EditNewsCategory-B.aspx?id=<%# Eval("Id") %>'>編輯</a> |
            <asp:LinkButton ID="btnDelete" runat="server" 
                CommandName="Delete" 
                CommandArgument='<%# Eval("Id") %>'
                OnClientClick="return confirm('確定要刪除嗎？');">刪除</asp:LinkButton>
        </div>
    </ItemTemplate>
</asp:Repeater>
    </div>
</asp:Content>
