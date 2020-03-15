#!/bin/bash
BUILD_ID="$1"

echo "Determining version"
YEAR="$(date +%Y)"
MONTH="$(date +%m)"
DAY="$(date +%d)"
VERSION="$YEAR.$MONTH.$DAY.$BUILD_ID"
echo "Version is $VERSION"
echo "$VERSION" > version.txt