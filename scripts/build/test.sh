#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
ROOT_FOLDER="$DIR/../.."
SRC_FOLDER="$ROOT_FOLDER/src"
cd ${SRC_FOLDER}
dotnet test