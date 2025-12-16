using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class Company_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRepeater();
            }
        }

        private void BindRepeater()  //顯示Repeater
        {
            string sql = @"select c.[content], c.CreatedAt , c.Id, c.UpdatedAt,
                                  cc.Name as CategoryName
                           from Company c 
                           join CompanyCategory cc on c.CategoryId =cc.Id
                           order by CategoryName ,c.CreatedAt desc
                          ";
            DataTable dt = db.SearchDB(sql);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string sql = "DELETE FROM Company WHERE Id = @Id";
                var dict = new Dictionary<string, object> { { "@Id", id } };
                db.ExecuteNonQuery(sql, dict);

                string success = "<script>alert('刪除成功'); window.location='Company-B.aspx';</script>";
                Response.Write(success);
            }
        }

        protected void btnAddContent_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddCompany.aspx");
        }
    }
}