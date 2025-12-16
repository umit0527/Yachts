<%@ Page Title="後台 | 編輯 Company" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditCompany-B.aspx.cs" Inherits="Yachts.BackEnd.EditCompany_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">編輯 Company 內容</h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-success card-outline">
                <div class="card-body">

                    <div class="mb-3">
                        <label for="txtTitle" class="form-label fw-bold">標題 *</label>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator10"
                            runat="server"
                            ControlToValidate="txtTitle"
                            ErrorMessage="請輸入標題"
                            ForeColor="Red"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Width="100%" />
                    </div>

                    <div class="mb-3">
                        <label for="CategoryList" class="form-label fw-bold">種類 *</label>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1"
                            runat="server"
                            ControlToValidate="CategoryList"
                            ErrorMessage="請選擇國家"
                            ForeColor="Red"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:DropDownList ID="CategoryList" runat="server"
                            AutoPostBack="True"
                            CssClass="form-select"
                            OnSelectedIndexChanged="CategoryList_SelectedIndexChanged" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">內容：</label>
                        <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="500px" />
                    </div>

                    <div class="mt-4">
                        <asp:Button ID="btnSubmit" runat="server" Text="儲 存" CssClass="btn btn-warning me-2" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取 消" CssClass="btn btn-secondary" OnClick="btnCancel_Click" CausesValidation="false"/>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
