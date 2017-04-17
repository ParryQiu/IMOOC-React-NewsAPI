using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace NewsAPI.MongoDB
{
    public class BLL
    {
        public static void Save(List<NewsData> newsData)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<NewsData>("NewsData");
            foreach (NewsData data in newsData)
            {
                if (dbWebSites.Count(x => x.uniquekey == data.uniquekey) == 0)
                {
                    dbWebSites.InsertOne(data);
                }
            }
        }

        public static void Register(User user)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<User>("User");
            var collection = dbWebSites.Find(Builders<User>.Filter.Empty).Sort(Builders<User>.Sort.Descending("UserId")).Limit(1).ToList();
            var userid = 1;
            if (collection.Count > 0)
            {
                userid = collection[0].UserId + 1;
            }
            user.UserId = userid;
            dbWebSites.InsertOne(user);
        }

        public static User Login(string username, string password)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<User>("User");
            var collection = dbWebSites.Find(x => x.NickUserName == username && x.UserPassword == password).ToList();
            if (collection.Count > 0)
            {
                return collection[0];
            }
            else
            {
                return null;
            }
        }

        public static User GetUser(string userid)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<User>("User");
            var collection = dbWebSites.Find(x => x.UserId == int.Parse(userid)).ToList();
            if (collection.Count > 0)
            {
                return collection[0];
            }
            else
            {
                return null;
            }
        }


        public static List<NewsData> GetNews(int count, string type)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<NewsData>("NewsData");
            var chineseType = string.Empty;
            switch (type)
            {
                case "top":
                    chineseType = "头条";
                    break;
                case "shehui":
                    chineseType = "社会";
                    break;
                case "guonei":
                    chineseType = "国内";
                    break;
                case "guoji":
                    chineseType = "国际";
                    break;
                case "yule":
                    chineseType = "娱乐";
                    break;
                case "tiyu":
                    chineseType = "体育";
                    break;
                case "junshi":
                    chineseType = "军事";
                    break;
                case "keji":
                    chineseType = "科技";
                    break;
                case "caijing":
                    chineseType = "财经";
                    break;
                case "shishang":
                    chineseType = "时尚";
                    break;
            }
            if (type == "top")
            {
                return dbWebSites.Find(x => x.type == chineseType).Sort(Builders<NewsData>.Sort.Descending("date")).Limit(count).ToList();
            }
            else
            {
                return dbWebSites.Find(x => x.realtype == chineseType).Sort(Builders<NewsData>.Sort.Descending("date")).Limit(count).ToList();
            }
        }

        public static List<NewsComments> GetComments(int useridcomment2)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<NewsComments>("NewsComments");
            var coll = dbWebSites.Find(x => x.UserId == useridcomment2).Sort(Builders<NewsComments>.Sort.Ascending("datetime")).ToList();
            return coll;
        }

        public static NewsItem GetNewsItem(string articleId)
        {
            var newsItem = new NewsItem();
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<NewsData>("NewsData");
            var collection = dbWebSites.Find(x => x.uniquekey == articleId).ToList();
            if (collection.Count > 0)
            {
                newsItem.date = collection[0].date;
                newsItem.author_name = collection[0].author_name;
                newsItem.thumbnail_pic_s = collection[0].thumbnail_pic_s;
                newsItem.thumbnail_pic_s02 = collection[0].thumbnail_pic_s02;
                newsItem.thumbnail_pic_s03 = collection[0].thumbnail_pic_s03;
                newsItem.url = collection[0].url;
                newsItem.uniquekey = collection[0].uniquekey;
                newsItem.type = collection[0].type;
                newsItem.title = collection[0].title;
                newsItem.realtype = collection[0].realtype;
                Regex regex = new Regex(@"(?<article><article\s+id=""J_article"".*?</article>)", RegexOptions.Singleline | RegexOptions.Multiline);
                var content = Common.Helper.NetworkHelper.GetPageContent(newsItem.url);
                var match = regex.Match(content);
                if (match.Success)
                {
                    newsItem.pagecontent = match.Groups["article"].ToString();
                }
                return newsItem;
            }
            return null;
        }

        public static void SaveComment(string useridcomment, string uniquekeycomment, string commnet)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<NewsComments>("NewsComments");
            var commentEntity = new NewsComments();
            var user = GetUser(useridcomment);
            commentEntity.uniquekey = uniquekeycomment;
            commentEntity.UserId = int.Parse(useridcomment);
            commentEntity.Comments = commnet;
            commentEntity.UserName = user.NickUserName;
            commentEntity.datetime = DateTime.Now.ToString("yyyy-MM-dd");
            dbWebSites.InsertOne(commentEntity);
        }

        public static List<NewsComments> GetComments(string uniquekeygetcomments)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<NewsComments>("NewsComments");
            var coll = dbWebSites.Find(x => x.uniquekey == uniquekeygetcomments).Sort(Builders<NewsComments>.Sort.Ascending("datetime")).ToList();
            return coll;
        }

        public static List<UserCollections> GetUserCollection(string useridgetuc)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<UserCollections>("UserCollections");
            var coll = dbWebSites.Find(x => x.UserId == int.Parse(useridgetuc)).ToList();
            return coll;
        }

        public static void SaveUserCollection(string useriduc, string uniquekeyuc)
        {
            var dbWebSites = MongoDBClient.GetMongoClient().GetCollection<UserCollections>("UserCollections");
            var uc = new UserCollections();
            uc.uniquekey = uniquekeyuc;
            uc.UserId = int.Parse(useriduc);
            var news = GetNewsItem(uc.uniquekey);
            uc.Title = news.title;
            dbWebSites.InsertOne(uc);
        }
    }
}
