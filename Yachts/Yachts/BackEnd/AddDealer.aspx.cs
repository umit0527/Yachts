using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class AddDealer : System.Web.UI.Page
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
                BindCountryList();
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

        private void BindCountryList()  // "國家"下拉式選單
        {
            DBHelper db = new DBHelper();
            string sql = @"select Id, Name 
                           from Country 
                           order by Name asc";

            DataTable dt = db.SearchDB(sql);

            if (dt.Rows.Count > 0)
            {
                CountryList.DataSource = dt;
                CountryList.DataTextField = "Name";   // 顯示名稱
                CountryList.DataValueField = "Id";  //抓取對應 ID 以便寫進資料庫
                CountryList.DataBind();

                // 加入提示選項
                CountryList.Items.Insert(0, new ListItem("請選擇國家", ""));
            }
        }
        //private void BindCityList(int countryId)  // "城市"下拉式選單
        //{
        //    DBHelper db = new DBHelper();
        //    string sql = @"select Id, Name 
        //                   from City
        //                   where CountryId = @CountryId
        //                   order by Name asc";

        //    var Param = new Dictionary<string, object>()
        //    {
        //        {"@CountryId", countryId }
        //    };
        //    DataTable dt = db.SearchDB(sql, Param);

        //    if (dt.Rows.Count > 0)
        //    {
        //        CityList.DataSource = dt;
        //        CityList.DataTextField = "Name";   // 顯示名稱
        //        CityList.DataValueField = "Id";  //抓取對應 ID 以便寫進資料庫
        //        CityList.DataBind();
        //    }
        //    else
        //    {
        //        CityList.Items.Clear();
        //    }
        //    // 加入提示選項
        //    CityList.Items.Insert(0, new ListItem("請選擇城市", ""));
        //}
        //protected void CountryList_SelectedIndexChanged(object sender, EventArgs e)  //根據國家選擇城市
        //{
        //    if (int.TryParse(CountryList.SelectedValue, out int countryId))
        //    {
        //        BindCityList(countryId);
        //    }
        //    else
        //    {
        //        CityList.Items.Clear();
        //        CityList.Items.Insert(0, new ListItem("請選擇城市", ""));
        //    }
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)  //送出按鈕
        {
            string content = CKEditor1.Text; // 取得編輯器內容
            string countryList = CountryList.SelectedValue;
            //string cityList = CityList.SelectedValue;

            if (!string.IsNullOrWhiteSpace(countryList) && !string.IsNullOrWhiteSpace(content))
            {
                //先不加入admin
                string sql = @"insert into Dealers (Content , CreatedAt , CountryId) 
                           values (@Content ,@CreatedAt ,@CountryId)";

                var Params = new Dictionary<string, object>()
            {
                { "@Content",content},
                { "@CreatedAt",DateTime.Now},
                { "@CountryId",countryList}
            };

                int result = db.ExecuteNonQuery(sql, Params);
                if (result > 0)
                {
                    // 提示用戶
                    Response.Write("<script>alert('新增成功！'); window.location='Dealers-B.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('新增失敗，請稍後再試！'); window.location='Dealers-B.aspx';</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('必填欄位填寫不完整'); </script>");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dealers-B.aspx");
        }
    }
}