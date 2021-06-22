using System;

namespace Eventures.App.Models.Api
{
    public class ResponseWithToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
