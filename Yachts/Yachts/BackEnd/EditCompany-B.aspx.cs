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
    public partial class EditCompany_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompanyData();
            }
        }
        private void LoadCompanyData()
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }
            else
            {
                int companyId = int.Parse(Request.QueryString["Id"]);
                DBHelper db = new DBHelper();

                string sql = @"select CategoryId,  Content 
                               from Company 
                               where Id = @Id";
                var param = new Dictionary<string, object> { { "@Id", companyId } };
                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    int categoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                    string content = dt.Rows[0]["Content"].ToString();

                    BindCategoryList(); // 所有種類
                    CategoryList.SelectedValue = categoryId.ToString();

                    CKEditor1.Text = content;
                }
            }
        }
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
        protected void btnSubmit_Click(object sender, EventArgs e)  //送出按鈕
        {
            int companyId = int.Parse(Request.QueryString["Id"]);
            string content = CKEditor1.Text; // 取得編輯器內容
            string categoryList = CategoryList.SelectedValue;

            if (Request.QueryString["Id"] != null)
            {
                if (!string.IsNullOrWhiteSpace(categoryList) &&!string.IsNullOrWhiteSpace(content))
                {

                    DBHelper db = new DBHelper();

                    //先不加入admin
                    string sql = @"update Company set Content=@Content  , 
                                                      CategoryId=@CategoryId ,
                                                      UpdatedAt=@UpdatedAt
                                   where Id=@Id
                                  ";

                    var Params = new Dictionary<string, object>()
            {
                { "@Content",content},
                { "@UpdatedAt",DateTime.Now},
                { "@CategoryId",categoryList} ,
                { "@Id",  companyId}
            };

                    int result = db.ExecuteNonQuery(sql, Params);
                    if (result > 0)
                    {
                        string success = "<script>alert('更新成功'); window.location='Company-B.aspx';</script>";
                        Response.Write(success);
                    }
                }
                else
                {
                    Response.Write("<script>alert('必填欄位填寫不完整'); </script>");
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)  //取消按鈕
        {
            Response.Redirect("Company-B.aspx");
        }
    }
}