using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace SampleActiveSetupApp
{
    public class ActiveSetupUserApp
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private string _UserAppEndWaitEventName = @"Global\SampleActiveSetupAppEndWait";

        public void PreEnd()
        {
            using (var ev = EventWaitHandle.OpenExisting(_UserAppEndWaitEventName))
            {
                ev.Set();
            }
        }
    }
}
