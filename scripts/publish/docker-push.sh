#!/bin/bash
if [ "$TRAVIS_BRANCH" = "master" ]; then
    DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
    ROOT_FOLDER="$DIR/../.."
    VERSION="$(cat $ROOT_FOLDER/version.txt)"
    REPO_NAME="dukeofharen/wolk"
    
    echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin
    docker build -t $REPO_NAME:$VERSION .
    docker tag $REPO_NAME:$VERSION $REPO_NAME:latest
    docker push $REPO_NAME:$VERSION
    docker push $REPO_NAME:latest
fi