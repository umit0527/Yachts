<%@ Page Title="後台 | Company" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="Company-B.aspx.cs" Inherits="Yachts.BackEnd.Company_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <div class="d-flex justify-content-between align-items-center">
                <h3 class="mb-0">Company 內容列表</h3>
                <asp:Button ID="btnAddContent" runat="server" CssClass="btn btn-warning" Text="＋ 新增內容" OnClick="btnAddContent_Click" />
            </div>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                <ItemTemplate>
                    <div class="card card-success card-outline mb-3">
                        <div class="card-body">
                            <div class="row mb-2">
                                <div class="col-12">
                                    <div class="d-flex justify-content-between align-items-center">
                                        <h5 class="card-title mb-1" title='<%# Eval("Title") %>'>
                                            <%# Eval("Title") %>
                                        </h5>
                                        <small class="text-muted"><%# Eval("CategoryName") %> |  <%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></small>
                                    </div>
                                </div>
                            </div>

                            <div class="row mb-2">
                                <div class="col-12">
                                    <p class="card-text pt-2 border-top" style="max-height: 4.5em; overflow: hidden; text-overflow: ellipsis;">
                                        <%# Eval("content") %>
                                    </p>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <div class="mt-2 d-flex justify-content-between">
                                        <div>
                                            <a href='<%# "EditCompany-B.aspx?id=" + Eval("Id") %>' class="btn btn-sm btn-success text-white me-1">編 輯</a>
                                            <asp:LinkButton ID="btnDelete" runat="server"
                                                CommandName="Delete"
                                                CommandArgument='<%# Eval("Id") %>'
                                                CssClass="btn btn-sm btn-secondary"
                                                OnClientClick="return confirm('確定要刪除嗎？');">
                                                刪 除
                                            </asp:LinkButton>
                                        </div>
                                        <small class="text-muted">最後更新：<%# Eval("UpdatedAt", "{0:yyyy-MM-dd HH:mm}") %>
                                        </small>
                                    </div>
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
                        <a href='Company-B.aspx?page=<%# Container.DataItem %>'
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
