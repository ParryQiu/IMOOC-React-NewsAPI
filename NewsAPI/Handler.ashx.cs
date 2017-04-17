using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Common.Helper;
using NewsAPI.MongoDB;

namespace NewsAPI
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            var result = string.Empty;

            switch (context.Request.QueryString["action"])
            {
                case "getnews":
                    var type = context.Request.QueryString["type"];
                    //var url = string.Format("http://v.juhe.cn/toutiao/index?type={0}&key=755e6a8cb5eb455e702c60fedc0529bf", type);
                    int count = int.Parse(context.Request.QueryString["count"]);
                    //因为接口到期的问题，不再从接口抓取新的新闻入库，直接返回已有的新闻数据供测试使用即可。
                    //var content = NetworkHelper.GetHtmlFromGet(url, Encoding.UTF8);
                    //var newsBase = javaScriptSerializer.Deserialize<NewsBase>(content);
                    //循环所有的数据，判断是否已经在数据库中存在了，如果有那么跳过，如果没有那么就存储起来。
                    //NewsAPI.MongoDB.BLL.Save(newsBase.result.data);

                    //然后从数据库中取数据即可。
                    List<NewsData> listNewsData = NewsAPI.MongoDB.BLL.GetNews(count, type);
                    result = javaScriptSerializer.Serialize(listNewsData);
                    break;

                case "getnewsitem":
                    var articleId = context.Request.QueryString["uniquekey"];
                    var newsItem = BLL.GetNewsItem(articleId);
                    result = javaScriptSerializer.Serialize(newsItem);
                    break;

                case "login":
                    var username = context.Request.QueryString["username"];
                    var password = context.Request.QueryString["password"];
                    var loginUser = BLL.Login(username, password);
                    result = javaScriptSerializer.Serialize(loginUser);
                    break;

                case "register":
                    var r_userName = context.Request.QueryString["r_userName"];
                    var r_password = context.Request.QueryString["r_password"];
                    var user = new User();
                    user.UserPassword = r_password;
                    user.NickUserName = r_userName;
                    BLL.Register(user);
                    result = javaScriptSerializer.Serialize(true);
                    break;

                case "getcomments":
                    var uniquekeygetcomments = context.Request.QueryString["uniquekey"];
                    List<NewsComments> collectionComments = BLL.GetComments(uniquekeygetcomments);
                    result = javaScriptSerializer.Serialize(collectionComments);
                    break;

                case "getusercomments":
                    var useridcomment2 = context.Request.QueryString["userid"];
                    List<NewsComments> collectionComments2 = BLL.GetComments(int.Parse(useridcomment2));
                    result = javaScriptSerializer.Serialize(collectionComments2);
                    break;

                case "comment":
                    var useridcomment = context.Request.QueryString["userid"];
                    var uniquekeycomment = context.Request.QueryString["uniquekey"];
                    var commnet = context.Request.QueryString["commnet"];
                    BLL.SaveComment(useridcomment, uniquekeycomment, commnet);
                    result = javaScriptSerializer.Serialize(true);
                    break;

                case "getuc":
                    var useridgetuc = context.Request.QueryString["userid"];
                    List<UserCollections> collection = BLL.GetUserCollection(useridgetuc);
                    result = javaScriptSerializer.Serialize(collection);
                    break;

                case "uc":
                    var useriduc = context.Request.QueryString["userid"];
                    var uniquekeyuc = context.Request.QueryString["uniquekey"];
                    BLL.SaveUserCollection(useriduc, uniquekeyuc);
                    result = javaScriptSerializer.Serialize(true);
                    break;
            }

            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}