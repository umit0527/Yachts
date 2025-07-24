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

        // 分頁設定
        int pageSize = 5;
        int currentPage = 1;
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
            // 計算總頁數
            string countSql = @"
                                SELECT COUNT(*) 
                                FROM NewsCategory n 
                               ";

            int totalRows = Convert.ToInt32(db.SearchDB(countSql).Rows[0][0]);
            int totalPages = (int)Math.Ceiling((double)totalRows / pageSize);

            List<int> pages = new List<int>();
            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(i);
            }

            rptPagination.DataSource = pages;
            rptPagination.DataBind();
        }
        private void BindRepeater()  //顯示種類Repeater
        {
            int offset = (currentPage - 1) * pageSize;

            //先不撈admin
            string sql = @"select *
                           from NewsCategory
                           order by Name , CreatedAt desc
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