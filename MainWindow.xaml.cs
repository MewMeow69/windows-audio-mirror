using System.Windows;
using AudioControlHub.Models;
using AudioControlHub.Services;

namespace AudioControlHub;

public partial class MainWindow : Window
{
    private readonly DuplicatorService _duplicator = new();
    private List<AudioDevice> _devices = [];
    private System.Windows.Forms.NotifyIcon? _notify;

    public MainWindow()
    {
        InitializeComponent();
        LoadWindowIcon();

        _duplicator.OnStopped += OnMirrorStopped;

        btnRefresh.Click += (_, _) => LoadDevices();
        btnStartMirror.Click += (_, _) => StartMirror();
        btnStopMirror.Click += (_, _) => StopMirror();

        cboSource.SelectionChanged += (_, _) => UpdateButtonStates();
        cboTarget.SelectionChanged += (_, _) => UpdateButtonStates();

        Loaded += OnLoaded;
        Closing += (_, _) =>
        {
            try { _duplicator.Dispose(); } catch { }
            if (_notify != null) { _notify.Visible = false; _notify.Dispose(); }
        };
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        LoadDevices();
        try
        {
            var iconPath = System.IO.Path.Combine(AppContext.BaseDirectory, "app.ico");
            var icon = System.IO.File.Exists(iconPath)
                ? new System.Drawing.Icon(iconPath)
                : System.Drawing.SystemIcons.Application;
            _notify = new System.Windows.Forms.NotifyIcon
            {
                Icon = icon,
                Text = "Windows Audio Mirror",
                Visible = true
            };
            var menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("Show", null, (_, _) => { Show(); WindowState = WindowState.Normal; Activate(); });
            menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            var exit = new System.Windows.Forms.ToolStripMenuItem("Exit");
            exit.Click += (_, _) => { _notify.Visible = false; _notify.Dispose(); _notify = null; System.Windows.Application.Current.Shutdown(); };
            menu.Items.Add(exit);
            _notify.ContextMenuStrip = menu;
            _notify.DoubleClick += (_, _) => { Show(); WindowState = WindowState.Normal; Activate(); };
        }
        catch { }
    }

    private void LoadDevices()
    {
        try
        {
            _devices = AudioDeviceService.GetRenderDevices();
            var list = _devices;

            cboSource.ItemsSource = null;
            cboTarget.ItemsSource = null;

            cboSource.ItemsSource = list;
            cboTarget.ItemsSource = list;

            if (list.Count == 0) return;

            cboSource.SelectedIndex = 0;
            cboTarget.SelectedIndex = list.Count > 1 ? 1 : 0;
        }
        catch { }
        UpdateButtonStates();
    }

    private void StartMirror()
    {
        var source = cboSource.SelectedItem as AudioDevice;
        var target = cboTarget.SelectedItem as AudioDevice;
        if (source == null || target == null) return;

        try
        {
            _duplicator.Start(target.Name);
            lblMirrorStatus.Text = $"Mirroring: {source.Name} → {target.Name}";
            lblMirrorStatus.Foreground = TryBrush("SuccessBrush");
            lblStatusBar.Text = $"Mirroring to {target.Name}";
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        UpdateButtonStates();
    }

    private void StopMirror()
    {
        try { _duplicator.Stop(); } catch { }
    }

    private void OnMirrorStopped()
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            lblMirrorStatus.Text = "Stopped";
            lblMirrorStatus.Foreground = TryBrush("TextSecondaryBrush");
            lblStatusBar.Text = "Ready";
            UpdateButtonStates();
        }));
    }

    private void UpdateButtonStates()
    {
        try
        {
            btnStartMirror.IsEnabled = !_duplicator.IsRunning
                && cboSource.SelectedItem != null
                && cboTarget.SelectedItem != null;
            btnStopMirror.IsEnabled = _duplicator.IsRunning;
        }
        catch { }
    }

    private System.Windows.Media.Brush TryBrush(string key)
    {
        try { return FindResource(key) as System.Windows.Media.Brush ?? System.Windows.Media.Brushes.Gray; }
        catch { return System.Windows.Media.Brushes.Gray; }
    }

    private void LoadWindowIcon()
    {
        var path = System.IO.Path.Combine(AppContext.BaseDirectory, "app.ico");
        if (System.IO.File.Exists(path))
            Icon = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(path));
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == System.Windows.WindowState.Minimized)
            Hide();
        base.OnStateChanged(e);
    }
}
