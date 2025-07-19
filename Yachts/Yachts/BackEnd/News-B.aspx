<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="News-B.aspx.cs" Inherits="Yachts.BackEnd.News_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .news img{
            max-width: 100%;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="btnAddNews" runat="server" Text="新增消息" OnClick="btnAddNews_Click" CssClass="btn btn-warning" />
    <br />
    <br />
    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" OnItemDataBound="Repeater1_ItemDataBound">
        <ItemTemplate>
            <div class="card ">
                <div class="row mx-0 mb-2">
                    <div class="col-5 ">
                        <div class="d-flex">
                            <div>
                                置頂：<asp:CheckBox ID="chbSticky" runat="server" Enabled="false"
                                    Checked='<%# Eval("Sticky") != DBNull.Value && Convert.ToBoolean(Eval("Sticky") ?? false) %>' />
                            </div>
                            <div class="ms-2" style="width: 300px; height:100%; object-fit:contain;">
                                <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("CoverPath") %>" alt="封面" style="width: 100%;" />
                            </div>
                        </div>
                    </div>
                    <div class="col-7 ">
                        <p>種類：<%# Eval("CategoryName") %></p>
                        <div class="d-flex">
    檔案下載：
    <div>
        <asp:Repeater ID="rptDownloads" runat="server">
            <ItemTemplate>
                <p><%# Eval("FilePath") %></p>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
                        
                    </div>
                </div>
                <div class="row mx-0">
                    <div class="col-12 news">
                        <p>標題：<%# Eval("title") %></p>
<p>內容：<%# Eval("content") %></p>
                        
                    </div>
                </div>
                <div class="row mx-0">
                    <div class="col-12">
                        <div class="d-flex">
                            <p>建立時間：<%# Eval("CreatedAt") %></p>
                            <p class="ms-2">修改時間：<%# Eval("UpdatedAt") %></p>
                        </div>
                    </div>
                </div>
                <div class="row mx-0">
                    <div class="col-12">
                        <a href='EditNews-B.aspx?id=<%# Eval("Id") %>'>編輯</a> |
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="Delete"
                                    CommandArgument='<%# Eval("Id") %>'
                                    OnClientClick="return confirm('確定要刪除嗎？');">刪除</asp:LinkButton>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
