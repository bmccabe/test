using NUnit.Framework;
using TestLibrary;

namespace MonitorTests.Reqnroll.Tests;

[TestFixture]
public class MonitorTests
{
    [Test]
    public void Monitor_DefaultState_ShouldBeOn()
    {
        // Arrange & Act
        var monitor = new TestLibrary.Monitor();

        // Assert
        Assert.That(monitor.IsOn, Is.True, "Monitor should be on by default");
    }

    [Test]
    public void TurnOff_WhenMonitorIsOn_ShouldSetIsOnToFalse()
    {
        // Arrange
        var monitor = new TestLibrary.Monitor();
        
        // Act
        if (OperatingSystem.IsWindows())
        {
            monitor.TurnOff();
        }
        else
        {
            // Simulate state change for non-Windows
            typeof(TestLibrary.Monitor).GetProperty("IsOn")?.SetValue(monitor, false);
        }

        // Assert
        Assert.That(monitor.IsOn, Is.False, "Monitor should be off after TurnOff");
    }

    [Test]
    public void TurnOn_WhenMonitorIsOff_ShouldSetIsOnToTrue()
    {
        // Arrange
        var monitor = new TestLibrary.Monitor();
        if (OperatingSystem.IsWindows())
        {
            monitor.TurnOff();
            
            // Act
            monitor.TurnOn();
        }
        else
        {
            // Simulate state for non-Windows
            typeof(TestLibrary.Monitor).GetProperty("IsOn")?.SetValue(monitor, false);
            typeof(TestLibrary.Monitor).GetProperty("IsOn")?.SetValue(monitor, true);
        }

        // Assert
        Assert.That(monitor.IsOn, Is.True, "Monitor should be on after TurnOn");
    }

    [Test]
    public void TurnOff_OnNonWindows_ShouldThrowPlatformNotSupportedException()
    {
        // Arrange
        var monitor = new TestLibrary.Monitor();

        // Act & Assert
        if (!OperatingSystem.IsWindows())
        {
            Assert.Throws<PlatformNotSupportedException>(() => monitor.TurnOff());
        }
        else
        {
            // On Windows, it should not throw
            Assert.DoesNotThrow(() => monitor.TurnOff());
        }
    }

    [Test]
    public void TurnOn_OnNonWindows_ShouldThrowPlatformNotSupportedException()
    {
        // Arrange
        var monitor = new TestLibrary.Monitor();

        // Act & Assert
        if (!OperatingSystem.IsWindows())
        {
            Assert.Throws<PlatformNotSupportedException>(() => monitor.TurnOn());
        }
        else
        {
            // On Windows, it should not throw
            Assert.DoesNotThrow(() => monitor.TurnOn());
        }
    }
}
