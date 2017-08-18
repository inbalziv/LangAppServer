using LangAppServer.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace LangAppServer.Controllers
{
    
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        static readonly IMongoConnection _mongoConnection = new MongoConnection();

        public object App_Start { get; private set; }

        public DataController()
        {
           
        }

        //GET eg: GET api/data/getdata/{uid}
        [HttpGet]
        [Route("getdata/{uid:string}")]
        public LangDic GetData(string uid)
        {
            LangDic langDic = _mongoConnection.GetData(uid); 
            if (langDic == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return langDic;
        }

        /*
        //POST
        public HttpResponseMessage PostData(string uid, [FromBody]string listName, [FromBody]Word[] words)
        {
            _mongoConnection.InsertData(uid, listName, words);
            string apiName = App_Start.WebApiConfig.DEFAULT_ROUTE_NAME;
            var response =
                this.Request.CreateResponse<userStorage>(HttpStatusCode.Created, listName, words);
            string uri = Url.Link(apiName, new { id = person.Id });
            response.Headers.Location = new Uri(uri);
            return response; 
    }
    */
        //POST eg: POST api/data/putremovedata/{uid}
        [HttpPost]
        [Route("removedata/{uid:string}?listname={listName}")]
        public bool RemoveData(string uid, [FromUri]string listName, [FromBody]Word word)
        {
            if (word.GetType().Equals(typeof(Word)))
            {
                if (!_mongoConnection.RemoveData(uid, listName, word))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return true;
            }
            return false;
        }

        //POST eg: POST api/data/putmodifylistname/{uid}
        [HttpPost]
        [Route("modifylistname/{uid:string}?listnameold={listNameOld}&listnamenew={listNameNew}")]
        public bool PostModifyListName(string uid, [FromUri]string listNameOld, [FromUri]string listNameNew)
        {
            if (!_mongoConnection.ModifyListName(uid, listNameOld, listNameNew))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return true;
        }
    }
}


