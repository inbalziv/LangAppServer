using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace LangAppServer
{
    [BsonIgnoreExtraElements]
    public class userStorage
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string UID { get; set; }
        [BsonIgnoreIfNull]
        public string _listName { get; set; }
        [BsonIgnoreIfNull]
        public IList<Word> _words { get; set; }
            

    }
    public class Word
    {
        public string _front { get; set; }
        public string _back { get; set; }

    }
    
}