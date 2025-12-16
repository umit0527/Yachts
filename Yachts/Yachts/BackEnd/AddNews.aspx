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
                                標題：<asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                種類：<asp:DropDownList ID="CategoryList" runat="server" AutoPostBack="True" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group form-check mt-md-4">
                                置頂 :
                                <asp:CheckBox ID="chbSticky" runat="server" RepeatDirection="Horizontal"></asp:CheckBox>
                                <label class="form-check-label" for="chbSticky"></label> <%-- 保持 label for 對應 chbSticky 但不顯示文字，因為文字在外面 --%>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-4">
                        <div class="col-md-6">
                            <div class="form-group">
                                封面：<asp:FileUpload ID="FUCoverPath" runat="server" ClientIDMode="Static" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <div>
                                    檔案下載：
                                    <asp:FileUpload ID="FUDownloadsFile" runat="server" ClientIDMode="Static" AllowMultiple="true" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="500px" />
                    <br />
                </div>
                <div class="card-footer text-right">
                    <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" CssClass="btn btn-warning" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" CssClass="btn btn-secondary ml-2" />
                </div>
            </div>
        </div>
    </section>

    <script>
        window.onload = function () {
            if (CKEDITOR.instances['<%= CKEditor1.ClientID %>']) {
                CKEDITOR.instances['<%= CKEditor1.ClientID %>'].destroy(true);
            }

            CKEDITOR.replace('<%= CKEditor1.ClientID %>', {
                enterMode: CKEDITOR.ENTER_BR,    // 按 Enter 插入 <br> 而不是 <p>
                allowedContent: true,            // 保留所有貼上的樣式與標籤
            });
        };
    </script>

</asp:Content>