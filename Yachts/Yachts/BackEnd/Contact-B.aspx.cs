using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Instrumentation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class Contact_B : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridview();
            }
        }
        private void BindGridview()
        {
            DBHelper db = new DBHelper();
            string sql = @"select c.Id, c.Name, c.Email, c.Phone, c.Comments, 
                           c.SendedAt,
                           cy.Name AS CountryName,
                           (RTRIM(b.Name) + ' '+CONVERT(varchar, b.Number)) as DisplayName
                           from Contact c
                           left Join Country cy on c.CountryId = cy.Id
                           left join Brochure b on c.BrochureId=b.id 
                           order by c.SendedAt desc
                          ";
            DataTable dt = db.SearchDB(sql);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}