# Building Ghost Scanner (For Developers)

This file is for developers who want to build Ghost Scanner from source.

## Prerequisites

- **Visual Studio 2022** (Community Edition - free)
- **.NET 6.0 SDK** (included with Visual Studio)

## Build Steps

1. Open `GhostScanner.sln` in Visual Studio 2022
2. Select **Release** configuration
3. Press **F6** to build (or Build → Build Solution)
4. The built files will be in: `GhostScanner\bin\Release\net6.0-windows\`

## Creating the Release Package

After building, create a `Release` folder in the repository root and copy:

```
Release/
├── GhostScanner.exe
├── *.dll (all DLL files)
├── *.runtimeconfig.json
└── *.pdb (optional, for debugging)
```

The installer (`INSTALL.bat`) will automatically find and use files from the `Release` folder.

## Publishing a Self-Contained Build

For a single-file executable (optional):

```bash
dotnet publish GhostScanner/GhostScanner.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

This creates a single `.exe` file that includes everything needed.

---

**Note**: End users don't need to build anything - they just run `INSTALL.bat`!
