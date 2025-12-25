#Requires -Version 5.1
<#
.SYNOPSIS
    kubec-cmd Installer for Windows

.DESCRIPTION
    Automatically downloads and installs kubec-cmd for Windows systems.

.EXAMPLE
    irm https://raw.githubusercontent.com/eddyv73/kubec-cmd-v2/main/install.ps1 | iex

.EXAMPLE
    .\install.ps1

.NOTES
    Author: Eddy Wister
    Repository: https://github.com/eddyv73/kubec-cmd-v2
#>

$ErrorActionPreference = "Stop"

# Configuration
$GitHubRepo = "eddyv73/kubec-cmd-v2"
$BinaryName = "kubec-cmd"
$InstallDir = "$env:LOCALAPPDATA\kubec-cmd"

# Colors
function Write-ColorOutput {
    param(
        [string]$Message,
        [string]$Color = "White"
    )
    Write-Host $Message -ForegroundColor $Color
}

# Print banner
function Show-Banner {
    $banner = @"

 __    __            __                                                 __
/  |  /  |          /  |                                               /  |
`$`$ | /`$`$/  __    __ `$`$ |____    ______    _______          _______  _____  ____    ____`$`$ |
`$`$ |/`$`$/  /  |  /  |`$`$      \  /      \  /       |______  /       |/     \/    \  /    `$`$ |
`$`$  `$`$<   `$`$ |  `$`$ |`$`$`$`$`$`$`$  |/`$`$`$`$`$`$  |/`$`$`$`$`$`$`$//      |/`$`$`$`$`$`$`$/ `$`$`$`$`$`$ `$`$`$`$  |/`$`$`$`$`$`$`$ |
`$`$`$`$`$  \  `$`$ |  `$`$ |`$`$ |  `$`$ |`$`$    `$`$ |`$`$ |     `$`$`$`$`$`$/ `$`$ |      `$`$ | `$`$ | `$`$ |`$`$ |  `$`$ |
`$`$ |`$`$  \ `$`$ \__`$`$ |`$`$ |__`$`$ |`$`$`$`$`$`$`$`$/ `$`$ \_____        `$`$ \_____ `$`$ | `$`$ | `$`$ |`$`$ \__`$`$ |
`$`$ | `$`$  |`$`$    `$`$/ `$`$    `$`$/  `$`$       |`$`$       |       `$`$       |`$`$ | `$`$ | `$`$ |`$`$    `$`$ |
`$`$/   `$`$/  `$`$`$`$`$`$/  `$`$`$`$`$`$`$/   `$`$`$`$`$`$`$/  `$`$`$`$`$`$`$/         `$`$`$`$`$`$`$/ `$`$/  `$`$/  `$`$/  `$`$`$`$`$`$`$/

"@
    Write-ColorOutput $banner "Cyan"
    Write-ColorOutput "Kubernetes Config Manager - Windows Installer" "Green"
    Write-Host ""
}

# Detect architecture
function Get-Architecture {
    $arch = [System.Environment]::GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
    switch ($arch) {
        "AMD64" { return "x64" }
        "ARM64" { return "arm64" }
        default { return "x64" }
    }
}

# Get latest release version
function Get-LatestVersion {
    try {
        $releases = Invoke-RestMethod -Uri "https://api.github.com/repos/$GitHubRepo/releases/latest"
        return $releases.tag_name
    }
    catch {
        Write-ColorOutput "Could not fetch latest version, using 'latest'" "Yellow"
        return "latest"
    }
}

# Download binary
function Get-Binary {
    param(
        [string]$Version,
        [string]$Architecture
    )

    $url = "https://github.com/$GitHubRepo/releases/download/$Version/$BinaryName-win-$Architecture.exe"

    Write-ColorOutput "Downloading from: $url" "Blue"

    $tempDir = New-Item -ItemType Directory -Path (Join-Path $env:TEMP "kubec-cmd-install") -Force
    $tempFile = Join-Path $tempDir.FullName "$BinaryName.exe"

    try {
        Invoke-WebRequest -Uri $url -OutFile $tempFile -UseBasicParsing
    }
    catch {
        Write-ColorOutput "Error downloading: $_" "Red"
        throw
    }

    return $tempFile
}

