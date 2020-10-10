#pragma once

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files

typedef unsigned int uint;
typedef void* object;
typedef unsigned char byte;
typedef char* nativeString;

#define windowsHandler __stdcall;

#include <windows.h>
#include <iostream>
#include "errors.h"

using namespace std;

string intToString(uint value, int radix, const char* alphabet);