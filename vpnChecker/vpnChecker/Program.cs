using System.Net.NetworkInformation;

namespace vpnChecker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var o_path = Path.Combine(Application.StartupPath, "img", "o.ico");
            var x_path = Path.Combine(Application.StartupPath, "img", "x.ico");

            var notifyIcon = new StatusNotifyIcon(x_path);
            Application.ApplicationExit += (_, _) => notifyIcon.Dispose();

            SetToolStripMenuItems(notifyIcon);

            var vpnConnection = false;

            var timer = new System.Timers.Timer(3000);
            timer.Elapsed += (sender, e) =>
            {
                if (checkVpnConnection())
                {
                    if (!vpnConnection)
                    {
                        vpnConnection = true;
                        notifyIcon.SetIcon(o_path);
                        notifyIcon.Text = "VPN 接続中";
                    }

                    return;
                }

                if (vpnConnection)
                {
                    vpnConnection = false;
                    notifyIcon.SetIcon(x_path);
                    notifyIcon.Text = "VPN 未接続";
                }
            };

            timer.Start();

            Application.Run();
        }

        /// <summary>
        /// ToolStripMenuItemを設定
        /// </summary>
        /// <param name="notifyIcon"></param>
        private static void SetToolStripMenuItems(StatusNotifyIcon notifyIcon)
        {
            notifyIcon.AddToolStripMenuItem(new ToolStripMenuItem("終了", null, (_, _) => Application.Exit()));
        }

        /// <summary>
        /// VPN接続判定
        /// </summary>
        /// <returns></returns>
        private static bool checkVpnConnection()
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ppp)
                {
                    // vpn接続中
                    return true;
                }
            }

            return false;
        }
    }
}