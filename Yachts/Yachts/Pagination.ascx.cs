using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Yachts
{
    public partial class Pagination : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        bool IsNumber(string inputData)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(inputData, "^[0-9]+$");
        }
        public static string getPaginationString(int page, int totalItems, int limit, int adjacents, string targetPage)
        {
            //判斷預設網頁有無帶有傳值用，如果有出現 ? 表示已有傳參數就在後面加 & 加掛，如無則補加 ? //預設在檔名後加問號
            targetPage = targetPage.IndexOf('?') != -1 ? targetPage + "&" : targetPage + "?";
            //前一頁 = 目前頁面-1
            int prev = page - 1;
            //下一頁 = 目前頁面+1
            int nextPage = page + 1;
            //總頁數數值 = 總資料筆數/每頁幾筆
            Double value = Convert.ToDouble((decimal)totalItems / limit);
            //最末頁(總頁數) = 總頁數數值無條件進位成整數
            int lastpage = Convert.ToInt16(Math.Ceiling(value));
            //倒數第二頁 = 最末頁-1
            int secondLast = lastpage - 1;
            //邏輯判斷共用參數
            int commonParameter = 3 + (adjacents * 2); //不可修改:"3"代表當前頁+首或末兩頁，"2"代表左右側頁
                                                       //建立分頁 HTML 字串邏輯
            StringBuilder paginationBuilder = new StringBuilder();
            //超過1頁才顯示分頁控制項
            if (lastpage >= 1)
            {
                //共計幾筆資料 HTML
                paginationBuilder.Append("<div class=\"pagination\"> 共 <span style=\"color:red\" >" + totalItems + "</span> 筆資料  ");
                //上一頁HTML，目前頁面大於1則啟用連結，否則就禁用
                paginationBuilder.Append(page > 1 ? string.Format("<a href=\"{0}page={1}\"> <<< </a>", targetPage, prev) : "<span class=\"disabled\"> <<< </span>");
                //頁碼選項 HTML 邏輯判斷
                //總頁數 不多於 (邏輯判斷共用參數+(3=代表當前頁+首或末兩頁)，就不隱藏頁碼
                if (lastpage <= commonParameter + 3)
                {
                    for (int counter = 1; counter <= lastpage; counter++)
                    {
                        //counter等於當前頁則不加入連結，否則就加入連結
                        paginationBuilder.Append(counter == page ? string.Format("<span class=\"current\">{0}</span>", counter) : string.Format("<a href=\"{0}page={1}\">{1}</a>", targetPage, counter));
                    }
                }
                //執行隱藏頁碼
                else
                {
                    //只隱藏右側頁碼
                    if (page < commonParameter)
                    {
                        for (int counter = 1; counter <= commonParameter; counter++)
                        {
                            paginationBuilder.Append(counter == page ? string.Format("<span class=\"current\">{0}</span>", counter) : string.Format("<a href=\"{0}page={1}\">{1}</a>", targetPage, counter));
                        }
                        //之後的頁碼用...省略
                        paginationBuilder.Append("...");
                        //加入倒數第2頁
                        paginationBuilder.Append(string.Format("<a href=\"{0}page={1}\">{1}</a>", targetPage, secondLast));
                        //加入最末頁
                        paginationBuilder.Append(string.Format("<a href=\"{0}page={1}\">{1}</a>", targetPage, lastpage));
                    }
                    //中間頁碼，隱藏兩側頁碼
                    else if (page >= commonParameter && page <= lastpage - commonParameter)
                    {
                        //加入第一頁+第二頁及...省略頁碼
                        paginationBuilder.Append(string.Format("<a href=\"{0}page=1\">1</a>", targetPage));
                        paginationBuilder.Append(string.Format("<a href=\"{0}page=2\">2</a>", targetPage));
                        paginationBuilder.Append("...");
                        for (int counter = page - adjacents; counter <= page + adjacents; counter++)
                        {
                            //從當前頁的左側鄰近頁到右側鄰近頁正常添加頁碼 (當前頁不加連結)
                            paginationBuilder.Append(counter == page ? string.Format("<span class=\"current\">{0}</span>", counter) : string.Format("<a href=\"{0}page={1}\">{1}</a>", targetPage, counter));
                        }
                        //之後的頁碼用...省略，加入倒數第二頁及最末頁
                        paginationBuilder.Append("...");
                        paginationBuilder.Append(string.Format("<a href=\"{0}page={1}\">{1}</a>", targetPage, secondLast));
                        paginationBuilder.Append(string.Format("<a href=\"{0}page={1}\">{1}</a>", targetPage, lastpage));
                    }
                    ////只隱藏左側頁碼
                    else
                    {
                        //加入第一頁+第二頁及...省略頁碼
                        paginationBuilder.Append(string.Format("<a href=\"{0}page=1\">1</a>", targetPage));
                        paginationBuilder.Append(string.Format("<a href=\"{0}page=2\">2</a>", targetPage));
                        paginationBuilder.Append("...");
                        for (int counter = lastpage - commonParameter; counter <= lastpage; counter++)
                        {
                            paginationBuilder.Append(counter == page ? string.Format("<span class=\"current\">{0}</span>", counter) : string.Format("<a href=\"{0}page={1}\">{1}</a>", targetPage, counter));
                        }
                    }
                }
                //下一頁的 HTML 內容，目前頁面小於最末頁則啟用連結，否則就禁用
                paginationBuilder.Append(page < lastpage ? string.Format("<a href=\"{0}page={1}\">>>></a>", targetPage, nextPage) : "<span class=\"disabled\"> >>> </span>");
                paginationBuilder.Append("</div>\r\n");
            }
            return paginationBuilder.ToString();
        }
        public void showPageControls()
        {
            // 清空之前的控制項
            litPage.Text = "";

            int page = 1;  // 預設為第 1 頁

            // 解析頁碼
            if (!string.IsNullOrEmpty(Request["page"]) && IsNumber(Request["page"]))
            {
                page = Convert.ToInt16(Request["page"]);
            }

            if (totalItems == 0 || limit == 0)
            {
                return;
            }

            // 確定目標頁面
            targetPage = targetPage ?? System.IO.Path.GetFileName(Request.PhysicalPath);

            // 渲染分頁控制項
            litPage.Text = getPaginationString(page, totalItems, limit, 2, targetPage);
        }

        //設定自製控制項屬性接收值的類型
        public int totalItems { get; set; } //總共幾筆資料
        public int limit { get; set; } //一頁幾筆資料
        public string targetPage { get; set; } //作用頁面完整網頁名稱
    }
}