using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LangAppServer.Models
{
    public class CardsLists
    {
        string listName { get; set; }
        List<Word> words { get; set; }
    }
}