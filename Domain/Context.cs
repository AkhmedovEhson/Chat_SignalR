using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Context
    {
        public HubCallerContext HubCallerContext { get; set; }
        public Hub IHubContext { get; set; }

        public Context(HubCallerContext hubCallerContext,Hub context)
        {
            HubCallerContext = hubCallerContext;
            var q = context.Context;
            
        }


    }
}
