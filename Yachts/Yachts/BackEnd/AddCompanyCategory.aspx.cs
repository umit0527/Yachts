using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class AddCompanyCategory : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            string categoryName = CategoryName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                string checkSql = @"select count(*) 
                                    from CompanyCategory 
                                    where Name = @Name";
                var checkParam = new Dictionary<string, object>
        {
            { "@Name", categoryName }
        };

                object checkResult = db.ExecuteScalar(checkSql, checkParam);
                int count = Convert.ToInt32(checkResult);

                if (count > 0)
                {
                    // 已存在相同的國家名稱
                    Response.Write("<script>alert('種類名稱已存在，請重新輸入！');</script>");
                    return;
                }

                //先不撈admin
                string sql = @"insert into CompanyCategory ( Name, CreatedAt) 
                               values (@Name, @CreatedAt)";

                var param = new Dictionary<string, object> {
                        { "@Name", categoryName },
                        { "@CreatedAt",DateTime.Now}
                    };

                int result = db.ExecuteNonQuery(sql, param);
                if (result > 0)
                {
                    Response.Write("<script>alert('新增成功！'); window.location=('CompanyCategory-B.aspx');</script>");
                }
                else
                {
                    Response.Write("<script>alert('新增失敗，請稍後再試！'); window.location=('CompanyCategory-B.aspx');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('請輸入種類名稱');</script>");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CompanyCategory-B.aspx");
        }
    }
}