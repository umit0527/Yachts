<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Yachts.FrontEnd.Home1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/style1.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contain">
        <div>
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
                            <%--大圖開始--%>
                            <asp:Repeater ID="rptBigBanner" runat="server" OnItemDataBound="rptBigBanner_ItemDataBound">
                                <ItemTemplate>
                                    <li class="info <%# Container.ItemIndex == 0 ? "on" : "" %>">
                                            <asp:Image runat="server" ImageUrl='<%# "~/Uploads/Photos/" + Eval("CarouselImageImgPath") %>' AlternateText="" style="width:966px; height:424px; object-fit:contain;"/>
                                            <asp:Image ID="imgLabel" runat="server" Style="left: 18px; top: 113px; position: absolute;" Visible="false" />
                                        <!--文字開始-->
                                        <div class="wordtitle">
                                            <%# Eval("ModelName") %> <span><%# Eval("ModelNumber") %></span><br />
                                            <p>SPECIFICATION SHEET</p>
                                        </div>
                                        <!--文字結束-->
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <!--小圖開始-->
                        <div class="bannerimg title">
                            <ul>
                                <asp:Repeater ID="rptCarouselImg" runat="server">
                                    <ItemTemplate>
                                        <li class="" data-index="<%# Container.ItemIndex %>">
                                            <a href="#">
                                                <p class="bannerimg_p">
                                                    <asp:Image ID="CarouselImageImgPath" runat="server" ImageUrl='<%# "~/Uploads/Photos/"+ Eval("CarouselImageImgPath") %>' />
                                                </p>
                                            </a>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
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
                <img src="/images/news.gif" alt="news" />
            </p>
            <p class="newstitlep2"><a href="News.aspx" style="width:95px;height:95px;">More>></a></p>
        </div>

        <ul>
            <!--最新消息-->
            <asp:Repeater ID="rptNews" runat="server">
                <ItemTemplate>
                    <li>
                        <div class="news01">
                            <div class="newstop">
                                <%# Convert.ToBoolean(Eval("Sticky")) ? "<img src='/images/new_top01.png' alt='TOP' />" : "" %>
                            </div>
                            <div class="news02p1">
                                <p class="news02p1img">
                                    <img src='<%# ResolveUrl("~/Uploads/Photos/" + Eval("CoverPath")) %>' alt="news image" width="100%"/>
                                </p>
                            </div>
                            <p class="news02p2">
                                <span><%# Convert.ToDateTime(Eval("CreatedAt")).ToString("yyyy-MM-dd") %></span>
                                <a href='NewsContent.aspx?Id=<%# Eval("NewsId") %>'><%# Eval("Title") %></a>
                            </p>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
        <!--------------------------------最新消息結束---------------------------------------------------->
    </div>
</asp:Content>
