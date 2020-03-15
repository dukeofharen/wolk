#!/bin/bash
# Building user interface
echo "Building Vue.js user interface"
GUI_DIST_FOLDER="ui/dist"
GUI_DESTINATION_FOLDER="Ducode.Wolk.Api/gui"

cd ui
npm install
if ! npm run build; then
    exit 1
fi

echo "Copying files from $GUI_DIST_FOLDER to $GUI_DESTINATION_FOLDER"
cp -a "$GUI_DIST_FOLDER/." "$GUI_DESTINATION_FOLDER"