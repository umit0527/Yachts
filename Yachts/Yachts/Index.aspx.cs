using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.FrontEnd
{
    public partial class Home1 : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            BindCarouselImg();
            BindNews();
        }
        private void BindCarouselImg()  //顯示 "輪播圖" 
        {
            string modelId = Request.QueryString["ModelId"];
            string sql = "";

            Dictionary<string, object> param = null;

            if (string.IsNullOrEmpty(modelId))
            {
                sql = @"WITH ValidImages AS (
                        SELECT yc.ModelId, yc.Id, yc.ImgPath
                        FROM YachtsCarouselImage yc
                        INNER JOIN Model m ON yc.ModelId = m.Id
                        WHERE yc.ImgPath IS NOT NULL AND yc.ImgPath <> ''
                    ),
                    LatestModelIds AS (
                        SELECT TOP 6 ModelId, MAX(Id) AS MaxId
                        FROM ValidImages
                        GROUP BY ModelId
                        ORDER BY MaxId DESC
                    ),
                    FirstImages AS (
                        SELECT *,
                            ROW_NUMBER() OVER (PARTITION BY ModelId ORDER BY Id ASC) AS rn
                        FROM ValidImages
                        WHERE ModelId IN (SELECT ModelId FROM LatestModelIds)
                    )
                    SELECT 
                        fi.ModelId,
                        fi.ImgPath AS CarouselImageImgPath,
                        m.Name AS ModelName,
                        m.Number AS ModelNumber,
                        m.Label AS ModelLabel
                    FROM FirstImages fi
                    JOIN Model m ON fi.ModelId = m.Id
                    WHERE fi.rn = 1
                    ORDER BY m.Label DESC, fi.ModelId DESC;
                       ";
                param = null;
            }
            DataTable dt = db.SearchDB(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                rptCarouselImg.DataSource = dt;
                rptCarouselImg.DataBind();

                rptBigBanner.DataSource = dt;
                rptBigBanner.DataBind();
            }
        }
        protected void rptBigBanner_ItemDataBound(object sender, RepeaterItemEventArgs e)  //顯示 輪播圖的標籤
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // 取得 ModelLabel 字段的值
                string modelLabel = DataBinder.Eval(e.Item.DataItem, "ModelLabel")?.ToString();

                // 取得標籤圖片的 `img` 控制項
                var imgLabel = (Image)e.Item.FindControl("imgLabel");

                // 設置顯示標籤
                if (!string.IsNullOrEmpty(modelLabel))
                {
                    switch (modelLabel.ToLower())
                    {
                        case "new design":
                            imgLabel.ImageUrl = "/images/new02.png";
                            imgLabel.AlternateText = "New Design";
                            imgLabel.Visible = true;
                            break;

                        case "new building":
                            imgLabel.ImageUrl = "/images/new01.png";
                            imgLabel.AlternateText = "New Building";
                            imgLabel.Visible = true;
                            break;

                        default:
                            imgLabel.Visible = false;
                            break;
                    }
                }
                else
                {
                    imgLabel.Visible = false;
                }
            }
        }
        private void BindNews()
        {
            string newsId = Request.QueryString["NewsId"];
            string sql = "";

            Dictionary<string, object> param = null;

            if (string.IsNullOrEmpty(newsId))
            {
                sql = @"SELECT TOP 3 Title, Content, Sticky, CoverPath, CreatedAt, Id as NewsId
                        FROM News
                        ORDER BY Id DESC";
            }

            DataTable dt = db.SearchDB(sql, param);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                rptNews.DataSource = dt;
                rptNews.DataBind();
            }
            
        }
    }
}