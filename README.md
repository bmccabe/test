# test

A .NET 9 class library project with sample calculator functionality.

## Project Structure

- `src/TestLibrary` - The main .NET 9 class library
- `tests/TestLibrary.Tests` - xUnit test project for the library

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

The library includes a `Calculator` class with the following operations:
- Add
- Subtract
- Multiply
- Divide (with divide-by-zero protection)

## Usage Example

```csharp
using TestLibrary;

var calculator = new Calculator();
var sum = calculator.Add(5, 3);        // Returns 8
var difference = calculator.Subtract(10, 4);  // Returns 6
var product = calculator.Multiply(6, 7);      // Returns 42
var quotient = calculator.Divide(10, 2);      // Returns 5.0
```