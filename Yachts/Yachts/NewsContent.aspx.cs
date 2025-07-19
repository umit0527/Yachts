using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts
{
    public partial class NewsContent : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNewsContent();
                BindCategory();

                string newsId = Request.QueryString["Id"];
                Session["CategoryId"] = Request.QueryString["CategoryId"];

                // 預設導向第一個種類的消息列表
                if (string.IsNullOrEmpty(newsId))
                {
                    // 從資料庫查目前存在的第一筆資料
                    string sql = @"SELECT TOP 1 Id FROM NewsCategory ORDER BY Id";
                    DataTable dt = db.SearchDB(sql);

                    if (dt.Rows.Count > 0)
                    {
                        string defaultId = dt.Rows[0]["Id"].ToString();
                        Response.Redirect("News.aspx?CategoryId=" + defaultId);
                        return;
                    }
                }
            }
        }
        private void BindNewsContent()  //顯示內容的Repeater
        {
            string categoryId = Request.QueryString["Id"];

            if (!string.IsNullOrEmpty(categoryId))
            {
                string sql = @"select n.Title, n.CreatedAt , n.Id, n.UpdatedAt, n.content ,
                                      nc.Name as CategoryName
                               from News n
                               join NewsCategory nc on nc.Id=n.CategoryId
                               where n.Id=@Id
                              ";

                var param = new Dictionary<string, object> { { "@Id", categoryId } };

                DataTable dt = db.SearchDB(sql, param);
                rptNewsContent.DataSource = dt;
                rptNewsContent.DataBind();

                if (dt.Rows.Count > 0)
                {
                    string title = dt.Rows[0]["Title"].ToString();
                    string categoryName = dt.Rows[0]["CategoryName"].ToString();

                    Label1.Text = categoryName;
                    Label2.Text = categoryName;
                }
            }
        }

        private void BindCategory()
        {
            string sql = @"select Id, Name 
                           from NewsCategory
                          ";

            DataTable dt = db.SearchDB(sql);
            rptNewsCategory.DataSource = dt;
            rptNewsCategory.DataBind();
        }
    }
}