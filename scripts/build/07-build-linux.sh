#!/bin/bash
LIN_BIN_DIR="$SRC_FOLDER/Ducode.Wolk.Api/bin/release/netcoreapp3.0/linux-x64/publish"
TAR_LOCATION="$DIST_FOLDER/wolk_linux.tar.gz"

echo "Building Wolk for Linux"
if ! dotnet publish "$MAIN_CSPROJ_PATH" -c release --runtime=linux-x64; then
    exit 1
fi

echo "Archiving binaries for Linux"
cd "$LIN_BIN_DIR"
tar -czvf "$TAR_LOCATION" -C "$LIN_BIN_DIR" .