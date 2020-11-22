using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Course.IPControl.Web.MiddleWares
{
    public class IPSafeMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IPList _ipList;

        public IPSafeMiddleWare(RequestDelegate next, IOptions<IPList> ipList)
        {
            _next = next;
            _ipList = ipList.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestIpAdress = context.Connection.RemoteIpAddress;
            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(requestIpAdress)).Any();
            if (!isWhiteList)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                return;
            }

            await _next(context);
        }
    }
}
