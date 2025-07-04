<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="AddCountry.aspx.cs" Inherits="Yachts.BackEnd.AddContry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    國家名稱：<asp:TextBox ID="CountryName" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" />
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" />
</asp:Content>
