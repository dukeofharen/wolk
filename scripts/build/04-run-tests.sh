#!/bin/bash
find "$SRC_FOLDER" -name "*.Tests.csproj" | while read FILENAME; do
    echo "Running unit tests in project $FILENAME"
    if ! dotnet test "$FILENAME"; then
        echo "Error when executing unit tests for $FILENAME"
        exit 1
    fi
done