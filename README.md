<div align="center">

```
 __    __            __                                                                 __
/  |  /  |          /  |                                                               /  |
$$ | /$$/  __    __ $$ |____    ______    _______          _______  _____  ____    ____$$ |
$$ |/$$/  /  |  /  |$$      \  /      \  /       |______  /       |/     \/    \  /    $$ |
$$  $$<   $$ |  $$ |$$$$$$$  |/$$$$$$  |/$$$$$$$//      |/$$$$$$$/ $$$$$$ $$$$  |/$$$$$$$ |
$$$$$  \  $$ |  $$ |$$ |  $$ |$$    $$ |$$ |     $$$$$$/ $$ |      $$ | $$ | $$ |$$ |  $$ |
$$ |$$  \ $$ \__$$ |$$ |__$$ |$$$$$$$$/ $$ \_____        $$ \_____ $$ | $$ | $$ |$$ \__$$ |
$$ | $$  |$$    $$/ $$    $$/ $$       |$$       |       $$       |$$ | $$ | $$ |$$    $$ |
$$/   $$/  $$$$$$/  $$$$$$$/   $$$$$$$/  $$$$$$$/         $$$$$$$/ $$/  $$/  $$/  $$$$$$$/
```

**Kubernetes Config Manager** ⚓

[![.NET Build & Test](https://github.com/eddyv73/kubec-cmd-v2/actions/workflows/dotnet.yml/badge.svg)](https://github.com/eddyv73/kubec-cmd-v2/actions/workflows/dotnet.yml)
[![Release](https://img.shields.io/github/v/release/eddyv73/kubec-cmd-v2?style=flat-square)](https://github.com/eddyv73/kubec-cmd-v2/releases)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg?style=flat-square)](https://dotnet.microsoft.com/)

*Switch between Kubernetes configurations with ease* 🚀

[Installation](#-installation) •
[Usage](#-usage) •
[Features](#-features) •
[Download](#-download)

</div>

---

## 📖 About

**kubec-cmd** is a command-line tool that simplifies managing multiple Kubernetes configuration files. Instead of manually copying and renaming files, just run a single command to switch between different cluster configurations.

### The Problem

Managing multiple Kubernetes clusters means juggling multiple `kubeconfig` files:
- `config_production`
- `config_staging`
- `config_development`
- `config_local`

Manually switching between them is tedious and error-prone.

### The Solution

```bash
# Switch to production config
kubec-cmd -t production

# Switch to staging config
kubec-cmd -t staging

# That's it! ✔
```

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| 🔄 **Quick Switch** | Switch configs with a single command |
| 📋 **List Configs** | See all available configurations |
| 💾 **Auto Backup** | Automatic backups before switching |
| 🧹 **Clean Backups** | Remove old backup files easily |
| 🖥️ **Cross-Platform** | Works on Linux, macOS, and Windows |
| ⚡ **Self-Contained** | No dependencies required |

---

## 📦 Installation

### Option 1: Quick Install (Recommended)

#### Linux / macOS

```bash
curl -fsSL https://raw.githubusercontent.com/eddyv73/kubec-cmd-v2/main/install.sh | bash
```

Or with wget:
```bash
wget -qO- https://raw.githubusercontent.com/eddyv73/kubec-cmd-v2/main/install.sh | bash
```

The installer will:
- Detect your OS and architecture automatically
- Download the correct binary
- Install to `/usr/local/bin`
- Remove macOS quarantine if needed
- Show instructions to add to PATH if necessary

#### Windows (PowerShell)

```powershell
irm https://raw.githubusercontent.com/eddyv73/kubec-cmd-v2/main/install.ps1 | iex
```

The installer will:
- Detect your architecture (x64/ARM64)
- Download the correct binary
- Install to `%LOCALAPPDATA%\kubec-cmd`
- Add to user PATH automatically

---

### Option 2: Manual Download

Download the latest release for your platform:

| Platform | Architecture | Download |
|----------|--------------|----------|
| **Linux** | x64 | [kubec-cmd-linux-x64](https://github.com/eddyv73/kubec-cmd-v2/releases/latest) |
| **Linux** | ARM64 | [kubec-cmd-linux-arm64](https://github.com/eddyv73/kubec-cmd-v2/releases/latest) |
| **macOS** | Intel | [kubec-cmd-osx-x64](https://github.com/eddyv73/kubec-cmd-v2/releases/latest) |
| **macOS** | Apple Silicon | [kubec-cmd-osx-arm64](https://github.com/eddyv73/kubec-cmd-v2/releases/latest) |
| **Windows** | x64 | [kubec-cmd-win-x64.exe](https://github.com/eddyv73/kubec-cmd-v2/releases/latest) |
| **Windows** | ARM64 | [kubec-cmd-win-arm64.exe](https://github.com/eddyv73/kubec-cmd-v2/releases/latest) |

#### Linux/macOS Manual Setup

```bash
# Download (example for macOS ARM64)
curl -LO https://github.com/eddyv73/kubec-cmd-v2/releases/latest/download/kubec-cmd-osx-arm64

# Make executable
chmod +x kubec-cmd-osx-arm64

# Move to PATH
sudo mv kubec-cmd-osx-arm64 /usr/local/bin/kubec-cmd

# Verify installation
kubec-cmd --help
```

#### macOS Gatekeeper Note

If macOS shows a security warning, run:
```bash
xattr -d com.apple.quarantine /usr/local/bin/kubec-cmd
```

---

### Option 3: Build from Source

```bash
# Clone the repository
git clone https://github.com/eddyv73/kubec-cmd-v2.git
cd kubec-cmd-v2

# Build
dotnet build --configuration Release

# Run
dotnet run --project kubec-cmd/kubec-cmd.csproj
```

---

## 🚀 Usage

### Prerequisites

Create your config files in `~/.kube/` with the prefix `config_`:

```
~/.kube/
├── config              # Active config (managed by kubec-cmd)
├── config_production   # Production cluster
├── config_staging      # Staging cluster
├── config_development  # Development cluster
└── config_local        # Local cluster (minikube, kind, etc.)
```

### Commands

#### Switch Configuration

```bash
# Switch to a specific config
kubec-cmd -t <suffix>

# Examples:
kubec-cmd -t production    # Activates config_production
kubec-cmd -t staging       # Activates config_staging
kubec-cmd -t local         # Activates config_local
```

#### List Available Configs

```bash
kubec-cmd --list
```

Output:
```
config_production
config_staging
config_development
config_local
```

#### Show Help

```bash
kubec-cmd --help
```

Output:
```
 __    __            __                                                 __
/  |  /  |          /  |                                               /  |
$$ | /$$/  __    __ $$ |____    ______    _______          _______  _____  ____    ____$$ |
...
===========================================================================================
Kubec-cmd ⚓
Formula Σ : V2.0 ⚙
By Eddy Wister
Github ➜ : https://github.com/eddyv73/kubec-cmd-v2
⚒-----------------------------------------------------------------------------------⚒
Target file ◎: kubec-cmd -t 'subfix'
⚒-----------------------------------------------------------------------------------⚒
Place Target file ℹ : config_'subfix'
⚒-----------------------------------------------------------------------------------⚒
List config files ☰: kubec-cmd --list
⚒-----------------------------------------------------------------------------------⚒
Clean backup files ♲: kubec-cmd --clean
⚒-----------------------------------------------------------------------------------⚒
```

---

## 📋 Examples

### Example 1: Basic Workflow

```bash
# 1. Check available configs
$ kubec-cmd --list
config_production
config_staging
config_local

# 2. Switch to staging
$ kubec-cmd -t staging
Target found ➜ staging
File exist
Completed ✔

# 3. Verify with kubectl
$ kubectl config current-context
staging-cluster
```

### Example 2: Working with Multiple Clusters

```bash
# Morning: Work on staging
$ kubec-cmd -t staging
Completed ✔

$ kubectl get pods
NAME                    READY   STATUS    RESTARTS   AGE
app-staging-xxx         1/1     Running   0          1h

# Afternoon: Deploy to production
$ kubec-cmd -t production
Completed ✔

$ kubectl get pods
NAME                    READY   STATUS    RESTARTS   AGE
app-production-xxx      3/3     Running   0          24h
```

### Example 3: Local Development

```bash
# Switch to local minikube/kind cluster
$ kubec-cmd -t local
Completed ✔

# Start developing
$ kubectl apply -f deployment.yaml
deployment.apps/my-app created
```

---

## 🏗️ Project Structure

```
kubec-cmd-v2/
├── kubec-cmd/
│   ├── Program.cs           # Entry point
│   ├── ArgsController.cs    # CLI argument handling
│   ├── DirHelper.cs         # Help display
│   ├── FilesManager.cs      # File operations
│   └── ListFilesInPath.cs   # Config discovery
├── kubec-cmd.Tests/
│   ├── ArgsControllerTests.cs
│   └── DirHelperTests.cs
└── .github/workflows/
    ├── dotnet.yml           # CI/CD pipeline
    └── release.yml          # Multi-platform releases
```

---

## 🔧 How It Works

1. **Discovery**: Scans `~/.kube/` for files matching `config_*`
2. **Backup**: Creates a timestamped backup in `~/.kube/.bk/`
3. **Switch**: Copies the selected config to `~/.kube/config`
4. **Ready**: kubectl now uses the new configuration

```
~/.kube/
├── config              ← Active (copied from config_staging)
├── config_production
├── config_staging      ← Source
├── config_local
└── .bk/
    └── config_2024-01-15_10-30-00  ← Backup
```

---

## 🤝 Contributing

Contributions are welcome! Feel free to:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Eddy Wister**

- GitHub: [@eddyv73](https://github.com/eddyv73)

---

<div align="center">

Made with ❤️ for the Kubernetes community

⭐ Star this repo if you find it useful!

</div>
