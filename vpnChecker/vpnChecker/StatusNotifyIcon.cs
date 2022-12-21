namespace vpnChecker;

internal class StatusNotifyIcon : IDisposable
{
    private readonly NotifyIcon _notifyIcon;

    public StatusNotifyIcon(string iconFilePath)
    {
        _notifyIcon = new NotifyIcon();
        _notifyIcon.ContextMenuStrip = new ContextMenuStrip();

        SetIcon(iconFilePath);

        _notifyIcon.Visible = true;
        _notifyIcon.Text = "";
    }

    /// <summary>
    /// ToolStripMenuItemを追加
    /// </summary>
    /// <param name="item"></param>
    public void AddToolStripMenuItem(ToolStripMenuItem item)
    {
        _notifyIcon.ContextMenuStrip.Items.Add(item);
    }

    /// <summary>
    /// アイコン画像を設定
    /// </summary>
    /// <param name="iconFilePath"></param>
    public void SetIcon(string iconFilePath)
    {
        if (!File.Exists(iconFilePath))
        {
            return;
        }

        _notifyIcon.Icon = new Icon(iconFilePath);
    }

    /// <summary>
    /// Text
    /// </summary>
    public string Text
    {
        get { return _notifyIcon.Text; }
        set { _notifyIcon.Text = value; }
    }

    /// <summary>
    /// クリック時のイベント
    /// </summary>
    public event EventHandler Click
    {
        add { _notifyIcon.Click += value; }
        remove { _notifyIcon.Click -= value; }
    }

    public void Dispose() => _notifyIcon.Dispose();
}
