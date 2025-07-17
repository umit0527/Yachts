<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="Dealers.aspx.cs" Inherits="Yachts.FrontEnd.Dealers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="/css/homestyle2.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!--遮罩-->
    <div class="bannermasks">
        <img src="/images/DEALERS01.jpg" alt="&quot;&quot;" width="967" height="371" />
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
                <p><span>DEALERS</span></p>
                <div>
                    <asp:Repeater ID="rptCountry" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href='Dealers.aspx?CountryId=<%# Eval("Id") %>'><%# Eval("Name") %></a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        <!--------------------------------左邊選單結束---------------------------------------------------->


        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb"><a href="#">Home</a> >> <a href="#">Dealers </a>>> 
            <a href="#">
                <asp:Label ID="Label1" runat="server" Text="Label" style="color:rgb(52, 169, 212);"></asp:Label>
            </a></div>
        <div class="right">
            <div class="right1">
                <div class="title"><span><asp:Label ID="Label2" runat="server" Text="Label" style="color:rgb(52, 169, 212);"></asp:Label></span></div>

                <!--------------------------------內容開始---------------------------------------------------->
                <asp:Repeater ID="rptContent" runat="server">
                    <ItemTemplate>
                        <div style="margin: 5px; padding: 10px; border-bottom-width: 1px; border-bottom-color: #CCCCCC; border-bottom-style: dashed; float: left;">
                            <p><%# Eval("content") %></p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <%--分頁開始--%>
                <div class="pagenumber">| <span>1</span> | <a href="#">2</a> | <a href="#">3</a> | <a href="#">4</a> | <a href="#">5</a> |  <a href="#">Next</a>  <a href="#">LastPage</a></div>
                <div class="pagenumber1">Items：<span>89</span>  |  Pages：<span>1/9</span></div>
                <%--分頁結束--%>
            </div>

            <!--------------------------------內容結束------------------------------------------------------>
        </div>

        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>

</asp:Content>
