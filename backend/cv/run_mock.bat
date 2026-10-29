@echo off
cd /d %~dp0
set USE_MOCK=true
set PORT=8000
python -m uvicorn app.main:app --reload --port %PORT% 
pause
