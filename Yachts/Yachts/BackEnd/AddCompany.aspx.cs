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
    public partial class AddCompany : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Session["userid"] == null)
                //{
                //    string logout = "<script>alert('請先登入'); window.location='LoginB.aspx';</script>";
                //    Response.Write(logout);
                //}
                //else  //有登入
                //{
                //}
                BindCategoryList();
            }
        }


        //protected void Button1_Click(object sender, EventArgs e)  //登出
        //{
        //    if (Session["userid"] != null)  //有登入的話，則有登出按鈕
        //    {
        //        Session.Clear();       // 清除所有 Session 資料
        //        Session.Abandon();     // 結束目前的 Session
        //        string logout = "<script>alert('登出成功'); window.location='LoginB.aspx';</script>";
        //        Response.Write(logout);
        //        Response.End();
        //    }
        //}

        private void BindCategoryList()  // "種類"下拉式選單
        {

            string sql = @"select Id, Name 
                           from CompanyCategory 
                           order by Name asc";

            DataTable dt = db.SearchDB(sql);

            if (dt.Rows.Count > 0)
            {
                CategoryList.DataSource = dt;
                CategoryList.DataTextField = "Name";   // 顯示名稱
                CategoryList.DataValueField = "Id";  //抓取對應 ID 以便寫進資料庫
                CategoryList.DataBind();

                // 加入提示選項
                CategoryList.Items.Insert(0, new ListItem("請選擇種類", ""));
            }
        }
        protected void CategoryList_SelectedIndexChanged(object sender, EventArgs e)  //檢查選擇的種類是否有資料
        {
            string selectedCategoryId = CategoryList.SelectedValue;

            if (!string.IsNullOrEmpty(selectedCategoryId))
            {
                string sql = "SELECT COUNT(*) FROM Company WHERE CategoryId = @CategoryId";
                var parameters = new Dictionary<string, object>()
        {
            { "@CategoryId", selectedCategoryId }
        };

                int count = Convert.ToInt32(db.SearchDBValue(sql, parameters));

                if (count > 0)
                {
                    // 顯示提示訊息
                    Response.Write("<script>alert('已有資料，無法新增');</script>");
                    CategoryList.SelectedIndex=0;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)  //送出按鈕
        {
            string content = CKEditor1.Text; // 取得編輯器內容
            string categoryList = CategoryList.SelectedValue;
            string title = txtTitle.Text.Trim();

            if (!string.IsNullOrWhiteSpace(categoryList) && !string.IsNullOrWhiteSpace(content))
            {
        //        // 檢查是否已有同樣種類的公司資料
        //        string checkSql = "SELECT COUNT(*) FROM Company WHERE CategoryId = @CategoryId";
        //        var checkParams = new Dictionary<string, object>()
        //{
        //    { "@CategoryId", categoryList }
        //};

        //        int count = Convert.ToInt32(db.SearchDBValue(checkSql, checkParams)); 

        //        if (count > 0)
        //        {
        //            Response.Write("<script>alert('已有資料，無法新增');</script>");
        //            return;
        //        }

                //先不加入admin
                string sql = @"insert into Company (Content , CreatedAt , CategoryId, Title ) 
                           values (@Content ,@CreatedAt ,@CategoryId , @Title  )";

                var Params = new Dictionary<string, object>()
            {
                { "@Content",content},
                { "@CreatedAt",DateTime.Now},
                { "@CategoryId",categoryList},
                { "@Title",title}
            };

                int result = db.ExecuteNonQuery(sql, Params);
                if (result > 0)
                {
                    // 提示用戶
                    Response.Write("<script>alert('新增成功！'); window.location='Company-B.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('新增失敗，請稍後再試！'); window.location='Company-B.aspx';</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('請選擇種類'); </script>");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)  //取消按鈕
        {
            Response.Redirect("Company-B.aspx");
        }
    }
}