<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="Yachts.News" %>
<%@ Register TagPrefix="uc1" TagName="Pagination" Src="~/Pagination.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/homestyle2.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!--遮罩-->
    <div class="bannermasks">
        <img src="/images/Company1.jpg" alt="&quot;&quot;" width="967" height="371" />
    </div>
    <!--遮罩結束-->

    <!--------------------------------換圖開始---------------------------------------------------->

    <div class="banner">
        <ul>
            <li>
                <img src="/images/newbanner.jpg" alt="Tayana Yachts" /></li>
        </ul>

    </div>
    <!--------------------------------換圖結束---------------------------------------------------->

    <div class="conbg">
        <!--------------------------------左邊選單開始---------------------------------------------------->
        <div class="left">
            <div class="left1">
                <p><span>NEWS</span></p>
                <div>
                    <asp:Repeater ID="rptNewsCategory" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href='News.aspx?CategoryId=<%# Eval("Id") %>'><%# Eval("Name") %></a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        <!--------------------------------左邊選單結束---------------------------------------------------->

        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb">
            <a href="Index.aspx">Home</a> >> <a href='<%= "News.aspx?CategoryId=" + Request.QueryString["CategoryId"] %>'>News </a>>> 
        <a href="<%= "News.aspx?CategoryId=" + Request.QueryString["CategoryId"] %>">
            <asp:Label ID="Label1" runat="server" Text="Label" Style="color: rgb(52, 169, 212);"></asp:Label>
        </a>
        </div>
        <div class="right">
            <div class="right1">
                <div class="title">
                    <span>
                        <asp:Label ID="Label2" runat="server" Text="Label" Style="color: rgb(52, 169, 212);"></asp:Label></span>
                </div>

                <!--------------------------------內容開始---------------------------------------------------->
                <div class="box2_list">
                    <asp:Repeater ID="rptNewsAlbum" runat="server">
                        <ItemTemplate>
                            <a href='<%# Eval("Id", "NewsContent.aspx?Id={0}") %>' class="link-dark">
                                <div class="d-flex list01" style="margin: 5px; padding: 10px; border-bottom-width: 1px; border-bottom-color: #CCCCCC; border-bottom-style: dashed; float: left;">
                                    <div class="news-cover-wrapper" style="position:relative ; float: left; overflow: hidden; padding: 5px; border: 1px solid #CCCCCC; margin-right: 10px;">
                                        <img class="me-0" src='<%# ResolveUrl("~/Uploads/Photos/") + Eval("CoverPath") %>' alt="封面圖片" width="181px" height="121px"/>
                                        <%# (bool)Eval("Sticky") ? "<img style='left:0px; top:0px; position:absolute;'   src='/images/new_top01.png' />" : "" %>
                                    </div>
                                    <div class="">
                                        <div>
                                            <span><%# Eval("CreatedAt", "{0:yyyy/MM/dd}") %></span>
                                        </div>
                                        <div>
                                            <p><%# Eval("Title") %></p>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <%--分頁開始--%>
            <div class="align-items-center">
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <uc1:pagination runat="server" id="Pagination" />
            </div>
            <!--------------------------------內容結束------------------------------------------------------>
        </div>
        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>
</asp:Content>
