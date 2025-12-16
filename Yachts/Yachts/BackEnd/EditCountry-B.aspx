<%@ Page Title="後台 | 編輯 Country" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditCountry-B.aspx.cs" Inherits="Yachts.BackEnd.EditCountry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">編輯 Country</h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-secondary card-outline">
                <div class="card-body">
                    <div class="form-group">
                        <label for="CountryName" class="fw-bold">國家名稱：</label>
                        <asp:TextBox ID="CountryName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="mt-4">
                        <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" CssClass="btn btn-warning" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" CssClass="btn btn-secondary" />
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
