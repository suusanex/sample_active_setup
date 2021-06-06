using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using NLog;

namespace SampleActiveSetupApp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        public class BindingSource : INotifyPropertyChanged
        {
            #region INotifyPropertyChanged実装 
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName = null));
            }
            #endregion

            public BindingSource()
            {
            }

            string _RunPath;
            public string RunPath
            {
                get => _RunPath;
                set { _RunPath = value; OnPropertyChanged(); }
            }

            string _RunOption;
            public string RunOption
            {
                get => _RunOption;
                set { _RunOption = value; OnPropertyChanged(); }
            }


            //string _DUMMY;
            //public string DUMMY
            //{
            //    get => _DUMMY;
            //    set { _DUMMY = value; OnPropertyChanged(); }
            //}

        }

        public BindingSource m_Bind;

        public MainWindow()
        {
            InitializeComponent();

            m_Bind = new BindingSource();

            DataContext = m_Bind;


            _logger.Info("Run");

        }

        private void OnClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClickRunApp(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var proc = Process.Start(m_Bind.RunPath, m_Bind.RunOption ?? string.Empty))
                {

                }
            }
            catch (Exception exception)
            {
                _logger.Info($"{m_Bind.RunPath}, {m_Bind.RunOption}, {exception}");
                MessageBox.Show($"プロセスの起動に失敗しました。{exception}");
            }
        }

        private static string _AppDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private ActiveSetupUserApp _ActiveSetup = new ActiveSetupUserApp();

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _ActiveSetup.PreEnd();
            _logger.Info("Active Setup PreEnd Success");

            using (var proc = Process.Start(Path.Combine(_AppDirPath, "SampleActiveSetupAfterApp.exe")))
            {
                _logger.Info("After App Run Success");
            }
        }
    }
}
