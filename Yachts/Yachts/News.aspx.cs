using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts
{
    public partial class News : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNewsAlbum();
                BindCategory();
                loadList();
                string categoryId = Request.QueryString["CategoryId"];

                // 如果沒有指定消息，預設導向「種類裡有資料」的第一筆
                if (string.IsNullOrEmpty(categoryId))
                {
                    // 從資料庫查目前存在的第一筆資料
                    string sql = @"SELECT TOP 1 nc.Id 
                                   FROM NewsCategory nc
                                   JOIN News n ON nc.Id = n.CategoryId
                                   ORDER BY n.Id";
                    DataTable dt = db.SearchDB(sql);

                    if (dt.Rows.Count > 0)
                    {
                        string defaultId = dt.Rows[0]["Id"].ToString();
                        Response.Redirect("News.aspx?CategoryId=" + defaultId);
                        return;
                    }
                }
            }
        }
        private void BindNewsAlbum()  //顯示相簿的Repeater
        {
            string categoryId = Request.QueryString["CategoryId"];

            if (!string.IsNullOrEmpty(categoryId))
            {
                string sql = @"select n.Title, n.CreatedAt , n.Id, n.UpdatedAt, n.categoryId, n.Sticky,  
                                      n.CoverPath,
                                      nc.Name as CategoryName
                               from News n
                               join NewsCategory nc on n.CategoryId =nc.Id
                               where n.CategoryId = @CategoryId
                               order by n.Sticky desc, n.CreatedAt desc
                              ";

                var param = new Dictionary<string, object> { { "@CategoryId", categoryId } };

                DataTable dt = db.SearchDB(sql, param);
                rptNewsAlbum.DataSource = dt;
                rptNewsAlbum.DataBind();

                if (dt.Rows.Count > 0)
                {
                    string categoryName = dt.Rows[0]["CategoryName"].ToString();

                    Label1.Text = categoryName;
                    Label2.Text = categoryName;
                }
            }
        }
        private void BindCategory()  //顯示側邊欄「種類」
        {
            string sql = @"select Id, Name 
                           from NewsCategory
                          ";

            DataTable dt = db.SearchDB(sql);
            rptNewsCategory.DataSource = dt;
            rptNewsCategory.DataBind();
        }
        private void loadList()  //顯示 分頁
        {
            // 取得頁面參數
            int page = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                if (int.TryParse(Request.QueryString["page"], out int parsedPage))
                {
                    page = parsedPage;
                }
            }

            // 設定分頁參數
            Pagination.limit = 5; 

            string categoryId = Request.QueryString["CategoryId"];
            if (!string.IsNullOrEmpty(categoryId))
            {
                Pagination.targetPage = $"News.aspx?CategoryId={categoryId}";
            }
            else
            {
                Pagination.targetPage = "News.aspx";
            }

            // 計算資料顯示範圍 
            var floor = (page - 1) * Pagination.limit;
            var limitPerPage = Pagination.limit;

            // 查詢該種類所有消息
            int totalCount = 0;

            // 根據是否有 CategoryId 來查詢總筆數
            if (!string.IsNullOrEmpty(categoryId))
            {
                string countSql = @"
                                    SELECT COUNT(*)
                                    FROM News n
                                    JOIN NewsCategory nc ON n.CategoryId = nc.Id
                                    WHERE n.CategoryId = @CategoryId";

                var countParam = new Dictionary<string, object>
        {
            { "@CategoryId", categoryId }
        };
                totalCount = Convert.ToInt32(db.SearchDB(countSql, countParam).Rows[0][0]);
            }

            Pagination.totalItems = totalCount;
            Literal1.Text = "<span style='color: white;'>.</span>";

            DataTable dt = new DataTable(); 

            if (!string.IsNullOrEmpty(categoryId))
            {
            string sql = @"
            SELECT n.Title, n.CreatedAt, n.Id, n.UpdatedAt, n.categoryId, n.Sticky,
                   n.CoverPath,
                   nc.Name AS CategoryName
            FROM News n
            JOIN NewsCategory nc ON n.CategoryId = nc.Id
            WHERE n.CategoryId = @CategoryId
            ORDER BY n.Sticky DESC, n.CreatedAt DESC
            OFFSET @Floor ROWS
            FETCH NEXT @LimitPerPage ROWS ONLY"; 

                var param = new Dictionary<string, object>
        {
            { "@CategoryId", categoryId },
            { "@Floor", floor },
            { "@LimitPerPage", limitPerPage } 
        };

                dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    string categoryName = dt.Rows[0]["CategoryName"].ToString();
                    Label1.Text = categoryName;
                    Label2.Text = categoryName;
                }
                else
                {
                    // 如果該分類下沒有資料，清空標籤
                    Label1.Text = "";
                    Label2.Text = "";
                }
            }
            else
            {
                // 如果沒有 CategoryId，顯示所有新聞的內容，並進行分頁
                string allNewsSql = @"
            SELECT n.Title, n.CreatedAt, n.Id, n.UpdatedAt, n.categoryId, n.Sticky,
                   n.CoverPath,
                   nc.Name AS CategoryName
            FROM News n
            JOIN NewsCategory nc ON n.CategoryId = nc.Id
            ORDER BY n.Sticky DESC, n.CreatedAt DESC
            OFFSET @Floor ROWS
            FETCH NEXT @LimitPerPage ROWS ONLY";

                var allNewsParam = new Dictionary<string, object>
        {
            { "@Floor", floor },
            { "@LimitPerPage", limitPerPage }
        };
                dt = db.SearchDB(allNewsSql, allNewsParam);
            }

            rptNewsAlbum.DataSource = dt;
            rptNewsAlbum.DataBind();

            Pagination.showPageControls();
        }
    }
}