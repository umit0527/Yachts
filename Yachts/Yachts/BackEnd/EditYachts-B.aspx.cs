using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;
using Microsoft.Ajax.Utilities;
using System.Reflection;
using static Yachts.BackEnd.AddInterior;

namespace Yachts.BackEnd
{
    public partial class EditYachts_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Session["userid"] == null)
                //{
                //    string logout = "<script>alert('請先登入'); window.location='LoginB.aspx';</script>";
                //    Response.Write(logout);
                //}
                //else  //有登入
                //{
                //}
                if (Request.QueryString["Id"] == null || !int.TryParse(Request.QueryString["Id"], out int yachtsContentId))
                {
                    Response.Redirect("Yachts-B.aspx");
                    return;
                }

                // 先載入主資料，把 ModelId 存進 ViewState
                LoadYachtsData();

                if (ViewState["ModelId"] != null)  // 從 ViewState 取得 ModelId
                {
                    int modelId = Convert.ToInt32(ViewState["ModelId"]);
                    
                    BindCarouselImgs(modelId);
                    BindFiles(modelId);
                    BindPrincipal(modelId);
                }

                //限制上傳檔案為圖片
                FUInteriorImg.Attributes["accept"] = "image/*";
                FUDeckImg1.Attributes["accept"] = "image/*";
                FUDeckImg2.Attributes["accept"] = "image/*";
                FUCarouselImgPath.Attributes["accept"] = "image/*";
            }
        }

        //protected void Button1_Click(object sender, EventArgs e)  //登出
        //{
        //    if (Session["userid"] != null)  //有登入的話，則有登出按鈕
        //    {
        //        Session.Clear();       // 清除所有 Session 資料
        //        Session.Abandon();     // 結束目前的 Session
        //        string logout = "<script>alert('登出成功'); window.location='LoginB.aspx';</script>";
        //        Response.Write(logout);
        //        Response.End();
        //    }
        //}
        private void BindFiles(int modelId, List<int> excludeIds = null)  //顯示 "檔案下載"
        {
            string sql = @"SELECT * 
                           FROM YachtsDownloads 
                           WHERE ModelId = @ModelId 
                           ORDER BY CreatedAt DESC";
            var param = new Dictionary<string, object> { { "@ModelId", modelId } };
            DataTable dt = db.SearchDB(sql, param);

            if (excludeIds != null)
            {
                foreach (int id in excludeIds)
                {
                    var rows = dt.Select("Id = " + id);
                    foreach (var row in rows)
                    {
                        dt.Rows.Remove(row);
                    }
                }
                dt.AcceptChanges();
            }

            rptEditFiles.DataSource = dt;
            rptEditFiles.DataBind();
        }
        private void LoadYachtsData()  //顯示原有資料
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }
            else
            {
                int yachtsContentId = int.Parse(Request.QueryString["Id"]);

                string sql = @"select y.[content], y.Id, y.ModelId, y.Specification, 
                                      yl.InteriorImgPath, yl.DeckImgPath1, yl.DeckImgPath2, yl.Id as YachtsLayoutImageId,
                                      m.Name, m.Number ,m.Label
                               from YachtsContent y
                               join Model m on m.Id=y.ModelId
                               join YachtsLayoutImage yl on yl.ModelId=m.Id
                               where y.Id=@Id
                                ";

                var param = new Dictionary<string, object> { { "@Id", yachtsContentId } };
                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    int modelId = Convert.ToInt32(dt.Rows[0]["ModelId"]);
                    //船型
                    string modelName = dt.Rows[0]["Name"].ToString();
                    string modelNum = dt.Rows[0]["Number"].ToString();
                    string modelLabel = dt.Rows[0]["Label"].ToString();
                    //內容
                    string content = dt.Rows[0]["content"].ToString();
                    //規格
                    string specification = dt.Rows[0]["Specification"].ToString();
                    //圖片
                    string interiorImgPath = dt.Rows[0]["InteriorImgPath"].ToString();
                    string deckImgPath1 = dt.Rows[0]["DeckImgPath1"].ToString();
                    string deckImgPath2 = dt.Rows[0]["DeckImgPath2"].ToString();

                    ModelName.Text = modelName;
                    ModelNum.Text = modelNum;
                    Introduce.Text = content;
                    CKEditor1.Text = specification;
                    rblModelLabel.SelectedValue = modelLabel;

                    InteriorImg.ImageUrl = "~/Uploads/Photos/" + interiorImgPath;
                    DeckImg1.ImageUrl = "~/Uploads/Photos/" + deckImgPath1;
                    DeckImg2.ImageUrl = "~/Uploads/Photos/" + deckImgPath2;

                    // 儲存 ID 到 ViewState 方便 btnSubmit 用
                    ViewState["ModelId"] = dt.Rows[0]["ModelId"];
                    ViewState["YachtsLayoutImageId"] = dt.Rows[0]["YachtsLayoutImageId"];
                }
            }
        }
        private void BindPrincipal(int modelId)  //顯示 "欄位" 原有資料
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }
            else
            {
                //int principalId = int.Parse(Request.QueryString["ModelId"]);

                string sql = @"select p.Name, p.Value
                               from Principal p
                               join Model m on m.Id=p.ModelId
                               where p.ModelId=@ModelId
                              ";

                var param = new Dictionary<string, object> { { "@ModelId", modelId } };
                DataTable dt = db.SearchDB(sql, param);

                var fieldList = new List<PrincipalField>();
                foreach (DataRow row in dt.Rows)
                {
                    fieldList.Add(new PrincipalField
                    {
                        Name = row["Name"].ToString(),
                        Value = row["Value"].ToString()
                    });
                }

                ViewState["PendingFields"] = fieldList;
                BindPendingFieldsRepeater();
            }
        }
        private void BindPendingFieldsRepeater()  //將 ViewState 資料繫結到 rptPendingFields
        {
            var fields = ViewState["PendingFields"] as List<PrincipalField>;
            if (fields != null)
            {
                rptPendingFields.DataSource = fields;
                rptPendingFields.DataBind();
            }
        }
        protected void btnAddNew_Click(object sender, EventArgs e)  //"新增欄位"的按鈕
        {
            pnlInputFields.Visible = true;
        }
        protected void btnAddField_Click(object sender, EventArgs e)   //欄位的編輯與刪除動作
        {
            var fields = ViewState["PendingFields"] as List<PrincipalField> ?? new List<PrincipalField>();

            fields.Add(new PrincipalField
            {
                Name = txtNewName.Text.Trim(),
                Value = txtNewValue.Text.Trim()
            });

            ViewState["PendingFields"] = fields;
            txtNewName.Text = "";
            txtNewValue.Text = "";
            pnlInputFields.Visible = false;

            BindPendingFieldsRepeater();
        }
        protected void rptPendingFields_ItemCommand(object source, RepeaterCommandEventArgs e)  //欄位的編輯與刪除核心
        {
            var fields = ViewState["PendingFields"] as List<PrincipalField>;

            if (fields == null) return;

            int index = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Edit":
                    ViewState["PendingEditIndex"] = index;
                    BindPendingFieldsRepeater();
                    break;

                case "Cancel":
                    ViewState["PendingEditIndex"] = -1;
                    BindPendingFieldsRepeater();
                    break;

                case "Delete":
                    fields.RemoveAt(index);
                    ViewState["PendingFields"] = fields;
                    ViewState["PendingEditIndex"] = -1;
                    BindPendingFieldsRepeater();
                    break;

                case "Update":
                    var txtName = (TextBox)e.Item.FindControl("txtEditName");
                    var txtValue = (TextBox)e.Item.FindControl("txtEditValue");
                    fields[index].Name = txtName.Text.Trim();
                    fields[index].Value = txtValue.Text.Trim();

                    ViewState["PendingFields"] = fields;
                    ViewState["PendingEditIndex"] = -1;
                    BindPendingFieldsRepeater();
                    break;
            }
        }
        protected void rptPendingFields_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        protected void btnCancelField_Click(object sender, EventArgs e)   //取消新增欄位
        {
            pnlInputFields.Visible = false;

            txtNewName.Text = "";
            txtNewValue.Text = "";
        }
        private void BindCarouselImgs(int modelId, List<int> excludeIds = null)  //顯示 "輪播圖"
        {
            string sql = @"SELECT Id, ImgPath as CarouselImgPath
                           FROM YachtsCarouselImage 
                           WHERE ModelId = @ModelId 
                           ORDER BY CreatedAt DESC";
            var param = new Dictionary<string, object> { { "@ModelId", modelId } };

            DataTable dt = db.SearchDB(sql, param);
            if (excludeIds != null)
            {
                foreach (int id in excludeIds)
                {
                    var rows = dt.Select("Id = " + id);
                    foreach (var row in rows)
                    {
                        dt.Rows.Remove(row);
                    }
                }
                dt.AcceptChanges();
            }

            rptCarouselImgs.DataSource = dt;
            rptCarouselImgs.DataBind();
        }
        protected void btnDeleteCarouselImg(object source, RepeaterCommandEventArgs e)  //刪除 "輪播圖"
        {
            if (e.CommandName == "DeleteCarouselImg")
            {
                int imgId = Convert.ToInt32(e.CommandArgument);

                // 加入 ViewState 裡的待刪除清單
                List<int> deleteList = ViewState["CarouselImgToDelete"] as List<int> ?? new List<int>();
                if (!deleteList.Contains(imgId))
                {
                    deleteList.Add(imgId);
                    ViewState["CarouselImgToDelete"] = deleteList;
                }

                // 重新綁定時排除已標記刪除的圖片
                int modelId = Convert.ToInt32(ViewState["ModelId"]);
                BindCarouselImgs(modelId, deleteList);
            }
        }
        private void SaveImage(HttpPostedFile file, out string savePath)  //儲存所有圖片的核心
        {
            // 先預設為空，避免未賦值錯誤
            savePath = "";
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif"))
            {
                Response.Write("<script>alert('圖片格式不支援')</script>");
                return;
            }

            string saveFileName = DateTime.Now.Ticks.ToString() + extension;
            savePath = Server.MapPath("~/Uploads/Photos/") + saveFileName;
            file.SaveAs(savePath);
            savePath = saveFileName;
        }
        private void InsertFile(string modelId)  //上傳 "檔案下載" 的核心
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
                        string fileName = DateTime.UtcNow.Ticks.ToString() + extension;
                        string filePath = "~/Uploads/" + fileName;  // 儲存路徑
                        string uploadsFolder = Server.MapPath("~/Uploads/"); // 取得實體路徑
                        Directory.CreateDirectory(uploadsFolder); // 確保資料夾存在         
                        string fullPath = Path.Combine(uploadsFolder, fileName);  //組合成完整檔案路徑

                        // 實際儲存檔案到 uploads 資料夾
                        file.SaveAs(fullPath);

                        string sql = @"INSERT INTO YachtsDownloads (ModelId, FilePath,  UpdatedAt)
                                       VALUES  (@ModelId, @FilePath, @UpdatedAt)
                                      ";

                        var param = new Dictionary<string, object>()
            {
                { "@ModelId", modelId },
                { "@FilePath", filePath ?? "" },
                { "@UpdatedAt",DateTime.Now}
            };

                        db.ExecuteNonQuery(sql, param);
                    }
                }
            }
        }
        private void InsertCarouselImg(string modelId)  //上傳 "輪播圖" 的核心
        {
            var uploadCarouselImg = FUCarouselImgPath.PostedFiles;

            //如果有上傳圖片
            if (uploadCarouselImg != null)
            {
                foreach (HttpPostedFile img in uploadCarouselImg)
                {
                    if (img != null && img.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(img.FileName);  //取得副檔名
                        string imgName = DateTime.Now.Ticks.ToString() + extension;
                        string uploadsFolder = Server.MapPath("~/Uploads/Photos"); // 取得實體路徑
                        Directory.CreateDirectory(uploadsFolder); // 確保資料夾存在         
                        string fullPath = Path.Combine(uploadsFolder, imgName);  //組合成完整檔案路徑

                        // 實際儲存檔案到 uploads 資料夾
                        img.SaveAs(fullPath);

                        string sql = @"INSERT INTO YachtsCarouselImage (ModelId, ImgPath,  UpdatedAt)
                                       VALUES  (@ModelId, @ImgPath, @UpdatedAt)
                                      ";

                        var param = new Dictionary<string, object>()
            {
                { "@ModelId", modelId },
                { "@ImgPath", imgName ?? "" },
                { "@UpdatedAt",DateTime.Now}
            };

                        db.ExecuteNonQuery(sql, param);
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)  //送出按鈕
        {
            int yachtsContentId = int.Parse(Request.QueryString["Id"]);
            int modelId = Convert.ToInt32(ViewState["ModelId"]);
            int yachtsLayoutImageId = Convert.ToInt32(ViewState["YachtsLayoutImageId"]);
            //船型
            string modelName = ModelName.Text.Trim();
            string modelNum = ModelNum.Text.Trim();
            string modelLabel = rblModelLabel.SelectedValue;
            //內容
            string introduce = Introduce.Text.Trim();
            string specification = CKEditor1.Text.Trim();
            //圖片
            string newInteriorImgPath = "";
            string newDeckImgPath1 = "";
            string newDeckImgPath2 = "";

            HttpFileCollection uploadedFiles = Request.Files;

            if (string.IsNullOrWhiteSpace(modelName) || string.IsNullOrWhiteSpace(introduce) ||
                string.IsNullOrWhiteSpace(modelNum) || string.IsNullOrWhiteSpace(specification))
            {
                Response.Write("<script>alert('必填欄位填寫不完整'); </script>");
            }else if (!int.TryParse(modelNum, out _))
            {
                Response.Write("<script>alert('型號請輸入數字');</script>");
                return;
            }
            else
            {
                // 取得原本的圖片路徑
                string getImgpATHSql = @"SELECT InteriorImgPath, DeckImgPath1, DeckImgPath2
                                         FROM YachtsLayoutImage 
                                         WHERE Id = @Id
                                        ";
                var paramImg = new Dictionary<string, object> { { "@Id", yachtsLayoutImageId } };
                DataTable dtImg = db.SearchDB(getImgpATHSql, paramImg);

                if (dtImg.Rows.Count > 0)
                {
                    newInteriorImgPath = dtImg.Rows[0]["InteriorImgPath"].ToString();
                    newDeckImgPath1 = dtImg.Rows[0]["DeckImgPath1"].ToString();
                    newDeckImgPath2 = dtImg.Rows[0]["DeckImgPath2"].ToString();
                }

                // 如果有上傳新圖片，則覆蓋原圖
                if (FUInteriorImg.HasFile)
                {
                    // 第一張：設計圖
                    SaveImage(FUInteriorImg.PostedFile, out newInteriorImgPath);
                }
                if (FUDeckImg1.HasFile)
                {
                    // 第二張：平面圖1
                    SaveImage(FUDeckImg1.PostedFile, out newDeckImgPath1);
                }
                if (FUDeckImg2.HasFile)
                {
                    // 第三張：平面圖2
                    SaveImage(FUDeckImg2.PostedFile, out newDeckImgPath2);
                }

                //先不加入admin
                string sql = @"update YachtsContent set Content=@Content , 
                                                        UpdatedAt=@UpdatedAt ,
                                                        Specification=@Specification
                               where Id=@YachtsContentId;
                               update YachtsLayoutImage set InteriorImgPath=@InteriorImgPath,
                                                            DeckImgPath1=@DeckImgPath1,
                                                            DeckImgPath2=@DeckImgPath2,
                                                            UpdatedAt=@UpdatedAt
                               where Id=@YachtsLayoutImageId;
                               update Model set Name=@Name,
                                                Number=@Number,
                                                UpdatedAt=@UpdatedAt
                               where Id=@ModelId
                              ";

                var Params = new Dictionary<string, object>()
            {
                { "@Content",introduce},
                { "@UpdatedAt",DateTime.Now},
                { "@InteriorImgPath",newInteriorImgPath},
                { "@DeckImgPath1",newDeckImgPath1},
                { "@DeckImgPath2",newDeckImgPath2},
                { "@Specification",specification},
                { "@ModelId",modelId },
                {"@Name", modelName},
                {"@Number", modelNum},
                { "@YachtsContentId", yachtsContentId },
                { "@YachtsLayoutImageId",yachtsLayoutImageId}
            };

                //處理刪除檔案
                if (ViewState["FilesToDelete"] is List<int> deleteFileList)
                {
                    foreach (int id in deleteFileList)
                    {
                        //找出要刪除的那個檔案名稱
                        string sqlFilePath = "SELECT FilePath FROM YachtsDownloads WHERE Id = @Id";
                        var param = new Dictionary<string, object> { { "@Id", id } };
                        object resultDeletFile = db.ExecuteScalar(sqlFilePath, param);
                        if (resultDeletFile != null)
                        {
                            string fileName = resultDeletFile.ToString();
                            string filePath = Server.MapPath(fileName);

                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }

                        }
                        string sqlDeletFile = "DELETE FROM YachtsDownloads WHERE Id = @Id";  //找出要刪除的那個檔案Id
                        db.ExecuteNonQuery(sqlDeletFile, param);
                    }
                }
                // 清除 ViewState
                ViewState["FilesToDelete"] = null;
                InsertFile(modelId.ToString());

                //處理刪除輪播圖
                if (ViewState["CarouselImgToDelete"] is List<int> deleteCarouselImgList)
                {
                    foreach (int id in deleteCarouselImgList)
                    {
                        //找出要刪除的那個檔案名稱
                        string sqlCarouselImgPath = "SELECT ImgPath as CarouselImgPath FROM YachtsCarouselImage WHERE Id = @Id";
                        var param = new Dictionary<string, object> { { "@Id", id } };
                        object resultDeletCarouselImg = db.ExecuteScalar(sqlCarouselImgPath, param);
                        if (resultDeletCarouselImg != null)
                        {
                            string CarouselName = resultDeletCarouselImg.ToString();
                            string CarouselImgPath = Server.MapPath(CarouselName);

                            if (File.Exists(CarouselImgPath))
                            {
                                File.Delete(CarouselImgPath);
                            }

                        }
                        string sqlDeletCarouselImg = "DELETE FROM YachtsCarouselImage WHERE Id = @Id";  //找出要刪除的那個檔案Id
                        db.ExecuteNonQuery(sqlDeletCarouselImg, param);
                    }
                }
                // 清除 ViewState
                ViewState["CarouselImgToDelete"] = null;
                InsertCarouselImg(modelId.ToString());

                //處理刪除欄位與值
                if (ViewState["ModelId"] != null && ViewState["PendingFields"] is List<PrincipalField> fields)
                {
                    modelId = Convert.ToInt32(ViewState["ModelId"]);

                    // 先刪除該 ModelId 所有舊有 Principal 欄位
                    string deleteSql = "DELETE FROM Principal WHERE ModelId = @ModelId";
                    db.ExecuteNonQuery(deleteSql, new Dictionary<string, object> { { "@ModelId", modelId } });

                    // 再新增目前 ViewState 裡的欄位
                    foreach (var field in fields)
                    {
                        string insertSql = @"INSERT INTO Principal (ModelId, Name, Value, CreatedAt)
                             VALUES (@ModelId, @Name, @Value, @CreatedAt)";
                        var param = new Dictionary<string, object>
        {
            { "@ModelId", modelId },
            { "@Name", field.Name },
            { "@Value", field.Value },
            { "@CreatedAt", DateTime.Now }
        };
                        db.ExecuteNonQuery(insertSql, param);
                    }

                    // 清除 ViewState
                    ViewState["PendingFields"] = null;
                    int result = db.ExecuteNonQuery(sql, Params);

                    if (result > 0)
                    {
                        // 提示用戶
                        Response.Write("<script>alert('更新成功！'); window.location='Yachts-B.aspx';</script>");
                    }

                    else
                    {
                        Response.Write("<script>alert('更新失敗，請稍後再試！'); </script>");
                    }
                }
            }
        }
        protected void btnDeleteFile(object source, RepeaterCommandEventArgs e)  //刪除 "檔案下載" 的核心
        {
            if (e.CommandName == "DeleteFile")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                // 加到待刪除清單
                List<int> deleteList = ViewState["FilesToDelete"] as List<int> ?? new List<int>();
                if (!deleteList.Contains(id))
                {
                    deleteList.Add(id);
                }
                ViewState["FilesToDelete"] = deleteList;

                // 隱藏該刪除的檔案（重新綁定排除）
                int modelId = Convert.ToInt32(ViewState["ModelId"]);
                // 重新綁定顯示，排除該檔案
                BindFiles(modelId, deleteList);
            }
        }
        protected void rptEditFiles_ItemCommand(object source, RepeaterCommandEventArgs e)  //
        {
            if (e.CommandName == "DeleteFile")
            {
                btnDeleteFile(source, e);
            }
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