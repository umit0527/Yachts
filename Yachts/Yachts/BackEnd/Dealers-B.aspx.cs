using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class Dealers : System.Web.UI.Page
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
            Repeater1.ItemDataBound += Repeater1_ItemDataBound;
        }
        private int GetCurrentPage()  //抓取目前分頁
        {
            int page;
            return int.TryParse(Request.QueryString["page"], out page) && page > 0 ? page : 1;
        }
        private void ShowPagination()  //顯示分頁
        {
            string countSql = "SELECT COUNT(*) FROM Dealers";
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
        private void BindRepeater()  //顯示 主要內容Repeater
        {
            string sql = @"
SELECT 
    c.Id AS CountryId,
    c.Name AS CountryName,
    d.Id AS DealerId,
    d.Content,
    d.CreatedAt,
    d.UpdatedAt
FROM Dealers d
JOIN Country c ON d.CountryId = c.Id
ORDER BY c.Name, d.CreatedAt DESC";

            DataTable dt = db.SearchDB(sql);

            // 分組：依 CountryId 和 CountryName
            var grouped = dt.AsEnumerable()
                .GroupBy(r => new {
                    CountryId = r.Field<int>("CountryId"),
                    CountryName = r.Field<string>("CountryName")
                })
                .Select(g => new {
                    CountryId = g.Key.CountryId,
                    CountryName = g.Key.CountryName,
                    Dealers = g.CopyToDataTable()
                }).ToList();

            Repeater1.DataSource = grouped;
            Repeater1.DataBind();
        }
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)  //綁定刪除
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string sql = "DELETE FROM Dealers WHERE Id = @Id";
                var dict = new Dictionary<string, object> { { "@Id", id } };
                db.ExecuteNonQuery(sql, dict);

                string success = "<script>alert('刪除成功'); window.location='Dealers-B.aspx';</script>";
                Response.Write(success);
            }
        }
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var countryGroup = (dynamic)e.Item.DataItem;
                Repeater rptDealers = (Repeater)e.Item.FindControl("rptDealers");
                rptDealers.DataSource = countryGroup.Dealers;
                rptDealers.DataBind();
            }
        }
        protected void btnAddDealer_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddDealer.aspx");
        }
    }
}