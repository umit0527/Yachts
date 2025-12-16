<%@ Page Title="後台 | 新增 News" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="AddNews.aspx.cs" Inherits="Yachts.BackEnd.AddNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">新增 News </h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-info card-outline">
                <div class="card-header">
                    <h3 class="card-title">消息資料</h3>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <div class="d-flex">
                                    <p class="fw-bold">
                                        標題 *
                                    </p>
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator1"
                                        runat="server"
                                        ControlToValidate="txtTitle"
                                        ErrorMessage="請輸入標題"
                                        ForeColor="Red"
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </div>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>

                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <div class="d-flex">
                                <p class="fw-bold">
                                    種類 *
                                </p>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator2"
                                    runat="server"
                                    ControlToValidate="CategoryList"
                                    ErrorMessage="請選擇種類"
                                    ForeColor="Red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>
                            <asp:DropDownList ID="CategoryList" runat="server" AutoPostBack="True" CssClass="form-control">
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group form-check d-flex">
                            <p class="fw-bold me-2">
                                置頂 :
                            </p>
                            <asp:CheckBox ID="chbSticky" runat="server" RepeatDirection="Horizontal"></asp:CheckBox>
                            <label class="form-check-label" for="chbSticky"></label>
                        </div>
                    </div>
                </div>
                <div class="row mb-4">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="d-flex">
                                <p class="fw-bold">封面 *</p>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator3"
                                    runat="server"
                                    ControlToValidate="FUCoverPath"
                                    ErrorMessage="請上傳封面"
                                    ForeColor="Red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>
                            <asp:FileUpload ID="FUCoverPath" runat="server" ClientIDMode="Static" CssClass="form-control" />

                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <div>
                                <p class="fw-bold">檔案下載</p>
                                <asp:FileUpload ID="FUDownloadsFile" runat="server" ClientIDMode="Static" AllowMultiple="true" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <br />
                        <div class="d-flex">
                            <p class="fw-bold">內容 *</p>
                            <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator4"
                                runat="server"
                                ControlToValidate="CKEditor1"
                                ErrorMessage="請輸入內容"
                                ForeColor="Red"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                        <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="400px" />
                        <br />
                    </div>
                </div>
            </div>
            <div class="card-footer text-right">
                <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" CssClass="btn btn-warning" />
                &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" CssClass="btn btn-secondary ml-2" CausesValidation="false"/>
            </div>
        </div>
        </div>
    </section>
</asp:Content>
