<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditNews-B.aspx.cs" Inherits="Yachts.BackEnd.EditNnews_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content-wrapper">
        <div>
            <div class="">
                <div class="">
                    標題：<asp:TextBox ID="txtTitle" runat="server" Width="450px"></asp:TextBox>
                    種類：<asp:DropDownList ID="CategoryList" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                    置頂 :
                    <asp:CheckBox ID="chbSticky" runat="server" RepeatDirection="Horizontal"></asp:CheckBox>
                    <br />
                    <br />
                    <div class="row align-items-center">
                        <div class="col-6 ">
                            <div class="d-flex align-items-center">
                                封面：<asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" AllowMultiple="true" />
                                <div>
                                    <asp:Image ID="imgCover" runat="server" Width="187px" Height="121px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            檔案下載：
                        <asp:FileUpload ID="FUDownloadsFile" runat="server" ClientIDMode="Static" AllowMultiple="true" />
                            <div class="">
                                <asp:Repeater ID="rptDownloads" runat="server">
                                    <ItemTemplate>
                                        <p>
                                            <asp:Literal ID="LilDownloads" runat="server" Text='<%# Eval("FilePath") %>'></asp:Literal>
                                        </p>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <asp:Repeater ID="rptEditFiles" runat="server" OnItemCommand="rptEditFiles_ItemCommand">
    <ItemTemplate>
        <p>
            <%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %>
            <asp:LinkButton ID="btnDeleteFile" runat="server"
                CommandName="DeleteFile"
                CommandArgument='<%# Eval("Id") %>'
                OnClientClick="return confirm('確定要刪除這個檔案嗎？');">
                刪除
            </asp:LinkButton>
        </p>
    </ItemTemplate>
</asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="500px" />
            <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" />
            &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
