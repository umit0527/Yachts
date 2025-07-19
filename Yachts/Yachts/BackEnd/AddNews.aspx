<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="AddNews.aspx.cs" Inherits="Yachts.BackEnd.AddNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content-wrapper">
        <div>
            <div class="">
                <div class="row mx-0">
                    <div class="col-6">
                        標題：<asp:TextBox ID="txtTitle" runat="server" Width="450px"></asp:TextBox>
                    </div>
                    <div class="col-3">
                        種類：<asp:DropDownList ID="CategoryList" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div class="col-3">
                        置頂 :
                    <asp:CheckBox ID="chbSticky" runat="server" RepeatDirection="Horizontal"></asp:CheckBox>
                    </div>
                    <br />
                    <br />
                </div>
                <div class="row mx-0">
    <div class="col-6">
        封面：<asp:FileUpload ID="FUCoverPath" runat="server" ClientIDMode="Static" />
    </div>
    <div class="col-6">
        <div>
            檔案下載：
    <asp:FileUpload ID="FUDownloadsFile" runat="server" ClientIDMode="Static" AllowMultiple="true" />
        </div>
    </div>
</div>
            </div>
            <br />
            <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="500px" />
            <br />
            <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" />
            &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" />
        </div>
    </div>
    <script>
        window.onload = function () {
            if (CKEDITOR.instances['<%= CKEditor1.ClientID %>']) {
                CKEDITOR.instances['<%= CKEditor1.ClientID %>'].destroy(true);
            }

            CKEDITOR.replace('<%= CKEditor1.ClientID %>', {
                enterMode: CKEDITOR.ENTER_BR,   // 按 Enter 插入 <br> 而不是 <p>
                allowedContent: true,           // 保留所有貼上的樣式與標籤
                autoParagraph: false            // 不自動加 <p>（防止手打出現 border）
            });
        };
    </script>

</asp:Content>
