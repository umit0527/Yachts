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