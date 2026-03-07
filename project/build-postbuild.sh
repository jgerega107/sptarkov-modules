#!/usr/bin/env bash
# Post-build script called by SPT.Build.csproj after compilation.
# Copies built DLLs into ../Build with proper BepInEx layout.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

BUILD_DIR="../Build"
BEPINEX_PATCH_DIR="$BUILD_DIR/BepInEx/patchers"
BEPINEX_SPT_DIR="$BUILD_DIR/BepInEx/plugins/spt"
RELEASE_DIR="./SPT.Build/bin/Release/netstandard2.1"
LICENSE_FILE="../LICENSE.md"

rm -rf "$BUILD_DIR"
mkdir -p "$BEPINEX_PATCH_DIR" "$BEPINEX_SPT_DIR"

cp "$RELEASE_DIR/spt-prepatch.dll"     "$BEPINEX_PATCH_DIR/"
cp "$RELEASE_DIR/spt-common.dll"       "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-reflection.dll"   "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-core.dll"         "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-custom.dll"       "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-singleplayer.dll" "$BEPINEX_SPT_DIR/"
cp "$RELEASE_DIR/spt-debugging.dll"    "$BEPINEX_SPT_DIR/"

if [[ -f "$LICENSE_FILE" ]]; then
    cp "$LICENSE_FILE" "$BUILD_DIR/LICENSE-Modules.txt"
fi

echo "Post-build: DLLs copied to $BUILD_DIR"
