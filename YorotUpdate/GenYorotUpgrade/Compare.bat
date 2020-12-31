@echo off
title Yorot Upgrade List
powershell -file ".\Compare.ps1"
del vNew.txt
del vOld.txt
rd /Q /S vOld >nul
ren vNew vOld
md vNew
echo Done!
pause >nul