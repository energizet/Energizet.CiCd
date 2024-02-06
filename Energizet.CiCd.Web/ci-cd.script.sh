#!/bin/sh
cd "$1" || exit
git pull
npm ci
npm run build
mv "$2"/wwwroot "$2"/wwwroot2
mv ./build "$2"/wwwroot
rm -rf "$2"/wwwroot2