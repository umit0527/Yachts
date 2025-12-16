<%@ Page Title="後台 | 編輯 CompanyCategory" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditCompanyCategory-B.aspx.cs" Inherits="Yachts.BackEnd.EditCompanyCategory_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">編輯種類</h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-success card-outline">
                <div class="card-body">

                    <div class="mb-3">
                        <label for="CategoryName" class="form-label fw-bold" >種類名稱：</label>
                        <asp:TextBox ID="CategoryName" runat="server" CssClass="form-control" />
                    </div>

                    <div class="mt-4">
                        <asp:Button ID="Submit" runat="server" Text="儲 存" CssClass="btn btn-warning me-2" OnClick="Submit_Click" />
                        <asp:Button ID="Cancel" runat="server" Text="取 消" CssClass="btn btn-secondary" OnClick="Cancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
