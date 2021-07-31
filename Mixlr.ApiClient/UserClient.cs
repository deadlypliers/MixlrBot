using Mixlr.ApiClient.Interfaces;
using Mixlr.ApiClient.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mixlr.ApiClient
{
    public class UserClient : BaseApiClient
    {
        private const string USERS_ENDPOINT = "/users/";
        private const string COMMENTS_ENDPOINT = "/comments";
        private const string BROADCAST_ACTIONS_ENDPOINT = "/broadcast_actions";

        public UserClient(string apiUrl) : base(apiUrl) { }

        /// <summary>
        /// Gets the Mixlr user account info given a user name or Id
        /// </summary>
        /// <param name="usernameOrId">Username or Id of Mixlr user</param>
        /// <returns>User account details for the specified user.</returns>
        public async Task<User> GetUserInfo(string usernameOrId)
        {
            var url = $"{USERS_ENDPOINT}{usernameOrId}";

            return await Get<User>(url);
        }

        /// <summary>
        /// Gets the latest comments from a user's Live Page.
        /// </summary>
        /// <param name="usernameOrId">Username or Id of Mixlr user</param>
        /// <param name="isUserId">Specifies if the "usernameOrId" field is a Mixlr user id</param>
        /// <returns>Comments on the user's Live Page</returns>
        public async Task<List<Comment>> GetLivePageComments(string usernameOrId, bool isUserId = false)
        {
            var user = new User();

            if (!isUserId)
                user = await GetUserInfo(usernameOrId);
            else
                user.Id = usernameOrId;

            var url = $"{USERS_ENDPOINT}{user.Id}{COMMENTS_ENDPOINT}";

            return await Get<List<Comment>>(url);
        }

        /// <summary>
        /// Gets the last 100 BroadcastActions for a user's Live Page.
        /// </summary>
        /// <param name="usernameOrId">Username or Id of Mixlr user</param>
        /// <param name="isUserId">Specifies if the "usernameOrId" field is a Mixlr user id</param>
        /// <returns>Last 100 BroadcastActions on the user's Live Page</returns>
        public async Task<List<BroadcastAction>> GetLivePageBroadcastActions(string usernameOrId, bool isUserId = false)
        {
            var user = new User();

            if (!isUserId)
                user = await GetUserInfo(usernameOrId);
            else
                user.Id = usernameOrId;

            var url = $"{USERS_ENDPOINT}{user.Id}{BROADCAST_ACTIONS_ENDPOINT}";

            return await Get<List<BroadcastAction>>(url);
        }
    }
}
