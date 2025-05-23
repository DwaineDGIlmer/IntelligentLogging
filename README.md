# IntelligentLogging and Fault Analysis for .NET Applications

## Overview

This solution provides advanced logging and fault analysis capabilities for .NET applications. It is organized for modularity and testability.

The solution includes a web application that demonstrates how to implement intelligent logging, eventing, and fault analysis.

The main features include:
- **Intelligent Logging:**  
  Capture and store logs with contextual information.
- **Eventing:**  
  Trigger events based on specific log entries or conditions.
- **Fault Analysis:**  
  Analyze logs to identify patterns and potential issues.
- **Unit Testing:**  
  Comprehensive unit tests to ensure the reliability of the logging and fault analysis features.

## Deployed Application
The application is deployed in Azure App Service, [IntelligentLogging](https://intelligentlogging-fcgtc5gfazcaaeej.centralus-01.azurewebsites.net/). The solution also includes a web interface for viewing logs and events.

## Solution Structure

```
IntelligentLogging/
├───licenses
├───src
│   └───WebApplication
│       ├───Controllers
│       ├───Models
│       ├───Pages
│       │   └───Shared
│       ├───Properties
│       └───wwwroot
│           ├───css
│           ├───js
│           └───lib
└───tests
    └───UnitTests
        ├───Controllers
        ├───Models
```

## Getting Started

1. **Clone the repository:**
   ```sh
   git clone https://github.com/your-org/IntelligentLogging.git
   cd IntelligentLogging
   ```

2. **Open the solution:**
   Open `IntelligentLogging.sln` in Visual Studio or VS Code.

3. **Build the solution:**
   ```sh
   dotnet build
   ```

4. **Run tests:**
   ```sh
   dotnet test
   ```

## Projects

- **WebApplication:**  
  Main web application containing logging, eventing, and fault analysis features.

- **UnitTests:**  
  Unit tests for the application's core features.

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements.

## License

This project is licensed under the MIT License.