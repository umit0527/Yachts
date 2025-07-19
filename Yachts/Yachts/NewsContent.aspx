<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="NewsContent.aspx.cs" Inherits="Yachts.NewsContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/homestyle2.css?v=1" rel="stylesheet" type="text/css" />
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
     <a href='<%= "News.aspx?CategoryId=2"  %>'>
         <asp:Label ID="Label1" runat="server" Text="Label" Style="color: rgb(52, 169, 212);"></asp:Label>
     </a>
            </div>
            <div class="right">
                <div class="right1">
                    <div class="" style="color: #34A9D4;
    font-weight: bold;
    font-size: 120%;
    padding-left: 21px;
    float: left;
    margin-top: 5px;
    width: 100%;
    background-image: url(../images/icon005.gif);
    background-repeat: no-repeat;
    border-bottom-width: 1px;
    border-bottom-style: solid;
    border-bottom-color: #d3d3d3;
    padding-bottom: 7px;
    background-position: 0px 0px;
    margin-bottom: 10px;">
                        <span>
                            <asp:Label ID="Label2" runat="server" Text="Label" Style="color: rgb(52, 169, 212);"></asp:Label></span>
                    </div>

                    <!--------------------------------內容開始---------------------------------------------------->
                    <%--<div></div>--%>
                    <asp:Repeater ID="rptNewsContent" runat="server">
                        <ItemTemplate>
                                <div class="d-flex box3">
                                    <div class="">
                                        <div class="fw-bold" style="background-image: url(../images/icon007.gif);
    background-repeat: no-repeat;
    background-position: 1px 3px;
    padding-left: 10px;
    color: #34A9D4;
    font-size: 110%;
    padding-bottom: 10px;">
                                           <%# Eval("Title") %>
                                        </div>
                                        <div>
                                            <%# Eval("content") %>
                                        </div>
                                    </div>
                                </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <!--------------------------------內容結束------------------------------------------------------>
            </div>
            <!--------------------------------右邊選單結束---------------------------------------------------->
        </div>
</asp:Content>
