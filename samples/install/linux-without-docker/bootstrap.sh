#!/bin/bash

# General
DIR="/vagrant"
SAMPLES_ROOT_PATH="/wolk/samples"
BACKUP_PATH="$SAMPLES_ROOT_PATH/install/wolk-backup-test.zip"
ENV_PATH="$SAMPLES_ROOT_PATH/install/env.json"
ENV=$(cat $ENV_PATH)

# Install required binaries
echo "Installing required software"
apt update
apt install nginx -y
snap install jq

# Find and download latest version of Wolk
LATEST_RELEASE_URL="https://api.github.com/repos/dukeofharen/wolk/releases/latest"
RELEASE_JSON=$(wget -qO- $LATEST_RELEASE_URL)
VERSION=$(echo $RELEASE_JSON | jq -r .tag_name)
DOWNLOAD_URL=$(echo $RELEASE_JSON | eval jq "-r '.assets | map(select(.name==\"wolk_linux.tar.gz\")) | .[0].browser_download_url'")
DOWNLOAD_PATH="/tmp/wolk_$VERSION.tar.gz"
if [ -f "$DOWNLOAD_PATH" ]; then
    echo "$DOWNLOAD_PATH already exists; not downloading again."
else
    echo "Downloading Wolk from $DOWNLOAD_URL to $DOWNLOAD_PATH"
    wget $DOWNLOAD_URL -q -O $DOWNLOAD_PATH
fi

# Stop Wolk service
systemctl stop wolk

# Extract Wolk
INSTALL_PATH="/opt/wolk"
if [ -d "$INSTALL_PATH" ]; then
    echo "Deleting $INSTALL_PATH"
    rm -r "$INSTALL_PATH"
fi

echo "Creating $INSTALL_PATH"
mkdir "$INSTALL_PATH"

tar -xvzf "$DOWNLOAD_PATH" -C "$INSTALL_PATH"

# Creating data directories
WOLK_ROOT="/srv/wolk"
if [ ! -d "$WOLK_ROOT" ]; then
    echo "Creating $WOLK_ROOT"
    mkdir "$WOLK_ROOT"
fi

export WOLK_UPLOADS_ROOT="$WOLK_ROOT/uploads"
if [ ! -d "$WOLK_UPLOADS_ROOT" ]; then
    echo "Creating $WOLK_UPLOADS_ROOT"
    mkdir "$WOLK_UPLOADS_ROOT"
fi

# Install Wolk service
echo "Installing Wolk service"
export WOLKDIR="$INSTALL_PATH"
export DB_PATH="$WOLK_ROOT/wolk.db"
export JWT_SECRET=$(echo "$ENV" | jq -r .JwtSecret)
export JWT_EXPIRATION=$(echo "$ENV" | jq -r .ExpirationInSeconds)
export DEFAULT_LOGIN_EMAIL=$(echo "$ENV" | jq -r .DefaultLoginEmail)
export DEFAULT_PASSWORD=$(echo "$ENV" | jq -r .DefaultPassword)

envsubst < $DIR/wolk.service.template > /etc/systemd/system/wolk.service
systemctl enable wolk
systemctl daemon-reload
systemctl start wolk

# Install Wolk in Nginx
echo "Adding site for Wolk in Nginx"
LOCAL_NGINX_CONF_PATH="$DIR/wolk.conf"
NGINX_ROOT_PATH="/etc/nginx"
NGINX_AVAILABLE_PATH="$NGINX_ROOT_PATH/sites-available"
NGINX_ENABLED_PATH="$NGINX_ROOT_PATH/sites-enabled"

NGINX_DEFAULT_PATH="$NGINX_ENABLED_PATH/default"
if [ -f "$NGINX_DEFAULT_PATH" ]; then
    echo "Deleting $NGINX_DEFAULT_PATH"
    rm "$NGINX_DEFAULT_PATH"
fi

cp "$LOCAL_NGINX_CONF_PATH" "$NGINX_AVAILABLE_PATH"
ln -s "$NGINX_AVAILABLE_PATH/wolk.conf" "$NGINX_ENABLED_PATH/wolk.conf"
systemctl restart nginx
echo "Sleeping for 5 seconds; waiting for Wolk and Nginx to initialize"
sleep 5

# First, retrieve a JWT from Wolk
echo "Requesting Wolk JSON web token"
TOKEN_RESPONSE=$(curl -s \
    -d "{\"email\": \"$DEFAULT_LOGIN_EMAIL\", \"password\": \"$DEFAULT_PASSWORD\"}" \
    -H "Content-Type: application/json" \
    -X POST http://localhost/api/user/authenticate)
JWT=$(echo $TOKEN_RESPONSE | jq -r .token)

# Upload the backup to Wolk
echo "Restoring backup for testing purposes"
BASE64_BACKUP=$(cat $BACKUP_PATH | base64 -w0)
echo "{\"zipBytes\": \"$BASE64_BACKUP\"}" > backup-body.json
curl -s \
    -d @backup-body.json \
    -H "Content-Type: application/json" \
    -H "Authorization: Bearer $JWT" \
    -X POST http://localhost/api/backup