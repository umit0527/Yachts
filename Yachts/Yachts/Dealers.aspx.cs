using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.BackEnd;
using Yachts.Helper;

namespace Yachts.FrontEnd
{
    public partial class Dealers : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindContent();
                BindCountry();

                string countryId = Request.QueryString["CountryId"];

                // 如果沒有指定經銷商國家，預設導向「國家內有經銷商內容」的第一筆資料
                if (string.IsNullOrEmpty(countryId))
                {
                    // 從資料庫查目前存在的第一筆資料
                    string sql = @"SELECT TOP 1 c.Id 
                                   FROM Country c
                                   JOIN Dealers d ON c.Id = d.CountryId
                                   ORDER BY c.Id";
                    DataTable dt = db.SearchDB(sql);

                    if (dt.Rows.Count > 0)
                    {
                        string defaultId = dt.Rows[0]["Id"].ToString();
                        Response.Redirect("Dealers.aspx?CountryId=" + defaultId);
                        return;
                    }
                }
            }
        }
        private void BindContent()  //顯示內容的Repeater
        {
            string countryId = Request.QueryString["countryId"];

            if (!string.IsNullOrEmpty(countryId))
            {
                string sql = @"select d.[content], d.CreatedAt , d.Id, d.UpdatedAt,
                                  Country.Name AS CountryName
                           from Dealers d 
                           join Country on d.CountryId =Country.Id
                           where d.CountryId = @CountryId
                           order by CountryName ,d.CreatedAt desc
                          ";

                var param = new Dictionary<string, object> { { "@CountryId", countryId } };

                DataTable dt = db.SearchDB(sql,param);
                rptContent.DataSource = dt;
                rptContent.DataBind();

                if (dt.Rows.Count > 0)
                {
                    Label1.Text = dt.Rows[0]["CountryName"].ToString();
                    Label2.Text= dt.Rows[0]["CountryName"].ToString();
                }
            }
        }

        private void BindCountry()  
        {
            string sql = @"select Id, Name 
                           from Country 
                          ";
            DataTable dt = db.SearchDB(sql);
            rptCountry.DataSource = dt;
            rptCountry.DataBind();
        }
    }
}