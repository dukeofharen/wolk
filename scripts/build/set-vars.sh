#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
. "$DIR/set-global-vars.sh"

echo "Determining version"
YEAR="$(date +%Y)"
MONTH="$(date +%m)"
DAY="$(date +%d)"
export VERSION="$YEAR.$MONTH.$DAY.$TRAVIS_BUILD_NUMBER"
echo "Version is $VERSION"
echo "$VERSION" > "$ROOT_FOLDER/version.txt"