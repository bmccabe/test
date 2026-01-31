# Monitor Management System

A C# library for managing and controlling monitors attached to a Windows system.

## Features

- **Detect Connected Monitors**: Discover all monitors currently connected to the system
- **Brightness Control**: Adjust monitor brightness levels (0-100)
- **Resolution Management**: Change monitor resolution settings
- **Power Management**: Switch monitors on/off programmatically

## Project Structure

- **MonitorManagement**: Main class library containing the Monitor class and interfaces
- **MonitorManagement.Tests.Reqnrol**: NUnit test project with comprehensive tests using NSubstitute for mocking

## Classes and Interfaces

### Monitor
The main class for managing monitor operations. Uses dependency injection to work with different monitor service implementations.

**Key Methods:**
- `DetectConnectedMonitors()` - Detects all connected monitors
- `GetMonitors()` - Gets all detected monitors
- `GetMonitor(deviceId)` - Gets a specific monitor by device ID
- `AdjustBrightness(deviceId, brightness)` - Adjusts brightness (0-100)
- `AdjustResolution(deviceId, width, height)` - Changes resolution
- `SwitchMonitor(deviceId, turnOn)` - Switches monitor power state
- `TurnOn(deviceId)` - Convenience method to turn monitor on
- `TurnOff(deviceId)` - Convenience method to turn monitor off

### IMonitorService
Interface for monitor service operations. Implement this interface to provide actual Windows API integration.

### MonitorInfo
Data model containing monitor information:
- DeviceId
- Name
- ResolutionWidth
- ResolutionHeight
- Brightness
- IsOn (power state)

## Usage Example

```csharp
// Create a monitor service implementation (not included in this library)
IMonitorService monitorService = new WindowsMonitorService();

// Initialize the Monitor class
var monitor = new Monitor(monitorService);

// Detect all connected monitors
var monitors = monitor.DetectConnectedMonitors();

foreach (var mon in monitors)
{
    Console.WriteLine($"Found monitor: {mon.Name} ({mon.DeviceId})");
}

// Adjust brightness of first monitor
if (monitors.Any())
{
    var firstMonitor = monitors.First();
    monitor.AdjustBrightness(firstMonitor.DeviceId, 80);
}

// Change resolution
monitor.AdjustResolution("MON1", 1920, 1080);

// Turn monitor off
monitor.TurnOff("MON1");

// Turn monitor back on
monitor.TurnOn("MON1");
```

## Testing

The project includes 27 comprehensive unit tests using NUnit and NSubstitute:

```bash
dotnet test
```

### Test Coverage

- Constructor validation
- Monitor detection functionality
- Brightness adjustment (valid/invalid inputs)
- Resolution adjustment (valid/invalid inputs)
- Power state management
- Error handling and state management
- Service failure scenarios

All tests verify that:
- Operations work correctly with valid inputs
- Invalid inputs are properly validated
- State is managed correctly
- Service failures are handled gracefully

## Building

```bash
dotnet build MonitorManagement.slnx
```

## Requirements

- .NET 8.0 or later
- Windows operating system (for actual monitor control)

## Implementation Notes

This library provides the abstraction and business logic for monitor management. To actually control monitors on a Windows system, you need to:

1. Implement the `IMonitorService` interface
2. Use Windows APIs such as:
   - Display Configuration APIs (SetDisplayConfig, QueryDisplayConfig)
   - Monitor Configuration API (GetMonitorBrightness, SetMonitorBrightness)
   - Display Device APIs (EnumDisplayDevices, EnumDisplaySettings)

## License

This is a sample implementation for educational purposes.
