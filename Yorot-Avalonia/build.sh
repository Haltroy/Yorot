#!/bin/bash

####################################
##                                 #
##  YOROT AVALONIA PUBLISH SCRIPT  #
## ------------------------------- #
## Use this script to publish to   #
## all supported operating systems #
## at once.			   #
##				   #
####################################

## These do support ReadyToRun, so we seperate them
declare -a rtrarray=("linux-x64" 
"linux-x86" 
"linux-arm32" 
"linux-arm64" 
"win-arm64"
"win-x64"
"win-arm32"
"win-arm64"
"win-x86" 
"osx-x64")

for i in "${rtrarray[@]}"
do
	dotnet publish --verbosity d --force --nologo --configuration Release --framework netcoreapp3.1 --runtime ${rtrarray[$i-1]} -p:PublishSingleFile=true -p:PublishReadyToRun=true --self-contained true
   echo 
done

## Do the rest of them
declare -a osarray=("debian-x64"
"debian-arm"
"debian-arm64"
"debian-x86"
"arch-x64"
"alpine-arm"
"alpine-arm64"
"alpine-x64"
"centos-arm64"
"centos-x64"
"fedora-arm64"
"fedora-x64"
"freebsd-x64"
"gentoo-x64"
"opensuse-x64"
"osx-x64"
"rhel-x64"
"rhel-arm64"
"sles-x64"
"ubuntu-arm"
"ubuntu-arm64"
"ubuntu-x64"
"ubuntu-x86"
"illumos-x64"
"openindiana-x64"
"osx-arm64"
"smartos-x64"
"solaris-x64"
"unix-arm"
"unix-arm64"
"unix-x64"
"unix-x86")

for i in "${osarray[@]}"
do
	dotnet publish --verbosity d --force --nologo --configuration Release --framework netcoreapp3.1 --runtime ${osarray[$i-1]} -p:PublishSingleFile=true --self-contained true
   echo 
done

