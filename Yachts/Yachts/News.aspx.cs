using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts
{
    public partial class News : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNewsAlbum();
                BindCategory();

            }
        }
        private void BindNewsAlbum()  //顯示相簿的Repeater
        {
            string categoryId = Request.QueryString["CategoryId"];

            if (!string.IsNullOrEmpty(categoryId))
            {
                string sql = @"select n.Title, n.CreatedAt , n.Id, n.UpdatedAt, n.categoryId, n.Sticky,  
                                      n.CoverPath,
                                      nc.Name as CategoryName
                               from News n
                               join NewsCategory nc on n.CategoryId =nc.Id
                               where n.CategoryId = @CategoryId
                               order by n.Sticky desc, n.CreatedAt desc
                              ";

                var param = new Dictionary<string, object> { { "@CategoryId", categoryId } };

                DataTable dt = db.SearchDB(sql, param);
                rptNewsAlbum.DataSource = dt;
                rptNewsAlbum.DataBind();

                if (dt.Rows.Count > 0)
                {
                    string categoryName = dt.Rows[0]["CategoryName"].ToString();

                    Label1.Text = categoryName;
                    Label2.Text = categoryName;
                }
            }
        }

        private void BindCategory()  //顯示側邊欄「種類」
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