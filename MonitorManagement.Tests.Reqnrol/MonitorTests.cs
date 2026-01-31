using NUnit.Framework;
using NSubstitute;
using MonitorManagement;

namespace MonitorManagement.Tests.Reqnrol;

[TestFixture]
public class MonitorTests
{
    private IMonitorService _mockMonitorService = null!;
    private Monitor _monitor = null!;

    [SetUp]
    public void Setup()
    {
        _mockMonitorService = Substitute.For<IMonitorService>();
        _monitor = new Monitor(_mockMonitorService);
    }

    [Test]
    public void Constructor_WithNullService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Monitor(null!));
    }

    [Test]
    public void DetectConnectedMonitors_ReturnsDetectedMonitors()
    {
        // Arrange
        var expectedMonitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", ResolutionWidth = 1920, ResolutionHeight = 1080, Brightness = 50, IsOn = true },
            new MonitorInfo { DeviceId = "MON2", Name = "Monitor 2", ResolutionWidth = 2560, ResolutionHeight = 1440, Brightness = 75, IsOn = true }
        };
        _mockMonitorService.DetectMonitors().Returns(expectedMonitors);

        // Act
        var result = _monitor.DetectConnectedMonitors();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        _mockMonitorService.Received(1).DetectMonitors();
    }

    [Test]
    public void DetectConnectedMonitors_ClearsPreviousMonitors()
    {
        // Arrange
        var firstBatch = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", ResolutionWidth = 1920, ResolutionHeight = 1080 }
        };
        var secondBatch = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON2", Name = "Monitor 2", ResolutionWidth = 2560, ResolutionHeight = 1440 }
        };

        _mockMonitorService.DetectMonitors().Returns(firstBatch, secondBatch);

        // Act
        _monitor.DetectConnectedMonitors();
        var result = _monitor.DetectConnectedMonitors();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().DeviceId, Is.EqualTo("MON2"));
    }

    [Test]
    public void GetMonitors_ReturnsAllDetectedMonitors()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1" }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.GetMonitors();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().DeviceId, Is.EqualTo("MON1"));
    }

    [Test]
    public void GetMonitor_WithValidDeviceId_ReturnsMonitorInfo()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1" }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.GetMonitor("MON1");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.DeviceId, Is.EqualTo("MON1"));
    }

    [Test]
    public void GetMonitor_WithInvalidDeviceId_ReturnsNull()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1" }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.GetMonitor("INVALID");

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void AdjustBrightness_WithValidParameters_ReturnsTrue()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", Brightness = 50 }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetBrightness("MON1", 75).Returns(true);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.AdjustBrightness("MON1", 75);

        // Assert
        Assert.That(result, Is.True);
        _mockMonitorService.Received(1).SetBrightness("MON1", 75);
        
        var updatedMonitor = _monitor.GetMonitor("MON1");
        Assert.That(updatedMonitor!.Brightness, Is.EqualTo(75));
    }

    [Test]
    public void AdjustBrightness_WithInvalidDeviceId_ReturnsFalse()
    {
        // Act
        var result = _monitor.AdjustBrightness("INVALID", 50);

        // Assert
        Assert.That(result, Is.False);
        _mockMonitorService.DidNotReceive().SetBrightness(Arg.Any<string>(), Arg.Any<int>());
    }

    [Test]
    [TestCase(-1)]
    [TestCase(101)]
    public void AdjustBrightness_WithInvalidBrightness_ThrowsArgumentException(int brightness)
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1" }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _monitor.DetectConnectedMonitors();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _monitor.AdjustBrightness("MON1", brightness));
    }

    [Test]
    [TestCase(0)]
    [TestCase(50)]
    [TestCase(100)]
    public void AdjustBrightness_WithValidBrightness_Succeeds(int brightness)
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1" }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetBrightness(Arg.Any<string>(), Arg.Any<int>()).Returns(true);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.AdjustBrightness("MON1", brightness);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void AdjustResolution_WithValidParameters_ReturnsTrue()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", ResolutionWidth = 1920, ResolutionHeight = 1080 }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetResolution("MON1", 2560, 1440).Returns(true);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.AdjustResolution("MON1", 2560, 1440);

        // Assert
        Assert.That(result, Is.True);
        _mockMonitorService.Received(1).SetResolution("MON1", 2560, 1440);
        
        var updatedMonitor = _monitor.GetMonitor("MON1");
        Assert.That(updatedMonitor!.ResolutionWidth, Is.EqualTo(2560));
        Assert.That(updatedMonitor.ResolutionHeight, Is.EqualTo(1440));
    }

    [Test]
    public void AdjustResolution_WithInvalidDeviceId_ReturnsFalse()
    {
        // Act
        var result = _monitor.AdjustResolution("INVALID", 1920, 1080);

        // Assert
        Assert.That(result, Is.False);
        _mockMonitorService.DidNotReceive().SetResolution(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());
    }

    [Test]
    [TestCase(0, 1080)]
    [TestCase(-1, 1080)]
    public void AdjustResolution_WithInvalidWidth_ThrowsArgumentException(int width, int height)
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1" }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _monitor.DetectConnectedMonitors();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _monitor.AdjustResolution("MON1", width, height));
    }

    [Test]
    [TestCase(1920, 0)]
    [TestCase(1920, -1)]
    public void AdjustResolution_WithInvalidHeight_ThrowsArgumentException(int width, int height)
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1" }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _monitor.DetectConnectedMonitors();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _monitor.AdjustResolution("MON1", width, height));
    }

    [Test]
    public void SwitchMonitor_WithValidParametersTurnOn_ReturnsTrue()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", IsOn = false }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetPowerState("MON1", true).Returns(true);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.SwitchMonitor("MON1", true);

        // Assert
        Assert.That(result, Is.True);
        _mockMonitorService.Received(1).SetPowerState("MON1", true);
        
        var updatedMonitor = _monitor.GetMonitor("MON1");
        Assert.That(updatedMonitor!.IsOn, Is.True);
    }

    [Test]
    public void SwitchMonitor_WithValidParametersTurnOff_ReturnsTrue()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", IsOn = true }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetPowerState("MON1", false).Returns(true);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.SwitchMonitor("MON1", false);

        // Assert
        Assert.That(result, Is.True);
        _mockMonitorService.Received(1).SetPowerState("MON1", false);
        
        var updatedMonitor = _monitor.GetMonitor("MON1");
        Assert.That(updatedMonitor!.IsOn, Is.False);
    }

    [Test]
    public void SwitchMonitor_WithInvalidDeviceId_ReturnsFalse()
    {
        // Act
        var result = _monitor.SwitchMonitor("INVALID", true);

        // Assert
        Assert.That(result, Is.False);
        _mockMonitorService.DidNotReceive().SetPowerState(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Test]
    public void TurnOn_WithValidDeviceId_ReturnsTrue()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", IsOn = false }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetPowerState("MON1", true).Returns(true);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.TurnOn("MON1");

        // Assert
        Assert.That(result, Is.True);
        _mockMonitorService.Received(1).SetPowerState("MON1", true);
    }

    [Test]
    public void TurnOff_WithValidDeviceId_ReturnsTrue()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", IsOn = true }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetPowerState("MON1", false).Returns(true);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.TurnOff("MON1");

        // Assert
        Assert.That(result, Is.True);
        _mockMonitorService.Received(1).SetPowerState("MON1", false);
    }

    [Test]
    public void AdjustBrightness_WhenServiceFails_ReturnsFalseAndDoesNotUpdateState()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", Brightness = 50 }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetBrightness("MON1", 75).Returns(false);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.AdjustBrightness("MON1", 75);

        // Assert
        Assert.That(result, Is.False);
        
        var monitor = _monitor.GetMonitor("MON1");
        Assert.That(monitor!.Brightness, Is.EqualTo(50)); // Should remain unchanged
    }

    [Test]
    public void AdjustResolution_WhenServiceFails_ReturnsFalseAndDoesNotUpdateState()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", ResolutionWidth = 1920, ResolutionHeight = 1080 }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetResolution("MON1", 2560, 1440).Returns(false);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.AdjustResolution("MON1", 2560, 1440);

        // Assert
        Assert.That(result, Is.False);
        
        var monitor = _monitor.GetMonitor("MON1");
        Assert.That(monitor!.ResolutionWidth, Is.EqualTo(1920)); // Should remain unchanged
        Assert.That(monitor.ResolutionHeight, Is.EqualTo(1080)); // Should remain unchanged
    }

    [Test]
    public void SwitchMonitor_WhenServiceFails_ReturnsFalseAndDoesNotUpdateState()
    {
        // Arrange
        var monitors = new List<MonitorInfo>
        {
            new MonitorInfo { DeviceId = "MON1", Name = "Monitor 1", IsOn = true }
        };
        _mockMonitorService.DetectMonitors().Returns(monitors);
        _mockMonitorService.SetPowerState("MON1", false).Returns(false);
        _monitor.DetectConnectedMonitors();

        // Act
        var result = _monitor.SwitchMonitor("MON1", false);

        // Assert
        Assert.That(result, Is.False);
        
        var monitor = _monitor.GetMonitor("MON1");
        Assert.That(monitor!.IsOn, Is.True); // Should remain unchanged
    }
}
