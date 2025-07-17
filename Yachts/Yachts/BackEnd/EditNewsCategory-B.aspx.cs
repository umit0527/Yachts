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
    public partial class EditNewsCategory_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategory();
            }
        }

        private void LoadCategory()
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }
            else
            {
                int categoryId = int.Parse(Request.QueryString["Id"]);
                DBHelper db = new DBHelper();

                string sql = @"select Id, Name 
                               from NewsCategory
                               where Id = @Id";
                var param = new Dictionary<string, object> { { "@Id", categoryId } };
                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    string categoryName = dt.Rows[0]["Name"].ToString();
                    CategoryName.Text = categoryName;
                }
            }
        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            int categoryId = int.Parse(Request.QueryString["Id"]);
            string categoryName = CategoryName.Text.Trim();
            if (Request.QueryString["Id"] != null)
            {
                if (!string.IsNullOrWhiteSpace(categoryName))
                {
                    //檢查編輯後是否有重複，「!=」排除掉自己，檢查自己以外的名稱
                    string checkSql = @"SELECT COUNT(*) FROM NewsCategory 
                                        WHERE Name = @Name AND Id != @Id";

                    var checkParams = new Dictionary<string, object>
            {
                { "@Name", categoryName },
                { "@Id", categoryId }
            };

                    int count = Convert.ToInt32(db.ExecuteScalar(checkSql, checkParams));

                    if (count > 0)
                    {
                        Response.Write("<script>alert('已存在相同名稱的種類');</script>");
                        return;
                    }

                    //先不加入admin
                    string sql = @"update NewsCategory set Name=@Name  , 
                                                           UpdatedAt=@UpdatedAt
                                   where Id=@Id
                                  ";

                    var Params = new Dictionary<string, object>()
                    {
                { "@Name",categoryName},
                { "@UpdatedAt",DateTime.Now},
                { "@Id",categoryId}
            };

                    int result = db.ExecuteNonQuery(sql, Params);
                    if (result > 0)
                    {
                        string success = "<script>alert('更新成功！'); window.location='NewsCategory-B.aspx';</script>";
                        Response.Write(success);
                    }
                    else
                    {
                        Response.Write("<script>alert('新增失敗，請稍後再試！'); window.location='Dealers-B.aspx';</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('請輸入種類名稱'); </script>");
                }
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewsCategory-B.aspx");
        }
    }
}