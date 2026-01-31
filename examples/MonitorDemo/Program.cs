using TestLibrary;

Console.WriteLine("=== Monitor Power Control Demo ===\n");

try
{
    var monitor = new TestLibrary.Monitor();
    
    Console.WriteLine($"Initial state - Monitor is on: {monitor.IsOn}");
    
    if (OperatingSystem.IsWindows())
    {
        Console.WriteLine("\nTurning monitor off...");
        monitor.TurnOff();
        Console.WriteLine($"Monitor is on: {monitor.IsOn}");
        
        Console.WriteLine("\nWaiting 2 seconds...");
        Thread.Sleep(2000);
        
        Console.WriteLine("Turning monitor back on...");
        monitor.TurnOn();
        Console.WriteLine($"Monitor is on: {monitor.IsOn}");
        
        Console.WriteLine("\n✓ Monitor power control is working!");
    }
    else
    {
        Console.WriteLine("\n⚠ This demo only works on Windows systems.");
        Console.WriteLine("The Monitor class will throw PlatformNotSupportedException on this platform.");
        
        try
        {
            monitor.TurnOff();
        }
        catch (PlatformNotSupportedException ex)
        {
            Console.WriteLine($"\n✓ Expected exception caught: {ex.Message}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\n✗ Error: {ex.Message}");
}

Console.WriteLine("\n=== Demo Complete ===");
