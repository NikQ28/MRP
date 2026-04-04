@echo off
start /b "Backend" cmd /k "cd backend && dotnet run"
start /b "Frontend" cmd /k "cd frontend && npm run dev"