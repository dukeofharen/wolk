#!/bin/bash
export SRC_FOLDER="$DIR/../../src"
export GUI_SRC_FOLDER="$DIR/../../ui"

echo "Determining version"
YEAR="$(date +%Y)"
MONTH="$(date +%m)"
DAY="$(date +%d)"
export VERSION="$YEAR.$MONTH.$DAY.$TRAVIS_BUILD_NUMBER"