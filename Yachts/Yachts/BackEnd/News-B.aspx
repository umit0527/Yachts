<%@ Page Title="後台 | News" Language="C#" MasterPageFile="~/Backend/Site-B.Master" AutoEventWireup="true" CodeBehind="News-B.aspx.cs" Inherits="Yachts.BackEnd.News_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .btn-outline-info:hover {
            background-color: #0dcaf0 !important;
            color: white !important;
            border-color: #0dcaf0 !important;
        }

        .article-content {
            color: #000 !important; /* 全部字設為黑色 */
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
                        <div class="position-relative p-3 d-flex justify-content-between align-items-center">
                            <a
                                href='<%# "#collapseNews" + Eval("Id") %>'
                                data-bs-toggle="collapse"
                                aria-expanded="false"
                                aria-controls='<%# "collapseNews" + Eval("Id") %>'
                                class="stretched-link d-block text-decoration-none text-dark">
                                <h5 class="mb-0">
                                    <%# Eval("Title") %>
                                    <%# Convert.ToBoolean(Eval("Sticky")) ? "<span class='badge bg-danger ms-2'>置頂</span>" : "" %>
                                </h5>
                            </a>

                            <small class="text-muted">
                                <%# Eval("CategoryName") %> ｜ <%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %>
                            </small>
                        </div>
                        <div id='<%# "collapseNews" + Eval("Id") %>' class="collapse">
                            <div class="border-top"></div>
                            <%--內容--%>
                            <div class="row">
                                <div class="col-12 article-content">
                                    <div class="card-text p-3" style="overflow: hidden; text-overflow: ellipsis;">
                                        <%# Eval("Content") %>
                                    </div>
                                </div>
                            </div>
                            <div class="border-top"></div>
                            <%--封面--%>
                            <div class="row ">
                                <div class="col-6 ">
                                    <div class="px-3 pt-2">
                                        <p class="fw-bold">封面：</p>
                                        <img src='<%# "/Uploads/Photos/" + Eval("CoverPath") %>' alt="封面圖片" style="max-width: 300px; height: auto; border-radius: 8px;" />
                                    </div>
                                </div>

                                <div class="col-6">
                                    <div class="px-3 pt-2">
                                        <asp:Repeater ID="rptDownloads" runat="server">
                                            <HeaderTemplate>
                                                <p class="fw-bold">檔案下載：</p>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li style="margin-bottom: 6px;">
                                                    <span style="display: inline-block; border-bottom: 1px solid #ccc; padding-bottom: 2px;">
                                                        <%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %>
                                                    </span>
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                            <%--更新時間與編輯刪除--%>
                            <div class="row">
                                <div class="col-12">
                                    <div class="d-flex justify-content-between p-3">
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
        </div>
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

    </section>
</asp:Content>
