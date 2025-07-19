<%@ Page Title="" Language="C#" MasterPageFile="~/Site-F.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Yachts.FrontEnd.Contact" %>
<%@ Register Assembly="Recaptcha.Web" Namespace="Recaptcha.Web.UI.Controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/homestyle2.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<!--遮罩-->
        <div class="bannermasks">
            <img src="/images/contact.jpg" alt="&quot;&quot;" width="967" height="371" />
        </div>
        <!--遮罩結束-->--%>
    <div class="banner">
        <ul>
            <li>
                <img src="/images/contact01.jpg" alt="Tayana Yachts" />
            </li>
        </ul>
    </div>
    <!--------------------------------換圖結束---------------------------------------------------->

    <div class="conbg">
        <!--------------------------------左邊選單開始---------------------------------------------------->
        <div class="left">
            <div class="left1">
                <p><span>CONTACT</span></p>
                <ul>
                    <li><a href="Contact.aspx">contacts</a></li>
                </ul>
            </div>
        </div>
        <!--------------------------------左邊選單結束---------------------------------------------------->


        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb"><a href="Index.aspx">Home</a> >> <a href="Contact.aspx"><span class="on1">Contact</span></a></div>
        <div class="right">
            <div class="right1">
                <div class="title"><span>Contact</span></div>
                <!--------------------------------內容開始---------------------------------------------------->
                <!--表單-->
                <div class="from01">
                    <p>
                        Please Enter your contact information<span class="span01">*Required</span>
                    </p>
                    <br />
                    <table>
                        <tr>
                            <td class="from01td01">Name :</td>
                            <td><span>*</span><asp:TextBox ID="TextName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="from01td01">Email :</td>
                            <td><span>*</span><asp:TextBox ID="TextEmail" runat="server" TextMode="Email"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="from01td01">Phone :</td>
                            <td><span>*</span><asp:TextBox ID="TextPhone" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="from01td01">Country :</td>
                            <td><span>*</span>
                                <asp:DropDownList ID="CountryList" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td colspan="2"><span>*</span>Brochure of interest  *Which Brochure would you like to view?</td>
                        </tr>
                        <tr>
                            <td class="from01td01">&nbsp;</td>
                            <td>
                                <asp:DropDownList ID="ModelList" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="from01td01">Comments:</td>
                            <td>
                                <asp:TextBox ID="TextComments" runat="server" TextMode="MultiLine" Columns="45" Rows="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                驗證
                            </td>
                            <td> 
                                <div class="d-flex align-items-center">
                                <cc1:RecaptchaApiScript ID="RecaptchaApiScript1" runat="server" />
                                <cc1:RecaptchaWidget ID="Recaptcha1" runat="server" RenderApiScript="False"/>
                                <asp:Label ID="lblMessage" Text="text" runat="server" Visible="false" ForeColor="Red"/>
                           </div> </td>
                        </tr>
                        <tr>
                            <td class="from01td01">&nbsp;</td>
                            <td class="f_right">
                                <asp:ImageButton ID="Submit" runat="server" Text="submit" OnClick="Submit_Click" width="59" height="25" src="/images/buttom03.gif"/></td>      
                        </tr>
                    </table>
                </div>
                <!--表單-->

                <div class="box1">
                    <span class="span02">Contact with us</span><br />
                    Thanks for your enjoying our web site as an introduction to the Tayana world and our range of yachts.
                    As all the designs in our range are semi-custom built, we are glad to offer a personal service to all our potential customers. 
                    If you have any questions about our yachts or would like to take your interest a stage further, please feel free to contact us.
                </div>

                <div class="list03">
                    <p>
                        <span>TAYANA HEAD OFFICE</span><br />
                        NO.60 Haichien Rd. Chungmen Village Linyuan Kaohsiung Hsien 832 Taiwan R.O.C<br />
                        tel. +886(7)641 2422<br />
                        fax. +886(7)642 3193<br />
                        info@tayanaworld.com<br />
                    </p>
                </div>

                <div class="list03">
                    <p>
                        <span>SALES DEPT.</span><br />
                        +886(7)641 2422  ATTEN. Mr.Basil Lin<br />
                        <br />
                    </p>
                </div>

                <div class="box4">
                    <h4>Location</h4>
                    <p>
                        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d14732.003166429073!2d120.29972145265626!3d22.61644562136512!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x346e0491b7febacd%3A0x24542bac2726199b!2z5a-25oiQ5LiW57SA5aSn5qiT!5e0!3m2!1szh-TW!2stw!4v1752724617135!5m2!1szh-TW!2stw" width="717" height="450" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
                    </p>
                </div>
                <!--------------------------------內容結束------------------------------------------------------>
            </div>
        </div>
        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>

</asp:Content>
