#!/bin/bash
export DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

. "$DIR/01-set-vars.sh"
. "$DIR/02-create-folders.sh"
. "$DIR/03-patch-csproj-versions.sh"
. "$DIR/04-run-tests.sh"
. "$DIR/05-build-gui.sh"
. "$DIR/06-build-windows.sh"
. "$DIR/07-build-linux.sh"