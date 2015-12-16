using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using dota2liveApi.Code;
using System.Threading.Tasks;
using System.Configuration;
using Dota2Api;
using Dota2Api.Exceptions;

namespace dota2liveApi.Hubs
{
    public class MainHub : Hub
    {
        private readonly GetLiveGames _liveGames;
        //Hub class instance is created for each operation of the hub. Data is generated and sent to client in a singleton GetLiveGames
        public MainHub() : this(GetLiveGames.Instance) { }

        public MainHub(GetLiveGames liveGames)
        {
            _liveGames = liveGames;
        }
        public override Task OnConnected()
        {

            //Check if last reponse is not empty and send it back to the client
            if (!string.IsNullOrEmpty(_liveGames.CurrentResponse))
            {
                Clients.Caller.broadcastMessage(_liveGames.CurrentResponse);
            }

            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            return base.OnConnected();
        }
    }
}