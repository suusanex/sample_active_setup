using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace sample_active_setup
{
    public partial class SampleActiveSetup : ServiceBase
    {
        public SampleActiveSetup()
        {
            InitializeComponent();
        }

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private ActiveSetup _ActiveSetup = new ActiveSetup();

        private static string _AppDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        protected override void OnStart(string[] args)
        {
            try
            {
                var appPath = Path.Combine(_AppDirPath, "SampleActiveSetupApp.exe");

                _ActiveSetup.EventServerCreate();

                _ActiveSetup.HKLMRegSet(appPath);

                Task.Run(() =>
                {
                    try
                    {
                        _ActiveSetup.WaitActiveSetupAppEnd();
                    }
                    catch (Exception e)
                    {
                        _logger.Warn($"WaitActiveSetupAppEnd Fail, {e}");
                    }
                });

            }
            catch (Exception e)
            {
                _logger.Warn($"OnStart Fail, {e}");
            }
        }

        protected override void OnStop()
        {
        }


        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            if (changeDescription.Reason == SessionChangeReason.SessionLogon)
            {
                _logger.Info($"SessionLogon Id={changeDescription.SessionId}");
            }
        }
    }
}
