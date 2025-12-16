<%@ Page Title="後台 | Dealers" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="Dealers-B.aspx.cs" Inherits="Yachts.BackEnd.Dealers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content-header">
        <div class="container-fluid">
            <br />
            <div class="d-flex justify-content-between align-items-center">
                <h3 class="mb-0 ">Dealers 列表</h3>
                <asp:Button ID="btnAddDealer" runat="server" Text="＋ 新增 Dealers" OnClick="btnAddDealer_Click" CssClass="btn btn-warning" />
            </div>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" OnItemDataBound="Repeater1_ItemDataBound">
                <ItemTemplate>
                    <div class="card card-secondary card-outline mb-3">
                        <div class="position-relative p-3 d-flex justify-content-between align-items-center">
                            <a
                                href='<%# "#collapseNews" + Eval("CountryId") %>'
                                data-bs-toggle="collapse"
                                class="stretched-link d-block text-decoration-none text-dark">
                                <h5 class="card-title mb-0 fs-4 fw-bold"><%# Eval("CountryName") %></h5>
                            </a>
                        </div>
                        <div id='<%# "collapseNews" + Eval("CountryId") %>' class="collapse">
                            <asp:Repeater ID="rptDealers" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                <ItemTemplate>
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="border-top p-3">
                                                <div class="d-flex justify-content-between">
                                                    <%# Eval("Content") %>
                                                    <small class="text-muted"><%# Eval("CreatedAt", "{0:yyyy-MM-dd HH:mm}") %></small>
                                                </div>
                                                <div class="mt-2 d-flex justify-content-between">
                                                    <div>
                                                    <a href='EditDealers-B.aspx?id=<%# Eval("DealerId") %>'
                                                        class="btn btn-sm btn-secondary text-white me-2">編 輯</a>
                                                    <asp:LinkButton ID="btnDelete" runat="server"
                                                        CommandName="Delete"
                                                        CommandArgument='<%# Eval("DealerId") %>'
                                                        CssClass="btn btn-sm btn-light text-dark"
                                                        OnClientClick="return confirm('確定要刪除這筆資料嗎？');">刪 除</asp:LinkButton>
                                                </div>
                                                <small class="text-muted">最後更新：<%# Eval("UpdatedAt", "{0:yyyy-MM-dd HH:mm}") %>
                                                </small></div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <%--分頁--%>
            <div class="text-center my-4">
                <asp:Repeater ID="rptPagination" runat="server">
                    <ItemTemplate>
                        <a href='Dealers-B.aspx?page=<%# Container.DataItem %>'
                            class='<%# (Request.QueryString["page"] == Container.DataItem.ToString() ||
                            (Request.QueryString["page"] == null && Convert.ToInt32(Container.DataItem) == 1))
                            ? "btn btn-secondary mx-1" : "btn btn-outline-secondary mx-1" %>'>
                            <%# Container.DataItem %>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:Content>
