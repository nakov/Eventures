using System;

namespace Eventures.WebAPI.Models
{
    public class ResponseWithToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
