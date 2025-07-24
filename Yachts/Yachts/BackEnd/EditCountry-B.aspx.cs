using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class EditCountry : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Id"] = Request.QueryString["Id"];

                if (Request.QueryString["Id"] == null)
                {
                    Response.Redirect("Country-B.aspx");
                    return;
                }
                LoadCountry();
            }
        }

        private void LoadCountry()
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }
            else
            {
                int countryId = int.Parse(Request.QueryString["Id"]);
                DBHelper db = new DBHelper();

                string sql = @"select Id, Name 
                               from Country
                               where Id = @Id";
                var param = new Dictionary<string, object> { { "@Id", countryId } };
                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    string countryName = dt.Rows[0]["Name"].ToString();
                    CountryName.Text = countryName; 
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int countryId = int.Parse(Request.QueryString["Id"]);
            string countryName = CountryName.Text.Trim();
            if (Request.QueryString["Id"] != null)
            {
                if (!string.IsNullOrWhiteSpace(countryName))
                {
                    //檢查編輯後是否有重複，「!=」排除掉自己，檢查自己以外的名稱
                    string checkSql = @"SELECT COUNT(*) FROM Country 
                                        WHERE Name = @Name AND Id != @Id";

                    var checkParams = new Dictionary<string, object>
            {
                { "@Name", countryName },
                { "@Id", countryId }
            };

                    int count = Convert.ToInt32(db.ExecuteScalar(checkSql, checkParams));

                    if (count > 0)
                    {
                        Response.Write("<script>alert('已存在相同名稱的國家');</script>");
                        return;
                    }

                    //先不加入admin
                    string sql = @"update Country set Name=@Name  , 
                                                      UpdatedAt=@UpdatedAt
                                   where Id=@Id
                                  ";

                    var Params = new Dictionary<string, object>()
                    {
                { "@Name",countryName},
                { "@UpdatedAt",DateTime.Now},
                { "@Id",countryId}
            };

                    int result = db.ExecuteNonQuery(sql, Params);
                    if (result > 0)
                    {
                        string success = "<script>alert('更新成功！'); window.location='Country-B.aspx';</script>";
                        Response.Write(success);
                    }
                    else
                    {
                        Response.Write("<script>alert('新增失敗，請稍後再試！'); window.location='Dealers-B.aspx';</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('請輸入國家名稱'); </script>");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Country-B.aspx");
        }
    }
}