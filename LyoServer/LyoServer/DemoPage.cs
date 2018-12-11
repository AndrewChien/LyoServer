using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyoServer
{
    public class DemoPage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<html>");
            sbText.Append("<head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/><title>DemoPage</title></head>");
            sbText.Append("<body style='margin:10px auto;text-align:center;'>");
            sbText.Append("<h1>用户信息列表</h1>");
            sbText.Append("<table align='center' cellpadding='1' cellspacing='1'><thead><tr><td>ID</td><td>用户名</td></tr></thead>");
            sbText.Append("<tbody>");
            sbText.Append("+++++++++++测试动态网页++++++++++++++");
            sbText.Append("</tbody></table>");
            sbText.Append(string.Format("<h3>更新时间：{0}</h3>", DateTime.Now.ToString()));
            sbText.Append("</body>");
            sbText.Append("</html>");

            context.Response.Body = Encoding.UTF8.GetBytes(sbText.ToString());
            context.Response.StateCode = "200";
            context.Response.ContentType = "text/html";
            context.Response.StateDescription = "OK";
        }

        private string GetDataList()
        {
            StringBuilder sbData = new StringBuilder();
            var strConn = Config.CreatInstance().GetConfigValue("ip");
            //using (SqlConnection conn = new SqlConnection(strConn))
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = "SELECT * FROM UserInfo";
            //        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            //        {
            //            DataTable dt = new DataTable();
            //            adapter.Fill(dt);

            //            if (dt != null)
            //            {
            //                foreach (DataRow row in dt.Rows)
            //                {
            //                    sbData.Append("<tr>");
            //                    sbData.Append(string.Format("<td>{0}</td>", row["ID"].ToString()));
            //                    sbData.Append(string.Format("<td>{0}</td>", row["UserName"].ToString()));
            //                    sbData.Append("</tr>");
            //                }
            //            }
            //        }
            //    }
            //}

            return sbData.ToString();
        }
    }
}
