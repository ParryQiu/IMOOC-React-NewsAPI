using MongoDB.Driver;

namespace NewsAPI.MongoDB
{
    public static class MongoDBClient
    {
        public static IMongoDatabase GetMongoClient()
        {
            var clientPath = Common.Helper.ConfigurationManagerHelper.GetAppSettingValue("mongodb", "mongodb://127.0.0.1:27001");
            var client = new MongoClient(clientPath);
            return client.GetDatabase("reactnews");
        }
    }
}
