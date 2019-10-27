#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
export ROOT_FOLDER="$DIR/../.."
export DIST_FOLDER="$ROOT_FOLDER/dist"
export SRC_FOLDER="$ROOT_FOLDER/src"
export MAIN_CSPROJ_PATH="$SRC_FOLDER/Ducode.Wolk.Api/Ducode.Wolk.Api.csproj"
export GUI_SRC_FOLDER="$ROOT_FOLDER/ui"