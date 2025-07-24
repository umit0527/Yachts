﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class Yachts : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        private int pageSize = 5; // 每頁顯示幾筆
        int currentPage = 1; // 預設頁碼
        public int CurrentPage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string searchQuery = Server.UrlDecode(Request.QueryString["search"]);

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    SearchYachts(searchQuery);
                    rptPagination.Visible = false;  // 搜尋時不顯示分頁
                }
                else
                {
                    currentPage = GetCurrentPage();
                    CurrentPage = currentPage;

                    BindRepeater();        // 一般模式顯示全部
                    ShowPagination();
                    rptPagination.Visible = true;
                }
            }
        }
        private void SearchYachts(string search)  //顯示 搜尋結果
        {
            string input = search;

            string sql = @"SELECT 
    m.Id AS ModelId,
    m.Name + ' ' + CONVERT(nvarchar, m.Number) AS ModelName,
    m.Label, m.CreatedAt,m.UpdatedAt,
    (SELECT TOP 1 FilePath FROM YachtsDownloads WHERE ModelId = m.Id) AS FilePath,
    (SELECT TOP 1 Content FROM YachtsContent WHERE ModelId = m.Id) AS Content,
    (SELECT TOP 1 Specification FROM YachtsContent WHERE ModelId = m.Id) AS Specification,
    (SELECT TOP 1 DeckImgPath1 FROM YachtsLayoutImage WHERE ModelId = m.Id) AS DeckImgPath1,
    (SELECT TOP 1 DeckImgPath2 FROM YachtsLayoutImage WHERE ModelId = m.Id) AS DeckImgPath2,
    (SELECT TOP 1 InteriorImgPath FROM YachtsLayoutImage WHERE ModelId = m.Id) AS InteriorImgPath,
    (SELECT STRING_AGG(p.Name + ': ' + p.Value, ', ') FROM Principal p WHERE p.ModelId = m.Id) AS PrincipalSummary
FROM Model m
WHERE 
    m.Name + ' ' + CONVERT(nvarchar, m.Number) LIKE '%' + @input + '%'
    OR m.Label LIKE '%' + @input + '%'
    OR EXISTS (SELECT 1 FROM YachtsDownloads yd WHERE yd.ModelId = m.Id AND yd.FilePath LIKE '%' + @input + '%')
    OR EXISTS (SELECT 1 FROM YachtsContent yc WHERE yc.ModelId = m.Id AND (yc.Content LIKE '%' + @input + '%' OR yc.Specification LIKE '%' + @input + '%'))
    OR EXISTS (SELECT 1 FROM Principal p WHERE p.ModelId = m.Id AND (p.Name LIKE '%' + @input + '%' OR p.Value LIKE '%' + @input + '%'))

";

            var param = new Dictionary<string, object> { { "@input", input } };
            DataTable dt = db.SearchDB(sql, param);
            if (dt.Rows.Count > 0)
            {
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            else
            {
                Response.Write("<script>alert('查無資料');</script>");
            }
        }
        private int GetCurrentPage()  //抓取目前分頁
        {
            int page;
            return int.TryParse(Request.QueryString["page"], out page) && page > 0 ? page : 1;
        }
        private void BindRepeater()  //顯示主要內容的Repeater
        {
            int offset = (currentPage - 1) * pageSize;

            string sql = @"
                           SELECT yc.[content], yc.Id, yc.CreatedAt, yc.UpdatedAt, yc.specification, yc.ModelId,
                                  y.DeckImgPath1, y.DeckImgPath2, y.InteriorImgPath,
                                  m.Name + ' ' + CONVERT(nvarchar, m.Number) AS ModelName, m.Label
                           FROM YachtsContent yc
                           LEFT JOIN Model m ON yc.ModelId = m.Id
                           LEFT JOIN YachtsLayoutImage y ON y.ModelId = m.Id
                           ORDER BY m.Label DESC, yc.CreatedAt DESC, yc.UpdatedAt DESC
                           OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                          ";
            var param = new Dictionary<string, object>
            {
                { "@Offset", offset },
                { "@PageSize", pageSize },
                { "@ModelId", Request.QueryString["modelId"] ?? (object)DBNull.Value }
            };
            DataTable dt = db.SearchDB(sql, param);

            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }
        private void ShowPagination()  //顯示分頁
        {
            string countSql = "SELECT COUNT(*) FROM YachtsContent";
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
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)  //顯示 "輪播圖、檔案下載與欄位" 的Repeater
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
                    string principalSql = @"SELECT Name, Value FROM Principal WHERE ModelId = @ModelId ORDER BY Name";
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
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int modelId = Convert.ToInt32(e.CommandArgument);  

            // 刪除 Downloads
            db.ExecuteNonQuery("DELETE FROM YachtsDownloads WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

            // 刪除 Layout Image
            db.ExecuteNonQuery("DELETE FROM YachtsLayoutImage WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

            // 刪除 YachtsContent
            db.ExecuteNonQuery("DELETE FROM YachtsContent WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

            // 刪除 Carousel Images
            db.ExecuteNonQuery("DELETE FROM YachtsCarouselImage WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

            // 刪除 Principal
            db.ExecuteNonQuery("DELETE FROM Principal WHERE ModelId = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

            // 最後刪除 Model
            db.ExecuteNonQuery("DELETE FROM Model WHERE Id = @ModelId", new Dictionary<string, object> { { "@ModelId", modelId } });

            Response.Write("<script>alert('刪除成功'); window.location='Yachts-B.aspx';</script>");
        }
        protected void btnAddYachts_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddYachts.aspx");
        }
    }
}