using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts
{
    public partial class YachtsTest1 : System.Web.UI.Page
    {
        public string Name = "", Value = "";
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindYachtsDetails();
                BindModel();
                BindDownloads();
                BindCarouselImg();
                this.DataBind();
                BindPrincipal();

                string modelId = Request.QueryString["ModelId"];

                // 如果沒有指定船型與子頁，預設導向有資料的第一筆
                //if (string.IsNullOrEmpty(modelId))
                //{
                //    // 從資料庫查目前存在的第一筆 Model 資料
                //    string sql = @"SELECT TOP 1 Id FROM Model ORDER BY Id";
                //    DataTable dt = db.SearchDB(sql);

                //    if (dt.Rows.Count > 0)
                //    {
                //        string defaultId = dt.Rows[0]["Id"].ToString();
                //        Response.Redirect("Yachts.aspx?ModelId=" + defaultId);
                //        return;
                //    }
                //}
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
        private void BindYachtsDetails()  //顯示 主要內容
        {
            string modelId = Request.QueryString["ModelId"];

            if (!string.IsNullOrEmpty(modelId))
            {
                string sql = @"select y.content,
                                      yl.InteriorImgPath,
                                      (m.Name+' '+convert(nvarchar,m.Number)) as ModelName  
                               from YachtsContent y
                               join Model m on y.ModelId =m.Id
                               join YachtsLayoutImage yl on y.ModelId =yl.ModelId 
                               where y.ModelId = @ModelId
                               order by m.Id desc, y.CreatedAt desc
                              ";

                var param = new Dictionary<string, object> { { "@ModelId", modelId } };

                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    string modelName = dt.Rows[0]["ModelName"].ToString();
                    string content = dt.Rows[0]["content"].ToString();

                    Model1.Text = modelName;
                    Model2.Text = modelName;

                    txtContent.Text = content;

                    InteriorImgPath.ImageUrl = ResolveUrl("~/Uploads/Photos/" + dt.Rows[0]["InteriorImgPath"].ToString());
                }
            }
        }
        private void BindPrincipal()  //顯示 Principal
        {
            string modelId = Request.QueryString["ModelId"];

            if (!string.IsNullOrEmpty(modelId))
            {
                string sql = @"select
                                     p.Name as PrincipalName, p.Value as PrincipalValue
                               from Principal p
                               join Model m on p.ModelId =m.Id
                               where p.ModelId = @ModelId
                               order by m.Id desc
                              ";
                var param = new Dictionary<string, object> { { "@ModelId", modelId } };

                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    rptPrincipal.DataSource = dt;
                    rptPrincipal.DataBind();
                }
            }
        }
        private void BindModel()
        {
            string sql = @"SELECT Id, (Name + ' ' + CONVERT(nvarchar, Number)) AS ModelName, Label 
                           FROM Model
                           order by label desc";
            DataTable dt = db.SearchDB(sql);
            rptModelList.DataSource = dt;
            rptModelList.DataBind();
        }

        protected void rptModelList_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
        private void BindDownloads()  //顯示 "檔案下載"
        {
            string modelId = Request.QueryString["ModelId"];
            if (!string.IsNullOrEmpty(modelId))
            {
                string sql = @"select *
                           from YachtsDownloads yd
                           join Model m on m.Id=yd.ModelId
                           where yd.ModelId=@ModelId
                          ";
                var param = new Dictionary<string, object> { { "@ModelId", modelId } };
                DataTable dt = db.SearchDB(sql, param);
                if (dt.Rows.Count > 0)
                {
                    rptDownloads.DataSource = dt;
                    rptDownloads.DataBind();
                }
            }
        }
        protected void lnkDownload_Click(object sender, EventArgs e)  //點擊 "檔案下載"
        {
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;

            // 從資料庫根據 fileId 取得檔案資訊
            string sql = "SELECT * FROM YachtsDownloads WHERE Id = @Id";

            var param = new Dictionary<string, object> { { "@Id", id } };

            DataTable dt = db.SearchDB(sql, param);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string filePath = Server.MapPath(row["FilePath"].ToString());
                string fileName = Path.GetFileName(filePath); // 從完整路徑中取得檔名（含副檔名）

                // 如果檔案存在
                if (File.Exists(filePath))
                {
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";  // 告訴瀏覽器檔案的類型
                    //設定檔案下載的回應，讓瀏覽器用指定的檔名下載。
                    Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + "\"");
                    Response.TransmitFile(filePath);  //把伺服器上的實體檔案「直接傳送」給使用者下載或瀏覽。
                    Response.End();
                }
                else
                {
                    Response.Write("<script>alert('下載失敗！')</script>");
                }
            }
        }
    }
}