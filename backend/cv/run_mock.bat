@echo off
cd /d %~dp0
set USE_MOCK=true
python -m uvicorn app.main:app --reload
pause