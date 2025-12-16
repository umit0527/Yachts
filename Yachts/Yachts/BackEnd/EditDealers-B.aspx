<%@ Page Title="後台 | 編輯 Dealers" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditDealers-B.aspx.cs" Inherits="Yachts.BackEnd.EditDealers_B" %>
<%@ Register TagPrefix="ckeditor" Namespace="CKEditor.NET" Assembly="CKEditor.NET" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">編輯 Dealers</h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-secondary card-outline">
                <div class="card-body">
                    <div class="mb-3">
                        <label for="CountryList" class="form-label fw-bold">國家 *</label>
                        <asp:RequiredFieldValidator
    ID="RequiredFieldValidator1"
    runat="server"
    ControlToValidate="CountryList"
    ErrorMessage="請選擇國家"
    ForeColor="Red"
    Display="Dynamic">
</asp:RequiredFieldValidator>
                        <asp:DropDownList ID="CountryList" runat="server" CssClass="form-select" />
                    </div>

                    <div class="mb-3">
                        <label for="CKEditor1" class="form-label fw-bold">內容 *</label>
                        <asp:RequiredFieldValidator
    ID="RequiredFieldValidator2"
    runat="server"
    ControlToValidate="CKEditor1"
    ErrorMessage="請輸入內容"
    ForeColor="Red"
    Display="Dynamic">
</asp:RequiredFieldValidator>
                        <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="300px" />
                    </div>

                    <div class="mt-4">
                        <asp:Button ID="btnSubmit" runat="server" Text="送 出" CssClass="btn btn-warning me-2" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取 消" CssClass="btn btn-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>

                </div>
            </div>
        </div>
    </section>
</asp:Content>
