#!/bin/bash

# Patch .csproj files with new version.
find $SRC_FOLDER -name "*.csproj" | while read FILENAME; do
    python $DIR/patch-csproj.py $VERSION $FILENAME
done