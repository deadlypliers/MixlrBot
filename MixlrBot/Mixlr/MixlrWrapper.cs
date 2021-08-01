using Mixlr.ApiClient;
using Mixlr.ApiClient.Models.Broadcasts;
using Mixlr.ApiClient.Models.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MixlrBot.Mixlr
{
    public class MixlrWrapper
    {
        public DateTime LastUpdate { get; set; }
        public List<string> MixlrUsers { get; set; }
        public List<User> LiveMixlrUsers { get; set; }

        private UserClient _userClient;
        private BroadcastClient _broadcastClient;
        

        public MixlrWrapper(string apiUrl, List<string> users)
        {
            _userClient = new UserClient(apiUrl);
            _broadcastClient = new BroadcastClient(apiUrl);
                        
            MixlrUsers = users;
            LiveMixlrUsers = new List<User>();
            LastUpdate = DateTime.MinValue;
        }

        public async Task<User> GetUser(string usernameOrId)
        {
            return await _userClient.GetUserInfo(usernameOrId);
        }

        public async Task<List<Comment>> GetComments(string usernameOrId, bool isUserId = false)
        {
            return await _userClient.GetLivePageComments(usernameOrId, isUserId);
        }

        public async Task<List<BroadcastAction>> GetBroadcastActions(string usernameOrId, bool isUserId = false)
        {
            return await _userClient.GetLivePageBroadcastActions(usernameOrId, isUserId);
        }

        public async Task<Broadcast> GetBroadcast(string broadcastId)
        {
            return await _broadcastClient.GetBroadcastInfo(broadcastId);
        }
    }
}
