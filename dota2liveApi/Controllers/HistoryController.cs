using Dota2Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace dota2liveApi.Controllers
{
    /// <summary>
    /// Used for getting history stats from a game. Implementation coming later
    /// </summary>
    [RoutePrefix("api/history")]
    public class HistoryController : ApiController
    {

      
        public IHttpActionResult Get()
        {
          

            return NotFound();
        }

    }
}
