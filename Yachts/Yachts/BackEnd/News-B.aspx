<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="News-B.aspx.cs" Inherits="Yachts.BackEnd.News_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="btnAddNews" runat="server" Text="新增新聞" OnClick="btnAddNews_Click" CssClass="btn btn-warning" />
    <div class="card">
        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
            <ItemTemplate>
                <div class="d-flex" style="border: 1px solid #ccc; margin: 5px; padding: 10px;">
                    封面：<img style="width:187px; height:121px;" src="<%# ResolveUrl("/Uploads/Photos/") + Eval("CoverPath") %>" alt="封面" />
                    <div class="ms-2" style="width:500px;">
                    <p>標題：<%# Eval("title") %></p>
                    <p>內容：<%# Eval("content") %></p>
                    <p>建立時間：<%# Eval("CreatedAt") %></p>
                    <p>修改時間：<%# Eval("UpdatedAt") %></p>
                    <a href='EditNews-B.aspx?id=<%# Eval("Id") %>'>編輯</a> |
            <asp:LinkButton ID="btnDelete" runat="server"
                CommandName="Delete"
                CommandArgument='<%# Eval("Id") %>'
                OnClientClick="return confirm('確定要刪除嗎？');">刪除</asp:LinkButton>
                </div>

                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
