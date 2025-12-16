using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class AddNews : System.Web.UI.Page
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
                BindCategoryList();

                //限制上傳檔案為圖片
                FileUpload1.Attributes["accept"] = "image/*";
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

        private void BindCategoryList()  // "種類"下拉式選單
        {
            string sql = @"select Id, Name 
                           from NewsCategory 
                           order by Name asc";

            DataTable dt = db.SearchDB(sql);

            if (dt.Rows.Count > 0)
            {
                CategoryList.DataSource = dt;
                CategoryList.DataTextField = "Name";   // 顯示名稱
                CategoryList.DataValueField = "Id";  //抓取對應 ID 以便寫進資料庫
                CategoryList.DataBind();

                // 加入提示選項
                CategoryList.Items.Insert(0, new ListItem("請選擇種類", ""));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string content = CKEditor1.Text.Trim();
            string categoryList = CategoryList.SelectedValue;
            string title = txtTitle.Text.Trim();
            HttpFileCollection uploadedFiles = Request.Files;

            // 驗證是否有上傳圖片
            if (uploadedFiles.Count == 0 || uploadedFiles[0].ContentLength == 0)
            {
                Response.Write("<script>alert('請上傳封面');</script>");
                return;
            }
            else if (string.IsNullOrWhiteSpace(categoryList) || string.IsNullOrWhiteSpace(title) ||
                     string.IsNullOrWhiteSpace(content))
            {
                Response.Write("<script>alert('請填寫所有欄位');</script>");
                return;
            }

                // 處理圖片
                HttpPostedFile file = uploadedFiles[0]; // 只取第一張圖片
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif"))
            {
                Response.Write("<script>alert('圖片格式僅限 JPG、JPEG、PNG、GIF');</script>");
                return;
            }

            // 儲存圖片
            string saveFileName = DateTime.Now.Ticks.ToString() + extension;
            string savePath = Server.MapPath("~/Uploads/Photos/") + saveFileName;
            file.SaveAs(savePath);

            // 插入新聞相簿資料（含封面路徑）
            string sqlAlbum = @"INSERT INTO NewsAlbum (CreatedAt, CategoryId, Title, CoverPath, content)
                                VALUES ( @CreatedAt, @CategoryId, @Title, @CoverPath, @content);
                               ";

            var albumParams = new Dictionary<string, object>()
    {
        { "@CreatedAt", DateTime.Now },
        { "@CategoryId", categoryList },
        { "@Title", title },
        { "@CoverPath", saveFileName },
        { "@content", content }
    };

            int resultAlbum = db.ExecuteNonQuery(sqlAlbum, albumParams);
            

            if (resultAlbum > 0)
            {
                Response.Write("<script>alert('成功送出！'); window.location='News-B.aspx';</script>");
            }
            else
            {
                Response.Write("<script>alert('送出失敗，請稍後再試。');</script>");
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("News-B.aspx");
        }
    }
}