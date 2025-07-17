using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class EditNnews_B : System.Web.UI.Page
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
                LoadNewsData();
                //BindCategoryList();


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

        private void LoadNewsData()
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }
            else
            {
                int newsId = int.Parse(Request.QueryString["Id"]);

                string sql = @"select CategoryId,Title, Content ,UpdatedAt ,CoverPath ,Sticky
                               from News
                               where Id = @Id";

                var param = new Dictionary<string, object> { { "@Id", newsId } };
                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    int categoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                    string content = dt.Rows[0]["Content"].ToString();
                    string coverPath = dt.Rows[0]["CoverPath"].ToString();
                    string title = dt.Rows[0]["Title"].ToString();
                    bool sticky = Convert.ToBoolean(dt.Rows[0]["Sticky"] != DBNull.Value && Convert.ToBoolean(dt.Rows[0]["Sticky"]));

                    txtTitle.Text = title;
                    imgCover.ImageUrl = "~/Uploads/Photos/" + coverPath;

                    BindCategoryList(); // 所有種類
                    CategoryList.SelectedValue = categoryId.ToString();

                    chbSticky.Checked = sticky;
                    CKEditor1.Text = content;
                }
            }
        }

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
            int newsId = int.Parse(Request.QueryString["Id"]);
            string content = CKEditor1.Text.Trim();
            // 把 HTML tag 移除，只留下純文字，用來檢查「送出時是否真的為空，還是保有html標籤」
            string plainText = Regex.Replace(content, "<.*?>", "").Trim();
            string categoryList = CategoryList.SelectedValue;
            string title = txtTitle.Text.Trim();
            string newCoverPath = "";
            HttpFileCollection uploadedFiles = Request.Files;
            bool sticky = chbSticky.Checked;

            if (Request.QueryString["Id"] != null)
            {
                // 驗證是否有上傳圖片、選擇種類、填寫標題與內容
                if (string.IsNullOrWhiteSpace(categoryList) || string.IsNullOrWhiteSpace(title) ||
                    string.IsNullOrWhiteSpace(plainText))
                {
                    Response.Write("<script>alert('必填欄位填寫不完整');</script>");
                    return;
                }

                // 取得原本的封面路徑
                string getCoverSql = "SELECT CoverPath FROM News WHERE Id = @Id";
                var paramCover = new Dictionary<string, object> { { "@Id", newsId } };
                DataTable dtCover = db.SearchDB(getCoverSql, paramCover);
                
                if (dtCover.Rows.Count > 0)
                {
                    newCoverPath = dtCover.Rows[0]["CoverPath"].ToString();
                }

                // 如果有上傳新圖片，則覆蓋原圖
                if (uploadedFiles.Count > 0 && uploadedFiles[0].ContentLength > 0)
                {
                    HttpPostedFile file = uploadedFiles[0];
                    string extension = Path.GetExtension(file.FileName).ToLower();

                    if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif"))
                    {
                        Response.Write("<script>alert('圖片格式僅限 JPG、JPEG、PNG、GIF');</script>");
                        return;
                    }

                    // 儲存新圖片
                    string saveFileName = DateTime.Now.Ticks.ToString() + extension;
                    string savePath = Server.MapPath("~/Uploads/Photos/") + saveFileName;
                    file.SaveAs(savePath);

                    newCoverPath = saveFileName; // 使用新圖片
                }

                // 更新新聞資料（含封面路徑）
                string sql = @"update News set 
                                              Content=@Content  , 
                                              CategoryId=@CategoryId ,
                                              Title=@Title,
                                              CoverPath=@CoverPath,
                                              UpdatedAt=@UpdatedAt,
                                              Sticky=@Sticky
                                              where Id=@Id
                              ";

                var Params = new Dictionary<string, object>()
    {
        { "@Content", content },
        { "@CategoryId", categoryList },
        { "@Title", title },
        { "@CoverPath", newCoverPath  },
        { "@UpdatedAt", DateTime.Now },
        {"@Sticky", sticky},
        { "@Id", newsId }
    };

                int result = db.ExecuteNonQuery(sql, Params);

                if (result > 0)
                {
                    Response.Write("<script>alert('更新成功！'); window.location='News-B.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('更新失敗，請稍後再試！');</script>");
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("News-B.aspx");
        }
    }
}