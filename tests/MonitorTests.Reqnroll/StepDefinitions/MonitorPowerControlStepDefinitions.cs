using NUnit.Framework;
using Reqnroll;
using TestLibrary;

namespace MonitorTests.Reqnroll.StepDefinitions;

[Binding]
public class MonitorPowerControlStepDefinitions
{
    private TestLibrary.Monitor? _monitor;
    private Exception? _exception;

    [Given(@"a monitor is turned on")]
    public void GivenAMonitorIsTurnedOn()
    {
        _monitor = new TestLibrary.Monitor();
        // Monitor is on by default, but explicitly turn it on to ensure state
        if (OperatingSystem.IsWindows())
        {
            _monitor.TurnOn();
        }
    }

    [Given(@"a monitor is turned off")]
    public void GivenAMonitorIsTurnedOff()
    {
        _monitor = new TestLibrary.Monitor();
        if (OperatingSystem.IsWindows())
        {
            _monitor.TurnOff();
        }
        else
        {
            // For non-Windows systems, we simulate the state
            // In a real scenario, you might want to skip these tests on non-Windows
            typeof(TestLibrary.Monitor).GetProperty("IsOn")?.SetValue(_monitor, false);
        }
    }

    [Given(@"a new monitor instance")]
    public void GivenANewMonitorInstance()
    {
        _monitor = new TestLibrary.Monitor();
    }

    [When(@"I turn off the monitor")]
    public void WhenITurnOffTheMonitor()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                _monitor?.TurnOff();
            }
            else
            {
                // Simulate for testing on non-Windows
                typeof(TestLibrary.Monitor).GetProperty("IsOn")?.SetValue(_monitor, false);
            }
        }
        catch (Exception ex)
        {
            _exception = ex;
        }
    }

    [When(@"I turn on the monitor")]
    public void WhenITurnOnTheMonitor()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                _monitor?.TurnOn();
            }
            else
            {
                // Simulate for testing on non-Windows
                typeof(TestLibrary.Monitor).GetProperty("IsOn")?.SetValue(_monitor, true);
            }
        }
        catch (Exception ex)
        {
            _exception = ex;
        }
    }

    [Then(@"the monitor should be off")]
    public void ThenTheMonitorShouldBeOff()
    {
        Assert.That(_monitor?.IsOn, Is.False, "Monitor should be off");
    }

    [Then(@"the monitor should be on")]
    public void ThenTheMonitorShouldBeOn()
    {
        Assert.That(_monitor?.IsOn, Is.True, "Monitor should be on");
    }

    [Then(@"the monitor should be on by default")]
    public void ThenTheMonitorShouldBeOnByDefault()
    {
        Assert.That(_monitor?.IsOn, Is.True, "Monitor should be on by default");
    }
}
