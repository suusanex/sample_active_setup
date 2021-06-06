using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SampleActiveSetupApp
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show($"下記の問題が起きたため、アプリを終了してログオン処理を再開します。{Environment.NewLine}{e?.Exception}");
            }
            catch (Exception e2)
            {
                Trace.WriteLine(e2.ToString());
            }

        }
    }
}
