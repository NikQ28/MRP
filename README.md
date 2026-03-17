# MRP — Позиции и спецификации

Фронтенд (Vite + React) и бэкенд (ASP.NET Core) в одном репозитории.

## Запуск всего одной командой

Из **корня проекта**:

```bash
npm install
npm run install-all
npm run dev
```

- **API**: http://localhost:5092  
- **Фронт**: http://localhost:5173 — открой в браузере.

Остановка: `Ctrl+C` в терминале (остановит и API, и фронт).

## Отдельный запуск

- **Только API**: `npm run dev:api` из корня или `dotnet run` из `backend/Mrp.API`
- **Только фронт**: `npm run dev:front` из корня или `npm run dev` из `frontend`

## Если порт 5092 занят

Ошибка «address already in use» значит, что API уже запущен или порт занят другим процессом. Останови предыдущий запуск (`Ctrl+C`) или освободи порт:

```bash
# Узнать процесс на порту 5092 (PowerShell):
Get-NetTCPConnection -LocalPort 5092 -ErrorAction SilentlyContinue | Select-Object OwningProcess

# Завершить процесс по PID (подставь номер из вывода):
Stop-Process -Id <PID> -Force
```

## Требования

- Node.js (для фронта и скрипта запуска)
- .NET 9 SDK (для API)
- PostgreSQL (строка подключения в `backend/Mrp.API/appsettings.Development.json`)
