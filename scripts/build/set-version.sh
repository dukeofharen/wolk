#!/bin/bash
echo "Determining version"
YEAR="$(date +%Y)"
MONTH="$(date +%m)"
DAY="$(date +%d)"
VERSION="$YEAR.$MONTH.$DAY.0"
echo "Version is $VERSION"
echo "$VERSION" > version.txt