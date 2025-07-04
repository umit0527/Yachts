<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="Index-F.aspx.cs" Inherits="Yachts.FrontEnd.Home1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="/css/style1.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contain">

        <div class="d-flex">
        <!--遮罩-->
        <div class="bannermasks">
            <img src="/images/banner00_masks.png" alt="&quot;&quot;" />
        </div>
        <!--遮罩結束-->

        <!--------------------------------換圖開始---------------------------------------------------->
        <div id="abgne-block-20110111">
            <div class="bd">
                <div class="banner">
                    <ul>
                        <li class="info on"><a href="#">
                            <img src="/images/banner001b.jpg" alt="" /></a><!--文字開始--><div class="wordtitle">TAYANA <span>48</span><br />
                                <p>SPECIFICATION SHEET</p>
                            </div>
                            <!--文字結束-->
                        </li>
                        <li class="info">
                            <a href="#">
                                <img src="/images/banner002b.jpg" alt="" /></a><!--文字開始--><div class="wordtitle">TAYANA <span>54</span><br />
                                    <p>SPECIFICATION SHEET</p>
                                </div>
                            <!--文字結束-->
                            <!--新船型開始  54型才出現其於隱藏 -->
                            <div class="new">
                                <img src="/images/new01.png" alt="new" /></div>
                            <!--新船型結束-->
                        </li>
                        <li class="info"><a href="#">
                            <img src="/images/banner003b.jpg" alt="" /></a><!--文字開始--><div class="wordtitle">TAYANA <span>37</span><br />
                                <p>SPECIFICATION SHEET</p>
                            </div>
                            <!--文字結束-->
                        </li>
                        <li class="info"><a href="#">
                            <img src="/images/banner004b.jpg" alt="" /></a><!--文字開始--><div class="wordtitle">TAYANA <span>64</span><br />
                                <p>SPECIFICATION SHEET</p>
                            </div>
                            <!--文字結束-->
                        </li>
                        <li class="info"><a href="#">
                            <img src="/images/banner005b.jpg" alt="" /></a><!--文字開始--><div class="wordtitle">TAYANA <span>58</span><br />
                                <p>SPECIFICATION SHEET</p>
                            </div>
                            <!--文字結束-->
                        </li>
                        <li class="info"><a href="#">
                            <img src="/images/banner006b.jpg" alt="" /></a><!--文字開始--><div class="wordtitle">TAYANA <span>55</span><br />
                                <p>SPECIFICATION SHEET</p>
                            </div>
                            <!--文字結束-->
                        </li>
                    </ul>
                    <!--小圖開始-->
                    <div class="bannerimg title">
                        <ul>
                            <li class="on">
                                <div>
                                    <p class="bannerimg_p">
                                        <img src="/images/i001.jpg" alt="&quot;&quot;" /></p>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <p class="bannerimg_p">
                                        <img src="/images/i002.jpg" alt="&quot;&quot;" /></p>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <p class="bannerimg_p">
                                        <img src="/images/i003.jpg" alt="&quot;&quot;" /></p>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <p class="bannerimg_p">
                                        <img src="/images/i004.jpg" alt="&quot;&quot;" /></p>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <p class="bannerimg_p">
                                        <img src="/images/i005.jpg" alt="&quot;&quot;" /></p>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <p class="bannerimg_p">
                                        <img src="/images/i006.jpg" alt="&quot;&quot;" /></p>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <!--小圖結束-->
                </div>
            </div>
        </div>
        <!--------------------------------換圖結束---------------------------------------------------->
            </div>

        <!--------------------------------最新消息---------------------------------------------------->
        <div class="news">
            <div class="newstitle">
                <p class="newstitlep1">
                    <img src="/images/news.gif" alt="news" /></p>
                <p class="newstitlep2"><a href="#">More>></a></p>
            </div>

            <ul>
                <!--TOP第一則最新消息-->
                <li>
                    <div class="newstop"><img src="/images/new_top01.png" alt="&quot;&quot;" />
                        </div>
                    <div class="news01">
                        <div class="news02p1">
                            <p class="news02p1img">
                                <img src="/images/pit002.jpg" alt="&quot;&quot;" /></p>
                        </div>
                        <p class="news02p2">
                            <span>Tayana 54 CE Certifica..</span>
                            <a href="#">For Tayana 54 entering the EU, CE Certificates are AVAILABLE to ensure conformity to all applicable European ...</a>
                        </p>
                    </div>
                </li>
                <!--TOP第一則最新消息結束-->
                <!--第二則-->
                <li>
                    <div class="news02">
                        <div class="news02p1">
                            <p class="news02p1img">
                                <img src="/images/pit001.jpg" alt="&quot;&quot;" /></p>
                        </div>
                        <p class="news02p2">
                            <span>Tayana 58 CE Certifica..</span>
                            <a href="#">For Tayana 58 entering the EU, CE Certificates are AVAILABLE to ensure conformity to all applicable European ...</a>
                        </p>
                    </div>
                </li>
                <!--第二則結束-->

                <li>
                    <div class="news02">
                        <div class="news02p1">
                            <p class="news02p1img">
                                <img src="/images/pit001.jpg" alt="&quot;&quot;" /></p>
                        </div>
                        <p class="news02p2">
                            <span>Big Cruiser in a Small ..</span>
                            <a href="#">Tayana 37 is our classical product and full of skilful craftsmanship. We only plan to build TWO units in a year.</a>
                        </p>
                    </div>
                </li>
            </ul>
        </div>
        <!--------------------------------最新消息結束---------------------------------------------------->
    </div>
</asp:Content>
