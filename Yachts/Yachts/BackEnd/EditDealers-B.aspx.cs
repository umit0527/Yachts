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
    public partial class EditDealers_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDealerData();
            }
        }
        private void LoadDealerData()
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }
            else
            {
                int dealersId = int.Parse(Request.QueryString["Id"]);
                DBHelper db = new DBHelper();

                string sql = @"select CountryId, CityId, Content 
                               from Dealers 
                               where Id = @Id";
                var param = new Dictionary<string, object> { { "@Id", dealersId } };
                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    int countryId = Convert.ToInt32(dt.Rows[0]["CountryId"]);
                    int cityId = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    string content = dt.Rows[0]["Content"].ToString();

                    BindCountryList(); // 所有國家
                    CountryList.SelectedValue = countryId.ToString();

                    //BindCityList(countryId); // 對應國家的城市
                    CityList.SelectedValue = cityId.ToString();

                    CKEditor1.Text = content;
                }
            }
        }
        private void BindCountryList()  // "國家"下拉式選單
        {
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
            int dealersId = int.Parse(Request.QueryString["Id"]);
            string content = CKEditor1.Text; // 取得編輯器內容
            string countryList = CountryList.SelectedValue;
            string cityList = CityList.SelectedValue;

            if (Request.QueryString["Id"] != null)
            {
                if (!string.IsNullOrWhiteSpace(countryList) &&
                    !string.IsNullOrWhiteSpace(cityList) && !string.IsNullOrWhiteSpace(content))
                {

                    DBHelper db = new DBHelper();

                    //先不加入admin
                    string sql = @"update Dealers set Content=@Content  , 
                                                      CountryId=@CountryId ,
                                                      CityId=@CityId,
                                                      UpdatedAt=@UpdatedAt
                                   where Id=@Id
                                  ";

                    var Params = new Dictionary<string, object>()
            {
                { "@Content",content},
                { "@UpdatedAt",DateTime.Now},
                { "@CountryId",countryList} ,
                { "@CityId",cityList},
                { "@Id",  dealersId}
            };

                    int result = db.ExecuteNonQuery(sql, Params);
                    if (result > 0)
                    {
                        string success = "<script>alert('更新成功'); window.location='Dealers-B.aspx';</script>";
                        Response.Write(success);
                    }
                }
                else
                {
                    Response.Write("<script>alert('必填欄位填寫不完整'); </script>");
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dealers-B.aspx");
        }
    }
}