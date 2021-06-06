using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using NLog;

namespace SampleActiveSetupAfterApp
{
    public class ActiveSetupUserReg
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 現ユーザーのActiveSetupレジストリが作成されたら、削除して次回も実行されるようにする。
        /// </summary>
        /// <returns></returns>
        public async Task HKURegDeleteAsync()
        {
            //とりあえず、いずれ作成されることを前提としてポーリングする。（変更監視などの手間のかかることは未実装）
            await Task.Run(() =>
            {
                while (true)
                {
                    using (var regBase = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                    using (var regRootKey =
                        regBase.CreateSubKey($@"Software\Microsoft\Active Setup\Installed Components", true))
                    {
                        try
                        {
                            regRootKey.DeleteSubKey("ActiveSetupTest1", true);
                            _logger.Info("Deleted");
                            break;
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(new TimeSpan(0, 0, 1));
                            continue;
                        }

                    }
                }
            });


        }
    }
}
