using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class Dealers : System.Web.UI.Page
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
            string sql = @"select d.[content], d.CreatedAt , d.Id, d.UpdatedAt,
                                  Country.Name AS CountryName
                           from Dealers d 
                           join Country on d.CountryId =Country.Id
                           order by CountryName ,d.CreatedAt desc
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
                string sql = "DELETE FROM Dealers WHERE Id = @Id";
                var dict = new Dictionary<string, object> { { "@Id", id } };
                db.ExecuteNonQuery(sql, dict);

                string success = "<script>alert('刪除成功'); window.location='Dealers-B.aspx';</script>";
                Response.Write(success);
            }
        }

        protected void btnAddDealer_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddDealer.aspx");
        }
    }
}