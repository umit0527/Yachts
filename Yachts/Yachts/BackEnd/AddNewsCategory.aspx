<%@ Page Title="後台 | 新增 NewsCategory" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="AddNewsCategory.aspx.cs" Inherits="Yachts.BackEnd.AddNewsCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">新增 News 種類</h3> 
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-info card-outline"> 
                <div class="card-header">
                    <h3 class="card-title">種類資料</h3> 
                </div>
                <div class="card-body">
                    <div class="form-group mb-3"> 
                        種類名稱：<asp:TextBox ID="CategoryName" runat="server" CssClass="form-control w-25"></asp:TextBox>
                    </div>
                    <br /> 
                </div>
                <div class="card-footer text-right"> 
                    <asp:Button ID="Submit" runat="server" Text="送 出" OnClick="Submit_Click" CssClass="btn btn-warning" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" CssClass="btn btn-secondary ml-2" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>