<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="Yachts.Company" %>

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
                <p><span>COMPANY</span></p>
                <div>
                    <asp:Repeater ID="rptCompany" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href='Company.aspx?CategoryId=<%# Eval("Id") %>'><%# Eval("Name") %></a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        <!--------------------------------左邊選單結束---------------------------------------------------->

        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb">
            <a href="#">Home</a> >> <a href="#">Company </a>>> 
        <a href="#">
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
                <asp:Repeater ID="rptContent" runat="server">
                    <ItemTemplate>
                        <div style="padding-bottom: 10px; border-bottom-width: 1px; border-bottom-color: #CCCCCC; border-bottom-style: dashed; float: left;">
                            <h4 style="font-weight:bold; background-image: url(../images/icon007.gif); background-repeat: no-repeat; background-position: 1px 3px; padding-left: 10px; color: #34A9D4; font-size: 110%; padding-bottom: 10px;">
                                <%# Eval("Title") %></h4>
                            <p><%# Eval("content") %></p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!--------------------------------內容結束------------------------------------------------------>
        </div>

        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>

</asp:Content>
