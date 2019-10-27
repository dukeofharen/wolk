#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
export TRAVIS_BUILD_NUMBER="123"
. "$DIR/set-vars.sh"
. "$DIR/create-folders.sh"
. "$DIR/build-ui.sh"
. "$DIR/build-server.sh"