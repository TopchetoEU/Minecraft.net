# Minecraft .net edition

## In general
This is a indie project - clone of Minecraft - Java edition, which has as an aim to fix some preformance issues with Minecraft and best of all - making a modding api for all Minecraft modders out there.
As we all know Java is kinda daty, so I tought it would be a great to develop a MC clone on .Net Core (yes, core)
The project is absolutely open source, so anyone can contribute, or take it and modify it.

## Sublibraries

### WinGL (c++)
The bridge between GLUT and OpenGL and .net core

### LinGL (Coming soon, c++)
The bridge between Linux's graphic library and .net core

### MacGL (Coming in distant future, c++ i hope)
The bridge between Mac's graphic library and .net core

### The beauty of this system
Say for example, you know how to use some unknown Linux distribution's graphics library and .Net supports is. You want to use all of your stuff from your OS, so you come here, write c++ wrapper for this OS, and compile your binaries. Then, you push your changes to this repository and I will probably merge it. After that, you can use all of the apps written on this platform, on your OS.
This allows for an easily growing cross-platform library.

### NetGL (C#)
The universal graphics library for .net core.
Includes all functionality from OpenGL, as well as some more, added from me.
Use this library freely in any project that you want.

## This project wouldn't be possible without:
1) freeglut - a free alternative to glut
2) GLEW - OpenGL extension wrangler

## The star of the show - *Minecraft.net*
