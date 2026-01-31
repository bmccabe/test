using System.Runtime.InteropServices;

namespace TestLibrary;

/// <summary>
/// Provides functionality to manage monitor power states on Windows systems
/// </summary>
public class Monitor
{
    // Windows API constants
    private const int WM_SYSCOMMAND = 0x0112;
    private const int SC_MONITORPOWER = 0xF170;
    private const int MONITOR_ON = -1;
    private const int MONITOR_OFF = 2;
    private const int HWND_BROADCAST = 0xFFFF;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Gets or sets the current power state of the monitor
    /// </summary>
    public bool IsOn { get; private set; } = true;

    /// <summary>
    /// Turns the monitor on
    /// </summary>
    public void TurnOn()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Monitor power control is only supported on Windows");
        }

        SendMonitorPowerCommand(MONITOR_ON);
        IsOn = true;
    }

    /// <summary>
    /// Turns the monitor off
    /// </summary>
    public void TurnOff()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Monitor power control is only supported on Windows");
        }

        SendMonitorPowerCommand(MONITOR_OFF);
        IsOn = false;
    }

    /// <summary>
    /// Sends a power command to the monitor
    /// </summary>
    /// <param name="powerState">The power state to set (MONITOR_ON or MONITOR_OFF)</param>
    private void SendMonitorPowerCommand(int powerState)
    {
        SendMessage(new IntPtr(HWND_BROADCAST), WM_SYSCOMMAND, new IntPtr(SC_MONITORPOWER), new IntPtr(powerState));
    }
}
