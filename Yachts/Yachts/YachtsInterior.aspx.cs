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
    public partial class YachtsInterior : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindYachts();
                BindModel();
                BindCarouselImg();

                string model = Request.QueryString["model"];
                //string tab = Request.QueryString["tab"];

                //// 如果沒有指定船型與子頁，預設導向 Dynasty 72 的 Interior
                //if (string.IsNullOrEmpty(model) )
                //{
                //    Response.Redirect("Yachts.aspx?Id=1");
                //}

            }
        }
        private void BindYachts()  //顯示船的Repeater
        {
            string Id = Request.QueryString["Id"];

            if (!string.IsNullOrEmpty(Id))
            {
                string sql = @"select y.CreatedAt , y.Id, y.UpdatedAt, y.ModelId, y.LOA, y.LWL, y.Beam, y.Draft, 
                                      y.Displacement, y.Ballast, y.Specification,
                                      (m.Name+' '+convert(nvarchar,m.Number)) as ModelName
                               from YachtsContent y
                               join Model m on y.ModelId =m.Id
                               where y.ModelId = @ModelId
                               order by m.Id desc, y.CreatedAt desc
                              ";

                var param = new Dictionary<string, object> { { "@Id", Id } };

                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    string modelName = dt.Rows[0]["ModelName"].ToString();
                    string content = dt.Rows[0]["content"].ToString();

                    txtContent.Text = content;
                    txtModel.Text = modelName;
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

                    rptCarouselMainImg.DataSource = dt;
                    rptCarouselMainImg.DataBind();
                }
            }
        }

        private void BindModel()  //顯示側邊欄「船型」
        {
            string sql = @"select Id, (Name+' '+convert(nvarchar,Number)) as ModelName 
                           from Model
                          ";

            DataTable dt = db.SearchDB(sql);
            rptMdoel.DataSource = dt;
            rptMdoel.DataBind();
        }
    }
}