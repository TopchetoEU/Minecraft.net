#pragma once

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files

typedef void* object;
typedef unsigned char byte;
typedef unsigned char ubyte;
typedef unsigned short ushort;
typedef unsigned int uint;
typedef unsigned long ulong;
typedef char* nativeString;

#define windowsHandler __stdcall;

const char* uintToString(uint num, int radix, const char* digits);