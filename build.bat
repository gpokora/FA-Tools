@echo off
REM Build script for IDNAC Calculator Revit 2024 Addin
REM Run this from the project directory

echo Building IDNAC Calculator for Revit 2024...

REM Check if MSBuild is available
where msbuild >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo MSBuild not found in PATH. Please run from Visual Studio Developer Command Prompt.
    pause
    exit /b 1
)

REM Build the project
echo Building project...
msbuild IDNACCalculator.csproj /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal

if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    pause
    exit /b 1
)

echo Build completed successfully!

REM Check if files were copied to Revit addins folder
set ADDIN_PATH=%APPDATA%\Autodesk\Revit\Addins\2024
echo Checking addin installation...

if exist "%ADDIN_PATH%\IDNACCalculator.dll" (
    echo ✓ IDNACCalculator.dll found in addins folder
) else (
    echo ⚠ IDNACCalculator.dll not found in addins folder
    echo Manually copy bin\Release\IDNACCalculator.dll to %ADDIN_PATH%
)

if exist "%ADDIN_PATH%\IDNACCalculator.addin" (
    echo ✓ IDNACCalculator.addin found in addins folder
) else (
    echo ⚠ IDNACCalculator.addin not found in addins folder
    echo Manually copy IDNACCalculator.addin to %ADDIN_PATH%
)

echo.
echo Installation complete! Start Revit 2024 to use the IDNAC Calculator.
echo The tool will appear in the "Fire Alarm Tools" ribbon panel.
echo.
pause