using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.IPControl.Web.MiddleWares
{
    public class IPList
    {
        public string[] WhiteList { get; set; }
        public string[] BlackList { get; set; }
    }
}
