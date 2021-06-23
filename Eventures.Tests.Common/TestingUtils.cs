using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Eventures.Data;

namespace Eventures.Tests.Common
{
    public class TestingUtils
    {
        public static void AssignCurrentUserForController(
            Controller controller, EventuresUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName + " " + user.LastName),
            };
            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userPrincipal }
            };
        }
    }
}
