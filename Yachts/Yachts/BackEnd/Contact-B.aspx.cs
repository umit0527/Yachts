using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
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
        DBHelper db = new DBHelper();
        private int pageSize =10; // 每頁顯示幾筆
        int currentPage = 1; // 預設頁碼
        public int CurrentPage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currentPage = GetCurrentPage();
                CurrentPage = currentPage;

                BindGridview();
                ShowPagination();
            }
        }
        private int GetCurrentPage()  //抓取目前分頁
        {
            int page;
            return int.TryParse(Request.QueryString["page"], out page) && page > 0 ? page : 1;
        }
        private void ShowPagination()  //顯示分頁
        {
            string countSql = "SELECT COUNT(*) FROM Contact";
            int totalCount = Convert.ToInt32(db.SearchDB(countSql).Rows[0][0]);

            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            List<int> pageNumbers = new List<int>();
            for (int i = 1; i <= totalPages; i++)
            {
                pageNumbers.Add(i);
            }

            rptPagination.DataSource = pageNumbers;
            rptPagination.DataBind();
        }
        private void BindGridview()
        {
            int offset = (currentPage - 1) * pageSize;

            DBHelper db = new DBHelper();
            string sql = @"select c.Id, c.Name, c.Email, c.Phone, c.Comments, 
                           c.SendedAt,
                           cy.Name AS CountryName,
                           (RTRIM(m.Name) + ' '+CONVERT(varchar, m.Number)) as DisplayName
                           from Contact c
                           left Join Country cy on c.CountryId = cy.Id
                           left join Model m on c.ModelId=m.id 
                           order by c.SendedAt desc
                           OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                          ";
            var param = new Dictionary<string, object>
            {
                { "@Offset", offset },
                { "@PageSize", pageSize }
            };
            DataTable dt = db.SearchDB(sql,param);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}