<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="Yachts-B.aspx.cs" Inherits="Yachts.BackEnd.Yachts" %>

<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <asp:Button ID="btnAddYachts" runat="server" Text="新增船型" OnClick="btnAddYachts_Click" CssClass="btn btn-warning" />
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
                        <div class="card-body">
                            <div class="row mb-2">
                                <div class="col-12">
                                    <div class="d-flex">
                                        <h3><%# Eval("ModelName") %></h3>
                                        <h4><%# Eval("Label") %></h4>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-12">
                                    <p>介紹：</p>
                                    <p><%# Eval("content") %></p>
                                </div>
                            </div>

                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <h6>尺寸與設計參數：</h6>
                                    <asp:Repeater ID="rptPrincipal" runat="server">
                                        <ItemTemplate>
                                            <div class="d-flex">
                                                <p><%# Eval("Name") %></p>
                                                ：
                                    <p><%# Eval("Value") %></p>
                                                <br />
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="col-md-6">
                                    <h6>設計圖：</h6>
                                    <div class="img-fluid " style="max-width: 300px;">
                                        <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("InteriorImgPath") %>" class="img-thumbnail" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <h6>詳細規格：</h6>
                                <p><%# Eval("Specification") %></p>
                            </div>
                        </div>

                        <div class="row mb-2">

                            <div class="col-md-6">
                                <h6>平面圖1：</h6>
                                <div class="img-fluid" style="max-width: 300px;">
                                    <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("DeckImgPath1") %>" class="img-thumbnail" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <h6>平面圖2：</h6>
                                <div class="img-fluid" style="max-width: 300px;">
                                    <img src="<%# ResolveUrl("/Uploads/Photos/") + Eval("DeckImgPath2") %>" class="img-thumbnail" />
                                </div>
                            </div>
                        </div>
                        <div class="row mb-2">            
                                <h6>輪播圖：</h6>
                                    <asp:Repeater ID="rptCarouselImgs" runat="server">
                                        <ItemTemplate>
                                            <div class="img-fluid mb-2 col-3 text-center align-content-center">
                                                <img src='<%# ResolveUrl("/Uploads/Photos/") + Eval("CarouselImgPath") %>' class="img-thumbnail" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                        </div>
                        <div class="row mb-2">
                            <div class="col-md-12">
                                <p>檔案下載：</p>
                                <asp:Repeater ID="rptFiles" runat="server">
                                    <ItemTemplate>
                                        <p><%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %></p>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="d-flex">
                                    <p>建立時間：</p>
                                    <%# Eval("CreatedAt") %>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="d-flex">
                                    <p>修改時間：</p>
                                    <%# Eval("UpdatedAt") %>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <a href='EditYachts-B.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-info">編輯</a>
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="Delete"
                                    CommandArgument='<%# Eval("Id") %>'
                                    OnClientClick="return confirm('確定要刪除嗎？');"
                                    CssClass="btn btn-sm btn-danger ml-2">刪除</asp:LinkButton>
                            </div>
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:Content>
