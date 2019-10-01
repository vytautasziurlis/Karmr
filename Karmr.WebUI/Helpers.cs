using System;
using System.Security.Principal;
using Karmr.Common.Types;
using Microsoft.AspNet.Identity;

namespace Karmr.WebUI
{
    public static class Helpers
    {
        public static Guid UserId(IPrincipal user)
        {
            return new Guid(user.Identity.GetUserId());
        }

        public static GeoLocation? GeoLocation(decimal latitude, decimal longitude)
        {
            if (latitude == 0 && longitude == 0)
            {
                return null;
            }
            return new GeoLocation(latitude, longitude);
        }
    }
}