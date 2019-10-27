#!/bin/bash

export DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
. "$DIR/set-vars.sh"

# Building user interface
echo "Building Vue.js user interface"
GUI_DIST_FOLDER="$GUI_SRC_FOLDER/dist"
GUI_DESTINATION_FOLDER="$SRC_FOLDER/Ducode.Wolk.Api/gui"

cd "$GUI_SRC_FOLDER"
npm install
if ! npm run build; then
    exit 1
fi

echo "Copying files from $GUI_DIST_FOLDER to $GUI_DESTINATION_FOLDER"
cp -a "$GUI_DIST_FOLDER/." "$GUI_DESTINATION_FOLDER"