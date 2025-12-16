<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Yachts.BackEnd.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--Yachts--%>
    <asp:Panel ID="pnlYachts" runat="server" Visible="false">
        <section class="content">
            <div class="container-fluid"><br />
                <div class="d-flex justify-content-between align-items-center">
    <h3 class="mb-0">Yachts 列表</h3>
    <asp:Button ID="btnAddYachts" runat="server" CssClass="btn btn-warning" Text="＋ 新增 Yachts" OnClick="btnAddYachts_Click" />
</div><br />
                <asp:Repeater ID="rptYachts" runat="server"
                    OnItemCommand="rptYachts_ItemCommand"
                    OnItemDataBound="rptYachts_ItemDataBound">
                    <ItemTemplate>
                        <div class="card card-primary card-outline mb-3">
                            <div class="card-body">
                                <div class="row mb-2">
                                    <div class="col-12">
                                        <div class="d-flex justify-content-between align-items-center">
                                            <%-- 船名與標籤 --%>
                                            <h5 class="card-title mb-1 fw-bold fs-4" title='<%# Eval("ModelName") %>' style="max-width: 70%; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                                <%# Eval("ModelName") %>
                                                <span class='badge bg-danger ms-2'><%# Eval("Label") %></span>
                                            </h5>
                                            <%-- 建立時間 --%>
                                            <small class="text-muted">
                                                <%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %>
                                            </small>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-2">
                                    <div class="col-12">
                                        <%-- 介紹 --%>
                                        <p class="card-text pt-2 border-top" style="max-height: 4.5em; overflow: hidden; text-overflow: ellipsis;">
                                            <p>Introduce：</p>
                                            <%# Eval("content") %>
                                        </p>
                                    </div>
                                </div>

                                <div class="row mb-2">
                                    <div class="col-md-6 border-top">
                                        <%-- 尺寸與設計參數 --%>
                                        <h6 class="pt-2 ">Principal Dimension：</h6>
                                        <ul class="list-group list-group-flush mb-3">
                                            <asp:Repeater ID="rptPrincipal" runat="server">
                                                <ItemTemplate>
                                                    <li class="d-flex align-items-center py-1">
                                                        <span class="font-weight-bold"><%# Eval("PrincipalName") %>：</span>
                                                        <span><%# Eval("PrincipalValue") %></span>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                    <div class="col-md-6 border-top">
                                        <%-- 設計圖 --%>
                                        <h6 class="pt-2 ">Interior Image：</h6>
                                        <div class="text-center mb-3">
                                            <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("InteriorImgPath") %>" class="img-thumbnail" style="max-width: 300px; height: auto; object-fit: cover;" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-2 pt-2 border-top">
                                    <div class="col-md-6">
                                        <%-- 平面圖1 --%>
                                        <h6>Layout Image 1：</h6>
                                        <div class="text-center mb-3">
                                            <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("DeckImgPath1") %>" class="img-thumbnail" style="max-width: 300px; height: auto; object-fit: cover;" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <%-- 平面圖2 --%>
                                        <h6>Layout Image 2：</h6>
                                        <div class="text-center mb-3">
                                            <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("DeckImgPath2") %>" class="img-thumbnail" style="max-width: 300px; height: auto; object-fit: cover;" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-2 pt-2 border-top">
                                    <div class="col-12">
                                        <h6>Carousel Images：</h6>
                                        <div class="row">
                                            <asp:Repeater ID="rptCarouselImgs" runat="server">
                                                <ItemTemplate>
                                                    <div class="col-6 col-md-4 col-lg-3 mb-2 text-center">
                                                        <img src='<%# ResolveUrl("/Uploads/Photos/") + Eval("CarouselImgPath") %>' class="img-thumbnail" style="width: 100%; height: auto; object-fit: cover;" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-2 pt-2 border-top">
                                    <div class="col-12">
                                        <p>Downloads：</p>
                                        <asp:Repeater ID="rptFiles" runat="server">
                                            <HeaderTemplate>
                                                <ul style="list-style: none; padding-left: 0; margin-top: 0.5rem;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li style="margin-bottom: 6px;">
                                                    <span style="display: inline-block; border-bottom: 1px solid #ccc; padding-bottom: 2px;"><%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %></span>
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>

                                <div class="row mb-2 pt-2 border-top">
                                    <div class="col-12">
                                        <h6>Specification：</h6>
                                        <div class="card card-body">
                                            <p class="mb-0"><%# Eval("Specification") %></p>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12">
                                        <div class="mt-2 d-flex justify-content-between align-items-center">
                                            <div>
                                                <a href='<%# "EditYachts-B.aspx?id=" + Eval("ModelId") %>' class="btn btn-sm bg-primary text-white me-1">編 輯</a>
                                                <asp:LinkButton ID="btnDelete" runat="server"
                                                    CommandName="Delete"
                                                    CommandArgument='<%# Eval("ModelId") %>'
                                                    CssClass="btn btn-sm btn-secondary"
                                                    OnClientClick="return confirm('確定要刪除這艘遊艇嗎？');">
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
        </section>
    </asp:Panel>
    <%--news--%>
    <asp:Panel ID="pnlNews" runat="server" Visible="false">
        <section class="content">
            <div class="container-fluid"><br />
                <div class="d-flex justify-content-between align-items-center">
    <h3 class="mb-0">News 列表</h3>
    <asp:Button ID="btnAddNews" runat="server" CssClass="btn btn-warning" Text="＋ 新增News" OnClick="btnAddNews_Click" />
</div><br />
                <asp:Repeater ID="rptNews" runat="server" OnItemCommand="rptNews_ItemCommand" OnItemDataBound="rptNews_ItemDataBound">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblResults" runat="server" Text=' <%# Eval("ContentText") %>'></asp:Label>--%>
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
            </div>
        </section>
    </asp:Panel>
</asp:Content>
