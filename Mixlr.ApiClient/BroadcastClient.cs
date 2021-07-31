using Mixlr.ApiClient.Interfaces;
using Mixlr.ApiClient.Models.Broadcasts;
using System.Threading.Tasks;

namespace Mixlr.ApiClient
{
    public class BroadcastClient : BaseApiClient
    {
        private const string BROADCAST_ENDPOINT = "/broadcasts/";

        public BroadcastClient(string apiUrl) : base(apiUrl) { }

        /// <summary>
        /// Gets the Broadcast information based on a broadcast id.
        /// </summary>
        /// <param name="broadcastId">ID of broadcast</param>
        /// <returns>Information regarding the specified broadcast.</returns>
        public async Task<Broadcast> GetBroadcastInfo(string broadcastId)
        {
            var url = $"{BROADCAST_ENDPOINT}{broadcastId}";

            return await Get<Broadcast>(url);
        }
    }
}
