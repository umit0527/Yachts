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
    public partial class News_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRepeater();
            }
        }
        private void BindRepeater()  //顯示 列表內容 的Repeater
        {

            string sql = @"select n.CreatedAt, n.Id, n.UpdatedAt, n.Title, n.content, n.Sticky,
                           n.CoverPath ,
                           nc.Name as CategoryName
                           from News n 
                           left join NewsCategory nc on n.CategoryId =nc.Id
                           order by n.Sticky desc ,CategoryName desc, n.CreatedAt desc
                          ";
            DataTable dt = db.SearchDB(sql);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)  //刪除的核心
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
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)  //顯示 檔案 的repeater
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
        protected void btnAddNews_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddNews.aspx");
        }
    }
}