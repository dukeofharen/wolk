#!/bin/bash
export DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
. "$DIR/01-set-vars.sh"
. "$DIR/02-patch-csproj-versions.sh"
. "$DIR/03-run-tests.sh"
. "$DIR/04-build-gui.sh"