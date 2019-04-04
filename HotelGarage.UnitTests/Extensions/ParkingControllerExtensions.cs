using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HotelGarage.UnitTests.Extensions
{
    public static class ParkingControllerExtensions
    {
        public static void MockCurrentUser(this Controller controller,string userId, string userName)
        {
            var identity = new GenericIdentity(userName);
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/identity/claims/name", userName));
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/identity/claims/nameidentifier", userId));

            var principal = new GenericPrincipal(identity, null);

            Thread.CurrentPrincipal = principal;
        }
    }
}
