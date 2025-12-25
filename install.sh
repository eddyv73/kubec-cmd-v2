#!/bin/bash
#
# kubec-cmd Installer
# Automatically downloads and installs kubec-cmd for your system
#
# Usage:
#   curl -fsSL https://raw.githubusercontent.com/eddyv73/kubec-cmd-v2/main/install.sh | bash
#
# Or:
#   wget -qO- https://raw.githubusercontent.com/eddyv73/kubec-cmd-v2/main/install.sh | bash
#

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Configuration
GITHUB_REPO="eddyv73/kubec-cmd-v2"
BINARY_NAME="kubec-cmd"
INSTALL_DIR="/usr/local/bin"

# Print banner
print_banner() {
    echo -e "${CYAN}"
    cat << 'EOF'
 __    __            __                                                 __
/  |  /  |          /  |                                               /  |
$$ | /$$/  __    __ $$ |____    ______    _______          _______  _____  ____    ____$$ |
$$ |/$$/  /  |  /  |$$      \  /      \  /       |______  /       |/     \/    \  /    $$ |
$$  $$<   $$ |  $$ |$$$$$$$  |/$$$$$$  |/$$$$$$$//      |/$$$$$$$/ $$$$$$ $$$$  |/$$$$$$$ |
$$$$$  \  $$ |  $$ |$$ |  $$ |$$    $$ |$$ |     $$$$$$/ $$ |      $$ | $$ | $$ |$$ |  $$ |
$$ |$$  \ $$ \__$$ |$$ |__$$ |$$$$$$$$/ $$ \_____        $$ \_____ $$ | $$ | $$ |$$ \__$$ |
$$ | $$  |$$    $$/ $$    $$/ $$       |$$       |       $$       |$$ | $$ | $$ |$$    $$ |
$$/   $$/  $$$$$$/  $$$$$$$/   $$$$$$$/  $$$$$$$/         $$$$$$$/ $$/  $$/  $$/  $$$$$$$/
EOF
    echo -e "${NC}"
    echo -e "${GREEN}Kubernetes Config Manager - Installer${NC}"
    echo ""
}

# Detect OS
detect_os() {
    OS="$(uname -s)"
    case "${OS}" in
        Linux*)     OS_TYPE="linux";;
        Darwin*)    OS_TYPE="osx";;
        CYGWIN*|MINGW*|MSYS*) OS_TYPE="windows";;
        *)          OS_TYPE="unknown";;
    esac
    echo "${OS_TYPE}"
}

# Detect Architecture
detect_arch() {
    ARCH="$(uname -m)"
    case "${ARCH}" in
        x86_64|amd64)   ARCH_TYPE="x64";;
        arm64|aarch64)  ARCH_TYPE="arm64";;
        *)              ARCH_TYPE="unknown";;
    esac
    echo "${ARCH_TYPE}"
}

# Get latest release version
get_latest_version() {
    curl -fsSL "https://api.github.com/repos/${GITHUB_REPO}/releases/latest" | \
        grep '"tag_name":' | \
        sed -E 's/.*"([^"]+)".*/\1/'
}

# Download binary
download_binary() {
    local os=$1
    local arch=$2
    local version=$3
    local url="https://github.com/${GITHUB_REPO}/releases/download/${version}/${BINARY_NAME}-${os}-${arch}"

    if [ "${os}" = "windows" ]; then
        url="${url}.exe"
    fi

    echo -e "${BLUE}Downloading from: ${url}${NC}" >&2

    # Create temp directory
    TMP_DIR=$(mktemp -d)
    TMP_FILE="${TMP_DIR}/${BINARY_NAME}"

    if command -v curl &> /dev/null; then
        curl -fsSL "${url}" -o "${TMP_FILE}"
    elif command -v wget &> /dev/null; then
        wget -q "${url}" -O "${TMP_FILE}"
    else
        echo -e "${RED}Error: Neither curl nor wget found. Please install one of them.${NC}"
        exit 1
    fi

    echo "${TMP_FILE}"
}

# Install binary
install_binary() {
    local tmp_file=$1
    local install_dir=$2
    local binary_name=$3

    # Make executable
    chmod +x "${tmp_file}"

    # Create install directory if it doesn't exist
    if [ ! -d "${install_dir}" ]; then
        echo -e "${YELLOW}Creating ${install_dir}...${NC}"
        if [ -w "$(dirname "${install_dir}")" ]; then
            mkdir -p "${install_dir}"
        else
            sudo mkdir -p "${install_dir}"
        fi
    fi

    # Check if we need sudo
    if [ -w "${install_dir}" ]; then
        mv "${tmp_file}" "${install_dir}/${binary_name}"
    else
        echo -e "${YELLOW}Need sudo to install to ${install_dir}${NC}"
        sudo mv "${tmp_file}" "${install_dir}/${binary_name}"
    fi

    # Remove quarantine on macOS
    if [ "$(detect_os)" = "osx" ]; then
        xattr -d com.apple.quarantine "${install_dir}/${binary_name}" 2>/dev/null || true
    fi
}

