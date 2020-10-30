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

### YourOsGL
If NetGL is not supported for your OS, but .Net is, the only thing you need to do to add support for your OS is to make c++ wrapper for your OS (you need to provide the standard wrapper API).

### NetGL (C#)
The universal graphics library for .net core.
Includes all functionality from OpenGL, as well as some more, added from me.
Use this library freely in any project that you want.

## This project wouldn't be possible without:
1) GLFW - A beautiful wrapper for 
2) GLEW - OpenGL extension wrangler

## The star of the show - *Minecraft.net*
