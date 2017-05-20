using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using MongoDB.Driver;
using MongoDB.Bson;
using System.ComponentModel;

//using MongoDB.Driver.Core;
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
            _data.insertData("1");
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
        public void insertData(string UID)
            {
                
                var client = new MongoClient();
                var db = client.GetDatabase("LanguageDB");
                var collection = db.GetCollection<userStorage>("userStorage");

                var builder = Builders<userStorage>.Filter;
                var filt = builder.Eq("UID", "1") & builder.Eq("_listName", "_listNameTest0");
                //List<userStorage> cursor = collection.Find(filt).;
            //  var filter = builder.ElemMatch(o => o.Users, user => user.Username == username && user.Password == password);
                var results = collection.Find(filt).To;
            foreach (userStorage item in results)
            {
                string json = item.ToJson();
            }
                // var id = results.Find(x => x.UID == "1");
                //  userStorage user = results.SingleOrDefault<userStorage>();
                //  string id = results.SingleOrDefault<userStorage>._id;
                // List<ObjectId> id = results.
                //  Console.Write(list);

                //{UID:'1','_listName':'_listNameTest0'}
                // var results = collection.Find(query).ToBsonDocument();

            }
            public void removeData()
            {
                MongoClient client = new MongoClient();
                IMongoDatabase db = client.GetDatabase("LanguageDB");
                var collection = db.GetCollection<userStorage>("userStorage");
            }
            public void getData()
            {
                MongoClient client = new MongoClient();
                IMongoDatabase db = client.GetDatabase("LanguageDB");
                var collection = db.GetCollection<userStorage>("userStorage");
            }















            

            public void insertUID(string UID)
            {
                MongoClient client = new MongoClient();                
                IMongoDatabase db = client.GetDatabase("LanguageDB");                
                var collection = db.GetCollection<userStorage>("userStorage");

                userStorage _userStorage = new userStorage
                {
                    UID = "2"                    
                };
                try
                {
                    collection.InsertOne(_userStorage);
                }
                catch (Win32Exception e)
                {
                    //UID already exist
                    if (e.ErrorCode == 11000)
                    {

                    }
                }
            }
            public void insertList(string UID, string listName)
            {
                if (isUserExist(UID))
                { }
                if (isListExist(listName))
                { }
            }
            public void insertWords(string UID, Word[] words, string listName)
            {
                if (isUserExist(UID))
                { }
                if (isListExist(listName))
                { }
            }
            public void removeWords(string UID, Word[] words, string listName)
            {
                if (isUserExist(UID))
                { }
                if (isListExist(listName))
                { }
            }
            public void removeList(string UID, string listName)
            {
                if (isUserExist(UID))
                { }
                if (isListExist(listName))
                { }
            }
            public void getLists(string UID)
            {
                string[] _lists;
                if (isUserExist(UID))
                { }

                //  return _lists;
            }
            public void getWords(string UID, string listName)
            {
                string[] _words;
                if (isUserExist(UID))
                { }
                if (isListExist(listName))
                { }

                // return _words;
            }

            private Boolean isUserExist(string UID)
            {
               // var collection = db.GetCollection<userStorage>("userStorage");
                //  var results = collection.Find(x => UID == "2").ToString();
                return false;
            }
            private Boolean isListExist(string listName)
            {
                return false;
            }

        }
}

