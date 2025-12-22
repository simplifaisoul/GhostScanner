@echo off
REM Ghost Scanner - Super Simple Installer
REM Made by Soul and Lapex
REM Just double-click this file to install!

echo.
echo ========================================
echo   👻 Ghost Scanner - Installer
echo   Made by Soul and Lapex
echo ========================================
echo.

REM Check for admin
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ⚠️  This installer needs administrator rights.
    echo.
    echo Please right-click this file and select:
    echo    "Run as administrator"
    echo.
    pause
    exit /b 1
)

set "INSTALL_DIR=%ProgramFiles%\GhostScanner"
set "EXE_PATH=%INSTALL_DIR%\GhostScanner.exe"

echo 📦 Installing Ghost Scanner...
echo    Location: %INSTALL_DIR%
echo.

REM Create directory
if not exist "%INSTALL_DIR%" (
    mkdir "%INSTALL_DIR%"
    echo ✓ Created installation folder
) else (
    echo ✓ Installation folder exists
)

REM Find GhostScanner.exe - check multiple locations
set "SOURCE_EXE="
set "SOURCE_DIR="

if exist "Release\GhostScanner.exe" (
    set "SOURCE_EXE=Release\GhostScanner.exe"
    set "SOURCE_DIR=Release"
    echo ✓ Found GhostScanner.exe in Release folder
) else if exist "GhostScanner.exe" (
    set "SOURCE_EXE=GhostScanner.exe"
    set "SOURCE_DIR=."
    echo ✓ Found GhostScanner.exe in current folder
) else if exist "GhostScanner\bin\Release\net6.0-windows\GhostScanner.exe" (
    set "SOURCE_EXE=GhostScanner\bin\Release\net6.0-windows\GhostScanner.exe"
    set "SOURCE_DIR=GhostScanner\bin\Release\net6.0-windows"
    echo ✓ Found GhostScanner.exe in build folder
) else (
    echo.
    echo ❌ ERROR: GhostScanner.exe not found!
    echo.
    echo The installer is looking for GhostScanner.exe in:
    echo   - Release\GhostScanner.exe
    echo   - GhostScanner.exe (current folder)
    echo   - GhostScanner\bin\Release\net6.0-windows\GhostScanner.exe
    echo.
    echo Please make sure you downloaded the complete package.
    echo.
    pause
    exit /b 1
)

echo.
echo 📋 Copying files...

REM Copy the executable
copy /Y "%SOURCE_EXE%" "%INSTALL_DIR%\" >nul
if %errorLevel% neq 0 (
    echo ❌ Failed to copy GhostScanner.exe
    pause
    exit /b 1
)
echo ✓ Copied GhostScanner.exe

REM Copy all DLLs from the source directory
if exist "%SOURCE_DIR%\*.dll" (
    copy /Y "%SOURCE_DIR%\*.dll" "%INSTALL_DIR%\" >nul 2>&1
    echo ✓ Copied required files
)

REM Copy runtimeconfig.json if it exists
if exist "%SOURCE_DIR%\*.runtimeconfig.json" (
    copy /Y "%SOURCE_DIR%\*.runtimeconfig.json" "%INSTALL_DIR%\" >nul 2>&1
)

REM Copy .pdb files if they exist (for debugging)
if exist "%SOURCE_DIR%\*.pdb" (
    copy /Y "%SOURCE_DIR%\*.pdb" "%INSTALL_DIR%\" >nul 2>&1
)

if not exist "%EXE_PATH%" (
    echo.
    echo ❌ ERROR: Installation failed!
    echo GhostScanner.exe was not copied successfully.
    echo.
    pause
    exit /b 1
)

echo.
echo 🔗 Creating shortcuts...

REM Desktop shortcut
set "DESKTOP=%USERPROFILE%\Desktop\Ghost Scanner.lnk"
powershell -Command "$WshShell = New-Object -ComObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%DESKTOP%'); $Shortcut.TargetPath = '%EXE_PATH%'; $Shortcut.WorkingDirectory = '%INSTALL_DIR%'; $Shortcut.Description = 'Ghost Scanner - Ultimate AI Helper'; $Shortcut.IconLocation = '%EXE_PATH%'; $Shortcut.Save()" 2>nul
if exist "%DESKTOP%" (
    echo ✓ Created desktop shortcut
) else (
    echo ⚠️  Could not create desktop shortcut
)

REM Start Menu shortcut
set "START_MENU=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Ghost Scanner.lnk"
powershell -Command "$WshShell = New-Object -ComObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%START_MENU%'); $Shortcut.TargetPath = '%EXE_PATH%'; $Shortcut.WorkingDirectory = '%INSTALL_DIR%'; $Shortcut.Description = 'Ghost Scanner - Ultimate AI Helper'; $Shortcut.IconLocation = '%EXE_PATH%'; $Shortcut.Save()" 2>nul
if exist "%START_MENU%" (
    echo ✓ Created Start Menu shortcut
) else (
    echo ⚠️  Could not create Start Menu shortcut
)

echo.
echo ========================================
echo   ✅ Installation Complete!
echo ========================================
echo.
echo 👻 Ghost Scanner has been installed!
echo.
echo Location: %INSTALL_DIR%
echo.
echo Would you like to launch Ghost Scanner now?
echo.
choice /C YN /N /M "Launch now? (Y/N): "
if errorlevel 2 goto :end
if errorlevel 1 (
    echo.
    echo 🚀 Starting Ghost Scanner...
    start "" "%EXE_PATH%"
    timeout /t 2 >nul
)

:end
echo.
echo Thank you for installing Ghost Scanner!
echo Made by Soul and Lapex 👻
echo.
pause
