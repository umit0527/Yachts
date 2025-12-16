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
        private void BindRepeater()  //顯示 列表內容 的Repeater
        {
            int offset = (currentPage - 1) * pageSize;

            string sql = @"select n.CreatedAt, n.Id, n.UpdatedAt, n.Title, n.content, n.Sticky,
                           n.CoverPath ,
                           nc.Name as CategoryName
                           from News n 
                           left join NewsCategory nc on n.CategoryId =nc.Id
                           order by n.Sticky desc ,CategoryName desc, n.CreatedAt desc
                           OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                          ";
            var param = new Dictionary<string, object>
            {
                { "@Offset", offset },
                { "@PageSize", pageSize }
            };
            DataTable dt = db.SearchDB(sql,param);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }
        private void ShowPagination()  //顯示分頁
        {
            // 計算總頁數
            string countSql = @"
                SELECT COUNT(*) 
                FROM News n 
                LEFT JOIN NewsCategory nc ON n.CategoryId = nc.Id";

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
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)  //刪除的核心
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
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)  //顯示 檔案 的repeater
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // 取得每筆資料的 NewsId
                int newsId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Id"));

                // 找到子 Repeater 控制項
                Repeater rptDownloads = (Repeater)e.Item.FindControl("rptDownloads");

                if (rptDownloads != null)
                {
                    // 查詢該新聞所有的下載檔案
                    string sql = "SELECT * FROM NewsDownloads WHERE NewsId = @NewsId";
                    var parameters = new Dictionary<string, object>
            {
                { "@NewsId", newsId }
            };

                    DataTable dtDownloads = db.SearchDB(sql, parameters);

                    // 綁定資料給子Repeater
                    rptDownloads.DataSource = dtDownloads;
                    rptDownloads.DataBind();
                }
            }
        }
        protected void btnAddNews_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddNews.aspx");
        }
    }
}