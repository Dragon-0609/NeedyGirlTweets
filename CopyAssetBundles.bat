@echo off

if not exist "%~1\AssetBundlesToCopy.txt" exit /b

for /f "usebackq delims=" %%F in ("%~1\AssetBundlesToCopy.txt") do (
    if exist "%~1\AssetBundles\%%F" (
        xcopy /Y "%~1\AssetBundles\%%F" "..\Libraries\dev_plugins\"
    )
)