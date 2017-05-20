using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace LangAppServer
{
    public class userStorage
    {
        
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