using System;

namespace Eventures.WebAPI.Models
{
    public class ResponseWithToken
    {
        public string Token { get; init; }
        public DateTime Expiration { get; init; }
    }
}
