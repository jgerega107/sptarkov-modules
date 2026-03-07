#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

VERSION="${1:-4.0.13}"
BUILD_DIR="../Build"
BEPINEX_PATCH_DIR="$BUILD_DIR/BepInEx/patchers"
BEPINEX_SPT_DIR="$BUILD_DIR/BepInEx/plugins/spt"
RELEASE_DIR="./SPT.Build/bin/Release/netstandard2.1"
LICENSE_FILE="../LICENSE.md"
TIMESTAMP="$(date +%Y%m%d-%H%M%S)"
ZIP_FILE="../spt-modules-${VERSION}-${TIMESTAMP}.zip"

echo "=== Building SPT Modules v${VERSION} ==="

# Build all projects via SPT.Build (which references everything)
# Skip the csproj post-build since this script handles the layout itself
dotnet build -c Release "-p:BepInExPluginVersion=${VERSION}" -p:SkipPostBuild=true

# Clean and create output directories
rm -rf "$BUILD_DIR"
mkdir -p "$BEPINEX_PATCH_DIR" "$BEPINEX_SPT_DIR"

# Copy DLLs to proper BepInEx layout
cp "$RELEASE_DIR/spt-prepatch.dll"     "$BEPINEX_PATCH_DIR/"
cp "$RELEASE_DIR/spt-common.dll"       "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-reflection.dll"   "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-core.dll"         "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-custom.dll"       "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-singleplayer.dll" "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-debugging.dll"    "$BEPINEX_SPT_DIR/"

# Copy license
if [[ -f "$LICENSE_FILE" ]]; then
    cp "$LICENSE_FILE" "$BUILD_DIR/LICENSE-Modules.txt"
fi

# Create ZIP
rm -f "$ZIP_FILE"
(cd "$BUILD_DIR" && zip -r "$SCRIPT_DIR/$ZIP_FILE" .)

echo ""
echo "=== Build complete ==="
echo "Output: $ZIP_FILE"
unzip -l "$ZIP_FILE"
