#!/bin/bash
git config --global user.email "git@ducode.org"
git config --global user.name "Ducode - Deployment"
VERSION=$(cat ./version.txt)
GIT_TAG="v$VERSION"
git tag "$GIT_TAG" -a -m "Generated tag from TravisCI for build $TRAVIS_BUILD_NUMBER"