<%@ Page Title="後台 | Contact" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="Contact-B.aspx.cs" Inherits="Yachts.BackEnd.Contact_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">Contact 紀錄</h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-outline card-warning">
                <div class="card-body table-responsive">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="table table-bordered table-hover text-nowrap">
                        <HeaderStyle CssClass="bg-primary text-white" />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="姓名" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Phone" HeaderText="電話" />
                            <asp:BoundField DataField="CountryName" HeaderText="國家" />
                            <asp:BoundField DataField="DisplayName" HeaderText="船型" />
                            <asp:BoundField DataField="Comments" HeaderText="內容" />
                            <asp:BoundField DataField="SendedAt" HeaderText="寄出時間" DataFormatString="{0:yyyy-MM-dd HH:mm}" HtmlEncode="false" />
                        </Columns>
                    </asp:GridView>
                    <%--分頁--%>
                    <div class="text-center my-4">
                        <asp:Repeater ID="rptPagination" runat="server">
                            <ItemTemplate>
                                <a href='Contact-B.aspx?page=<%# Container.DataItem %>'
                                    class='<%# (Request.QueryString["page"] == Container.DataItem.ToString() ||
    (Request.QueryString["page"] == null && Convert.ToInt32(Container.DataItem) == 1))
    ? "btn btn-warning mx-1 text-light" : "btn btn-outline-warning mx-1" %>'>
                                    <%# Container.DataItem %>
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
