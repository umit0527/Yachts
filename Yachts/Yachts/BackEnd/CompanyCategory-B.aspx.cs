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
    public partial class CompantCategory_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        private int pageSize = 5; // 每頁顯示幾筆
        int currentPage = 1; // 預設頁碼
        public int CurrentPage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currentPage = GetCurrentPage();
                CurrentPage = currentPage;

                BindRepeater();

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
            string countSql = "SELECT COUNT(*) FROM CompanyCategory";
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
        private void BindRepeater()  //顯示Repeater
        {
            int offset = (currentPage - 1) * pageSize;

            //先不撈admin
            string sql = @"select *
                           from CompanyCategory
                           order by CreatedAt desc,Name 
                           OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                          ";
            var param = new Dictionary<string, object>
            {
                { "@Offset", offset },
                { "@PageSize", pageSize }
            };
            DataTable dt = db.SearchDB(sql, param);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }
        protected void btnAddCompanyCategory_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddCompanyCategory.aspx");
        }
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string sql = "delete from CompanyCategory where Id = @Id";
                var dict = new Dictionary<string, object> { { "@Id", id } };
                db.ExecuteNonQuery(sql, dict);

                string success = "<script>alert('刪除成功'); window.location='CompanyCategory-B.aspx';</script>";
                Response.Write(success);
            }
        }
    }
}