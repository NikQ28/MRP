@echo off
start /b "Backend" cmd /k "cd backend && dotnet run --launch-profile https"
timeout /t 5
start /b "Frontend" cmd /k "cd frontend && npm run dev"