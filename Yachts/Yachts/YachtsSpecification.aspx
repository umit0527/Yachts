<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="YachtsSpecification.aspx.cs" Inherits="Yachts.YachtsSpecification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/homestyle2.css" rel="stylesheet" type="text/css" />
    <!-- JS Libraries -->
    <link rel="stylesheet" type="text/css" href="/css/jquery.ad-gallery.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.ad-gallery.js"></script>
    <script type="text/javascript">
        $(function () {
            var galleries = $('.ad-gallery').adGallery();
            galleries[0].settings.effect = 'slide-hori';
        });
</script>

    <!--[if lt IE 7]>
    <script type="text/javascript" src="javascript/iepngfix_tilebg.js"></script>
    <![endif]-->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- 遮罩 -->
    <div class="bannermasks">
        <img src="images/banner01_masks.png" alt="" />
    </div>

    <!--------------------------------換圖開始---------------------------------------------------->
    <div class="banner">
        <div id="gallery" class="ad-gallery">
            <div class="ad-image-wrapper">
            </div>
            <div class="ad-controls" style="display: none"></div>
            <div class="ad-nav">
                <div class="ad-thumbs">
                    <ul class="ad-thumb-list">
                        <asp:Repeater ID="rptCarouselImg" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href='<%# ResolveUrl("~/Uploads/Photos/" + Eval("CarouselImgPath")) %>'>
                                        <img src='<%# ResolveUrl("~/Uploads/Photos/" + Eval("CarouselImgPath")) %>' alt="輪播圖" style="width: 99px; height: 59px;" />
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <!--------------------------------換圖結束---------------------------------------------------->


    <div class="conbg">
        <!--------------------------------左邊選單開始---------------------------------------------------->
        <div class="left">
            <div class="left1">
                <p><span>YACHTS</span></p>
                <div>
                    <asp:Repeater ID="rptModelList" runat="server" OnItemDataBound="rptModelList_ItemDataBound">
                        <ItemTemplate>
                            <li style="position: relative;">
                                <div class="row">
                                    <div class="d-flex align-items-center col-12 ">
                                        <a class="w-100" href='Yachts.aspx?ModelId=<%# Eval("Id") %>'>
                                            <%# Eval("ModelName") %>
                                            <asp:Label ID="lblTag" Text="text" runat="server" CssClass="me-1 text-end" /></a>
                                    </div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        <!--------------------------------左邊選單結束---------------------------------------------------->


        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb">
            <a href="Index.aspx">Home</a> >> <a href="<%= "Yachts.aspx?ModelId=" + Request.QueryString["ModelId"] %>">Yachts </a>>> 
            <a href="<%= "YachtsSpecification.aspx?ModelId=" + Request.QueryString["ModelId"] %>">
                <asp:Label ID="txtModel1" runat="server" Text="Label" Style="color: rgb(52, 169, 212);"></asp:Label>
            </a>
        </div>
        <div class="right">
            <div class="right1">
                <div class="title">   
                        <asp:Label ID="txtModel2" runat="server" Text="Label" Style="color: rgb(52, 169, 212);"></asp:Label>
                </div>
                <!--次選單-->
                <div class="menu_y">
                    <ul>
                        <li class="menu_y00">YACHTS</li>
                        <li><a class="menu_yli01" href='<%= "Yachts.aspx?ModelId=" + Request.QueryString["ModelId"] %>'>Interior</a></li>
                        <li><a class="menu_yli02" href='<%= "YachtsLayout.aspx?ModelId=" + Request.QueryString["ModelId"] %>'>Layout & deck plan</a></li>
                        <li><a class="menu_yli03" href='<%="YachtsSpecification.aspx?ModelId=" + Request.QueryString["ModelId"] %>'>Specification</a></li>
                    </ul>
                </div>
                <!--次選單-->
                <!--------------------------------內容開始---------------------------------------------------->

                <div class="box5">
                    <h4>DETAIL SPECIFICATION</h4>
                    <asp:Label ID="txtSpecificaiotn" Text="text" runat="server" />
                </div>
                
                <p class="topbuttom">
                    <a href="#">
                        <img src="images/top.gif" alt="top" /></a>
                </p>
                <!--------------------------------內容結束------------------------------------------------------>
            </div>
            <!--------------------------------右邊選單結束---------------------------------------------------->
        </div>
    </div>
</asp:Content>
