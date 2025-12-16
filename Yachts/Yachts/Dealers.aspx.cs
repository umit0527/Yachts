using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

            }
        }
        private void BindContent()  //顯示內容的Repeater
        {
            string countryId = Request.QueryString["countryId"];

            if (!string.IsNullOrEmpty(countryId))
            {
                string sql = @"select d.[content], d.CreatedAt , d.Id, d.UpdatedAt,
                                  Country.Name AS CountryName, City.Name AS CityName
                           from Dealers d 
                           join Country on d.CountryId =Country.Id
                           join City on d.CityId=City.Id
                           where d.CountryId = @CountryId
                           order by CountryName , CityName  ,d.CreatedAt desc
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