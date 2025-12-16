using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts
{
    public partial class YachtsLayout : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindYachtsMenu();
                BindYachtsDetails();
                BindModel();
                this.DataBind();
                BindCarouselImg();

                string modelId = Request.QueryString["modelId"];

                // 如果沒有指定船型與子頁，預設導向有資料的第一筆
                if (string.IsNullOrEmpty(modelId))
                {
                    // 從資料庫查目前存在的最新一筆 Model 資料
                    string sql = @"SELECT TOP 1 Id FROM Model ORDER BY Label desc , Id desc";
                    DataTable dt = db.SearchDB(sql);

                    if (dt.Rows.Count > 0)
                    {
                        string defaultId = dt.Rows[0]["Id"].ToString();
                        Response.Redirect("Yachts.aspx?ModelId=" + defaultId);
                        return;
                    }
                }
            }
        }
        private void BindYachtsDetails()  //顯示 主要內容
        {
            string modelId = Request.QueryString["ModelId"];

            if (!string.IsNullOrEmpty(modelId))
            {
                string sql = @"select y.DeckImgPath1, y.DeckImgPath2,
                                      (m.Name+' '+convert(nvarchar,m.Number)) as ModelName
                               from YachtsLayoutImage y
                               join Model m on y.ModelId =m.Id
                               where y.ModelId = @ModelId
                               order by m.Id desc, y.CreatedAt desc
                              ";

                var param = new Dictionary<string, object> { { "@ModelId", modelId } };

                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    string modelName = dt.Rows[0]["ModelName"].ToString();

                    txtModel1.Text = modelName;
                    txtModel2.Text = modelName;

                    DeckImgPath1.ImageUrl = ResolveUrl("~/Uploads/Photos/" + dt.Rows[0]["DeckImgPath1"].ToString());
                    DeckImgPath2.ImageUrl = ResolveUrl("~/Uploads/Photos/" + dt.Rows[0]["DeckImgPath2"].ToString());
                }
            }
        }
        private void BindCarouselImg()  //顯示 輪播圖
        {
            string modelId = Request.QueryString["ModelId"];
            if (!string.IsNullOrEmpty(modelId))
            {
                string sql = @"SELECT ModelId, ImgPath as CarouselImgPath 
                               FROM YachtsCarouselImage
                               where ModelId=@ModelId
                               order by Id";
                var param = new Dictionary<string, object>() { { "@ModelId", modelId } };
                DataTable dt = db.SearchDB(sql, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    rptCarouselImg.DataSource = dt;
                    rptCarouselImg.DataBind();
                }
            }
        }
        private void BindModel()  //顯示側邊欄「船型」
        {
            string sql = @"select Id, (Name+' '+convert(nvarchar,Number)) as ModelName ,Label
                           from Model
                           order by label desc , CreatedAt desc
                          ";

            DataTable dt = db.SearchDB(sql);
            rptModelList.DataSource = dt;
            rptModelList.DataBind();
        }
        protected void rptModelList_ItemDataBound(object sender, RepeaterItemEventArgs e)  //綁定並顯示 標籤
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var lblTag = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblTag");
                string label = DataBinder.Eval(e.Item.DataItem, "Label")?.ToString();

                if (!string.IsNullOrEmpty(label))
                {
                    switch (label.ToLower())
                    {
                        case "new design":
                            lblTag.Text = " <span style='color: gray;'>New Design</span>";
                            break;

                        case "new building":
                            lblTag.Text = " <span style='color: gray;'>New Building</span>";
                            break;

                        default:
                            lblTag.Text = "";
                            break;
                    }
                }
                else
                {
                    lblTag.Text = "";
                }
            }
        }
    }
}