using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace ClipboardSqlFormatter
{
    public partial class ClipboardMonitor : Control
    {
        IntPtr nextClipboardViewer;

        public event EventHandler<ClipboardChangedEventArgs> ClipboardChanged;

        public ClipboardMonitor()
        {
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)Handle);
        }

        public ClipboardMonitor(EventHandler<ClipboardChangedEventArgs> handler):this()
        {
            ClipboardChanged += handler;
        }

        protected override void Dispose(bool disposing)
        {
            ChangeClipboardChain(this.Handle, nextClipboardViewer);
        }

        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    OnClipboardChanged();
                    SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        void OnClipboardChanged()
        {
            try
            {
                IDataObject iData = Clipboard.GetDataObject();
                if (ClipboardChanged != null)
                {
                    ClipboardChanged(this, new ClipboardChangedEventArgs(iData));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }

    public class ClipboardChangedEventArgs : EventArgs
    {
        public readonly IDataObject DataObject;

        public ClipboardChangedEventArgs(IDataObject dataObject)
        {
            DataObject = dataObject;
        }
    }
}