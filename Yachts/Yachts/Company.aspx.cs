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

            }
        }
        private void BindContent()  //顯示內容的Repeater
        {
            string categoryId = Request.QueryString["CategoryId"];

            if (!string.IsNullOrEmpty(categoryId))
            {
                string sql = @"select c.[content], c.CreatedAt , c.Id, c.UpdatedAt, c.categoryId,
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