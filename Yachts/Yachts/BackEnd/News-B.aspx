<%@ Page Title="後台 | News" Language="C#" MasterPageFile="~/Backend/Site-B.Master" AutoEventWireup="true" CodeBehind="News-B.aspx.cs" Inherits="Yachts.BackEnd.News_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .btn-outline-info:hover {
            background-color: #0dcaf0 !important;
            color: white !important;
            border-color: #0dcaf0 !important;
        }
    </style>
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <div class="d-flex justify-content-between align-items-center">
                <h3 class="mb-0">News 列表</h3>
                <asp:Button ID="btnAddNews" runat="server" CssClass="btn btn-warning" Text="＋ 新增News" OnClick="btnAddNews_Click" />
            </div>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" OnItemDataBound="Repeater1_ItemDataBound">
                <ItemTemplate>
                    <div class="card card-info card-outline mb-3">
                        <div class="card-body">
                            <div class="row mb-2">
                                <div class="col-12">
                                    <div class="d-flex justify-content-between align-items-center">
                                        <h5 class="card-title mb-1 fw-bold fs-4" title='<%# Eval("Title") %>' style="max-width: 70%; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                            <%# Eval("Title") %>
                                            <%# Convert.ToBoolean(Eval("Sticky")) ? "<span class='badge bg-danger ms-2'>置頂</span>" : "" %>
                                        </h5>
                                        <small class="text-muted">
                                            <%# Eval("CategoryName") %> ｜ <%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %>
                                        </small>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-12">
                                    <p class="card-text pt-2 border-top" style="overflow: hidden; text-overflow: ellipsis;">
                                        <%# Eval("Content") %>
                                    </p>
                                </div>
                            </div>
                            <div class="row mb-2 ">
                                <div class="col-6 border-top">
                                    <p class="pt-2">封面：</p>
                                    <img src='<%# "/Uploads/Photos/" + Eval("CoverPath") %>' alt="封面圖片" style="max-width: 300px; height: auto; border-radius: 8px;" />
                                </div>

                                <div class="col-6 border-top">
                                    <asp:Repeater ID="rptDownloads" runat="server">
                                        <HeaderTemplate>
                                            <div class="pt-2 ">
                                                <p>檔案下載：</p>
                                                <ul style="list-style: none; padding-left: 0; margin-top: 0.5rem;">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li style="margin-bottom: 6px;">
                                                <span style="display: inline-block; border-bottom: 1px solid #ccc; padding-bottom: 2px;">
                                                    <%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %>
                                                </span>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                            </div>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-12">
                                    <div class="mt-2 d-flex justify-content-between">
                                        <div>
                                            <a href='<%# "EditNews-B.aspx?Id=" + Eval("Id") %>' class="btn btn-sm btn-info text-white me-1">編 輯</a>
                                            <asp:LinkButton ID="btnDelete" runat="server"
                                                CommandName="Delete"
                                                CommandArgument='<%# Eval("Id") %>'
                                                CssClass="btn btn-sm btn-secondary"
                                                OnClientClick="return confirm('確定要刪除這則新聞嗎？');">
                                                刪 除
                                            </asp:LinkButton>
                                        </div>
                                        <small class="text-muted">最後更新：<%# Eval("UpdatedAt", "{0:yyyy-MM-dd HH:mm}") %></small>
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
                        <a href='News-B.aspx?page=<%# Container.DataItem %>'
                            class='<%# (Request.QueryString["page"] == Container.DataItem.ToString() ||
                                        (Request.QueryString["page"] == null && Convert.ToInt32(Container.DataItem) == 1))
                                        ? "btn btn-info mx-1 text-light" : "btn btn-outline-info mx-1 " %>'>
                            <%# Container.DataItem %>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:Content>
