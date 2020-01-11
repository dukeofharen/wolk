#!/bin/bash
echo "Installing Docker Compose"
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

if ! [ -x "$(command -v docker-compose)" ]; then
    echo "Docker Compose not installed; installing now."
    curl -s -L "https://github.com/docker/compose/releases/download/1.25.1/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
    chmod +x /usr/local/bin/docker-compose
else
    echo "Docker Compose already installed."
fi