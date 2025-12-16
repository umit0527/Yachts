<%@ Page Title="" Language="C#" MasterPageFile="~/BackEnd/Site-B.Master" AutoEventWireup="true" CodeBehind="EditYachts-B.aspx.cs" Inherits="Yachts.BackEnd.EditYachts_B" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <div class="container-fluid">
            <br />
            <h3 class="mb-0">編輯遊艇資料</h3>
        </div>
    </div>
    <br />

    <section class="content">
        <div class="container-fluid">
            <div class="card card-primary card-outline">
                <div class="card-header">
                    <h3 class="card-title">基本資料</h3>
                </div>
                <div class="card-body">
                    <div class="row mb-3 align-items-center">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="ModelName" class="fw-bold">YahctsName *</label>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator9"
                                    runat="server"
                                    ControlToValidate="ModelName"
                                    ErrorMessage="請輸入名稱"
                                    ForeColor="Red"
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                                <asp:TextBox ID="ModelName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="ModelNum" class="fw-bold">YahctsNumber *</label>
                                <asp:RequiredFieldValidator
    ID="RequiredFieldValidator1"
    runat="server"
    ControlToValidate="ModelNum"
    ErrorMessage="請輸入數字"
    ForeColor="Red"
    Display="Dynamic">
</asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator 
    ID="revModelNum" 
    runat="server" 
    ControlToValidate="ModelNum" 
    ErrorMessage="請輸入數字" 
    ValidationExpression="^\d+$" 
    ForeColor="Red" 
    Display="Dynamic">
</asp:RegularExpressionValidator>
                                <asp:TextBox ID="ModelNum" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="fw-bold">Label *</label>
                                <asp:RadioButtonList ID="rblModelLabel" runat="server" RepeatDirection="Horizontal" CssClass="d-flex justify-content-between w-100">
                                    <asp:ListItem Text="Genaral" CssClass="form-check form-check-inline mx-0"></asp:ListItem>
                                    <asp:ListItem Text="New Design" CssClass="form-check form-check-inline mx-0"></asp:ListItem>
                                    <asp:ListItem Text="New Building" CssClass="form-check form-check-inline mx-0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="Introduce" class="fw-bold">Introduce *</label>
                                <asp:RequiredFieldValidator
    ID="RequiredFieldValidator3"
    runat="server"
    ControlToValidate="Introduce"
    ErrorMessage="請輸入介紹"
    ForeColor="Red"
    Display="Dynamic">
