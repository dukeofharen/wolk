#!/bin/bash
# Building user interface
echo "Building Vue.js user interface"
cd ui
npm install
if ! npm run build; then
    exit 1
fi