# Install binary
function Install-Binary {
    param(
        [string]$SourcePath,
        [string]$DestinationDir
    )

    # Create install directory if it doesn't exist
    if (-not (Test-Path $DestinationDir)) {
        New-Item -ItemType Directory -Path $DestinationDir -Force | Out-Null
    }

    $destinationPath = Join-Path $DestinationDir "$BinaryName.exe"

    # Copy file
    Copy-Item -Path $SourcePath -Destination $destinationPath -Force

    return $destinationPath
}

# Check if directory is in PATH
function Test-InPath {
    param([string]$Directory)

    $paths = $env:PATH -split ";"
    return $paths -contains $Directory
}

# Add to PATH
function Add-ToPath {
    param([string]$Directory)

    $currentPath = [Environment]::GetEnvironmentVariable("PATH", "User")

    if ($currentPath -notlike "*$Directory*") {
        $newPath = "$currentPath;$Directory"
        [Environment]::SetEnvironmentVariable("PATH", $newPath, "User")
        $env:PATH = "$env:PATH;$Directory"
        return $true
    }
    return $false
}

# Main installation
function Main {
    Show-Banner

    # Detect system
    Write-ColorOutput "Detecting system..." "Blue"
    $arch = Get-Architecture
    Write-ColorOutput "  OS:   Windows" "Green"
    Write-ColorOutput "  Arch: $arch" "Green"
    Write-Host ""

    # Get latest version
    Write-ColorOutput "Fetching latest version..." "Blue"
    $version = Get-LatestVersion
    Write-ColorOutput "  Version: $version" "Green"
    Write-Host ""

    # Download
    Write-ColorOutput "Downloading $BinaryName..." "Blue"
    $tempFile = Get-Binary -Version $version -Architecture $arch
    Write-ColorOutput "  Download complete" "Green"
    Write-Host ""

    # Install
    Write-ColorOutput "Installing to $InstallDir..." "Blue"
    $installedPath = Install-Binary -SourcePath $tempFile -DestinationDir $InstallDir
    Write-ColorOutput "  Installation complete" "Green"
    Write-Host ""

    # Cleanup temp files
    Remove-Item -Path (Split-Path $tempFile -Parent) -Recurse -Force -ErrorAction SilentlyContinue

    # Add to PATH if needed
    if (-not (Test-InPath $InstallDir)) {
        Write-ColorOutput "Adding to PATH..." "Blue"
        $added = Add-ToPath $InstallDir

        if ($added) {
            Write-Host ""
            Write-ColorOutput "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" "Yellow"
            Write-ColorOutput "NOTE: PATH has been updated for future sessions." "Yellow"
            Write-ColorOutput "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" "Yellow"
            Write-Host ""
            Write-ColorOutput "To use kubec-cmd in this session, run:" "White"
            Write-Host ""
            Write-ColorOutput "  `$env:PATH += `";$InstallDir`"" "Cyan"
            Write-Host ""
            Write-ColorOutput "Or simply open a new PowerShell/CMD window." "White"
            Write-Host ""
            Write-ColorOutput "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" "Yellow"
        }
    }
    else {
        Write-ColorOutput "  PATH already configured" "Green"
    }

    Write-Host ""
    Write-ColorOutput "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" "Green"
    Write-ColorOutput "  Installation complete! Enjoy kubec-cmd" "Green"
    Write-ColorOutput "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" "Green"
    Write-Host ""
    Write-ColorOutput "Installed to: $installedPath" "White"
    Write-ColorOutput "Run 'kubec-cmd --help' to get started." "White"
}

# Run
Main
