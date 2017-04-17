using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace NewsAPI.MongoDB
{
    public class User
    {
        public ObjectId _id { get; set; }

        public int UserId { get; set; }

        public string NickUserName { get; set; }

        public string UserFace { get; set; }

        public string UserPassword { get; set; }

    }

    public class UserCollections
    {
        public ObjectId Id { get; set; }

        public int UserId { get; set; }

        public string uniquekey { get; set; }

        public string Title { get; set; }

    }
}
