<%@ Page Title="後台 | Yachts" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="Yachts-B.aspx.cs" Inherits="Yachts.BackEnd.Yachts" %>

<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="/css/homestyle2.css" rel="stylesheet" type="text/css" />--%>
    <style>
        .box5 li {
            background-image: url(../images/icon006.gif);
            width: 48%;
            background-position: 2px 8px;
            background-repeat: no-repeat;
            padding-left: 10px;
            display: inline-block;
            vertical-align: top;
            padding-bottom: 5px;
            zoom: 1;
        }

        .box5 p {
            background-image: url(../images/icon008.gif);
            background-repeat: no-repeat;
            background-position: 1px 4px;
            padding-left: 20px;
            border-bottom-width: 1px;
            border-bottom-style: solid;
            border-bottom-color: #bdbdbd;
            color: #068fb0;
            margin-bottom: 5px;
            padding-bottom: 2px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <div class="d-flex justify-content-between align-items-center">
                <h3 class="mb-0">Yachts 列表</h3>
                <asp:Button ID="btnAddYachts" runat="server" CssClass="btn btn-warning" Text="＋ 新增 Yachts" OnClick="btnAddYachts_Click" />
            </div>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <asp:Repeater ID="Repeater1" runat="server"
                OnItemCommand="Repeater1_ItemCommand"
                OnItemDataBound="Repeater1_ItemDataBound">
                <ItemTemplate>
                    <div class="card card-primary card-outline mb-3">
                        <div class="position-relative p-3 d-flex justify-content-between align-items-center">
                            <a
                                href='<%# "#collapseNews" + Eval("Id") %>'
                                data-bs-toggle="collapse"
                                aria-expanded="false"
                                aria-controls='<%# "collapseNews" + Eval("Id") %>'
                                class="stretched-link d-block text-decoration-none text-dark">

                                <%-- 船名與標籤 --%>
                                <h5 class="card-title mb-1 fw-bold fs-4" title='<%# Eval("ModelName") %>'>
                                    <%# Eval("ModelName") %>
                                    <span class='badge bg-danger ms-2'><%# Eval("Label") %></span>
                                </h5>
                            </a>
                            <%-- 建立時間 --%>
                            <small class="text-muted">
                                <%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %>
                            </small>
                        </div>

                        <div id='<%# "collapseNews" + Eval("Id") %>' class="collapse">
                            <%-- 介紹 --%>
                            <div class="row">
                                <div class="col-12">
                                    <div class="border-top"></div>
                                    <div class="card-text p-3" style="overflow: hidden; text-overflow: ellipsis;">
                                        <p class="fw-bold">Introduce：</p>
                                        <%# Eval("content") %>
                                    </div>
                                </div>
                            </div>
                            <div class="border-top"></div>
                            <div class="row mb-2">
                                <%-- 尺寸與設計參數 --%>
                                <div class="col-md-6 ">
                                    <div class="px-3 pt-2">
                                        <h6 class="fw-bold">Principal Dimension：</h6>
                                        <ul class="list-group list-group-flush mb-3">
                                            <asp:Repeater ID="rptPrincipal" runat="server">
                                                <ItemTemplate>
                                                    <li class="d-flex align-items-center py-1">
                                                        <span class="font-weight-bold"><%# Eval("Name") %>：</span>
                                                        <span><%# Eval("Value") %></span>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                                <%-- 設計圖 --%>
                                <div class="col-md-6">
                                    <div class="px-3 pt-2">
                                        <h6 class="fw-bold">Interior Image：</h6>
                                        <div class="text-center mb-3">
                                            <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("InteriorImgPath") %>" class="img-thumbnail" style="max-width: 300px; height: auto; object-fit: cover;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="border-top"></div>
                            <div class="row mb-2 pt-2">
                                <%-- 平面圖1 --%>
                                <div class="col-md-6">
                                    <div class="px-3 pt-2">
                                        <h6 class="fw-bold">Layout Image 1：</h6>
                                        <div class="text-center mb-3">
                                            <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("DeckImgPath1") %>" class="img-thumbnail" style="max-width: 300px; height: auto; object-fit: cover;" />
                                        </div>
                                    </div>
                                </div>
                                <%-- 平面圖2 --%>
                                <div class="col-md-6">
                                    <div class="px-3 pt-2">
                                        <h6 class="fw-bold">Layout Image 2：</h6>
                                        <div class="text-center mb-3">
                                            <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("DeckImgPath2") %>" class="img-thumbnail" style="max-width: 300px; height: auto; object-fit: cover;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="border-top"></div>
                            <%--輪播圖--%>
                            <div class="row mb-2 pt-2">
                                <div class="col-12">
                                    <div class="px-3 pt-2">
                                        <h6 class="fw-bold">Carousel Images：</h6>
                                        <div class="row">
                                            <asp:Repeater ID="rptCarouselImgs" runat="server">
                                                <ItemTemplate>
                                                    <div class="col-3">
                                                        <div class="py-2">
                                                            <img src='<%# ResolveUrl("/Uploads/Photos/") + Eval("CarouselImgPath") %>' class="img-thumbnail" style="width: 100%; height: auto; object-fit: cover;" />
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="border-top"></div>
                            <%--檔案下載--%>
                            <div class="row">
                                <div class="col-12">
                                    <div class="p-3 pt-2">
                                        <p class="fw-bold">Downloads：</p>
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
                            </div>
                            <div class="border-top"></div>
                            <%--詳細規格--%>
                            <div class="row">
                                <div class="col-12">
                                    <div class="px-3 pt-2">
                                        <div class="box5">
                                            <h6 class="fw-bold">Specification：</h6>
                                            <div class="card card-body">
                                                <%# Eval("Specification") %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--更新時間、編輯與刪除--%>
                            <div class="row">
                                <div class="col-12">
                                    <div class="p-3">
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
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <%--分頁--%>
            <div class="text-center my-4">
                <asp:Repeater ID="rptPagination" runat="server">
                    <ItemTemplate>
                        <a href='Yachts-B.aspx?page=<%# Container.DataItem %>'
                            class='<%# (Request.QueryString["page"] == Container.DataItem.ToString() ||
                                        (Request.QueryString["page"] == null && Convert.ToInt32(Container.DataItem) == 1))
                                        ? "btn btn-primary mx-1" : "btn btn-outline-primary mx-1" %>'>
                            <%# Container.DataItem %>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:Content>
