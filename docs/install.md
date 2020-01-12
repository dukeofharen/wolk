# Installation

Installation of Wolk is relatively straightforward. As of now, Wolk can be installed on Linux and Windows. It can be installed by deploying the binaries or by using a Docker container. A more in-depth explanation is given on this page.

[Click here](https://github.com/dukeofharen/wolk/releases/latest) to get the latest release for Windows or Linux.

For more information about the configuration of Wolk, [click here](configuration.md). In short; whenever Wolk is installed, a bunch of environment variables need to be set.

## Install on Windows

The recommended way of installing Wolk on Windows is by hosting it in IIS by using the [.NET Core Hosting Bundle](https://dotnet.microsoft.com/download/dotnet-core/current/runtime). A [Vagrant](https://www.vagrantup.com/) box which completely configures a Windows Server 2019 with IIS and Wolk can be found [here](https://github.com/dukeofharen/wolk/tree/master/samples/install/windows). In short, the following steps will be executed:

- Install IIS.
- Download and install .NET Core Hosting bundle.
- Download Wolk binary and extract it to disk.
- Make sure the IIS_IUSRS Windows user has read/write access to several important files and directories.
- Add the correct settings to the `web.config` file.
- Add the site to IIS.
- Restore a backup containg a few examples.

## Install on Linux

If you want to install Wolk on Linux, you can either use a pre-built [Docker image](https://hub.docker.com/r/dukeofharen/wolk) or you can download and install the Linux binaries. Two [Vagrant](https://www.vagrantup.com/) boxes have been scripted to show how hosting [with Docker](https://github.com/dukeofharen/wolk/tree/fmaster/samples/install/linux-with-docker) and [without Docker](https://github.com/dukeofharen/wolk/tree/master/samples/install/linux-without-docker) is done. In short, the following steps will be executed:

**With Docker**

- Install Nginx.
- Install Docker and Docker Compose.
- Add the correct parameters to the Docker Compose file and copy this file to `/opt/wolk`.
- Downloading the latest version of the Wolk container.
- Create a Systemd service file and copy it to `/etc/systemd/system/wolk.service`
- Copy file `wolk.conf` to `/etc/nginx/sites-available` and put a symlink in `/etc/nginx/sites-enabled`. Also remove the default site. Restart Nginx.
- Restore a backup containg a few examples.

**Without Docker**

- Install Nginx.
- Download the latest binaries of Wolk from GitHub.
- Extract the release of wolk to `/opt/wolk`.
- Create a Systemd service file and copy it to `/etc/systemd/system/wolk.service`
- Copy file `wolk.conf` to `/etc/nginx/sites-available` and put a symlink in `/etc/nginx/sites-enabled`. Also remove the default site. Restart Nginx.
- Restore a backup containg a few examples.

## General

When cloning this repository (and you installed Vagrant) go to any of the aforementioned Vagrant directories and run `vagrant up`. After the server has been configured correctly, you can go to `http://localhost:8080` and log in with `wolk@example.com` and `Password123!@`.