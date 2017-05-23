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
            // connectToDB();
            // _data.modifyListName("1", "_listNameNew", "_listNameNew2");
            //_data.getData("2");
            _data.insertData("1");
            // _data.removeData("1", "_listNameDel", new Word { _front = "frontDel1", _back = "backDel1" });
            //  connectToDB();
            _data.removeData("1", "_listNameDel", null);


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
            Word[] _words = new Word[]
                {
                    new Word {_front = "f1U1", _back =  "b1U1" },
                    new Word {_front = "f2U1", _back =  "b2U1" }
                };
            userStorage _userStorage = new userStorage
            {
                UID = "2",
                _listName = "list3"


            };

            collection.InsertOne(_userStorage);

        }

    }
    public class data
    {
        public void insertData(string uid)
        {

            string _listName = "_listNameDel";
            Word[] _words = new Word[]
                {
                    new Word {_front = "frontDel1", _back =  "backDel1" },
                    new Word {_front = "frontDel2", _back =  "backDel2" }
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
                collection.InsertOne(new userStorage { UID = uid, _listName = _listName, _words = _words });
            }
            //Replace existing listName with the updated words - ObjectId("59206633c2d07835c4ccefbf")
            else
            {
                collection.ReplaceOne(filterListName, new userStorage { _id = resultsModifyWords[0]._id, UID = uid, _listName = _listName, _words = _words });
            }

            //change listName - assuming it's possible _words is null


        }
        public void removeData(string UID, string listName, Word word)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("LanguageDB");
            var collection = db.GetCollection<userStorage>("userStorage");
            var builder = Builders<userStorage>.Filter;
            var filterListName = builder.Eq("UID", UID) & builder.Eq("_listName", listName);

            if (word != null)
            {

                var update = Builders<userStorage>.Update.PullFilter(c => c._words, s => s._front == word._front);
                var result = collection.FindOneAndUpdate(filterListName, update);
            }
            else
            {
                var updateWords = Builders<userStorage>.Update.Unset(c => c._words);
                var resultsWords = collection.FindOneAndUpdate(filterListName, updateWords);
                var updateListName = Builders<userStorage>.Update.Unset(c => c._listName);
                var resultsListName = collection.FindOneAndUpdate(filterListName, updateListName);              
                
            }
        }



        public LangDic getData(string uid)
        {

            var client = new MongoClient();
            var db = client.GetDatabase("LanguageDB");
            var collection = db.GetCollection<userStorage>("userStorage");

            //build dictionary by UID
            var builder = Builders<userStorage>.Filter;
            var filterUID = builder.Eq("UID", uid);
            var results = collection.Find(filterUID).ToList();
            LangDic _langDic = new LangDic();

            foreach (userStorage item in results)
            {
                if (item._listName != null)
                {
                    if (item._words != null)
                        _langDic.CardLists.Add(item._listName, new List<Word>(item._words.ToList()));
                    else _langDic.CardLists.Add(item._listName, new List<Word>());
                }
            }
            return _langDic;

        }

        public void modifyListName(string uid, string oldListName, string newListName)
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

