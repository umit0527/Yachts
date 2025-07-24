<%@ Page Title="後台 | 新增 Country" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="AddCountry.aspx.cs" Inherits="Yachts.BackEnd.AddContry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">新增 Country</h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-secondary card-outline">
                <div class="card-body">
                    <div class="form-group">
                        <label for="CountryName">國家名稱：</label>
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

