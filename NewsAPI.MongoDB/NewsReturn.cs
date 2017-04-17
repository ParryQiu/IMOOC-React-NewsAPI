using System.Collections.Generic;
using MongoDB.Bson;

namespace NewsAPI.MongoDB
{
    public class NewsBase
    {
        public string reason { get; set; }
        public NewsResut result { get; set; }
        public string error_code { get; set; }
    }

    public class NewsResut
    {
        public string stat { get; set; }
        public List<NewsData> data { get; set; }
    }

    public class NewsData
    {
        public ObjectId Id { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string author_name { get; set; }
        public string thumbnail_pic_s { get; set; }
        public string thumbnail_pic_s02 { get; set; }
        public string thumbnail_pic_s03 { get; set; }
        public string url { get; set; }
        public string uniquekey { get; set; }
        public string type { get; set; }
        public string realtype { get; set; }
    }

    public class NewsItem : NewsData
    {
        public string pagecontent { get; set; }
    }

    public class NewsComments
    {
        public ObjectId Id { get; set; }
        public string uniquekey { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFace { get; set; }
        public string Comments { get; set; }
        public string datetime { get; set; }
    }
}