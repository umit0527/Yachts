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
    public partial class Yachts : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRepeater();
            }
        }
        private void BindRepeater()  //顯示主要內容的Repeater
        {
            string sql = @"select yc.[content],  yc.Id, yc.CreatedAt, yc.UpdatedAt, yc.specification, yc.ModelId,
                                  y.DeckImgPath1, y.DeckImgPath2 ,y.InteriorImgPath,
                                  m.Name+' '+(convert(nvarchar,Number)) as ModelName , m.Label,
                                  p.Name, p.Value
                           from YachtsContent yc
                           left join Model m on yc.ModelId =m.Id
                           left join YachtsLayoutImage y on y.ModelId=m.Id
                           left join Principal p on p.ModelId=m.Id
                           order by m.Label desc, yc.CreatedAt desc ,yc.UpdatedAt desc
                          ";
            DataTable dt = db.SearchDB(sql);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
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
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)  //刪除的核心
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
        protected void btnAddYachts_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddYachts.aspx");
        }

        protected void SearchBox_TextChanged(object sender, EventArgs e)
        {

        }
        protected void SearchButton_Click(object sender, EventArgs e)
        {

        }
    }
}