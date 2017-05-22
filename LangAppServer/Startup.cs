using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using MongoDB.Driver;
using MongoDB.Bson;
using System.ComponentModel;

using MongoDB.Driver.Core;
//using MongoDB.Driver.Builders;
//using MongoDB.Driver.GridFS;
//using MongoDB.Driver.Linq;

[assembly: OwinStartup(typeof(LangAppServer.Startup))]

namespace LangAppServer
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            data _data = new data();
           // _data.modifyListName("1", "_listNameNew", "_listNameNew2");
            _data.getData("1");
           // _data.insertData("1");
           //  connectToDB();

        }
        public void connectToDB()
        {


            /* if mongo is not local
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString); */
            MongoClient client = new MongoClient();
            //create DB
            IMongoDatabase db = client.GetDatabase("LanguageDB");

            // create a collection
            var collection = db.GetCollection<userStorage>("userStorage");

            // create the document 
            
                userStorage _userStorage = new userStorage
                {
                    UID = "2"
                    
                };

                collection.InsertOne(_userStorage);
            
        }

    }
    public class data
    {//, Word[] words, string listName
        public void insertData(string uid)
        {
            //mock
            uid = "1";
            string _listName = "_listNameNew";
            Word[] _words = new Word[]
                {
                    new Word {_front = "f1ee", _back =  "b1" },
                    new Word {_front = "f2ee", _back =  "b2" }
                };
            string id;
            //connect to Mongo            
            var client = new MongoClient();
            var db = client.GetDatabase("LanguageDB");
            var collection = db.GetCollection<userStorage>("userStorage");
            //create filter
            var builder = Builders<userStorage>.Filter;

            // search for UID only (with listName null) --> insert listName
            var filterOnlyUID = builder.Eq("UID", uid) & builder.Eq("_listName", BsonNull.Value);
            var insertListName = Builders<userStorage>.Update.Set("_listName", _listName);
            var results = collection.UpdateOne(filterOnlyUID, insertListName);

            //insert or update words for UID and listName, if listName not exist creates a new document
            var filterListName = builder.Eq("UID", uid) & builder.Eq("_listName", _listName);
            var modifyWords = Builders<userStorage>.Update.Set("_words", _words);
            
        // var resultsModifyWords = collection.FindOneAndUpdate(filterOnlyUID, insertListName);
            var resultsModifyWords = collection.Find(filterListName).ToList();

            //insert a new document
           if (resultsModifyWords.Count == 0)
            {
                collection.InsertOne(new userStorage { UID = uid, _listName = _listName , _words = _words});
            }
            //Replace existing listName with the updated words - ObjectId("59206633c2d07835c4ccefbf")
            else
            {
                collection.ReplaceOne(filterListName, new userStorage { _id = resultsModifyWords[0]._id, UID = uid, _listName = _listName, _words = _words });
            }

           //change listName - assuming it's possible _words is null


        }
            public void removeData()
            {
                MongoClient client = new MongoClient();
                IMongoDatabase db = client.GetDatabase("LanguageDB");
                var collection = db.GetCollection<userStorage>("userStorage");
            // var results = collection.Find(filterOnlyUID).ToList();
        }
        public void getData(string uid)
        {
           // var connectionString = "mongodb://localhost:27017";
          //  var client = new MongoClient(connectionString);
            var client = new MongoClient();
            var db = client.GetDatabase("LanguageDB");
            var collection = db.GetCollection<userStorage>("userStorage");

            var builder = Builders<userStorage>.Filter;
            var filterUID = builder.Eq("UID", uid);
            var results = collection.Find(filterUID).ToList();
            //_cardsList.Add(_textBoxListName, new List<Card>());
            LangDic _langDic = new LangDic();
          //  Word[] wordsDB; ;
            foreach (userStorage item in results)
            {
              //  wordsDB = new Word[item._words.Count()];

                _langDic.CardLists.Add(item._listName, new List<Word>(item._words.ToList())); // item._id = null;

            }

        }
        public void modifyListName(string uid,string oldListName, string newListName)
        {
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("LanguageDB");
            var collection = db.GetCollection<userStorage>("userStorage");

            //search for UID + oldListName and modify to newListName if exist
            var builder = Builders<userStorage>.Filter;
            var filter = builder.Eq("UID", uid) & builder.Eq("_listName", oldListName);
            var updateListName = Builders<userStorage>.Update.Set("_listName", newListName);
            var results = collection.UpdateOne(filter, updateListName);            
        }        
    }
}

