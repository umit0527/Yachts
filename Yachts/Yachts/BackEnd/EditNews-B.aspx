<%@ Page Title="後台 | 編輯 News" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditNews-B.aspx.cs" Inherits="Yachts.BackEnd.EditNnews_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">編輯 News </h3> 
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
            <p class="fw-bold me-2">置頂 : </p>
            <asp:CheckBox ID="chbSticky" runat="server" RepeatDirection="Horizontal"></asp:CheckBox>
            <label class="form-check-label" for="chbSticky"></label> 
        </div>
    </div>
</div>

                    <div class="row mb-4">
                        <div class="col-md-6">
                            <label for="FileUpload1" class="fw-bold">封面 *</label>
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                            <div class="mt-2">
                                <asp:Image ID="imgCover" runat="server" Width="187px" Height="121px" CssClass="img-thumbnail" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label for="FUDownloadsFile" class="fw-bold">檔案下載：</label>
                            <asp:FileUpload ID="FUDownloadsFile" runat="server" CssClass="form-control" AllowMultiple="true" />
                            <div class="mt-2">
                                <asp:Repeater ID="rptEditFiles" runat="server" OnItemCommand="rptEditFiles_ItemCommand">
                                    <ItemTemplate>
                                        <div class="d-flex justify-content-between align-items-center border-bottom py-1">
                                            <span><%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %></span>
                                            <asp:LinkButton ID="btnDeleteFile" runat="server" CssClass="btn btn-sm btn-danger"
                                                CommandName="DeleteFile"
                                                CommandArgument='<%# Eval("Id") %>'
                                                OnClientClick="return confirm('確定要刪除這個檔案嗎？');">
                                                刪 除
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mb-4">
                        <label for="CKEditor1" class="fw-bold">內容：</label>
                        <asp:RequiredFieldValidator
    ID="RequiredFieldValidator4"
    runat="server"
    ControlToValidate="CKEditor1"
    ErrorMessage="請輸入內容"
    ForeColor="Red"
    Display="Dynamic">
</asp:RequiredFieldValidator>
                        <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="800px" />
                    </div>
                </div>
                <div class="card-footer text-right"> 
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-warning" Text="送 出" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary ml-2" Text="取 消" OnClick="btnCancel_Click" CausesValidation="false"/>
                </div>
            </div>
        </div>
    </section>
</asp:Content>