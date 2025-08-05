# 🧠 IntelligentLogging & Fault Analysis for .NET

[![Build Status](https://img.shields.io/github/actions/workflow/status/your-org/IntelligentLogging/dotnet.yml?branch=main)](https://github.com/your-org/IntelligentLogging/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Deployed App](https://img.shields.io/badge/Azure-Live-blue)](https://intelligentlogging-fcgtc5gfazcaaeej.centralus-01.azurewebsites.net/)

> **Advanced logging, eventing, and AI-powered fault analysis for .NET applications.**

---

## 📚 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Live Demo](#-live-demo)
- [Solution Structure](#-solution-structure)
- [Getting Started](#-getting-started)
- [Projects](#-projects)
- [Links](#project-links)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🚀 Overview

IntelligentLogging is a modular solution for capturing, analyzing, and visualizing logs and exceptions in .NET applications. It features real-time eventing, OpenTelemetry (OTEL) support, and a web UI for monitoring and AI-driven analysis.

Depends on AIEventing developed by [Dwain Gilmer](mailto:dwaine.gilmer@protonmail.com), which is a .NET solution for advanced, AI-assisted event logging and fault analysis. It provides structured logging, resilient HTTP clients, and integration with AI models (such as GPT-4) to analyze exceptions and stack traces, offering actionable insights for developers.

---

## ✨ Features

- **Intelligent Logging:**  
  Context-rich log capture and storage.
- **Eventing:**  
  Trigger and visualize events based on log activity.
- **Fault Analysis:**  
  Analyze logs for patterns and potential issues using AI.
- **Web UI:**  
  Real-time log/event dashboard with exception simulation.
- **Unit Testing:**  
  Comprehensive tests for reliability.

---

## 🌐 Live Demo

Try it now:  
[**IntelligentLogging Azure App Service**](https://intelligentlogging-fcgtc5gfazcaaeej.centralus-01.azurewebsites.net/)

---

## 🗂️ Solution Structure

```
IntelligentLogging/
├── licenses/
├── src/
│   └── WebApplication/
│       ├── Controllers/
│       ├── Models/
│       ├── Pages/
│       │   └── Shared/
│       ├── Properties/
│       └── wwwroot/
│           ├── css/
│           ├── js/
│           └── lib/
└── tests/
    └── UnitTests/
        ├── Controllers/
        ├── Models/
```

---

## 🛠️ Getting Started

1. **Clone the repository**
   ```sh
   git clone https://github.com/your-org/IntelligentLogging.git
   cd IntelligentLogging
   ```

2. **Open the solution**  
   Open `IntelligentLogging.sln` in Visual Studio or VS Code.

3. **Build the solution**
   ```sh
   dotnet build
   ```

4. **Run tests**
   ```sh
   dotnet test
   ```

---

## 📦 Projects

- **WebApplication**  
  Main web app for logging, eventing, and fault analysis.

- **UnitTests**  
  Unit tests for core features.

---

## Project Links

- [Project Homepage](https://github.com/DwaineDGIlmer/IntelligentLogging)
- [EzLeadGenerator Homepage](https://github.com/DwaineDGIlmer/EzLeadGenerator)
- [AiEventing Homepage](https://github.com/DwaineDGIlmer/AiEventing)

---

## 🤝 Contributing

Contributions are welcome!  
Please [open an issue](https://github.com/your-org/IntelligentLogging/issues) or submit a pull request.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).
