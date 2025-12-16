using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class Search : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        //private int pageSize = 5; // 每頁顯示幾筆
        //int currentPage = 1; // 預設頁碼
        //public int CurrentPage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string searchQuery = Server.UrlDecode(Request.QueryString["search"]);
                string modelId = Request.QueryString["Id"];

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    bool hasYachts = SearchYachts(searchQuery);
                    bool hasNews = SearchNews(searchQuery);
                    if (hasYachts)
                    {
                        pnlYachts.Visible = true;
                    }
                    if (hasNews)
                    {
                        pnlNews.Visible = true;
                    }
                    if (!hasYachts && !hasNews)
                    {
                        Response.Write("<script>alert('查無資料');</script>");
                    }
                }
            }
        }
        public static string RemoveHtmlTags(string input)  //去除html標籤
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            else
            {
                string pattern = "<.*?>";
                return Regex.Replace(input, pattern, string.Empty);
            }
        }
        private bool SearchYachts(string search)  //顯示 Yachts的搜尋結果
        {
            string input = "%" + search + "%";

            string sql = @"
SELECT 
    m.Id AS ModelId,
    m.Name + ' ' + CONVERT(nvarchar, m.Number) AS ModelName,
    m.Label, m.CreatedAt, m.UpdatedAt,
    (SELECT TOP 1 FilePath FROM YachtsDownloads WHERE ModelId = m.Id) AS FilePath,
    (SELECT TOP 1 Content FROM YachtsContent WHERE ModelId = m.Id) AS Content,
    (SELECT TOP 1 Specification FROM YachtsContent WHERE ModelId = m.Id) AS Specification,
    (SELECT TOP 1 DeckImgPath1 FROM YachtsLayoutImage WHERE ModelId = m.Id) AS DeckImgPath1,
    (SELECT TOP 1 DeckImgPath2 FROM YachtsLayoutImage WHERE ModelId = m.Id) AS DeckImgPath2,
    (SELECT TOP 1 InteriorImgPath FROM YachtsLayoutImage WHERE ModelId = m.Id) AS InteriorImgPath,
(SELECT TOP 1 Name FROM Principal WHERE ModelId = m.Id) AS PrincipalName,
(SELECT TOP 1 Value FROM Principal WHERE ModelId = m.Id) AS PrincipalValue
    
FROM Model m
WHERE 
    m.Name + ' ' + CONVERT(nvarchar, m.Number) LIKE @input
    OR m.Label LIKE @input
    OR EXISTS (
        SELECT 1 FROM YachtsDownloads yd 
        WHERE yd.ModelId = m.Id AND yd.FilePath LIKE @input
    )
    OR EXISTS (
        SELECT 1 FROM YachtsContent yc 
        WHERE yc.ModelId = m.Id AND 
              (yc.Content LIKE @input OR yc.Specification LIKE @input)
    )
    OR EXISTS (
        SELECT 1 FROM Principal p 
        WHERE p.ModelId = m.Id AND 
              (p.Name LIKE @input OR p.Value LIKE @input)
    )
order by m.CreatedAt desc
";

            var param = new Dictionary<string, object> { { "@input", input } };

            DataTable dt = db.SearchDB(sql, param);

            if (dt.Rows.Count > 0)
            {
                // 加入新的欄位：純文字版本
                if (!dt.Columns.Contains("SpecificationText"))
                    dt.Columns.Add("SpecificationText", typeof(string));
                if (!dt.Columns.Contains("ContentText"))
                    dt.Columns.Add("ContentText", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    // 去除 Specification 欄位的 HTML 標籤
                    if (dt.Columns.Contains("Specification") && row["Specification"] != DBNull.Value)
                    {
                        string rawSpec = row["Specification"].ToString();
                        row["SpecificationText"] = RemoveHtmlTags(rawSpec);
                    }

                    // 移除 Content 的 HTML
                    if (dt.Columns.Contains("Content") && row["Content"] != DBNull.Value)
                    {
                        string rawContent = row["Content"].ToString();
                        row["ContentText"] = RemoveHtmlTags(rawContent);
                    }

                }
                rptYachts.DataSource = dt;
                rptYachts.DataBind();
                return true;
            }
            return false;
        }
        private bool SearchNews(string search)  //顯示 News的搜尋結果
        {
            string input = "%" + search + "%";

            string sql = @"
SELECT 
    n.Id , n.Title, n.Content, n.CreatedAt, n.Sticky, n.CoverPath, n.UpdatedAt,
    (SELECT TOP 1 FilePath FROM NewsDownloads WHERE NewsId = n.Id) AS FilePath, 
nc.Name as CategoryName
                           from News n 
                           left join NewsCategory nc on n.CategoryId =nc.Id
WHERE 
    n.Title LIKE @input
    OR n.Content LIKE @input
    OR EXISTS (
        SELECT 1 FROM NewsDownloads nd
        WHERE nd.NewsId = n.Id AND nd.FilePath LIKE @input
    )
";

            var param = new Dictionary<string, object> { { "@input", input } };

            DataTable dt = db.SearchDB(sql, param);
            if (dt.Rows.Count > 0)
            {
                // 加入新的欄位：純文字版本
                if (!dt.Columns.Contains("ContentText"))
                    dt.Columns.Add("ContentText", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    // 移除 Content 的 HTML，並儲存至 ContentText
                    if (dt.Columns.Contains("Content") && row["Content"] != DBNull.Value)
                    {
                        string rawContent = row["Content"].ToString();
                        row["ContentText"] = RemoveHtmlTags(rawContent);
                    }
                }

                // 進行程式中的搜尋過濾
                if (!string.IsNullOrEmpty(search))
                {
                    var filteredRows = dt.AsEnumerable()
                    .Where(row => row["ContentText"].ToString().Contains(search) ||
                           row["Title"].ToString().Contains(search));

                    if (filteredRows.Any())
                    {
                        rptNews.DataSource = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        // 若無符合條件的資料則清空，不顯示資料
                        rptNews.DataSource = null;
                    }
                }
                else
                {
                    rptNews.DataSource = dt;
                }

                rptNews.DataBind();
                return true;
            }
            return false;
        }
        protected void rptYachts_ItemDataBound(object sender, RepeaterItemEventArgs e)  //顯示 Yachts"輪播圖、檔案下載與欄位" 的Repeater
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView rowView = (DataRowView)e.Item.DataItem;
                int modelId = Convert.ToInt32(rowView["ModelId"]);

                // 綁定輪播圖
                Repeater rptCarouselImgs = (Repeater)e.Item.FindControl("rptCarouselImgs");
                if (rptCarouselImgs != null)
                {
                    string carouselSql = @"SELECT ImgPath as CarouselImgPath 
                                           FROM YachtsCarouselImage 
                                           WHERE ModelId = @ModelId 
                                           ORDER BY CreatedAt DESC";
                    var carouselParams = new Dictionary<string, object> { { "@ModelId", modelId } };

                    DataTable dtCarousel = db.SearchDB(carouselSql, carouselParams);
                    if (dtCarousel != null && dtCarousel.Rows.Count > 0)
                    {
                        rptCarouselImgs.DataSource = dtCarousel;
                        rptCarouselImgs.DataBind();
                    }
                }

                // 綁定檔案下載
                Repeater rptFiles = (Repeater)e.Item.FindControl("rptFiles");
                if (rptFiles != null)
                {
                    string fileSql = @"SELECT FilePath 
                                       FROM YachtsDownloads 
                                       WHERE ModelId = @ModelId 
                                       ORDER BY CreatedAt DESC";
                    var fileParams = new Dictionary<string, object> { { "@ModelId", modelId } };

                    DataTable dt = db.SearchDB(fileSql, fileParams);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        rptFiles.DataSource = dt;
                        rptFiles.DataBind();
                    }
                }

                // 綁定 Principal 參數列表
                Repeater rptPrincipal = (Repeater)e.Item.FindControl("rptPrincipal");
                if (rptPrincipal != null)
                {
                    string principalSql = @"SELECT Name as PrincipalName, Value as PrincipalValue FROM Principal WHERE ModelId = @ModelId ORDER BY Name";
                    var principalParams = new Dictionary<string, object> { { "@ModelId", modelId } };

                    DataTable dtPrincipal = db.SearchDB(principalSql, principalParams);
                    if (dtPrincipal != null && dtPrincipal.Rows.Count > 0)
                    {
                        rptPrincipal.DataSource = dtPrincipal;
                        rptPrincipal.DataBind();
                    }
                }
            }
        }
        protected void rptYachts_ItemCommand(object source, RepeaterCommandEventArgs e)  //Yachts 的刪除核心
        {
            int id = Convert.ToInt32(e.CommandArgument);

            // 取得 ModelId
            string getModelSql = "SELECT ModelId FROM YachtsContent WHERE Id = @Id";
            var getParam = new Dictionary<string, object> { { "@Id", id } };
            DataTable dt = db.SearchDB(getModelSql, getParam);
            if (dt.Rows.Count > 0)
            {
                int modelId = Convert.ToInt32(dt.Rows[0]["ModelId"]);

                // 刪除 Downloads
                db.ExecuteNonQuery("DELETE FROM YachtsDownloads WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

                // 刪除 Layout Image
                db.ExecuteNonQuery("DELETE FROM YachtsLayoutImage WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

                // 刪除 YachtsContent
                db.ExecuteNonQuery("DELETE FROM YachtsContent WHERE Id = @Id", new Dictionary<string, object> { { "@Id", id } });

                // 刪除 Model
                db.ExecuteNonQuery("DELETE FROM Model WHERE Id = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });
                //刪除 輪播圖    
                db.ExecuteNonQuery("DELETE FROM YachtsCarouselImage WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });
                //刪除 Principal
                db.ExecuteNonQuery("DELETE FROM Principal WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

            }

            Response.Write("<script>alert('刪除成功'); window.location='Yachts-B.aspx';</script>");
        }
        protected void rptNews_ItemCommand(object source, RepeaterCommandEventArgs e)  //刪除的核心
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
        protected void rptNews_ItemDataBound(object sender, RepeaterItemEventArgs e)  //顯示 檔案 的repeater
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
        protected void btnAddYachts_Click(object sender, EventArgs e)  //新增yachts
        {
            Response.Redirect("AddYachts.aspx");
        }
        protected void btnAddNews_Click(object sender, EventArgs e)  //新增news
        {
            Response.Redirect("AddNews.aspx");
        }
    }
}