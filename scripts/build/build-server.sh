#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
. "$DIR/set-global-vars.sh"
VERSION="$($ROOT_FOLDER/version.txt")"

# Patch .csproj files with new version.
find $SRC_FOLDER -name "*.csproj" | while read FILENAME; do
    python $DIR/patch-csproj.py $VERSION $FILENAME
done

# Run tests
find "$SRC_FOLDER" -name "*.Tests.csproj" | while read FILENAME; do
    echo "Running unit tests in project $FILENAME"
    if ! dotnet test "$FILENAME"; then
        echo "Error when executing unit tests for $FILENAME"
        exit 1
    fi
done

# Build for Windows
WIN_BIN_DIR="$SRC_FOLDER/Ducode.Wolk.Api/bin/release/netcoreapp3.0/win-x64/publish"
ZIP_LOCATION="$DIST_FOLDER/wolk_windows.zip"

echo "Building Wolk for Windows"
if ! dotnet publish "$MAIN_CSPROJ_PATH" -c release --runtime=win-x64; then
    exit 1
fi

echo "Zipping binaries for Windows"
cd "$WIN_BIN_DIR"
zip -r "$ZIP_LOCATION" .

# Build for Linux
LIN_BIN_DIR="$SRC_FOLDER/Ducode.Wolk.Api/bin/release/netcoreapp3.0/linux-x64/publish"
TAR_LOCATION="$DIST_FOLDER/wolk_linux.tar.gz"

echo "Building Wolk for Linux"
if ! dotnet publish "$MAIN_CSPROJ_PATH" -c release --runtime=linux-x64; then
    exit 1
fi

echo "Archiving binaries for Linux"
cd "$LIN_BIN_DIR"
tar -czvf "$TAR_LOCATION" -C "$LIN_BIN_DIR" .