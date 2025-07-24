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
                Session["Id"] = Request.QueryString["Id"];

                if (Request.QueryString["Id"] == null)
                {
                    Response.Redirect("Dealers-B.aspx");
                    return;
                }
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

                string sql = @"select CountryId, Content 
                               from Dealers 
                               where Id = @Id";
                var param = new Dictionary<string, object> { { "@Id", dealersId } };
                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    int countryId = Convert.ToInt32(dt.Rows[0]["CountryId"]);
                    string content = dt.Rows[0]["Content"].ToString();

                    BindCountryList(); // 所有國家
                    CountryList.SelectedValue = countryId.ToString();

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
        protected void btnSubmit_Click(object sender, EventArgs e)  //送出按鈕
        {
            int dealersId = int.Parse(Request.QueryString["Id"]);
            string content = CKEditor1.Text; // 取得編輯器內容
            string countryList = CountryList.SelectedValue;

            if (Request.QueryString["Id"] != null)
            {
                if (!string.IsNullOrWhiteSpace(countryList) && !string.IsNullOrWhiteSpace(content))
                {

                    DBHelper db = new DBHelper();

                    //先不加入admin
                    string sql = @"update Dealers set Content=@Content  , 
                                                      CountryId=@CountryId ,
                                                      UpdatedAt=@UpdatedAt
                                   where Id=@Id
                                  ";

                    var Params = new Dictionary<string, object>()
            {
                { "@Content",content},
                { "@UpdatedAt",DateTime.Now},
                { "@CountryId",countryList} ,
                { "@Id",  dealersId}
            };

                    int result = db.ExecuteNonQuery(sql, Params);
                    if (result > 0)
                    {
                        string success = "<script>alert('更新成功！'); window.location='Dealers-B.aspx';</script>";
                        Response.Write(success);
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
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dealers-B.aspx");
        }
    }
}