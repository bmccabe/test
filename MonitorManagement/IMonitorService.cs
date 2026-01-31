namespace MonitorManagement;

/// <summary>
/// Interface for monitor service operations
/// </summary>
public interface IMonitorService
{
    /// <summary>
    /// Detects all connected monitors
    /// </summary>
    /// <returns>Collection of detected monitors</returns>
    IEnumerable<MonitorInfo> DetectMonitors();

    /// <summary>
    /// Sets the brightness of a specific monitor
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <param name="brightness">Brightness level (0-100)</param>
    /// <returns>True if successful, false otherwise</returns>
    bool SetBrightness(string deviceId, int brightness);

    /// <summary>
    /// Sets the resolution of a specific monitor
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    /// <returns>True if successful, false otherwise</returns>
    bool SetResolution(string deviceId, int width, int height);

    /// <summary>
    /// Turns a monitor on or off
    /// </summary>
    /// <param name="deviceId">The device identifier</param>
    /// <param name="powerOn">True to turn on, false to turn off</param>
    /// <returns>True if successful, false otherwise</returns>
    bool SetPowerState(string deviceId, bool powerOn);
}
