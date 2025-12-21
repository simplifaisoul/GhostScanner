@echo off
REM Ghost Scanner - Simple Installer
REM Made by Soul and Lapex

echo ========================================
echo   Ghost Scanner - Installer
echo   Made by Soul and Lapex
echo ========================================
echo.

REM Check for admin
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERROR: This installer requires administrator privileges.
    echo Please right-click and select "Run as administrator"
    pause
    exit /b 1
)

set "INSTALL_DIR=%ProgramFiles%\GhostScanner"
set "EXE_PATH=%INSTALL_DIR%\GhostScanner.exe"

echo Installing to: %INSTALL_DIR%
echo.

REM Create directory
if not exist "%INSTALL_DIR%" mkdir "%INSTALL_DIR%"

REM Find GhostScanner.exe
set "SOURCE_EXE="
if exist "GhostScanner.exe" (
    set "SOURCE_EXE=GhostScanner.exe"
) else if exist "GhostScanner\bin\Release\net6.0-windows\GhostScanner.exe" (
    set "SOURCE_EXE=GhostScanner\bin\Release\net6.0-windows\GhostScanner.exe"
) else if exist "bin\Release\net6.0-windows\GhostScanner.exe" (
    set "SOURCE_EXE=bin\Release\net6.0-windows\GhostScanner.exe"
) else (
    echo ERROR: GhostScanner.exe not found!
    echo.
    echo Please build the application first:
    echo   1. Open GhostScanner.sln in Visual Studio
    echo   2. Press F6 to build
    echo   3. Run this installer again
    echo.
    pause
    exit /b 1
)

echo Copying files...
copy /Y "%SOURCE_EXE%" "%INSTALL_DIR%\" >nul

REM Copy DLLs
if exist "GhostScanner\bin\Release\net6.0-windows\*.dll" (
    copy /Y "GhostScanner\bin\Release\net6.0-windows\*.dll" "%INSTALL_DIR%\" >nul
)
if exist "bin\Release\net6.0-windows\*.dll" (
    copy /Y "bin\Release\net6.0-windows\*.dll" "%INSTALL_DIR%\" >nul
)

if not exist "%EXE_PATH%" (
    echo ERROR: Installation failed!
    pause
    exit /b 1
)

echo Creating shortcuts...

REM Desktop shortcut
set "DESKTOP=%USERPROFILE%\Desktop\Ghost Scanner.lnk"
powershell -Command "$WshShell = New-Object -ComObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%DESKTOP%'); $Shortcut.TargetPath = '%EXE_PATH%'; $Shortcut.WorkingDirectory = '%INSTALL_DIR%'; $Shortcut.Description = 'Ghost Scanner'; $Shortcut.Save()" 2>nul

REM Start Menu shortcut
set "START_MENU=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Ghost Scanner.lnk"
powershell -Command "$WshShell = New-Object -ComObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%START_MENU%'); $Shortcut.TargetPath = '%EXE_PATH%'; $Shortcut.WorkingDirectory = '%INSTALL_DIR%'; $Shortcut.Description = 'Ghost Scanner'; $Shortcut.Save()" 2>nul

echo.
echo ========================================
echo   Installation Complete!
echo ========================================
echo.
echo Ghost Scanner installed to: %INSTALL_DIR%
echo.
echo Launch Ghost Scanner now? (Y/N)
choice /C YN /N /M "Your choice: "
if errorlevel 2 goto :end
if errorlevel 1 start "" "%EXE_PATH%"

:end
pause
