#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
export TRAVIS_BUILD_NUMBER="123"
. "$DIR/build/build.sh"