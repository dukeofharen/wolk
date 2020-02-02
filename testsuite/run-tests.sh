#!/bin/bash
REBUILD="$1"

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
COMPOSE_TEMPLATE_PATH="$DIR/../docker-compose.yml.template"
COMPOSE_PATH="/tmp/wolk-compose.yml"
export DOCKERFILE="$DIR/.."

export WOLK_PATH="/tmp/$(uuidgen)"
mkdir "$WOLK_PATH"
mkdir "$WOLK_PATH/uploads"
echo "Wolk path: $WOLK_PATH"
envsubst < "$COMPOSE_TEMPLATE_PATH" > "$COMPOSE_PATH"

if [[ "$REBUILD" = "1" ]]; then
    sudo docker-compose -f ${COMPOSE_PATH} up -d --build
else
    sudo docker-compose -f ${COMPOSE_PATH} up -d
fi