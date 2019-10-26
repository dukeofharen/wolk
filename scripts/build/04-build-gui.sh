#!/bin/bash
GUI_DIST_FOLDER="$GUI_SRC_FOLDER/dist"
GUI_DESTINATION_FOLDER="$SRC_FOLDER/Ducode.Wolk.Api"

cd "$GUI_SRC_FOLDER"
npm install
if ! npm build; then
    exit 1
fi
