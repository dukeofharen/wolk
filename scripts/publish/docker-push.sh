#!/bin/bash
VERSION="$(cat version.txt)"
REPO_NAME="dukeofharen/wolk"

echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin
docker build -t $REPO_NAME:$VERSION .
docker tag $REPO_NAME:$VERSION $REPO_NAME:latest
docker push $REPO_NAME:$VERSION
docker push $REPO_NAME:latest