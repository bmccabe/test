# test

A .NET 9 class library project with sample calculator functionality and monitor power control.

## Project Structure

- `src/TestLibrary` - The main .NET 9 class library
- `tests/TestLibrary.Tests` - xUnit test project for the library
- `tests/MonitorTests.Reqnroll` - Reqnroll/NUnit test project for monitor functionality

## Requirements

- .NET 9 SDK or later

## Building

```bash
dotnet build
```

## Running Tests

```bash
dotnet test
```

## Features

### Calculator Class
The library includes a `Calculator` class with the following operations:
- Add
- Subtract
- Multiply
- Divide (with divide-by-zero protection)

### Monitor Class
The library includes a `Monitor` class for managing monitor power states on Windows:
- `TurnOn()` - Turns the monitor on
- `TurnOff()` - Turns the monitor off
- `IsOn` - Gets the current power state of the monitor

**Note:** Monitor power control is Windows-only and throws `PlatformNotSupportedException` on other platforms.

## Usage Example

### Calculator
```csharp
using TestLibrary;

var calculator = new Calculator();
var sum = calculator.Add(5, 3);        // Returns 8
var difference = calculator.Subtract(10, 4);  // Returns 6
var product = calculator.Multiply(6, 7);      // Returns 42
var quotient = calculator.Divide(10, 2);      // Returns 5.0
```

### Monitor Power Control
```csharp
using TestLibrary;

var monitor = new Monitor();

// Check current state
Console.WriteLine($"Monitor is on: {monitor.IsOn}"); // true by default

// Turn off the monitor
monitor.TurnOff();
Console.WriteLine($"Monitor is on: {monitor.IsOn}"); // false

// Turn on the monitor
monitor.TurnOn();
Console.WriteLine($"Monitor is on: {monitor.IsOn}"); // true
```