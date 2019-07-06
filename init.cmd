@echo off

pushd "%~dp0"

powershell -ExecutionPolicy Bypass -Command .\scripts\setup\Initialize-Environment.ps1