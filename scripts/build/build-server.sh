#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
ROOT_PATH="$DIR/../.."
VERSION=$(cat version.txt)
API_ROOT_PATH="$ROOT_PATH/src/Ducode.Wolk.Api"

mkdir "$ROOT_PATH/dist"

# Copy UI to project
mkdir "$API_ROOT_PATH/gui"
cp "$ROOT_PATH/ui/dist/*" "$API_ROOT_PATH/gui"

# Patch .csproj files with new version.
find src -name "*.csproj" | while read FILENAME; do
    python scripts/build/patch-csproj.py $VERSION $FILENAME
done

# Build for Windows
WIN_BIN_DIR="$ROOT_PATH/src/Ducode.Wolk.Api/bin/release/netcoreapp3.1/win-x64/publish"
TAR_LOCATION="$ROOT_PATH/dist/wolk_windows.tar.gz"

echo "Building Wolk for Windows"
if ! dotnet publish "$API_ROOT_PATH/Ducode.Wolk.Api.csproj" -c release --runtime=win-x64; then
    exit 1
fi

echo "Zipping binaries for Windows"
cd "$WIN_BIN_DIR"
tar -czvf "$TAR_LOCATION" -C "$WIN_BIN_DIR" .
cd "$ROOT_PATH"

# Build for Linux
LIN_BIN_DIR="$ROOT_PATH/src/Ducode.Wolk.Api/bin/release/netcoreapp3.1/linux-x64/publish"
TAR_LOCATION="$ROOT_PATH/dist/wolk_linux.tar.gz"

echo "Building Wolk for Linux"
if ! dotnet publish "$API_ROOT_PATH/Ducode.Wolk.Api.csproj" -c release --runtime=linux-x64; then
    exit 1
fi

echo "Archiving binaries for Linux"
cd "$LIN_BIN_DIR"
tar -czvf "$TAR_LOCATION" -C "$LIN_BIN_DIR" .
cd "$ROOT_PATH"

# Creating Swagger file
SWAGGERGEN_ROOT_PATH="$ROOT_PATH/src/Ducode.Wolk.SwaggerGenerator"
SWAGGER_PATH="$SWAGGERGEN_ROOT_PATH/bin/Release/netcoreapp3.1/swagger.json"
echo "Creating swagger.json file"
if ! dotnet run -c Release --project "$SWAGGERGEN_ROOT_PATH"; then
    exit 1
fi

echo "swagger.json file created successfully!"
cp "$SWAGGER_PATH" "$ROOT_PATH/dist"