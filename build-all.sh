#!/bin/bash
# build-all.sh - Compila y publica binarios .NET para todas las arquitecturas principales
# Uso: ./build-all.sh

set -e

PROJECT_PATH="kubec-cmd/kubec-cmd.csproj"
OUTPUT_DIR="publish"
CONFIGURATION="Release"

# Plataformas objetivo
RIDS=(
  "linux-x64"
  "linux-arm64"
  "win-x64"
  "osx-x64"
  "osx-arm64"
)

# Limpiar carpeta de salida
rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

echo "Compilando $PROJECT_PATH para todas las arquitecturas..."

for RID in "${RIDS[@]}"; do
  echo -e "\nPublicando para $RID..."
  dotnet publish "$PROJECT_PATH" \
    -c "$CONFIGURATION" \
    -r "$RID" \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -o "$OUTPUT_DIR/$RID" || { echo "Error al publicar para $RID"; exit 1; }
done

echo -e "\nBinarios single file generados en $OUTPUT_DIR/{linux-x64,linux-arm64,win-x64,osx-x64,osx-arm64}"
