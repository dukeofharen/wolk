#!/bin/bash
# Script for dealing with NTFS / FAT partitions.
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
ORIGINAL_PATH="$DIR/.vagrant/machines/default/virtualbox/private_key"
NEW_PATH="$HOME/.ssh/linux-without-docker"
mkdir $NEW_PATH
mv $ORIGINAL_PATH $NEW_PATH
chmod 600 $NEW_PATH/private_key
ln -s $NEW_PATH/private_key $ORIGINAL_PATH