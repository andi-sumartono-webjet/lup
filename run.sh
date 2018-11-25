#!/usr/bin/env bash
# AUTHOR: Jeremy Cade <me@jeremycade.com>
# CREATED: 2017-01-09
# DESCRIPTION: Helper script to run debugging environment

# Get OS Name
OS=`uname -s`

# Set Development Environment Variable.
export ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_URLS=http://*:3254

# Bug with OSX 10.12 runtimes
if [ "$OS" = Darwin ]; then
    export DOTNET_RUNTIME_ID=osx.10.11-x64
fi

# Delete .IncrementalCache..
find . -name ".IncrementalCache" -delete

# CWD, Run bundle command and run watch
dotnet restore
cd src/Lup.Software.Engineering
dotnet watch run