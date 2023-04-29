CHCP 850
cls
call vcvars64.bat

echo ****************************************
echo ** Building project
echo ****************************************

set repositoryPath=%cd%

rmdir /s /q build
mkdir build
cd build

set configuration=%1
set ilcPath=%2

IF "%1"=="" (
    set configuration=Debug
)

REM IF "%2"=="" (
	REM set ilcPath="%CORERT_PATH%/Windows_NT.x64.%configuration%"
REM )

dotnet publish ../Platform/Foundation/XPDev.Foundation/XPDev.Foundation.csproj -r win-x64 -c %configuration% -o .
dotnet publish ../Platform/Modularization/XPDev.Modularization/XPDev.Modularization.csproj -r win-x64 -c %configuration% -o .
dotnet publish ../Platform/Audio/XPDev.Audio/XPDev.Audio.csproj -r win-x64 -c %configuration% -o .
dotnet publish ../Platform/XPlugin/XPlugin/XPlugin.csproj -r win-x64 -c %configuration% -o .
dotnet publish ../Modules/FlightManagement/XPDev.FlightManagement/XPDev.FlightManagement.csproj -r win-x64 -c %configuration% -o .
dotnet publish ../Modules/FlightSoundsManagement/XPDev.FlightSoundsManagement/XPDev.FlightSoundsManagement.csproj -r win-x64 -c %configuration% -o .
dotnet publish ../Products/ExtensionPack/XPDev.ExtensionPack/XPDev.ExtensionPack.csproj -r win-x64 -c %configuration% /p:IlcPath=%ilcPath% /t:LinkNative -o .
PAUSE