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
    public partial class News_B : System.Web.UI.Page
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

            string sql = @"select n.CreatedAt, n.Id, n.UpdatedAt, n.Title, n.content, n.Sticky,
                           n.CoverPath ,nc.Name as CategoryName
                           from News n 
                           join NewsCategory nc on n.CategoryId =nc.Id
                           order by n.Sticky desc, nc.Name , n.CreatedAt desc
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
                string sql = "delete from News WHERE Id = @Id";
                var dict = new Dictionary<string, object> { { "@Id", id } };
                db.ExecuteNonQuery(sql, dict);

                string success = "<script>alert('刪除成功'); window.location='News-B.aspx';</script>";
                Response.Write(success);
            }
        }

        protected void btnAddNews_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddNews.aspx");
        }
    }
}