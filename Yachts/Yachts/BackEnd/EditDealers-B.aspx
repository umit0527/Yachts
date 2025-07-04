<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditDealers-B.aspx.cs" Inherits="Yachts.BackEnd.EditDealers_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content-wrapper">
        <div>
            <div class="d-flex">
                <div>
                    國家：<asp:DropDownList ID="CountryList" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="CountryList_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="d-flex">
                    城市：<asp:DropDownList ID="CityList" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/"
                Height="300px" />
            <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
