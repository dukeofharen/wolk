#!/bin/bash
if [ -d "$DIST_FOLDER" ]; then
    echo "Removing folder $DIST_FOLDER"
    rm -r "$DIST_FOLDER"
fi

echo "Creating folder $DIST_FOLDER"
mkdir "$DIST_FOLDER"