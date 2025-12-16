using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class AddInterior : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //動態新增欄位
                ViewState["PendingFields"] = new List<PrincipalField>();
                ViewState["PendingEditIndex"] = -1;
                BindPrincipal();

                //限制上傳檔案為圖片
                FUInteriorImg.Attributes["accept"] = "image/*";
                FUDeckImg1.Attributes["accept"] = "image/*";
                FUDeckImg2.Attributes["accept"] = "image/*";

            }
        }
        private void BindPrincipal()  //顯示Principal的欄位
        {
            // 建立暫時的表格用來加入新的欄位資料
            List<PrincipalField> pendingFields = ViewState["PendingFields"] as List<PrincipalField>;

            // 顯示剛輸入的欄位名稱與值
            rptPendingFields.DataSource = pendingFields;
            rptPendingFields.DataBind();
        }
        protected void btnAddNew_Click(object sender, EventArgs e)  //"新增欄位"的按鈕
        {
            pnlInputFields.Visible = true;
        }
        protected void btnAddNewField_Click(object sender, EventArgs e)  //欄位與值的送出按鈕
        {
            string name = txtNewName.Text.Trim();
            string value = txtNewValue.Text.Trim();

            List<PrincipalField> pendingFields = ViewState["PendingFields"] as List<PrincipalField>;
            if (pendingFields == null)
            {
                pendingFields = new List<PrincipalField>();
            }

            //加入待新增的清單
            pendingFields.Add(new PrincipalField
            {
                Name = name,
                Value = value
            });

            ViewState["PendingFields"] = pendingFields;

            txtNewName.Text = "";
            txtNewValue.Text = "";
            pnlInputFields.Visible = false;

            BindPrincipal();
        }
        protected void btnCancelField_Click(object sender, EventArgs e)  //取消編輯欄位
        {
            pnlInputFields.Visible = false;

            txtNewName.Text = "";
            txtNewValue.Text = "";
        }
        protected void rptPendingFields_ItemCommand(object source, RepeaterCommandEventArgs e)  //編輯欄位
        {
            List<PrincipalField> pendingFields = ViewState["PendingFields"] as List<PrincipalField> ?? new List<PrincipalField>();

            int index;
            bool validIndex = int.TryParse(e.CommandArgument?.ToString(), out index);

            switch (e.CommandName)
            {
                case "Edit":
                    if (validIndex)
                    {
                        ViewState["PendingEditIndex"] = index;
                    }
                    break;

                case "Cancel":
                    ViewState["PendingEditIndex"] = -1;
                    break;

                case "Update":
                    if (validIndex && index >= 0 && index < pendingFields.Count)
                    {
                        TextBox txtName = e.Item.FindControl("txtEditName") as TextBox;
                        TextBox txtValue = e.Item.FindControl("txtEditValue") as TextBox;

                        string newName = txtName.Text.Trim();
                        string newValue = txtValue.Text.Trim();

                        // 驗證欄位不可為空
                        if ((pendingFields == null || pendingFields.Count == 0))
                        {
                            Response.Write("<script>alert('請填寫欄位名稱與值');</script>");
                            return;
                        }

                        pendingFields[index].Name = newName;
                        pendingFields[index].Value = newValue;
                        ViewState["PendingEditIndex"] = -1;

                        if (txtName != null && txtValue != null)
                        {
                            pendingFields[index].Name = txtName.Text.Trim();
                            pendingFields[index].Value = txtValue.Text.Trim();
                            ViewState["PendingEditIndex"] = -1;
                        }
                    }
                    break;

                case "Delete":
                    if (validIndex && index >= 0 && index < pendingFields.Count)
                    {
                        pendingFields.RemoveAt(index);
                        ViewState["PendingEditIndex"] = -1;
                    }
                    break;
            }

            ViewState["PendingFields"] = pendingFields;
            BindPrincipal();
        }
        protected void rptPendingFields_ItemDataBound(object sender, RepeaterItemEventArgs e)  //編輯欄位
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    int id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Id"));
            //    var pnlView = e.Item.FindControl("lblPrincipalName").Parent; // 這邊示意，沒用到 Panel

            //    if (id == editId)
            //    {
            //        // 切換成 Edit 模板
            //        // Repeater 本身沒有像 GridView、DetailsView 的自動模式切換
            //        // 我們在 BindRepeater() 裡要透過 EditItemTemplate 自動切換，
            //        // 所以要在 BindRepeater 時設定 EditIndex 或自己處理呈現

            //        // 這裡可以改用 ItemCreated 或改用更完整範例自行管理切換
            //    }
            //}
        }
        private void InsertField(List<PrincipalField> fields, string modelId)  //新增欄位的核心
        {
            foreach (var field in fields)
            {
                string sql = "INSERT INTO Principal (ModelId, Name, Value, CreatedAt) VALUES (@ModelId, @Name, @Value, @CreatedAt)";
                var param = new Dictionary<string, object>
        {
            {"@ModelId",modelId},
            { "@Name", field.Name },
            { "@Value", field.Value },
            { "@CreatedAt",DateTime.Now}
        };

                db.ExecuteNonQuery(sql, param);
            }
        }
        private string InsertModel(string modelName, string modelNum, string modelLabel)  //新增船型
        {
            //先不加入admin
            string sql = @"insert into Model (Name, Number, CreatedAt ,Label) 
                               values (@Name, @Number, @CreatedAt, @Label);
                               SELECT SCOPE_IDENTITY();
                              ";

            var param = new Dictionary<string, object>()
            {
                { "@Name",modelName},
                { "@Number",modelNum},
                { "@CreatedAt",DateTime.Now},
                { "@Label",modelLabel}
            };

            object result = db.ExecuteScalar(sql, param);

            return result?.ToString(); // 回傳新產生的 ModelId 或 null

        }
        private void InsertContent(string modelId) //新增內容(包含船型與)
        {
            string introduce = Introduce.Text.Trim();
            //string loa = LOA.Text.Trim();
            //string lwl = LWL.Text.Trim();
            //string beam = Beam.Text.Trim();
            //string draft = Draft.Text.Trim();
            //string displacement = Displacement.Text.Trim();
            //string ballast = Ballast.Text.Trim();
            string specification = CKEditor1.Text.Trim();

            //先不加入admin
            string sql = @"insert into YachtsContent (ModelId ,content ,CreatedAt ,Specification ) 
                               values (@ModelId ,@content ,@CreatedAt ,@Specification) 
                              ";

            var param = new Dictionary<string, object>()
            {
                { "@ModelId",modelId},
                { "@content",introduce},
                { "@CreatedAt",DateTime.Now},
                {"@Specification", specification}
            };

            db.ExecuteNonQuery(sql, param);
        }
        private string SaveImage(FileUpload fu)  //新增 所有圖片 的動作核心
        {
            if (fu.HasFile)
            {
                string ext = Path.GetExtension(fu.FileName).ToLower();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    string fileName = DateTime.Now.Ticks.ToString() + ext;
                    string savePath = Server.MapPath("~/Uploads/Photos/") + fileName;
                    fu.SaveAs(savePath);
                    return fileName;
                }
            }
            return null;
        }
        private void InsertImg(string modelId)  //新增 設計、平面與輪播圖片
        {
            // 圖片儲存檔名
            string interiorImgPath = SaveImage(FUInteriorImg);
            string deckImg1Path = SaveImage(FUDeckImg1);
            string deckImg2Path = SaveImage(FUDeckImg2);
            string carouselImgPath = SaveImage(FUCarouselImgPath);

            //插入平面圖與設計圖
            string sqlLayout = @"INSERT INTO YachtsLayoutImage (ModelId, InteriorImgPath, DeckImgPath1, 
                                                                DeckImgPath2, CreatedAt)
                                 VALUES  (@ModelId, @InteriorImgPath, @DeckImgPath1, @DeckImgPath2, @CreatedAt)";
            var layoutParam = new Dictionary<string, object>()
    {
        { "@ModelId", modelId },
        { "@InteriorImgPath", interiorImgPath ?? "" },
        { "@DeckImgPath1", deckImg1Path ?? "" },
        { "@DeckImgPath2", deckImg2Path ?? "" },
        { "@CreatedAt", DateTime.Now }
    };
            db.ExecuteNonQuery(sqlLayout, layoutParam);

            // 插入輪播圖
            HttpFileCollection uploadedFiles = Request.Files;
            List<HttpPostedFile> validImages = new List<HttpPostedFile>();

            foreach (HttpPostedFile file in FUCarouselImgPath.PostedFiles)
            {
                // 只處理 上傳輪播圖 的欄位
                if (file != null && file.ContentLength > 0)
                {                    
                        validImages.Add(file);
                }
            }

            int uploadedCount = 0;
            foreach (HttpPostedFile img in validImages)
            {
                string extension = Path.GetExtension(img.FileName);
                string imgName = DateTime.Now.Ticks.ToString() + extension;
                string uploadsFolder = Server.MapPath("~/Uploads/Photos");
                Directory.CreateDirectory(uploadsFolder);
                string fullPath = Path.Combine(uploadsFolder, imgName);

                img.SaveAs(fullPath);

                string sql = @"INSERT INTO YachtsCarouselImage (ModelId, ImgPath, CreatedAt)
                   VALUES (@ModelId, @ImgPath, @CreatedAt)";
                var param = new Dictionary<string, object>
    {
        { "@ModelId", modelId },
        { "@ImgPath", imgName },
        { "@CreatedAt", DateTime.Now }
    };

                try
                {
                    db.ExecuteNonQuery(sql, param);
                    uploadedCount++;
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('資料庫寫入失敗: {ex.Message}');</script>");
                    return;
                }
            }
        }
        private void InsertFile(string modelId)  //新增 檔案下載
        {
            var uploadFiles = FileUpload1.PostedFiles;

            //如果有上傳檔案
            if (uploadFiles != null)
            {
                foreach (HttpPostedFile file in uploadFiles)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(file.FileName);  //取得副檔名
                        string fileName = DateTime.Now.Ticks.ToString() + extension;
                        string filePath = "~/Uploads/" + fileName;  // 儲存路徑
                        string uploadsFolder = Server.MapPath("~/Uploads/"); // 取得實體路徑
                        Directory.CreateDirectory(uploadsFolder); // 確保資料夾存在         
                        string fullPath = Path.Combine(uploadsFolder, fileName);  //組合成完整檔案路徑

                        // 實際儲存檔案到 uploads 資料夾
                        file.SaveAs(fullPath);

                        string sql = @"INSERT INTO YachtsDownloads (ModelId, FilePath,  CreatedAt)
                           VALUES  (@ModelId, @FilePath, @CreatedAt)
                          ";

                        var param = new Dictionary<string, object>()
            {
                { "@ModelId", modelId },
                { "@FilePath", filePath ?? "" },
                { "@CreatedAt",DateTime.Now}
            };

                        db.ExecuteNonQuery(sql, param);
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)  //頁面送出按鈕
        {
            //取得輸入值
            //標籤
            string modelLabel = rblModelLabel.SelectedValue;
            //船型
            string modelName = ModelName.Text.Trim();
            string modelNum = ModelNum.Text.Trim();
            //內容
            string introduce = Introduce.Text.Trim();
            //基本規格的欄位名稱與值
            string name = txtNewName.Text.Trim();
            string value = txtNewValue.Text.Trim();
            List<PrincipalField> pendingFields = ViewState["PendingFields"] as List<PrincipalField>;

            // 驗證內容與規格
            if (string.IsNullOrWhiteSpace(introduce))
            {
                Response.Write("<script>alert('請填寫所有必填欄位');</script>");
                return;
            }
            //驗證圖片
            else if (!FUInteriorImg.HasFile || !FUDeckImg1.HasFile || !FUDeckImg2.HasFile)
            {
                Response.Write("<script>alert('請上傳設計圖、平面圖');</script>");
                return;
            }
            //驗證輪播圖
            else if (!FUCarouselImgPath.HasFile)
            {
                Response.Write("<script>alert('請上傳至少一張輪播圖');</script>");
                return;
            }
            //驗證船型
            else if (string.IsNullOrWhiteSpace(modelName) || (string.IsNullOrWhiteSpace(modelNum)))
            {
                Response.Write("<script>alert('請輸入船型名字與型號');</script>");
                return;
            }
            //型號是否為數字
            else if (!int.TryParse(modelNum, out _))
            {
                Response.Write("<script>alert('型號請輸入數字');</script>");
                return;
            }
            //驗證標籤
            else if (string.IsNullOrWhiteSpace(modelLabel))
            {
                Response.Write("<script>alert('請選擇標籤');</script>");
                return;
            }
            //驗證基本規格的欄位與值
            //else if ((pendingFields == null || pendingFields.Count == 0))
            //{
            //    Response.Write("<script>alert('請填寫欄位名稱與值')</script>");
            //    return;
            //}

            // 通過驗證後才開始新增資料
            string modelId = InsertModel(modelName, modelNum, modelLabel);  //新增船型
            if (modelId == null)
            {
                Response.Write("<script>alert('新增失敗，請稍後再試！');</script>");
                return;
            }
            else
            {
                InsertContent(modelId);  //新增內容(包含基本規格與詳細規格)
                InsertImg(modelId);  //新增圖片(包含平面圖、設計圖、輪播圖)
                InsertFile(modelId);  //新增檔案上傳

                //新增欄位名稱與值
                if (pendingFields != null && pendingFields.Count > 0)
                {
                    InsertField(pendingFields, modelId);

                    ViewState["PendingFields"] = null;
                }
            }

            Response.Write("<script>alert('新增成功'); window.location='Yachts-B.aspx';</script>");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Yachts-B.aspx");
        }
        private int editId
        {
            get => ViewState["EditId"] == null ? -1 : (int)ViewState["EditId"];
            set => ViewState["EditId"] = value;
        }
        [Serializable]
        public class PrincipalField
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}