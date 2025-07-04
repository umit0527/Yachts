<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditCountry-B.aspx.cs" Inherits="Yachts.BackEnd.EditCountry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    國家名稱：<asp:TextBox ID="CountryName" runat="server"></asp:TextBox>
<br />
<asp:Button ID="Submit" runat="server" Text="送 出" OnClick="Submit_Click" />
&nbsp;&nbsp;&nbsp;
<asp:Button ID="Cancel" runat="server" Text="取 消" OnClick="Cancel_Click" />
</asp:Content>
