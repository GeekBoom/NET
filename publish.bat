@echo off
chcp 65001 >nul
echo ============================================
echo    电商小助手 - 一键编译工具 (.NET Framework 4.8)
echo ============================================
echo.

REM 查找 MSBuild
set MSBUILD_PATH=

REM 尝试查找 Visual Studio 2022
for /f "delims=" %%i in ('dir /b /s "C:\Program Files\Microsoft Visual Studio\2022\*MSBuild.exe" 2^>nul ^| findstr /i "Current\\Bin\\MSBuild.exe"') do (
    set MSBUILD_PATH=%%i
    goto :found
)

REM 尝试查找 Visual Studio 2019
for /f "delims=" %%i in ('dir /b /s "C:\Program Files (x86)\Microsoft Visual Studio\2019\*MSBuild.exe" 2^>nul ^| findstr /i "Current\\Bin\\MSBuild.exe"') do (
    set MSBUILD_PATH=%%i
    goto :found
)

REM 尝试查找 Build Tools
for /f "delims=" %%i in ('dir /b /s "C:\Program Files (x86)\Microsoft Visual Studio\*MSBuild.exe" 2^>nul ^| findstr /i "Current\\Bin\\MSBuild.exe"') do (
    set MSBUILD_PATH=%%i
    goto :found
)

:found
if "%MSBUILD_PATH%"=="" (
    echo [错误] 未检测到 MSBuild！
    echo.
    echo 请安装以下任一工具：
    echo 1. Visual Studio 2022 (Community/Professional/Enterprise)
    echo    下载地址: https://visualstudio.microsoft.com/zh-hans/downloads/
    echo    安装时勾选 ".NET 桌面开发" 工作负载
    echo.
    echo 2. Visual Studio Build Tools 2022
    echo    下载地址: https://visualstudio.microsoft.com/zh-hans/visual-cpp-build-tools/
    echo    安装时勾选 "MSBuild" 和 ".NET Framework 4.8 目标包"
    echo.
    echo 安装完成后重新运行此脚本即可生成 exe 文件。
    pause
    exit /b 1
)

echo 找到 MSBuild: %MSBUILD_PATH%
echo.

echo [1/3] 正在恢复 NuGet 包...
nuget restore ECommerceAssistant\ECommerceAssistant.csproj 2>nul
if %errorlevel% neq 0 (
    echo [提示] nuget.exe 未找到，尝试使用 MSBuild 直接编译...
)

echo.
echo [2/3] 正在编译项目...
"%MSBUILD_PATH%" ECommerceAssistant\ECommerceAssistant.csproj /p:Configuration=Release /p:Platform=AnyCPU /verbosity:minimal

if %errorlevel% neq 0 (
    echo.
    echo [错误] 编译失败！请检查上方错误信息。
    pause
    exit /b 1
)

echo.
echo [3/3] 编译完成！
echo.
echo ============================================
echo  可执行文件位置:
echo  ECommerceAssistant\bin\Release\电商小助手.exe
echo.
echo  你可以将此文件复制到任意位置双击运行！
echo ============================================
echo.
explorer ECommerceAssistant\bin\Release
pause
@echo off
chcp 65001 >nul
echo ============================================
echo    电商小助手 - 一键发布工具
echo ============================================
echo.

REM 检查 dotnet SDK 是否安装
where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo [错误] 未检测到 .NET 8 SDK！
    echo.
    echo 请先安装 .NET 8 SDK:
    echo 下载地址: https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    echo 安装完成后重新运行此脚本即可生成 exe 文件。
    pause
    exit /b 1
)

echo [1/3] 正在恢复依赖包...
dotnet restore
if %errorlevel% neq 0 (
    echo [错误] 依赖恢复失败！
    pause
    exit /b 1
)

echo.
echo [2/3] 正在发布为 Windows 可执行文件...
dotnet publish ECommerceAssistant\ECommerceAssistant.csproj -c Release -r win-x64 --self-contained true -o publish -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

if %errorlevel% neq 0 (
    echo [错误] 发布失败！
    pause
    exit /b 1
)

echo.
echo [3/3] 发布完成！
echo.
echo ============================================
echo  可执行文件位置:
echo  publish\电商小助手.exe
echo.
echo  你可以将此文件复制到任意位置双击运行！
echo ============================================
echo.
explorer publish
pause
