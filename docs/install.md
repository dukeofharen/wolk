# Installation

Installation of Wolk is relatively straightforward. As of now, Wolk can be installed on Linux and Windows. It can be installed by deploying the binaries or by using a Docker container. A more in-depth explanation is given on this page.

[Click here](https://github.com/dukeofharen/wolk/releases/latest) to get the latest release for Windows or Linux.

## Install on Windows

The recommended way of installing Wolk on Windows is by hosting it in IIS by using the [.NET Core Hosting Bundle](https://dotnet.microsoft.com/download/dotnet-core/current/runtime). A [Vagrant](https://www.vagrantup.com/) box which completely configures a Windows Server 2019 with IIS and Wolk can be found [here](https://github.com/dukeofharen/wolk/tree/master/samples/install/windows). In short, the following steps will be executed

- Install IIS.
- Download and install .NET Core Hosting bundle.
- Download Wolk binary and extract it to disk.
- Make sure the IIS_IUSRS Windows user has read/write access to several important files and directories.
- Add the correct settings to the `web.config` file (for more information about the configuration of Wolk, [click here](configuration.md)).
- Add the site to IIS.
- Restore a backup containg a few examples.

## Install on Linux (without Docker)

## Install on Linux (with Docker)

## General

When cloning this repository (and you installed Vagrant) go to any of the aforementioned Vagrant directories and run `vagrant up`. After the server has been configured correctly, you can go to `http://localhost:8080` and log in with `wolk@example.com` and `Password123!`.