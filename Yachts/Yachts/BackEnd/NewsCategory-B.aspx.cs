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
    public partial class NewsCategory_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRepeater();
            }
        }

        private void BindRepeater()  //顯示種類Repeater
        {
            //先不撈admin
            string sql = @"select *
                           from NewsCategory
                           order by Name , CreatedAt desc
                          ";
            DataTable dt = db.SearchDB(sql);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }

        protected void btnAddNewsCategory_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddNewsCategory.aspx");
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string sql = "delete from NewsCategory where Id = @Id";
                var dict = new Dictionary<string, object> { { "@Id", id } };
                db.ExecuteNonQuery(sql, dict);

                string success = "<script>alert('刪除成功'); window.location='NewsCategory-B.aspx';</script>";
                Response.Write(success);
            }
        }
    }
}