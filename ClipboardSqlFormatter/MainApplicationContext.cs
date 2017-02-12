using ClipboardSqlFormatter.Properties;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardSqlFormatter
{
    class MainApplicationContext:ApplicationContext
    {
        #region Регистратор
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        NotifyIcon notifyIcon;
        MenuItem enabledMenuItem;
        MenuItem closeMenuItem;
        ClipboardMonitor clipboardMonitor;
        bool inSettingToClipboard = false;

        SqlFormatter sqlFormatter;

        public MainApplicationContext()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(
                                new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            _log.InfoFormat("Version {0}", Application.ProductVersion);

            enabledMenuItem = new MenuItem("Enabled", OnEnabledClicked) { Checked = Settings.Default.IsEnabled };
            closeMenuItem = new MenuItem("Close", OnCloseClicked);

            notifyIcon = new NotifyIcon
            {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                ContextMenu = new ContextMenu(new[] { enabledMenuItem, closeMenuItem }),
                Visible = true
            };
            
            clipboardMonitor = new ClipboardMonitor(OnClipboardChanged);
            sqlFormatter = new SqlFormatter();

            SetTrayHint();
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            clipboardMonitor.Dispose();
            Application.Exit();
        }

        private void OnEnabledClicked(object sender, EventArgs e)
        {
            Settings.Default.IsEnabled = enabledMenuItem.Checked = !enabledMenuItem.Checked;

            Properties.Settings.Default.Save();

            SetTrayHint();
        }

        private void SetTrayHint()
        {
            var hint = "ClipboardSqlFormatter " + (Settings.Default.IsEnabled ? "is enabled" : "is disabled");

            notifyIcon.Text = hint;
        }

        private void OnClipboardChanged(object sender, ClipboardChangedEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.UnicodeText) && !inSettingToClipboard && Settings.Default.IsEnabled)
            {
                var inputText = e.DataObject.GetData(DataFormats.UnicodeText) as string;

                string outputText;
                if (sqlFormatter.FormatSql(inputText, out outputText))
                {
                    PutToClipboard(outputText);

                    if (!_log.IsDebugEnabled)
                        _log.InfoFormat("Format and put to clipboard string\n {0} to\n {1}", inputText.Truncate(50), outputText.Truncate(50));
                    else
                        _log.DebugFormat("Format and put to clipboard string\n {0} to\n {1}", inputText, outputText);
                }
            }
        }

        private void PutToClipboard(string text)
        {
            inSettingToClipboard = true;
            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception e)
            {
                _log.Error(string.Format("PutToClipboard: an error occured while putting to clipboard '{0}':", text), e);

                MessageBox.Show("An error while putting to clipboard string. See log for more details", "ClipboardSqlFormatter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                inSettingToClipboard = false;
            }
        }
    }
}