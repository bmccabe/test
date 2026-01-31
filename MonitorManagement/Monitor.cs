namespace MonitorManagement;

/// <summary>
/// Manages and controls monitors attached to a Windows system
/// </summary>
public class Monitor
{
    private readonly IMonitorService _monitorService;
    private readonly Dictionary<string, MonitorInfo> _monitors;

    /// <summary>
    /// Initializes a new instance of the Monitor class
    /// </summary>
    /// <param name="monitorService">The monitor service implementation</param>
    public Monitor(IMonitorService monitorService)
    {
        _monitorService = monitorService ?? throw new ArgumentNullException(nameof(monitorService));
        _monitors = new Dictionary<string, MonitorInfo>();
    }

    /// <summary>
    /// Detects all connected monitors and stores them internally
    /// </summary>
    /// <returns>Collection of detected monitors</returns>
    public IEnumerable<MonitorInfo> DetectConnectedMonitors()
    {
        var detectedMonitors = _monitorService.DetectMonitors();
        _monitors.Clear();
        
        foreach (var monitor in detectedMonitors)
        {
            _monitors[monitor.DeviceId] = monitor;
        }
        
        return detectedMonitors.ToList();
    }

    /// <summary>
    /// Gets information about all currently detected monitors
    /// </summary>
    /// <returns>Collection of monitor information</returns>
    public IEnumerable<MonitorInfo> GetMonitors()
    {
        return _monitors.Values.ToList();
    }

    /// <summary>
    /// Gets information about a specific monitor
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <returns>Monitor information, or null if not found</returns>
    public MonitorInfo? GetMonitor(string deviceId)
    {
        return _monitors.TryGetValue(deviceId, out var monitor) ? monitor : null;
    }

    /// <summary>
    /// Adjusts the brightness of a specific monitor
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <param name="brightness">Brightness level (0-100)</param>
    /// <returns>True if successful, false otherwise</returns>
    /// <exception cref="ArgumentException">Thrown when brightness is out of range</exception>
    public bool AdjustBrightness(string deviceId, int brightness)
    {
        if (brightness < 0 || brightness > 100)
        {
            throw new ArgumentException("Brightness must be between 0 and 100", nameof(brightness));
        }

        if (!_monitors.ContainsKey(deviceId))
        {
            return false;
        }

        var success = _monitorService.SetBrightness(deviceId, brightness);
        
        if (success && _monitors.TryGetValue(deviceId, out var monitor))
        {
            monitor.Brightness = brightness;
        }
        
        return success;
    }

    /// <summary>
    /// Adjusts the resolution of a specific monitor
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    /// <returns>True if successful, false otherwise</returns>
    /// <exception cref="ArgumentException">Thrown when resolution values are invalid</exception>
    public bool AdjustResolution(string deviceId, int width, int height)
    {
        if (width <= 0)
        {
            throw new ArgumentException("Width must be greater than 0", nameof(width));
        }

        if (height <= 0)
        {
            throw new ArgumentException("Height must be greater than 0", nameof(height));
        }

        if (!_monitors.ContainsKey(deviceId))
        {
            return false;
        }

        var success = _monitorService.SetResolution(deviceId, width, height);
        
        if (success && _monitors.TryGetValue(deviceId, out var monitor))
        {
            monitor.ResolutionWidth = width;
            monitor.ResolutionHeight = height;
        }
        
        return success;
    }

    /// <summary>
    /// Switches a monitor on or off
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <param name="turnOn">True to turn on, false to turn off</param>
    /// <returns>True if successful, false otherwise</returns>
    public bool SwitchMonitor(string deviceId, bool turnOn)
    {
        if (!_monitors.ContainsKey(deviceId))
        {
            return false;
        }

        var success = _monitorService.SetPowerState(deviceId, turnOn);
        
        if (success && _monitors.TryGetValue(deviceId, out var monitor))
        {
            monitor.IsOn = turnOn;
        }
        
        return success;
    }

    /// <summary>
    /// Turns a monitor on
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <returns>True if successful, false otherwise</returns>
    public bool TurnOn(string deviceId)
    {
        return SwitchMonitor(deviceId, true);
    }

    /// <summary>
    /// Turns a monitor off
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <returns>True if successful, false otherwise</returns>
    public bool TurnOff(string deviceId)
    {
        return SwitchMonitor(deviceId, false);
    }
}
