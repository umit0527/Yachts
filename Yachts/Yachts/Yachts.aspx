<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="Yachts.aspx.cs" Inherits="Yachts.Yachts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- CSS -->
    <link href="/css/homestyle2.css?v=10" rel="stylesheet" type="text/css" />

    <!-- JS Libraries -->
    <script type="text/javascript" src="Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.cycle.all.2.74.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // 反轉輪播中的圖片順序
            var $slideshow = $('.slideshow');
            $slideshow.find('ul').html($slideshow.find('ul').children().get().reverse());

            // 初始化 Cycle 插件，從反轉後的第一張開始
            $slideshow.cycle({
                fx: 'fade',
                startingSlide: 7,  // 現在第一張是原本的最後一張
                timeout: 5000,      // 每 4 秒自動播放
                next: null,         // 不自動指定下一張
                prev: null,         // 不自動指定上一張

            });

            // 事件代理處理點擊切換大圖
            $('.bannerimg ul').delegate('li.info', 'click', function (e) {
                e.preventDefault();
                var index = $(this).data('index');
                console.log('切換到大圖索引:', index);
                $('.slideshow').cycle(index);
            });

            // 右箭頭點擊事件
            $('#arrowRight').click(function () {
                $('.slideshow').cycle('prev');  // 使用 Cycle 的 prev 方法切換到上一張
            });

            // 左箭頭點擊事件
            $('#arrowLeft').click(function () {
                $('.slideshow').cycle('next');  // 使用 Cycle 的 next 方法切換到下一張
            });
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
    <div id="abgne-block-20110111">
        <div class="bd">
            <div class="banner">
                <!--大圖開始-->
                <ul class="slideshow">
                    <asp:Repeater ID="rptCarouselMainImg" runat="server">
                        <ItemTemplate>
                            <li class="info" style="width:100%;">
                                <asp:Image ID="MainImgPath" runat="server" ImageUrl='<%# "~/Uploads/Photos/"+ Eval("CarouselImgPath") %>' style="width:967px; height:371px;"/>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <!--大圖結束-->
                <div id="buttom01">
                    <a href="#">
                        <img id="arrowLeft" src="images/buttom01.gif" alt="next"/>
                    </a>
                </div>
                <!--小圖開始-->
                <div class="bannerimg text-center">
                    <ul>
                        <asp:Repeater ID="rptCarouselImg" runat="server">
                            <ItemTemplate>
                                <li class="info" data-index="<%# Container.ItemIndex %>">
                                    <a href="#">
                                        <div class="">
                                            <p class="bannerimg_p">
                                                <asp:Image ID="SecondImgPath" runat="server" ImageUrl='<%# "~/Uploads/Photos/"+ Eval("CarouselImgPath") %>' />
                                            </p>
                                        </div>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <!--小圖結束-->
                <div id="buttom02">
                    <a href="#">
                        <img id="arrowRight" src="images/buttom02.gif" alt="next" />
                    </a>
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
                    <asp:Repeater ID="rptMdoel" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href='Yachts.aspx?ModelId=<%# Eval("Id") %>'><%# Eval("ModelName") %></a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        <!--------------------------------左邊選單結束---------------------------------------------------->


        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb">
            <a href="Index.aspx">Home</a> >> <a href='<%= "Yachts.aspx?ModelId=" + Request.QueryString["ModelId"] %>'>Yachts </a>>> 
    <a href="<%= "Yachts.aspx?ModelId=" + Request.QueryString["ModelId"] %>">
        <asp:Label ID="Model1" runat="server" Text="Label" Style="color: rgb(52, 169, 212);"></asp:Label>
    </a>
        </div>
        <div class="right">
            <div class="right1">
                <div class="title">
                    <span>
                        <asp:Label ID="Model2" runat="server" Text="Label" Style="color: rgb(52, 169, 212);"></asp:Label></span>
                </div>
                <!--次選單-->
                <div class="menu_y">
                    <ul>
                        <li class="menu_y00">YACHTS</li>
                        <li><a class="menu_yli01" href='<%= "Yachts.aspx?ModelId=" + Request.QueryString["ModelId"] %>'>Interior</a></li>
                        <li><a class="menu_yli02" href='<%= "YachtsLayout.aspx?ModelId=" + Request.QueryString["ModelId"] %>'>Layout & deck plan</a></li>
                        <li><a class="menu_yli03" href="<%= "YachtsSpecification.aspx?ModelId=" + Request.QueryString["ModelId"] %>">Specification</a></li>
                    </ul>
                </div>
                <!--次選單-->
                <!--------------------------------內容開始---------------------------------------------------->
                <div class="box1">
                    <asp:Label ID="txtContent" runat="server" Text="Label"></asp:Label>
                    <br />
                    <br />
                </div>
                <div class="box3">
                    <h4 class="fw-bold">PRINCIPAL DIMENSION</h4>
                    <%-- 您可以移除 lblPrincipalCount，因為這個版型中沒有明確的位置顯示計數。如果您需要顯示，可以考慮放在 <h4> 標籤附近。 --%>
                    <%-- <asp:Label ID="lblPrincipalCount" runat="server" ForeColor="Red" /> --%>

                    <table class="table02">
                        <tr>
                            <td class="table02td01">
                                <table>
                                    <asp:Repeater ID="rptPrincipal" runat="server">
                                        <ItemTemplate>
                                            <tr class='<%# Container.ItemIndex % 2 != 0 ? "tr003" : "" %>'>
                                                <th><%# Eval("PrincipalName") %></th>
                                                <td><%# Eval("PrincipalValue").ToString().Replace("\n", "<br />") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                            <td>
                                <asp:Image ID="InteriorImgPath" runat="server" Style="max-width: 100%; height: auto;" />
                            </td>
                        </tr>
                    </table>
                </div>
                <p class="topbuttom">
                    <a href="#">
                        <img src="images/top.gif" alt="top" /></a>
                </p>

                <!--下載開始-->
                <div class="downloads">
                    <p>
                        <img src="images/downloads.gif" alt="&quot;&quot;" />
                    </p>
                    <ul>
                        <asp:Repeater ID="rptDownloads" runat="server">
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton
                                        ID="lnkDownload"
                                        runat="server"
                                        Text='<%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %>'
                                        CommandArgument='<%# Eval("Id") %>'
                                        OnClick="lnkDownload_Click" />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
            <!--下載結束-->
            <!--------------------------------內容結束------------------------------------------------------>
        </div>
        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>
</asp:Content>
