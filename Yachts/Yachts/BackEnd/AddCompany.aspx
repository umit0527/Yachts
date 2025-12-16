<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="AddCompany.aspx.cs" Inherits="Yachts.BackEnd.AddCompany" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content-wrapper">
    <div>
        <div class="d-flex">
            <div>
                種類：<asp:DropDownList ID="CategoryList" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </div>
        </div>
        <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" />
        <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" />
    </div>
</div>
</asp:Content>