</asp:RequiredFieldValidator>
                                <asp:TextBox ID="Introduce" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="fw-bold">Principal Dimension</label>
                                <div class="card card-default card-outline">
                                    <div class="card-body">
                                        <asp:Repeater ID="rptPendingFields" runat="server"
                                            OnItemCommand="rptPendingFields_ItemCommand"
                                            OnItemDataBound="rptPendingFields_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="d-flex justify-content-between align-items-center mb-2 pb-2 border-bottom">
                                                    <%-- 檢查是否是編輯狀態 --%>
                                                    <asp:PlaceHolder ID="phView" runat="server" Visible='<%# Container.ItemIndex != (int)(ViewState["PendingEditIndex"] ?? -1) %>'>
                                                        <div>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' CssClass="font-weight-bold" />
                                                            ：
                                                            <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' />
                                                        </div>
                                                        <div>
                                                            <asp:Button ID="btnEditPending" runat="server" Text="編 輯" CommandName="Edit" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-sm btn-primary"  CausesValidation="false"/>&nbsp;&nbsp;
                                                            <asp:Button ID="btnDeletePending" runat="server" Text="刪 除" CommandName="Delete" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-sm btn-danger ml-2"  CausesValidation="false"/>
                                                        </div>
                                                    </asp:PlaceHolder>

                                                    <%-- 編輯模式 --%>
                                                    <asp:PlaceHolder ID="phEdit" runat="server" Visible='<%# Container.ItemIndex == (int)(ViewState["PendingEditIndex"] ?? -1) %>'>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtEditName" runat="server" Text='<%# Eval("Name") %>' CssClass="form-control form-control-sm" Placeholder="欄位名稱"  CausesValidation="false"/>
                                                            <asp:TextBox ID="txtEditValue" runat="server" Text='<%# Eval("Value") %>' CssClass="form-control form-control-sm ml-2" Placeholder="欄位值"  CausesValidation="false"/>
                                                            <div class="input-group-append ml-2">
                                                                &nbsp;&nbsp;<asp:Button ID="btnUpdatePending" runat="server" Text="更 新" CommandName="Update" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-sm btn-warning"  CausesValidation="false"/>&nbsp;&nbsp;
                                                                <asp:Button ID="btnCancelPending" runat="server" Text="取 消" CommandName="Cancel" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-sm btn-secondary ml-1"  CausesValidation="false"/>
                                                            </div>
                                                        </div>
                                                    </asp:PlaceHolder>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <asp:Panel ID="pnlInputFields" runat="server" Visible="false" CssClass="mb-3">
                                            <div class="input-group mb-2">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">欄位名稱：</span>
                                                </div>
                                                <asp:TextBox ID="txtNewName" runat="server" CssClass="form-control"  CausesValidation="false"/>
                                            </div>
                                            <div class="input-group mb-2">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">欄位值：</span>
                                                </div>
                                                <asp:TextBox ID="txtNewValue" runat="server" CssClass="form-control"  CausesValidation="false"/>
                                            </div>
                                            <div class="text-right">
                                                <asp:Button ID="btnAddField" runat="server" Text="送 出" OnClick="btnAddField_Click" CssClass="btn btn-warning btn-sm"  CausesValidation="false"/>&nbsp;
                                                <asp:Button ID="btnCancelField" runat="server" Text="取 消" OnClick="btnCancelField_Click" CssClass="btn btn-secondary btn-sm ml-2"  CausesValidation="false"/>
                                            </div>
                                        </asp:Panel>
                                        <div class="text-right">
                                            <asp:Button ID="btnAddNew" runat="server" Text="新增欄位" OnClick="btnAddNew_Click" CssClass="btn btn-primary btn-sm"  CausesValidation="false"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="FileUpload1" class="fw-bold">Downloads</label>
                                <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" CssClass="form-control"  CausesValidation="false"/>
                            </div>
                            <asp:Repeater ID="rptEditFiles" runat="server" OnItemCommand="rptEditFiles_ItemCommand">
                                <ItemTemplate>
                                    <div class="d-flex justify-content-between align-items-center border-bottom py-1">
                                        <span><%# System.IO.Path.GetFileName(Eval("FilePath").ToString()) %></span>
                                        <asp:LinkButton ID="btnDeleteFile" runat="server"
                                            CommandName="DeleteFile"
                                            CommandArgument='<%# Eval("Id") %>'
                                            OnClientClick="return confirm('確定要刪除這個檔案嗎？');"
                                            CssClass="btn btn-sm btn-danger">
                                            刪除
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="FUInteriorImg" class="fw-bold">Interior Image *</label>
                                
                                <asp:FileUpload ID="FUInteriorImg" runat="server" CssClass="form-control" />
                                <div class="mt-2 text-center">
                                    <asp:Image ID="InteriorImg" runat="server" CssClass="img-thumbnail" Style="max-width: 100%; height: auto;" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="FUDeckImg1" class="fw-bold">Layout Image 1 *</label>
                                
                                <asp:FileUpload ID="FUDeckImg1" runat="server" CssClass="form-control" />
                                <div class="mt-2 text-center">
                                    <asp:Image ID="DeckImg1" runat="server" CssClass="img-thumbnail" Style="max-width: 100%; height: auto;" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="FUDeckImg2" class="fw-bold">Layout Image 2 *</label>
                                
                                <asp:FileUpload ID="FUDeckImg2" runat="server" CssClass="form-control" />
                                <div class="mt-2 text-center">
                                    <asp:Image ID="DeckImg2" runat="server" CssClass="img-thumbnail" Style="max-width: 100%; height: auto;" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-4">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="FUCarouselImgPath" class="fw-bold">Carousel Images (預設第一張為封面) *</label>
                                <%--<asp:RequiredFieldValidator
    ID="RequiredFieldValidator8"
    runat="server"
    ControlToValidate="FUCarouselImgPath"
    ErrorMessage="請新增圖片"
    ForeColor="Red"
    Display="Dynamic">
</asp:RequiredFieldValidator>--%>
                                <asp:FileUpload ID="FUCarouselImgPath" runat="server" AllowMultiple="true" CssClass="form-control" />
                            </div>
                            <div class="row mt-2">
                                <asp:Repeater ID="rptCarouselImgs" runat="server" OnItemCommand="btnDeleteCarouselImg">
                                    <ItemTemplate>
                                        <div class="col-4 col-md-3 col-lg-2 mb-2 text-center">
                                            <img src='<%# ResolveUrl("/Uploads/Photos/") + Eval("CarouselImgPath") %>' class="img-thumbnail" style="width: 100%; height: auto; object-fit: cover;" />
                                            <br />
                                            <asp:LinkButton ID="btnDeleteCarouselImg" runat="server" Text="刪 除"
                                                CommandName="DeleteCarouselImg"
                                                CommandArgument='<%# Eval("Id") %>'
                                                CssClass="btn btn-danger btn-sm mt-1"
                                                OnClientClick="return confirm('確定要刪除這張圖片嗎？');"
                                                CausesValidation="false">
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mb-4">
                        <label for="CKEditor1" class="fw-bold">Specification *</label>
                        <asp:RequiredFieldValidator
    ID="RequiredFieldValidator10"
    runat="server"
    ControlToValidate="CKEditor1"
    ErrorMessage="請輸入規格"
    ForeColor="Red"
    Display="Dynamic">
</asp:RequiredFieldValidator>
                        <ckeditor:CKEditorControl ID="CKEditor1" runat="server" BasePath="~/Scripts/ckeditor/" Height="500" />
                    </div>
                </div>
                <div class="card-footer text-right">
                    <asp:Button ID="btnSubmit" runat="server" Text="送 出" OnClick="btnSubmit_Click" CssClass="btn btn-warning" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="取 消" OnClick="btnCancel_Click" CssClass="btn btn-secondary ml-2"  CausesValidation="false"/>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
