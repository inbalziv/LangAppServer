using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangAppServer.Models
{
    interface IMongoConnection
    {
        void InsertData(string uid, string listName, Word[] words);
        bool RemoveData(string UID, string listName, Word word);
        LangDic GetData(string uid);
        bool ModifyListName(string uid, string oldListName, string newListName);
    }
}
