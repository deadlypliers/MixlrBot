using System;

namespace Mixlr.ApiClient.Models.Exceptions
{
    class MixlrException : Exception
    {
        public string Endpoint { get; set; }

        public string Error { get; set; }
    }
}