# Check if directory is in PATH
check_path() {
    local dir=$1
    if [[ ":$PATH:" == *":${dir}:"* ]]; then
        return 0
    else
        return 1
    fi
}

# Suggest PATH addition
suggest_path_addition() {
    local install_dir=$1
    local shell_name=$(basename "$SHELL")
    local rc_file=""

    case "${shell_name}" in
        bash)
            if [ -f "$HOME/.bashrc" ]; then
                rc_file="$HOME/.bashrc"
            elif [ -f "$HOME/.bash_profile" ]; then
                rc_file="$HOME/.bash_profile"
            fi
            ;;
        zsh)
            rc_file="$HOME/.zshrc"
            ;;
        fish)
            rc_file="$HOME/.config/fish/config.fish"
            ;;
        *)
            rc_file="$HOME/.profile"
            ;;
    esac

    echo ""
    echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${YELLOW}NOTE: ${install_dir} is not in your PATH${NC}"
    echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo ""
    echo -e "To add it, run one of these commands:"
    echo ""

    if [ "${shell_name}" = "fish" ]; then
        echo -e "${CYAN}  fish_add_path ${install_dir}${NC}"
    else
        echo -e "${CYAN}  echo 'export PATH=\"${install_dir}:\$PATH\"' >> ${rc_file}${NC}"
    fi

    echo ""
    echo -e "Then reload your shell:"
    echo ""

    case "${shell_name}" in
        bash)
            echo -e "${CYAN}  source ${rc_file}${NC}"
            ;;
        zsh)
            echo -e "${CYAN}  source ${rc_file}${NC}"
            ;;
        fish)
            echo -e "${CYAN}  source ${rc_file}${NC}"
            ;;
        *)
            echo -e "${CYAN}  source ${rc_file}${NC}"
            ;;
    esac

    echo ""
    echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
}

# Main installation
main() {
    print_banner

    # Detect system
    echo -e "${BLUE}Detecting system...${NC}"
    OS=$(detect_os)
    ARCH=$(detect_arch)

    echo -e "  OS:   ${GREEN}${OS}${NC}"
    echo -e "  Arch: ${GREEN}${ARCH}${NC}"
    echo ""

    # Validate detection
    if [ "${OS}" = "unknown" ]; then
        echo -e "${RED}Error: Unsupported operating system${NC}"
        exit 1
    fi

    if [ "${ARCH}" = "unknown" ]; then
        echo -e "${RED}Error: Unsupported architecture${NC}"
        exit 1
    fi

    if [ "${OS}" = "windows" ]; then
        echo -e "${YELLOW}For Windows, please use the PowerShell installer:${NC}"
        echo -e "${CYAN}  irm https://raw.githubusercontent.com/${GITHUB_REPO}/main/install.ps1 | iex${NC}"
        exit 0
    fi

    # Get latest version
    echo -e "${BLUE}Fetching latest version...${NC}"
    VERSION=$(get_latest_version)

    if [ -z "${VERSION}" ]; then
        echo -e "${YELLOW}Could not detect latest version, using 'latest'${NC}"
        VERSION="latest"
    else
        echo -e "  Version: ${GREEN}${VERSION}${NC}"
    fi
    echo ""

    # Download
    echo -e "${BLUE}Downloading ${BINARY_NAME}...${NC}"
    TMP_FILE=$(download_binary "${OS}" "${ARCH}" "${VERSION}")
    echo -e "  ${GREEN}Download complete${NC}"
    echo ""

    # Install
    echo -e "${BLUE}Installing to ${INSTALL_DIR}...${NC}"
    install_binary "${TMP_FILE}" "${INSTALL_DIR}" "${BINARY_NAME}"
    echo -e "  ${GREEN}Installation complete${NC}"
    echo ""

    # Cleanup
    rm -rf "$(dirname "${TMP_FILE}")"

    # Verify installation
    if command -v ${BINARY_NAME} &> /dev/null; then
        echo -e "${GREEN}✓ ${BINARY_NAME} installed successfully!${NC}"
        echo ""
        echo -e "Run ${CYAN}${BINARY_NAME} --help${NC} to get started."
    else
        # Check if install dir is in PATH
        if ! check_path "${INSTALL_DIR}"; then
            suggest_path_addition "${INSTALL_DIR}"
        else
            echo -e "${GREEN}✓ ${BINARY_NAME} installed to ${INSTALL_DIR}/${BINARY_NAME}${NC}"
        fi
    fi

    echo ""
    echo -e "${GREEN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${GREEN}  Installation complete! Enjoy kubec-cmd ⚓${NC}"
    echo -e "${GREEN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
}

# Run main
main
