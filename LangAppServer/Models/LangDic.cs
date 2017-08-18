using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LangAppServer
{
    public class LangDic
    {
        private Dictionary<string, List<Word>> langDic { get; set; }

        public LangDic()
        {
            langDic = new Dictionary<string, List<Word>>();

        }
        public Dictionary<string, List<Word>> CardLists
        {
            get
            {
                if (langDic != null)
                    return langDic;
                else return new Dictionary<string, List<Word>>();
            }
            set
            {
                langDic = value;

            }
        }
    }
   /* public class Card
    {
        [JsonProperty("_front")]
        public string _front { get; set; }
        [JsonProperty("_back")]
        public string _back { get; set; }

        public Card(Card card)
        {
            this._front = card._front;
            this._back = card._back;
        }
    } */
}