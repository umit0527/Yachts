using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Yachts.BackEnd
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {

            Session.Clear();       // 清除所有 Session 資料
            Session.Abandon();     // 結束目前的 Session
            string logout = "<script>alert('登出成功'); window.location='Login-B.aspx';</script>";
            Response.Write(logout);
            Response.End();
        }
    }
}