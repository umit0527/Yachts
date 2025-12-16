<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditNews-B.aspx.cs" Inherits="Yachts.BackEnd.EditNnews_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content-wrapper">
        <div>
            <div class="d-flex">
                <div class="">
                    標題：<asp:TextBox ID="txtTitle" runat="server" Width="450px"></asp:TextBox>
                
                <div>
                種類：<asp:DropDownList ID="CategoryList" runat="server" AutoPostBack="True">
                </asp:DropDownList>
                    <br />
                    <div>
                        封面：<asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" AllowMultiple="true" />

                        <asp:Image ID="imgCover" runat="server" Width="187px" Height="121px" />
                    </div>
                </div>
                <br />
            </div></div>
            <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="500px" />
            <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" />
            &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
