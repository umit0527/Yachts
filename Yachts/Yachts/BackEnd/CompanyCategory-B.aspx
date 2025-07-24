<%@ Page Title="後台 | CompanyCategory" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="CompanyCategory-B.aspx.cs" Inherits="Yachts.BackEnd.CompantCategory_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <div class="d-flex justify-content-between align-items-center">
                <h3 class="mb-0">Company 種類列表</h3>
                <asp:Button ID="btnAddCompanyCategory" runat="server" Text="＋ 新增種類"
                    OnClick="btnAddCompanyCategory_Click"
                    CssClass="btn btn-warning" />
            </div>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                <ItemTemplate>
                    <div class="card card-success card-default card-outline mb-3">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-8">
                                    <p class="mb-1">種類名稱：<strong><%# Eval("Name") %></strong></p>
                                    <small class="text-muted">建立時間：<%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></small><br />
                                    <small class="text-muted">最後更新：<%# Eval("UpdatedAt", "{0:yyyy-MM-dd HH:mm}") %></small>
                                </div>
                                <div class="col-md-4 text-right align-self-center">
                                    <a href='EditCompanyCategory-B.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-success text-white">編 輯</a>
                                    <asp:LinkButton ID="btnDelete" runat="server"
                                        CommandName="Delete"
                                        CommandArgument='<%# Eval("Id") %>'
                                        OnClientClick="return confirm('確定要刪除嗎？');"
                                        CssClass="btn btn-sm btn-secondary ml-2">刪 除</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <%--分頁--%>
            <div class="text-center my-4">
                <asp:Repeater ID="rptPagination" runat="server">
                    <ItemTemplate>
                        <a href='CompanyCategory-B.aspx?page=<%# Container.DataItem %>'
                            class='<%# (Request.QueryString["page"] == Container.DataItem.ToString() ||
(Request.QueryString["page"] == null && Convert.ToInt32(Container.DataItem) == 1))
? "btn btn-success mx-1" : "btn btn-outline-success mx-1" %>'>
                            <%# Container.DataItem %>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:Content>
