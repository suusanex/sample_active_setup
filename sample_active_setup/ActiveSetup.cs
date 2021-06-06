using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using NLog;

namespace sample_active_setup
{
    public class ActiveSetup : IDisposable
    {
        public void EventServerCreate()
        {
            var sec = new EventWaitHandleSecurity();
            sec.AddAccessRule(new EventWaitHandleAccessRule("EveryOne", EventWaitHandleRights.FullControl, AccessControlType.Allow));

            _UserAppEndWait =
                new EventWaitHandle(false, EventResetMode.AutoReset, _UserAppEndWaitEventName, out _, sec);

            _logger.Info("Event Created");
        }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void HKLMRegSet(string runAppPath)
        {
            using(var regBase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var regRootKey = regBase.CreateSubKey(@"Software\Microsoft\Active Setup\Installed Components", true))
            using (var regKey = regRootKey.CreateSubKey(@"ActiveSetupTest1", true))
            {
                regKey.SetValue("Locale", "ja", RegistryValueKind.String);
                regKey.SetValue("StubPath", runAppPath, RegistryValueKind.String);
                regKey.SetValue(null, "ActiveSetup Test Name", RegistryValueKind.String);
            }

            _logger.Info($"Set Success, {runAppPath}");
        }


        private string _UserAppEndWaitEventName = @"Global\SampleActiveSetupAppEndWait";
        private bool disposedValue;

        private EventWaitHandle _UserAppEndWait;

        public void WaitActiveSetupAppEnd()
        {
            //ユーザーログオンが発生するたびに、ActiveSetupの終了を待ってログを出す、実験用コード。（ログを出すだけで、他の役割は無い）
            while (true)
            {
                _logger.Info("Wait Start");
                _UserAppEndWait.WaitOne();
                _logger.Info("Wait End");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _UserAppEndWait.Dispose();
                    _UserAppEndWait = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
