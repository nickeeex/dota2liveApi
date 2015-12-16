using Dota2Api;
using Dota2Api.Exceptions;
using Dota2Api.Results;
using dota2liveApi.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace dota2liveApi.Code
{

    public  class GetLiveGames 
    {
        //Singleton instance
        //Hub clients are given to this
        private readonly static Lazy<GetLiveGames> _instance = new Lazy<GetLiveGames>(() => new GetLiveGames(GlobalHost.ConnectionManager.GetHubContext<MainHub>().Clients));
        private readonly Timer _timer;
        private string _currentResponse { get; set; }
        public string CurrentResponse    // the Name property
        {
            get
            {
                return _currentResponse;
            }
        }
        private GetLiveGames(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            //Timer to get the live game data from valve dota 2 WebApi. Games are updated approximately one time per minute
            _timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }

        public static GetLiveGames Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

      
        private async void Callback(object state)
        {
           
            var key = ConfigurationManager.AppSettings["ApiKey"];
            using (ApiHandler handler = new ApiHandler(key))
            {
                try {
                var matchResult = await handler.GetLiveLeagueGames();
                    //not great, save response for later use, to send when client connects before update is cycled
                    _currentResponse = matchResult;
                    BroadcastMatchResults(matchResult);
                }
                catch (ServiceUnavailableException ex)
                {
                    BroadcastFailureToUpdate("APIERROR", ex.Message);
                    //Have to se what kind of exceptions the c# dota 2 api wrapper generates
                } 
            }
        }



        private void BroadcastMatchResults(string results)
        {
            try
            {
                //Send result string to all connected clients
                Clients.All.broadcastMessage(results);

            } catch(Exception ex)
            {
                BroadcastFailureToUpdate("JSONERROR", "Could not serialize object to json" +ex.ToString());
            }
            
        }
        private void BroadcastFailureToUpdate(string code, string reason)
        {
            //Lazy, should use enums
            var obj = new CustomException { Code = code, Message = reason };
            Clients.All.broadcastMessage(JsonConvert.SerializeObject(obj));
        }
    }

 }


