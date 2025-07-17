<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditYachts-B.aspx.cs" Inherits="Yachts.BackEnd.EditYachts_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mx-0">
        <div class="col-12">
            <div class="d-flex">
                船名：<asp:TextBox ID="ModelName" runat="server"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;
    型號：<asp:TextBox ID="ModelNum" runat="server"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        標籤：<asp:RadioButtonList ID="rblModelLabel" runat="server" RepeatDirection="Horizontal" Width="280px">
            <asp:ListItem Text="Genaral" />
            <asp:ListItem Text="New Design" />
            <asp:ListItem Text="New Building" />
        </asp:RadioButtonList>
            </div>
        </div>
    </div>
    <br />
    <div class="row mx-0">
        <div class="col-6">
            介紹：<br />
            <asp:TextBox ID="Introduce" runat="server" TextMode="MultiLine" Width="500px" Height="319px"></asp:TextBox>

        </div>
        <div class="col-6">
            尺寸與設計參數：
        <br />
            <!-- 開始新增欄位 -->
            <div>
                <asp:Repeater ID="rptPendingFields" runat="server"
                    OnItemCommand="rptPendingFields_ItemCommand"
                    OnItemDataBound="rptPendingFields_ItemDataBound">
                    <ItemTemplate>
                        <div>
                            <%-- 檢查是否是編輯狀態 --%>
                            <asp:PlaceHolder ID="phView" runat="server" Visible='<%# Container.ItemIndex != (int)(ViewState["PendingEditIndex"] ?? -1) %>'>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' />
                                ：
                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' />
                                <asp:Button ID="btnEditPending" runat="server" Text="編輯" CommandName="Edit" CommandArgument='<%# Container.ItemIndex %>' />
                                <asp:Button ID="btnDeletePending" runat="server" Text="刪除" CommandName="Delete" CommandArgument='<%# Container.ItemIndex %>' />
                            </asp:PlaceHolder>

                            <%-- 編輯模式 --%>
                            <asp:PlaceHolder ID="phEdit" runat="server" Visible='<%# Container.ItemIndex == (int)(ViewState["PendingEditIndex"] ?? -1) %>'>
                                <asp:TextBox ID="txtEditName" runat="server" Text='<%# Eval("Name") %>' />
                                <asp:TextBox ID="txtEditValue" runat="server" Text='<%# Eval("Value") %>' />
                                <asp:Button ID="btnUpdatePending" runat="server" Text="更新" CommandName="Update" CommandArgument='<%# Container.ItemIndex %>' />
                                <asp:Button ID="btnCancelPending" runat="server" Text="取消" CommandName="Cancel" CommandArgument='<%# Container.ItemIndex %>'/>
                            </asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlInputFields" runat="server" Visible="false">
                    欄位名稱：<asp:TextBox ID="txtNewName" runat="server" />
                    欄位值：<asp:TextBox ID="txtNewValue" runat="server" />
                    <asp:Button ID="btnAddField" runat="server" Text="送 出" OnClick="btnAddField_Click" />
                    <asp:Button ID="btnCancelField" runat="server" Text="取 消" OnClick="btnCancelField_Click" />
                </asp:Panel>
                <br />
                <asp:Button ID="btnAddNew" runat="server" Text="新增欄位" OnClick="btnAddNew_Click" />

            </div>
            <!-- 結束新增欄位 -->
        </div>
        <div class="row mx-0">
            <div class="col-12">
                檔案下載：
            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
            </div>
        </div>
    </div>
    <asp:Repeater ID="rptEditFiles" runat="server" OnItemCommand="rptEditFiles_ItemCommand">
        <ItemTemplate>
            <p>
                <%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %>
                <asp:LinkButton ID="btnDeleteFile" runat="server"
                    CommandName="DeleteFile"
                    CommandArgument='<%# Eval("Id") %>'
                    OnClientClick="return confirm('確定要刪除這個檔案嗎？');">
                    刪除
                </asp:LinkButton>
            </p>
        </ItemTemplate>
    </asp:Repeater>

    <div class="row mx-0">
        <div class="col-4">
            <br />
            設計圖：
            <asp:FileUpload ID="FUInteriorImg" runat="server" />
            <div style="width: 300px;">
                <br />
                <asp:Image ID="InteriorImg" runat="server" Width="100%" />
            </div>
        </div>
        <div class="col-4">
            <br />
            平面圖1：
            <asp:FileUpload ID="FUDeckImg1" runat="server" />
            <div style="width: 300px;">
                <br />
                <asp:Image ID="DeckImg1" runat="server" Width="100%" />
            </div>
        </div>
        <div class="col-4">
            <br />
            平面圖2：
        <asp:FileUpload ID="FUDeckImg2" runat="server" />
            <div style="width: 300px;">
                <br />
                <asp:Image ID="DeckImg2" runat="server" Width="100%" />
            </div>
        </div>
    </div>
    <br />
    <div class="row mx-0">
        <div class="col-12">
            輪播圖：
            <asp:FileUpload ID="FUCarouselImgPath" runat="server" AllowMultiple="true" />
            <br />
            <br />
            <div class="row mx-0">
                <asp:Repeater ID="rptCarouselImgs" runat="server" OnItemCommand="btnDeleteCarouselImg">
                    <ItemTemplate>
                        <div class="img-fluid mb-2 col-2 text-center">
                            <img src='<%# ResolveUrl("/Uploads/Photos/") + Eval("CarouselImgPath") %>' class="img-thumbnail" />
                            <br />
                            <asp:LinkButton ID="btnDeleteCarouselImg" runat="server" Text="刪除"
                                CommandName="DeleteCarouselImg"
                                CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-danger btn-sm mt-1"
                                OnClientClick="return confirm('確定要刪除這張圖片嗎？');">
                            </asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <div class="row mx-0">
        <div class="col-12">
            <br />
            詳細規格：
        <br />
            <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="500" />
        </div>
        <br />
        <div class="col-12">
            <br />
            <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" />
            &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" />
        </div>
    </div>
    <br />
</asp:Content>
