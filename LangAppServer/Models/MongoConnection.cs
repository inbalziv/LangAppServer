using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using MongoDB.Driver;
using MongoDB.Bson;
using System.ComponentModel;

using MongoDB.Driver.Core;
using System.Diagnostics;

namespace LangAppServer.Models
{
    public class MongoConnection : IMongoConnection 
    {
        public void InsertData(string uid, string listName, Word[] words)
        {                   
            //connect to Mongo            
            var client = new MongoClient();
            var db = client.GetDatabase("LanguageDB");
            var collection = db.GetCollection<userStorage>("userStorage");
            //create filter
            var builder = Builders<userStorage>.Filter;

            // search for UID only (with listName null) --> insert listName
            var filterOnlyUID = builder.Eq("UID", uid) & builder.Eq("_listName", BsonNull.Value);
            var insertListName = Builders<userStorage>.Update.Set("_listName", listName);
            var results = collection.UpdateOne(filterOnlyUID, insertListName);

            //insert or update words for UID and listName, if listName not exist creates a new document
            var filterListName = builder.Eq("UID", uid) & builder.Eq("_listName", listName);
            var modifyWords = Builders<userStorage>.Update.Set("_words", words);

            // var resultsModifyWords = collection.FindOneAndUpdate(filterOnlyUID, insertListName);
            var resultsModifyWords = collection.Find(filterListName).ToList();

            //insert a new document
            if (resultsModifyWords.Count == 0)
            {
                collection.InsertOne(new userStorage { UID = uid, _listName = listName, _words = words });
            }
            //Replace existing listName with the updated words - ObjectId("59206633c2d07835c4ccefbf")
            else
            {
                collection.ReplaceOne(filterListName, new userStorage { _id = resultsModifyWords[0]._id, UID = uid, _listName = listName, _words = words });
            }                 


        }
        public bool RemoveData(string UID, string listName, Word word)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("LanguageDB");
            var collection = db.GetCollection<userStorage>("userStorage");
            var builder = Builders<userStorage>.Filter;
            var filterListName = builder.Eq("UID", UID) & builder.Eq("_listName", listName);
            //remove a word object

            try
            {
                if (word != null)
                {

                    var update = Builders<userStorage>.Update.PullFilter(c => c._words, s => s._front == word._front);
                    var result = collection.FindOneAndUpdate(filterListName, update);
                }
                //remove a list by listName
                else
                {
                    var updateWords = Builders<userStorage>.Update.Unset(c => c._words);
                    var resultsWords = collection.FindOneAndUpdate(filterListName, updateWords);
                    var updateListName = Builders<userStorage>.Update.Unset(c => c._listName);
                    var resultsListName = collection.FindOneAndUpdate(filterListName, updateListName);

                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }        
        public LangDic GetData(string uid)
        {
           // Stopwatch stopwatch = new Stopwatch();
           // stopwatch.Restart();
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
           // TimeSpan time = stopwatch.Elapsed;
           // stopwatch.Stop();

            return _langDic;

        }
        public bool ModifyListName(string uid, string oldListName, string newListName)
        {
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("LanguageDB");
            var collection = db.GetCollection<userStorage>("userStorage");

            //search for UID + oldListName and modify to newListName if exist
            var builder = Builders<userStorage>.Filter;
            var filter = builder.Eq("UID", uid) & builder.Eq("_listName", oldListName);
            var updateListName = Builders<userStorage>.Update.Set("_listName", newListName);
            try
            {
                var results = collection.UpdateOne(filter, updateListName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}