#!/bin/bash
WIN_BIN_DIR="$SRC_FOLDER/Ducode.Wolk.Api/bin/release/netcoreapp3.0/win-x64/publish"
ZIP_LOCATION="$DIST_FOLDER/wolk_windows.zip"

echo "Building Wolk for Windows"
if ! dotnet publish "$MAIN_CSPROJ_PATH" -c release --runtime=win-x64; then
    exit 1
fi

echo "Zipping binaries for Windows"
cd "$WIN_BIN_DIR"
zip -r "$ZIP_LOCATION" .