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
    public partial class Company : System.Web.UI.Page
    {
        DBHelper db =new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindContent();
                BindCategory();

                string categoryId = Request.QueryString["CategoryId"];

                // 如果沒有指定種類，預設導向有資料(第一筆)的種類
                if (string.IsNullOrEmpty(categoryId))
                {
                    // 從資料庫查目前存在的第一筆資料
                    string sql = @"SELECT TOP 1 Id FROM Company ORDER BY Id";
                    DataTable dt = db.SearchDB(sql);

                    if (dt.Rows.Count > 0)
                    {
                        string defaultId = dt.Rows[0]["Id"].ToString();
                        Response.Redirect("Company.aspx?CategoryId=" + defaultId);
                        return;
                    }
                }
            }
        }
        private void BindContent()  //顯示內容的Repeater
        {
            string categoryId = Request.QueryString["CategoryId"];

            if (!string.IsNullOrEmpty(categoryId))
            {
                string sql = @"select c.[content], c.CreatedAt , c.Id, c.UpdatedAt, c.categoryId, Title,
                                      cc.Name as CategoryName
                               from Company c
                               join CompanyCategory cc on c.CategoryId =cc.Id
                               where c.CategoryId = @CategoryId
                               order by CategoryName , c.CreatedAt desc
                              ";

                var param = new Dictionary<string, object> { { "@CategoryId", categoryId } };

                DataTable dt = db.SearchDB(sql, param);
                rptContent.DataSource = dt;
                rptContent.DataBind();

                if (dt.Rows.Count > 0)
                {
                    string categoryName = dt.Rows[0]["CategoryName"].ToString();
                    Label1.Text = categoryName;
                    Label2.Text = categoryName;
                }
            }
        }

        private void BindCategory()
        {
            string sql = @"select Id, Name 
                           from CompanyCategory
                          ";
            DataTable dt = db.SearchDB(sql);
            rptCompany.DataSource = dt;
            rptCompany.DataBind();
        }
    }
}