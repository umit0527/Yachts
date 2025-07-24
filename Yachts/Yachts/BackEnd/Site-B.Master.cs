using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class Site : System.Web.UI.MasterPage
    {
        DBHelper db=new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["AdminiId"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }
                BindUserName();
                BindUserImage();
            }
        }
        private void BindUserName() //顯示 使用者名字
        {
            string adminiId = Session["AdminiId"].ToString();
            if (string.IsNullOrEmpty(adminiId))
            {
                Response.Redirect("Login.aspx");
                return;
            }
            else
            {
                string sql = @"select Name as Username
                               from Administrator a
                               where Id=@Id
                              ";
                var param = new Dictionary<string, object> { { "@Id", adminiId } };

                DataTable dt = db.SearchDB(sql,param);
                rptUserName.DataSource = dt;
                rptUserName.DataBind();
            }
        }
        private void BindUserImage() //顯示 使用者大頭照
        {
            string adminiId = Session["AdminiId"].ToString();
            if (string.IsNullOrEmpty(adminiId))
            {
                Response.Redirect("Login.aspx");
                return;
            }
            else
            {
                string sql = @"select ImgPath as AdminiImg
                               from Administrator a
                               where Id=@Id
                              ";
                var param = new Dictionary<string, object> { { "@Id", adminiId } };

                DataTable dt = db.SearchDB(sql, param);
                if (dt.Rows.Count > 0)
                {
                    //string adminiImg = dt.Rows[0]["AdminiImg"].ToString();
                    //AdminiImg1.ImageUrl = "~/images/" + adminiImg;  //封面

                    Repeater1.DataSource = dt;
                    Repeater1.DataBind();
                    Repeater2.DataSource = dt;
                    Repeater2.DataBind();
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)  //搜尋 的送出按鈕
        {
            //傳入 % 關鍵字 % 來模糊比對
            string input = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(input))
            {
                Response.Redirect("Search.aspx?search=" + Server.UrlEncode(input));
            }
        }
        protected void btnSignout_Click(object sender, EventArgs e)  //登出
        {

            Session.Clear();       // 清除所有 Session 資料
            Session.Abandon();     // 結束目前的 Session
            string logout = "<script>alert('登出成功'); window.location='Login.aspx';</script>";
            Response.Write(logout);
            Response.End();
        }
    }
}