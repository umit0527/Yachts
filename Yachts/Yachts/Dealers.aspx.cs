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
                loadList();

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
        private void BindCountry()  //顯示 國家
        {
            string sql = @"select Id, Name 
                           from Country 
                          ";
            DataTable dt = db.SearchDB(sql);
            rptCountry.DataSource = dt;
            rptCountry.DataBind();
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

            string countryId = Request.QueryString["CountryId"];
            if (!string.IsNullOrEmpty(countryId))
            {
                Pagination.targetPage = $"Dealers.aspx?CountryId={countryId}";
            }
            else
            {
                Pagination.targetPage = "Dealers.aspx";
            }

            // 計算資料顯示範圍 
            var floor = (page - 1) * Pagination.limit;
            var limitPerPage = Pagination.limit;

            // 查詢該國家所有經銷商
            int totalCount = 0;

            // 根據是否有 CountryId 來查詢總筆數
            if (!string.IsNullOrEmpty(countryId))
            {
                string countSql = @"
                                    SELECT COUNT(*)
                                    FROM Dealers d
                                    JOIN Country c ON d.CountryId = c.Id
                                    WHERE d.CountryId = @CountryId";

                var countParam = new Dictionary<string, object>
        {
            { "@CountryId", countryId }
        };
                totalCount = Convert.ToInt32(db.SearchDB(countSql, countParam).Rows[0][0]);
            }

            Pagination.totalItems = totalCount;
            Literal1.Text = "<span style='color: white;'>.</span>";

            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(countryId))
            {
                string sql = @"
                                SELECT d.Name as DealerName, d.content, c.Name as CountryName
                                FROM Dealers d
                                JOIN Country c ON d.CountryId = c.Id
                                WHERE d.CountryId = @CountryId
                                ORDER BY DealerName DESC, d.CreatedAt DESC
                                OFFSET @Floor ROWS
                                FETCH NEXT @LimitPerPage ROWS ONLY";

                var param = new Dictionary<string, object>
        {
            { "@CountryId", countryId },
            { "@Floor", floor },
            { "@LimitPerPage", limitPerPage }
        };

                dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    string categoryName = dt.Rows[0]["CountryName"].ToString();
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
                // 如果沒有 CountryId，顯示所有新聞的內容，並進行分頁
                string allNewsSql = @"
                                      SELECT d.Name as DealerName, d.content, c.Name as CountryName
                                FROM Dealers d
                                JOIN Country c ON d.CountryId = c.Id
                                ORDER BY DealerName DESC, d.CreatedAt DESC
                                OFFSET @Floor ROWS
                                FETCH NEXT @LimitPerPage ROWS ONLY";

                var allNewsParam = new Dictionary<string, object>
        {
            { "@Floor", floor },
            { "@LimitPerPage", limitPerPage }
        };
                dt = db.SearchDB(allNewsSql, allNewsParam);
            }

            rptContent.DataSource = dt;
            rptContent.DataBind();

            Pagination.showPageControls();
        }
    }
}