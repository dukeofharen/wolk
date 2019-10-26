import sys
import os.path
from xml.etree import ElementTree as et

if len(sys.argv) != 3:
    raise Exception("Expected at least 2 args, {} given!".format(len(sys.argv) - 1))

version = sys.argv[1]
csprojPath = sys.argv[2]

if not os.path.isfile(csprojPath):
    raise Exception("File {} does not exist!".format(csprojPath))

tree = et.parse(csprojPath)
root = tree.getroot()
versionLeaf = root.find('PropertyGroup[1]/Version')
if versionLeaf != None:
    versionLeaf.text = version

tree.write(csprojPath)