using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if (!string.IsNullOrEmpty(txtMessage.Text))
                {
                    if (lstCleintIP.SelectedItem != null)
                    {
                    //    TcpListener serverS = null;
                    //    try
                    //    { 
                    //        serverS = new TcpListener(IPAddress.Parse((lstCleintIP.SelectedItem.ToString().Split(':')[0].ToString())), Convert.ToInt32((lstCleintIP.SelectedItem.ToString().Split(':')[1].ToString())));
                    //    }
                    //    catch { }
                    //    Thread t = new Thread(() =>
                    //{
                    //    while (true)
                    //    {
                    //        MessageBox.Show("Waiting for a connection... ");

                    //            // Perform a blocking call to accept requests.
                    //            // You could also user server.AcceptSocket() here.
                    //            TcpClient client = serverS.AcceptTcpClient();
                    //        MessageBox.Show("Server Connected!");
                    //    }
                    //});
                                server.Send(lstCleintIP.SelectedItem.ToString(), txtMessage.Text);
                        txtInfo.Text += $"Server : { txtMessage.Text } {Environment.NewLine}";
                        txtMessage.Text = string.Empty;
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
           
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                 if (lstCleintIP.SelectedItem.ToString() == e.IpPort && Encoding.UTF8.GetString(e.Data) == "1")
                {
                    IsAccept = false;
                    return;
                }
           
                txtInfo.Text += $"{e.IpPort}:{Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
                lstCleintIP.Items.Remove(e.IpPort);
                ClientCount.Text = "Clients:" + lstCleintIP.Items.Count;
            });
            //throw new NotImplementedException();
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate { 
               
            txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
            lstCleintIP.Items.Add(e.IpPort);
                ClientCount.Text = "Clients:" + lstCleintIP.Items.Count;
            });

        }

        SimpleTcpServer server;
        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $"Starting {Environment.NewLine}";
            btnStart.Enabled=btnLoading.Enabled = false;
            btnSend.Enabled = true;
        }

        private void btnSload_Click(object sender, EventArgs e)
        {
            server = new(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected; ;
            server.Events.DataReceived += Events_DataReceived;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            btnStart.Enabled = true;
        }
        
       static bool IsAccept = true;
        private void lstCleintIP_DoubleClick(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                server.Send(lstCleintIP.SelectedItem.ToString(), "Admin want to sent files");
                //   AutoClosingMessageBox.Show("It will alert after accepted by Client", "Info", 1000);
                //MessageBox.Show("It will alert after accepted by Client", "Info");
                //while (IsAccept)
                //{
                //    Thread.Sleep(1000);
                //}
                MessageBox.Show("Accepted", "Info", MessageBoxButtons.OK);
            });
           

        }

        private void lstCleintIP_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }
        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                using (_timeoutTimer)
                    MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }



    }
}
