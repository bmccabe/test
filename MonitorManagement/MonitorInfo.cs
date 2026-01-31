namespace MonitorManagement;

/// <summary>
/// Represents information about a monitor device
/// </summary>
public class MonitorInfo
{
    /// <summary>
    /// Gets or sets the unique identifier for the monitor
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the monitor
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the horizontal resolution in pixels
    /// </summary>
    public int ResolutionWidth { get; set; }

    /// <summary>
    /// Gets or sets the vertical resolution in pixels
    /// </summary>
    public int ResolutionHeight { get; set; }

    /// <summary>
    /// Gets or sets the brightness level (0-100)
    /// </summary>
    public int Brightness { get; set; }

    /// <summary>
    /// Gets or sets whether the monitor is currently powered on
    /// </summary>
    public bool IsOn { get; set; }
}
