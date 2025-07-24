<%@ Page Title="後台 | 新增 Yachts" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="AddYachts.aspx.cs" Inherits="Yachts.BackEnd.AddInterior" %>

<%@ Register TagPrefix="ckeditor" Namespace="CKEditor.NET" Assembly="CKEditor.NET" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">新增 Yachts</h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-primary card-outline">
                <div class="card-header">
                    <h3 class="card-title">基本資料</h3>
                </div>
                <div class="card-body">
                    <div class="row mb-3 align-items-center">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="ModelName">船名：</label>
                                <asp:TextBox ID="ModelName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="ModelNum">型號：</label>
                                <asp:TextBox ID="ModelNum" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label>標籤：</label>
                                <asp:RadioButtonList ID="rblModelLabel" runat="server" RepeatDirection="Horizontal" CssClass="d-flex justify-content-between w-100">
                                    <asp:ListItem Text="Genaral" CssClass="form-check form-check-inline mx-0"></asp:ListItem>
                                    <asp:ListItem Text="New Design" CssClass="form-check form-check-inline mx-0"></asp:ListItem>
                                    <asp:ListItem Text="New Building" CssClass="form-check form-check-inline mx-0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="Introduce">介紹：</label>
                                <asp:TextBox ID="Introduce" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="8"></asp:TextBox> 
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>尺寸與設計參數：</label>
                                <div class="card card-default card-outline"> 
                                    <div class="card-body">
                                        <asp:Repeater ID="rptPendingFields" runat="server"
                                            OnItemCommand="rptPendingFields_ItemCommand"
                                            OnItemDataBound="rptPendingFields_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="d-flex justify-content-between align-items-center mb-2 pb-2 border-bottom"> 
                                                    <%-- 檢查是否是編輯狀態 --%>
                                                    <asp:PlaceHolder ID="phView" runat="server" Visible='<%# Container.ItemIndex != (int)(ViewState["PendingEditIndex"] ?? -1) %>'>
                                                        <div>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' CssClass="font-weight-bold" />
                                                            ：
                                                            <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' />
                                                        </div>
                                                        <div>
                                                            <asp:Button ID="btnEditPending" runat="server" Text="編 輯" CommandName="Edit" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-sm btn-primary" />&nbsp;&nbsp;
                                                            <asp:Button ID="btnDeletePending" runat="server" Text="刪 除" CommandName="Delete" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-sm btn-danger ml-2" />
                                                        </div>
                                                    </asp:PlaceHolder>

                                                    <%-- 編輯模式 --%>
                                                    <asp:PlaceHolder ID="phEdit" runat="server" Visible='<%# Container.ItemIndex == (int)(ViewState["PendingEditIndex"] ?? -1) %>'>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtEditName" runat="server" Text='<%# Eval("Name") %>' CssClass="form-control form-control-sm" Placeholder="欄位名稱" />
                                                            <asp:TextBox ID="txtEditValue" runat="server" Text='<%# Eval("Value") %>' CssClass="form-control form-control-sm ml-2" Placeholder="欄位值" />
                                                            <div class="input-group-append ml-2">
                                                                &nbsp;&nbsp;<asp:Button ID="btnUpdatePending" runat="server" Text="更 新" CommandName="Update" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-sm btn-warning" />&nbsp;&nbsp;
                                                                <asp:Button ID="btnCancelPending" runat="server" Text="取 消" CommandName="Cancel" CssClass="btn btn-sm btn-secondary ml-1" />
                                                            </div>
                                                        </div>
                                                    </asp:PlaceHolder>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <asp:Panel ID="pnlInputFields" runat="server" Visible="false" CssClass="mb-3">
                                            <div class="input-group mb-2">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">欄位名稱：</span>
                                                </div>
                                                <asp:TextBox ID="txtNewName" runat="server" CssClass="form-control" />
                                            </div>
                                            <div class="input-group mb-2">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">欄位值：</span>
                                                </div>
                                                <asp:TextBox ID="txtNewValue" runat="server" CssClass="form-control" />
                                            </div>
                                            <div class="text-right">
                                                <asp:Button ID="btnAddField" runat="server" Text="送 出" OnClick="btnAddNewField_Click" CssClass="btn btn-warning btn-sm" />&nbsp;
                                                <asp:Button ID="btnCancelField" runat="server" Text="取 消" OnClick="btnCancelField_Click" CssClass="btn btn-secondary btn-sm ml-2" />
                                            </div>
                                        </asp:Panel>
                                        <div class="text-right"> 
                                            <asp:Button ID="btnAddNew" runat="server" Text="新增欄位" OnClick="btnAddNew_Click" CssClass="btn btn-primary btn-sm" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="FileUpload1">檔案下載：</label>
                                <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="FUInteriorImg">設計圖：</label>
                                <asp:FileUpload ID="FUInteriorImg" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="FUDeckImg1">平面圖1：</label>
                                <asp:FileUpload ID="FUDeckImg1" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="FUDeckImg2">平面圖2：</label>
                                <asp:FileUpload ID="FUDeckImg2" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="FUCarouselImgPath">輪播圖：</label>
                                <asp:FileUpload ID="FUCarouselImgPath" runat="server" AllowMultiple="true" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="form-group mb-4">
                        <label for="CKEditor1">詳細規格：</label>
                        <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="500" />
                    </div>
                </div>
                <div class="card-footer text-right">
                    <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" CssClass="btn btn-warning" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" CssClass="btn btn-secondary ml-2" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>